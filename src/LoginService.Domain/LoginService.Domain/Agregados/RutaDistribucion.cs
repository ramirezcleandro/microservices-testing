using Joseco.DDD.Core.Abstractions;
using Logistica.Domain.Events;
using LogisticaService.Domain.Enums;
using LogisticaService.Domain.Events;
using LogisticaService.Domain.ValueObjects;

namespace LogisticaService.Domain.Agregados
{
    public  class RutaDistribucion : AggregateRoot
    {

       
        public EstadoRuta EstadoRuta { get; private set; }
        public DateOnly Fecha { get; private set; } 
        public Guid PersonalEntregaId { get; private set; }
       
        public DireccionGeolocalizada AlmacenUbicacion { get; private set; }

        
        private readonly List<PuntoEntrega> _puntos = new();


        public IReadOnlyCollection<PuntoEntrega> Puntos => _puntos.AsReadOnly();


        private RutaDistribucion() { }

        
        public RutaDistribucion(Guid id, DateOnly fecha, Guid personalId, DireccionGeolocalizada almacen) : base(id)
        {
            if (almacen == null)
                throw new ArgumentNullException(nameof(almacen));

            Fecha = fecha;
            PersonalEntregaId = personalId;
            AlmacenUbicacion = almacen;
            EstadoRuta = EstadoRuta.Creada;
        }

        public void Iniciar()
        {
            if (EstadoRuta != EstadoRuta.Creada)
                throw new InvalidOperationException("Solo se puede iniciar una ruta en estado 'Creada'.");
            if (_puntos.Count == 0)
                throw new InvalidOperationException("No se puede iniciar una ruta sin puntos.");

            EstadoRuta = EstadoRuta.EnCurso;
            AddDomainEvent(new RutaIniciadaEvent(Id, PersonalEntregaId));
        }


        public PuntoEntrega AgregarPaquete(Guid paqueteId)
        {
            if (EstadoRuta == EstadoRuta.Completada)
                throw new InvalidOperationException("No se pueden agregar paquetes a una ruta completada.");

            if (_puntos.Any(p => p.PaqueteId == paqueteId))
                throw new InvalidOperationException($"El paquete {paqueteId} ya está en la ruta.");

            var nuevoPunto = new PuntoEntrega(Guid.NewGuid(), paqueteId, _puntos.Count + 1);
            _puntos.Add(nuevoPunto);

            AddDomainEvent(new PaqueteAgregadoARutaEvent(Id, paqueteId));


            return nuevoPunto;
        }

        public void MarcarPuntoEntregadoPorPaquete(Guid paqueteId)
        {
            var punto = _puntos.FirstOrDefault(p => p.PaqueteId == paqueteId);
            if (punto is null)
                throw new InvalidOperationException("El paquete no pertenece a esta ruta.");
            punto.MarcarComoEntregado();
        }


        public void OptimizarRuta(Dictionary<Guid, int> nuevoOrdenPaquetes)
        {
            if (EstadoRuta != EstadoRuta.Creada)
                throw new InvalidOperationException("Solo se pueden optimizar rutas en estado 'Creada'.");

            if (_puntos.Count != nuevoOrdenPaquetes.Count)
                throw new InvalidOperationException("El nuevo orden debe incluir todos los puntos existentes.");

            var idsExistentes = _puntos.Select(p => p.PaqueteId).OrderBy(x => x).ToArray();
            var idsNuevoOrden = nuevoOrdenPaquetes.Keys.OrderBy(x => x).ToArray();
            if (!idsExistentes.SequenceEqual(idsNuevoOrden))
                throw new InvalidOperationException("El nuevo orden contiene paquetes inexistentes o faltan paquetes.");

            foreach (var punto in _puntos)
            {
                if (nuevoOrdenPaquetes.TryGetValue(punto.PaqueteId, out var nuevaSecuencia))
                    punto.ReasignarSecuencia(nuevaSecuencia);
            }

            EstadoRuta = EstadoRuta.Optimizada;

            AddDomainEvent(new RutaOptimizadaEvent(
                this.Id,
                this.PersonalEntregaId,
                this.Puntos.OrderBy(p => p.Secuencia).ToList()
            ));
        }
        public void CompletarSiTodosEntregados()
        {
            if (_puntos.Count == 0) return;
            if (_puntos.All(p => p.EstadoPunto == EstadoPuntoEntrega.Entregado))
            {
                EstadoRuta = EstadoRuta.Completada;
                AddDomainEvent(new RutaCompletadaEvent(Id, PersonalEntregaId));
            }
        }
    }
}

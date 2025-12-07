using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistica.Infrastructure.Persistence.PersistenceModel.EFCoreEntities
{
    [Table("RutasDistribucion")]
    public class RutaDistribucionPersistence
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(20)]
        public string EstadoRuta { get; set; } = default!;

        [Required]
        public DateOnly Fecha { get; set; }

        [Required]
        public Guid PersonalEntregaId { get; set; }

        // Propiedades del Almacén
        [Column("Almacen_DireccionCompleta")]
        [StringLength(250)]
        public string AlmacenDireccionCompleta { get; set; } = default!;

        [Column("Almacen_Latitud")]
        public double AlmacenLatitud { get; set; }

        [Column("Almacen_Longitud")]
        public double AlmacenLongitud { get; set; }

        // Relación con puntos de entrega
        public ICollection<PuntoEntregaPersistence> PuntosEntrega { get; set; } = new List<PuntoEntregaPersistence>();
    }
}

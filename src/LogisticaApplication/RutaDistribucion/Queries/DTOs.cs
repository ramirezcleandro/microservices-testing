namespace Logistica.Application.RutaDistribucion.Queries
{
    public record PuntoEntregaDto(Guid Id, Guid PaqueteId, int Secuencia, string Estado);

    public record DetalleRutaDto(
        Guid RutaId,
        string EstadoRuta,
        DateOnly Fecha,
        Guid PersonalEntregaId,
        string AlmacenDireccion,
        double AlmacenLat,
        double AlmacenLng,
        List<PuntoEntregaDto> Puntos
    );

    public record ProgresoRutaDto(
        Guid RutaId,
        string EstadoRuta,
        DateOnly Fecha,
        Guid PersonalEntregaId,
        int Total,
        int Entregados,
        int Pendientes,
        double Porcentaje,
        int? SiguienteSecuencia,
        List<PuntoEstadoDto> Puntos
    );

    public record RutaItemDto(
        Guid RutaId,
        DateOnly Fecha,
        string EstadoRuta,
        int TotalPuntos
    );
    public record PuntoEstadoDto(Guid PuntoId, Guid PaqueteId, int Secuencia, string Estado);

}

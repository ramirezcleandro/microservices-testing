using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistica.Infrastructure.Persistence.PersistenceModel.EFCoreEntities
{
    [Table("PaquetesEntrega")]
    public class PaqueteEntregaPersistence
    {
        
        [Key]
        public Guid Id { get; set; }

        
        [Required]
        [StringLength(50)]
        public string EtiquetaId { get; set; }

        [Required]
        [StringLength(20)]
        public string EstadoPaquete { get; set; } // Se mapea el Enum como string

        
        
        [Column("Destino_DireccionCompleta")]
        [StringLength(250)]
        public string DestinoDireccionCompleta { get; set; }

        [Column("Destino_Latitud")]
        public double DestinoLatitud { get; set; }

        [Column("Destino_Longitud")]
        public double DestinoLongitud { get; set; }

  

        // Propiedades de RegistroEntrega
        [Column("Registro_TimestampConfirmacion")]
        public DateTime? RegistroTimestampConfirmacion { get; set; } 

        [Column("Registro_TipoPrueba")]
        [StringLength(50)]
        public string? RegistroTipoPrueba { get; set; }

        [Column("Registro_UrlEvidencia")]
        [StringLength(500)]
        public string? RegistroUrlEvidencia { get; set; }

        
        [Column("Registro_Latitud")]
        public double? RegistroLatitud { get; set; } 

        [Column("Registro_Longitud")]
        public double? RegistroLongitud { get; set; } 
    }
}

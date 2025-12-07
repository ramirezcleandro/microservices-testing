using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistica.Infrastructure.Persistence.PersistenceModel.EFCoreEntities
{
    [Table("PuntosEntrega")]
    public class PuntoEntregaPersistence
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid PaqueteId { get; set; }

        [Required]
        public int Secuencia { get; set; }

        [Required]
        [StringLength(20)]
        public string EstadoPunto { get; set; } = default!;

        // FK hacia RutaDistribucion
        [ForeignKey(nameof(RutaDistribucion))]
        public Guid RutaDistribucionId { get; set; }

        public RutaDistribucionPersistence RutaDistribucion { get; set; } = default!;
    }
}

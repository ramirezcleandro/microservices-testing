using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticaService.Domain.Enums
{
    public enum EstadoRuta
    {
        // 0: El estado inicial
        Creada = 0,
        
        // 1: Después de la optimización del algoritmo
        Optimizada = 1,
        
        // 2: Cuando el personal ha iniciado el recorrido
        Iniciada = 2,
        
        // 3: Cuando todos los puntos han sido completados o marcados como fallidos
        Completada = 3,
        
        // 4: Si se cancela por alguna razón (ej. clima)
        Cancelada = 4,

        // 5: Cuando Inicia la Ruta
        EnCurso = 5
    }
}

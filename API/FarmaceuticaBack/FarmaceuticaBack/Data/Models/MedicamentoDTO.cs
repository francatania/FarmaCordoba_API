using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmaceuticaBack.Data.Models
{
    public class MedicamentoDTO
    {
        public int IdMedicamento { get; set; }
        public string NombreMedicamento { get; set; }
        public string Laboratorio { get; set; } 

        public string Monodroga { get; set; }

        public string Presentacion { get; set; }

        public string Marca { get; set; }

        public string Descripcion { get; set; }
        public bool VentaLibre { get; set; }

        public decimal Precio { get; set; }

        public bool? Activo { get; set; }


    }
}

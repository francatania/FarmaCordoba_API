using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmaceuticaBack.Data.Models
{
    public class MedicamentoSaveDTO
    {

        public int IdMedicamento { get; set; }
        public string NombreMedicamento { get; set; }
        public int IdLaboratorio { get; set; }

        public int IdMonodroga { get; set; }

        public int IdPresentacion { get; set; }

        public int IdMarca { get; set; }

        public string Descripcion { get; set; }
        public bool VentaLibre { get; set; }

        public decimal Precio { get; set; }

        public bool? Activo { get; set; }
    }
}

using Laboratorio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Laboratorio.ViewModel
{
    public class OrdemServicoViewModel
    {

        public int OrdemServicoId { get; set; }
        public int PacienteId { get; set; }
        public string Convenio { get; set; }
        public int PostoColetaId { get; set; }
        public int MedicoId { get; set; }
        public System.DateTime Data { get; set; }
        public virtual List<OrdemServicoExame> OrdemServicoExame { get; set; }

    }
}
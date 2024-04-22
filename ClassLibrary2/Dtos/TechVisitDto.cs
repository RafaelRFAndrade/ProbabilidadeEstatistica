using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary2.Dtos
{
    public class TechVisitDto
    {
        public string Id { get; set; }
        public string DataDeAbertura { get; set; }
        public string NomeCliente { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string FilialDoServico { get; set; }
        public string Motivo { get; set; }
        public string DataProgramada { get; set; }
        public string DataDoServico { get; set; }
        public string HoraInicio { get; set; }
        public string HoraTermino { get; set; }
        public string TempoAtendimento { get; set; }
        public string DataReprogramacao { get; set; }
    }
}

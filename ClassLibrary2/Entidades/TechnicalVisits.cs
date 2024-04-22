using Abp.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassLibrary2.Entidades
{
    [Table("AppTechnicalVisits")]
    [Audited]
    public class TechnicalVisits
    {
        [Key]
        public string Id { get; set; }

        public DateTime DataDeAbertura { get; set; }
        public string NomeCliente { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string FilialDoServico { get; set; }
        public string Motivo { get; set; }
        public DateTime DataProgramada { get; set; }
        public DateTime DataDoServico { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraTermino { get; set; }
        public TimeSpan TempoAtendimento { get; set; }
        public DateTime DataReprogramacao { get; set; }
    }
}
using Abp.Domain.Repositories;
using ClassLibrary1;
using ClassLibrary2.Dtos;
using ClassLibrary2.Entidades;
using ClassLibrary2.Repositorio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace ProbabilidadeEstatistica.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TechVisitsController : ControllerBase
    {
        private Contexto _db;
        public TechVisitsController(Contexto db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        #region GetAll
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var techVisits = _db.TechnicalVisits.ToList();
            return Ok(techVisits.Count);
        }
        #endregion

        #region GetByState
        [HttpGet("GetByState")]
        public IActionResult GetByState(string state)
        {
            var techVisits = _db.TechnicalVisits.Where(x => x.Estado == state || x.Estado == state.ToUpper()).ToList();

            return Ok(techVisits.Count);
        }
        #endregion

        #region GetByCity
        [HttpGet("GetByCity")]
        public IActionResult GetByCity(string city)
        {
            var techVisits = _db.TechnicalVisits.Where(x => x.Cidade == city).ToList();

            return Ok(techVisits.Count);
        }
        #endregion

        #region GetByMonth
        [HttpGet("GetByMonth")]
        public IActionResult GetByMonth(int month)
        {
            var techVisits = _db.TechnicalVisits.Where(x => x.DataDoServico.Month == month).ToList();

            return Ok(techVisits.Count);
        }
        #endregion

        #region GetPerMonths
        [HttpGet("GetPerMonth")]
        public IActionResult GetperMonths()
        {
            var techVisits = _db.TechnicalVisits.ToList();

            var jan = techVisits.Where(x => x.DataDoServico.Month == 01).Count().ToString();
            var feb = techVisits.Where(x => x.DataDoServico.Month == 02).Count().ToString();
            var mar = techVisits.Where(x => x.DataDoServico.Month == 03).Count().ToString();
            var apr = techVisits.Where(x => x.DataDoServico.Month == 04).Count().ToString();
            var may = techVisits.Where(x => x.DataDoServico.Month == 05).Count().ToString();
            var jun = techVisits.Where(x => x.DataDoServico.Month == 06).Count().ToString();
            var jul = techVisits.Where(x => x.DataDoServico.Month == 07).Count().ToString();
            var aug = techVisits.Where(x => x.DataDoServico.Month == 08).Count().ToString();
            var sep = techVisits.Where(x => x.DataDoServico.Month == 09).Count().ToString();
            var oct = techVisits.Where(x => x.DataDoServico.Month == 10).Count().ToString();
            var nov = techVisits.Where(x => x.DataDoServico.Month == 11).Count().ToString();
            var dec = techVisits.Where(x => x.DataDoServico.Month == 12).Count().ToString();

            return Ok(jan + ";" + feb + ";" + mar + ";"+ apr + ";" + may + ";" + jun + ";" + jul + ";" + aug + ";" + sep + ";" + oct + ";" + nov + ";" + dec);
        }
        #endregion

        #region AddTechVisits
        [HttpPost("AddTechVisits")]
        public IActionResult AddTechVisits(List<TechVisitDto> techVisits)
        {
            if (techVisits.Count > 0)
            {
                foreach (var inputModel in techVisits)
                {
                    var techVisit = new TechnicalVisits
                    {
                        Id = inputModel.Id,
                        DataDeAbertura = DateTime.Parse(inputModel.DataDeAbertura).ToUniversalTime().Date,
                        NomeCliente = inputModel.NomeCliente,
                        Cidade = inputModel.Cidade,
                        Estado = inputModel.Estado,
                        FilialDoServico = inputModel.FilialDoServico,
                        Motivo = inputModel.Motivo,
                        DataProgramada = DateTime.Parse(inputModel.DataProgramada).ToUniversalTime().Date,
                        DataDoServico = DateTime.Parse(inputModel.DataDoServico).ToUniversalTime().Date,
                        HoraInicio = TimeSpan.Parse(inputModel.HoraInicio),
                        HoraTermino = TimeSpan.Parse(inputModel.HoraTermino),
                        TempoAtendimento = TimeSpan.Parse(inputModel.TempoAtendimento),
                        DataReprogramacao = DateTime.Parse(inputModel.DataReprogramacao).ToUniversalTime().Date
                    };

                    _db.TechnicalVisits.Add(techVisit);
                }

                _db.SaveChanges();
            }
            return Ok();
        }
        #endregion

        #region VarianciaPopulacional
        [HttpPost("VariancaPopulacional")]
        public ActionResult<double> CalcularVP(List<double> numeros)
        {
            double variancia = numeros.Sum(val => Math.Pow(val - Media(numeros), 2)) / numeros.Count;

            return variancia;
        }
        #endregion

        #region VarianciaAmostral
        [HttpPost("VariancaAmostral")]
        public ActionResult<double> CalcularVA(List<double> numeros)
        {
            double variancia = numeros.Sum(val => Math.Pow(val - Media(numeros), 2)) / (numeros.Count - 1);

            return variancia;
        }
        #endregion

        #region DesvioPopulacional
        [HttpPost("DP")]
        public ActionResult<double> CalcularDP(List<double> numeros)
        {
            return Math.Sqrt(numeros.Sum(val => Math.Pow(val - Media(numeros), 2)) / numeros.Count);
        }
        #endregion

        #region DesvioAmostral
        [HttpPost("DA")]
        public ActionResult<double> CalcularDA(List<double> numeros)
        {
            return Math.Sqrt(numeros.Sum(val => Math.Pow(val - Media(numeros), 2)) / (numeros.Count - 1));
        }
        #endregion

        #region CV
        [HttpPost("Coeficiente")]
        public ActionResult<string> CalcularCoeficiente(List<double> numeros)
        {
            string amostral = (((Math.Sqrt(numeros.Sum(val => Math.Pow(val - Media(numeros), 2)) / (numeros.Count - 1))) / Media(numeros)) * 100).ToString("F2") + "%";
            string populacional = (((Math.Sqrt(numeros.Sum(val => Math.Pow(val - Media(numeros), 2)) / numeros.Count)) / Media(numeros)) * 100).ToString("F2") + "%";

            return Ok("amostral = " + amostral + "\n" + "populacional = " + populacional);
        }
        #endregion

        #region Amplitude
        [HttpPost("Amplitude")]
        public ActionResult<double> Amplitude(List<double> numeros)
        {
            double maior = numeros.Max();
            double menor = numeros.Min();

            double amplitude = maior - menor;

            return Ok(amplitude);
        }
        #endregion

        #region GetManutencaoPrev
        [HttpGet("PegarTodosDaManutençãoPreventiva")]
        public ActionResult<double> GetManutencaoPrev()
        {
            var consultas = _db.TechnicalVisits.Where(x => x.Motivo == "Manutenção preventiva").ToList();

            return consultas.Count();
        }
        #endregion

        #region GetManutencaoPreventiva
        [HttpGet("GetManutencaoPreventiva")]
        public ActionResult<double> GetManutencaoPreventiva(string motivo)
        {
            var consultas = _db.TechnicalVisits.Where(x => x.Motivo == motivo).ToList();

            return consultas.Count();
        }
        #endregion

        #region GetDataEspecifica
        [HttpGet("GetDataEspecifica")]
        public ActionResult<double> GetDataEspecifica(string dia, string mes)
        {
            string data = $"2023/{mes}/{dia}";

            DateTime dataAtualizada = DateTime.Parse(data).ToUniversalTime().Date;

            var consultas = _db.TechnicalVisits.Where(x => x.DataDeAbertura == dataAtualizada).ToList();

            return consultas.Count();
        }
        #endregion

        #region MediaModaMediana
        [HttpPost("Estatisticas")]
        public IActionResult Estatisticas(List<double> numeros)
        {
            if (numeros == null || !numeros.Any())
            {
                return BadRequest("A lista de números está vazia ou nula.");
            }

            var numerosOrdenados = numeros.OrderBy(x => x).ToList();

            double media = numerosOrdenados.Average();

            var grupos = numerosOrdenados.GroupBy(x => x)
                                         .OrderByDescending(g => g.Count())
                                         .ToList();

            var moda = grupos.First().Count() == 1 ? null : grupos.Where(g => g.Count() == grupos.First().Count()).Select(g => g.Key).ToList();

            double mediana;

            if (numerosOrdenados.Count % 2 == 0)
            {
                mediana = (numerosOrdenados[(numerosOrdenados.Count / 2) - 1] + numerosOrdenados[numerosOrdenados.Count / 2]) / 2.0;
            }
            else
            {
                mediana = numerosOrdenados[numerosOrdenados.Count / 2];
            }

            var resultado = new
            {
                ListaOrdenada = numerosOrdenados,
                Media = media.ToString("F2"),
                Moda = moda,
                Mediana = mediana
            };

            return Ok(resultado);
        }
        #endregion

        private double Media(List<double> valores)
        {
            return valores.Average();
        }
    }
}

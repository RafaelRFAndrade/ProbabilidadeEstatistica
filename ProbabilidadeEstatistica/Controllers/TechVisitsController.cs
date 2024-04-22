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

        #region AddTechVisits
        [HttpPost("AddTechVisits")]
        public IActionResult AddTechVisits(List<TechVisitDto> techVisits)
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

            return Ok();
        }

        [HttpPost("VariancaPopulacional")]
        public ActionResult<double> CalcularVP(List<double> numeros)
        {
            double variancia = numeros.Sum(val => Math.Pow(val - Media(numeros), 2)) / numeros.Count;

            return variancia;
        }

        [HttpPost("VariancaAmostral")]
        public ActionResult<double> CalcularVA(List<double> numeros)
        {
            double variancia = numeros.Sum(val => Math.Pow(val - Media(numeros), 2)) / (numeros.Count - 1);

            return variancia;
        }

        [HttpPost("DP")]
        public ActionResult<double> CalcularDP(List<double> numeros)
        {
            return Math.Sqrt(numeros.Sum(val => Math.Pow(val - Media(numeros), 2)) / numeros.Count);
        }

        [HttpPost("DA")]
        public ActionResult<double> CalcularDA(List<double> numeros)
        {
            return Math.Sqrt(numeros.Sum(val => Math.Pow(val - Media(numeros), 2)) / (numeros.Count - 1));
        }

        [HttpPost("Coeficiente")]
        public ActionResult<string> CalcularCoeficiente(bool EhAmostral, List<double> numeros)
        {
            if (EhAmostral)
            {
                return (((Math.Sqrt(numeros.Sum(val => Math.Pow(val - Media(numeros), 2)) / (numeros.Count - 1))) / Media(numeros))* 100).ToString("F2") + "%";
            }

            return (((Math.Sqrt(numeros.Sum(val => Math.Pow(val - Media(numeros), 2)) / numeros.Count)) / Media(numeros)) * 100).ToString("F2") + "%";
        }

        [HttpPost("PegarTodosDaManutençãoPreventiva")]
        public ActionResult<double> CalcularNumeroPorAno()
        {
            var consultas = _db.TechnicalVisits.Where(x => x.Motivo == "Manutenção preventiva").ToList();

            return consultas.Count();
        }

        [HttpPost("PegarTodosPorData")]
        public ActionResult<double> CalcularNumeroPorAno(string dia, string mes)
        {
            string data = $"2023/{mes}/{dia}";

            DateTime dataAtualizada = DateTime.Parse(data).ToUniversalTime().Date;

            var consultas = _db.TechnicalVisits.Where(x => x.DataDeAbertura == dataAtualizada).ToList();

            return consultas.Count();
        }

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

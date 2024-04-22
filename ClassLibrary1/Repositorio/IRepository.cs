using ClassLibrary1;
using ClassLibrary2.Dtos;
using ClassLibrary2.Entidades;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary2.Repositorio
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> GetAll();
        TEntity GetById(string id);
        void Insert(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        List<TEntity> ExecuteSqlToList(string sql);
    }
    
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly Contexto _context;

        public Repository(Contexto context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _context.Set<TEntity>().ToList();
        }

        public TEntity GetById(string id)
        {
            return _context.Set<TEntity>().Find(id);
        }

        public void Insert(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
        }

        public void Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
        }

        public void Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public List<TEntity>ExecuteSqlToList(string sql)
        {
            return _context.Set<TEntity>().FromSqlRaw(sql).ToList();
        }
    }


    //class Program
    //{
    //    private readonly IRepository<TechnicalVisits> _productRepository;
    //    public Program(IRepository<TechnicalVisits> productRepository)
    //    {
    //        _productRepository = productRepository;
    //    }


    //    public async Task<List<TechVisitDto>> Teste(string state)
    //    {
    //        var toma = _productRepository.GetAll().Where(x => x.Estado == state).ToList();

    //        var lista = new List<TechVisitDto>();

    //        foreach (var item in toma)
    //        {
    //            var output = new TechVisitDto
    //            {
    //                Cidade = item.Cidade,
    //                DataDeAbertura = item.DataDeAbertura.ToString(),
    //                DataDoServico = item.DataDoServico.ToString(),
    //                DataProgramada = item.DataProgramada.ToString(),
    //                DataReprogramacao = item.DataReprogramacao.ToString(),
    //                Estado = item.Estado,
    //                FilialDoServico = item.FilialDoServico,
    //                HoraInicio = item.HoraInicio.ToString(),
    //                HoraTermino = item.HoraTermino.ToString(),
    //                Id = item.Id,
    //                Motivo = item.Motivo,
    //                NomeCliente = item.NomeCliente,
    //                TempoAtendimento = item.TempoAtendimento.ToString()
    //            };

    //            lista.Add(output);
    //        }
    //        return lista;
    //    }
    //}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Data;
using System.Data.Entity;
using System.Web;

using ISP.GestaoMatriculas.Contracts;
using ISP.GestaoMatriculas.Model;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.SqlServer;

namespace ISP.GestaoMatriculas.Repositories
{
    public class IndicadorRepository : IIndicadorRepository
    {
        DomainModels context = new DomainModels();

        public IQueryable<Indicador> All
        {
            get {
                return context.Indicadores;
            }
        }

        public IQueryable<Indicador> AllIncluding(params Expression<Func<Indicador, object>>[] includeProperties)
        {
            IQueryable<Indicador> query = context.Indicadores;

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public Indicador Find(int id)
        {
            return context.Indicadores.Find(id);
        }


        public IQueryable<Indicador> Find(int entidadeId, int tipologiaId)
        {
            return context.Indicadores.Where(i => i.entidadeId == entidadeId && i.tipologiaId == tipologiaId);
        }

        public IQueryable<Indicador> Find(int entidadeId, int tipologiaId, DateTime date)
        {
            return context.Indicadores.Where(i => i.entidadeId == entidadeId && i.tipologiaId == tipologiaId && i.dataIndicador == date);
        }

        public IQueryable<Indicador> Find(int entidadeId, int tipologiaId, string aux, DateTime date)
        {
            return context.Indicadores.Where(i => i.entidadeId == entidadeId && i.tipologiaId == tipologiaId && i.subTipo == aux && i.dataIndicador == date);
        }

        

        public void InsertOrUpdate(Indicador indicador){
            if (indicador.indicadorId == default(int))
            {
                context.Indicadores.Add(indicador);
            }
            else
            {
                context.Entry(indicador).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var indicador = context.Indicadores.Find(id);
            context.Indicadores.Remove(indicador);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }

        /// <summary>
        /// Pocessa indicador do número de eventos submetidos pelas entidades seguradoras
        /// </summary>
        public void processaIndicadorNrEventos()
        {
            int mesIndicador = DateTime.Now.Month;
            DateTime dataIndicador = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            int indicadorId = context.ValoresSistema.Where(i => i.tipologia == "TIPO_INDICADOR" && i.valor == "NR_EVENTOS").Single().valorSistemaId;

            //---------------------------------------------------------------------------
            //NrEventos - Numero de eventos de reporte por seguradora no mês corrente
            //---------------------------------------------------------------------------

            var queryEventos = from en in context.Entidades
                                join ind in
                                    (from ev in
                                         ((from evento in context.EventosHistorico
                                           where EntityFunctions.TruncateTime(evento.dataReporte) == EntityFunctions.TruncateTime(dataIndicador)
                                           select new { evento.entidade.nome, entidadeId = evento.entidadeId.Value, id = evento.eventoHistoricoId }).Concat  
                                          (from evento in context.EventosStagging
                                           where EntityFunctions.TruncateTime(evento.dataReporte) == EntityFunctions.TruncateTime(dataIndicador) && evento.estadoEvento.valor != "PROCESSADO"
                                           select new { evento.entidade.nome, entidadeId = evento.entidadeId.Value, id = evento.eventoStaggingId }))
                                     group new { ev } by new { entidadeNome = ev.nome, ev.entidadeId } into g
                                     select new { g.Key.entidadeNome, g.Key.entidadeId, nrEventos = g.Count() })
                                 on en.entidadeId equals ind.entidadeId into res
                                from x in res.DefaultIfEmpty()
                                where en.ativo == true
                                select new { entidadeNome =  en.nome, en.entidadeId, nrEventos = (x.entidadeId == null? 0 : x.nrEventos) };
                               
            foreach (var evento in queryEventos.ToList())
            {
                Indicador indicador;
                IQueryable<Indicador> indicadores = context.Indicadores.Where(i => i.entidadeId == evento.entidadeId && i.tipologiaId == indicadorId && i.dataIndicador == dataIndicador);

                if (indicadores != null && indicadores.Count() != 0)
                {
                    indicador = indicadores.Single();
                    indicador.valor = evento.nrEventos;
                }
                else
                    indicador = new Indicador
                    {
                        entidadeId = evento.entidadeId,
                        tipologiaId = indicadorId,
                        dataIndicador = dataIndicador,
                        publico = true,
                        descricao = string.Format("Número de eventos reportados pela entidade {0} para data {1}.", evento.entidadeNome, dataIndicador.ToShortDateString()),
                        valor = evento.nrEventos
                    };

               this.InsertOrUpdate(indicador);
            }

            this.Save();
        }

        /// <summary>
        /// Pocessa indicador do número de eventos processados pelas entidades seguradoras
        /// </summary>
        public void processaIndicadorNrEventosProcessados()
        {
            int mesIndicador = DateTime.Now.Month;
            DateTime dataIndicador = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            int indicadorId = context.ValoresSistema.Where(i => i.tipologia == "TIPO_INDICADOR" && i.valor == "NR_EVENTOS_PROCESSADOS").Single().valorSistemaId;
   
            
            var queryEventos = from en in context.Entidades
                               join ind in
                                (from ev in context.EventosHistorico
                                 where SqlFunctions.DateDiff("DAY", ev.dataReporte, dataIndicador) == 0
                                 group new { ev } by new { ev.entidade.nome, ev.entidadeId } into g
                                 select new { entidadeNome = g.Key.nome, entidadeId = g.Key.entidadeId.Value, nrEventos = g.Count() })
                               on en.entidadeId equals ind.entidadeId into res
                               from x in res.DefaultIfEmpty()
                               where en.ativo == true
                               select new { entidadeNome = en.nome, en.entidadeId, nrEventos = (x.entidadeId == null ? 0 : x.nrEventos) };

            foreach (var evento in queryEventos.ToList())
            {
                Indicador indicador;
                IQueryable<Indicador> indicadores = context.Indicadores.Where(i => i.entidadeId == evento.entidadeId && i.tipologiaId == indicadorId && i.dataIndicador == dataIndicador);

                if (indicadores != null && indicadores.Count() != 0)
                {
                    indicador = indicadores.Single();
                    indicador.valor = evento.nrEventos;
                }
                else
                    indicador = new Indicador
                    {
                        entidadeId = evento.entidadeId,
                        tipologiaId = indicadorId,
                        dataIndicador = dataIndicador,
                        publico = true,
                        descricao = string.Format("Número de eventos processados pela entidade {0} para a data {1}.", evento.entidadeNome, dataIndicador.ToShortDateString()),
                        valor = evento.nrEventos
                    };

                this.InsertOrUpdate(indicador);
            }

            this.Save();
        } 

        /// <summary>
        /// Processa indicador do número de eventos com erros identificados nos eventos submetidos pelas entidades seguradoras
        /// </summary>  
        public void processaIndicadorNrErros()
        {
            int mesIndicador = DateTime.Now.Month;
            DateTime dataIndicador = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            int indicadorId = context.ValoresSistema.Where(i => i.tipologia == "TIPO_INDICADOR" && i.valor == "NR_ERROS_EVENTOS").Single().valorSistemaId;
   
            //-----------------------------------------------------------
            //NrErros - Numero de erros por seguradora no mês corrente
            //-----------------------------------------------------------
            
            var queryErros = from en in context.Entidades
                             join ind in
                                 (from evento in context.EventosStagging
                                  where SqlFunctions.DateDiff("DAY", evento.dataReporte, dataIndicador) == 0 && evento.estadoEvento.valor == "ERRO"
                                  group new { evento } by new { evento.entidade.nome, evento.entidadeId } into g
                                  select new { entidadeNome = g.Key.nome, entidadeId = g.Key.entidadeId.Value, nrErros = g.Count() })
                              on en.entidadeId equals ind.entidadeId into res
                             from x in res.DefaultIfEmpty()
                             where en.ativo == true
                             select new { entidadeNome = en.nome, en.entidadeId, nrErros = (x.entidadeId == null ? 0 : x.nrErros) };

            foreach (var erro in queryErros.ToList())
            {
                //ERRO
                Indicador indicadorErro;
                IQueryable<Indicador> indicadoresErro = context.Indicadores.Where(i => i.entidadeId == erro.entidadeId && i.tipologiaId == indicadorId && i.dataIndicador == dataIndicador);

                if (indicadoresErro != null && indicadoresErro.Count() != 0)
                {
                    indicadorErro = indicadoresErro.Single();
                    indicadorErro.valor = erro.nrErros;
                }
                else
                    indicadorErro = new Indicador
                    {
                        entidadeId = erro.entidadeId,
                        tipologiaId = indicadorId,
                        dataIndicador = dataIndicador,
                        publico = true,
                        descricao = string.Format("Número de eventos reportados com erro pela entidade {0} para a data {1}.", erro.entidadeNome, dataIndicador.ToShortDateString()),
                        valor = erro.nrErros
                    };

                this.InsertOrUpdate(indicadorErro);
            }

            this.Save();
        }


        /// <summary>
        /// Processa indicador do número de eventos com erros identificados nos eventos submetidos pelas entidades seguradoras
        /// </summary>  
        public void processaIndicadorNrErrosPendentes()
        {
            int mesIndicador = DateTime.Now.Month;
            DateTime dataIndicador = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            int indicadorId = context.ValoresSistema.Where(i => i.tipologia == "TIPO_INDICADOR" && i.valor == "NR_ERROS_PENDENTES").Single().valorSistemaId;

            //-----------------------------------------------------------
            //NrErros - Numero de erros por seguradora no mês corrente
            //-----------------------------------------------------------

            var queryErros = from en in context.Entidades
                             join ind in
                                 (from evento in context.EventosStagging
                                  where evento.estadoEvento.valor == "ERRO" && evento.arquivado == false
                                  group new { evento } by new { evento.entidade.nome, evento.entidadeId } into g
                                  select new { entidadeNome = g.Key.nome, entidadeId = g.Key.entidadeId.Value, nrErros = g.Count() })
                              on en.entidadeId equals ind.entidadeId into res
                             from x in res.DefaultIfEmpty()
                             where en.ativo == true
                             select new { entidadeNome = en.nome, en.entidadeId, nrErros = (x.entidadeId == null ? 0 : x.nrErros) };

            foreach (var erro in queryErros.ToList())
            {
                //ERRO
                Indicador indicadorErro;
                IQueryable<Indicador> indicadoresErro = context.Indicadores.Where(i => i.entidadeId == erro.entidadeId && i.tipologiaId == indicadorId && i.dataIndicador == dataIndicador);

                if (indicadoresErro != null && indicadoresErro.Count() != 0)
                {
                    indicadorErro = indicadoresErro.Single();
                    indicadorErro.valor = erro.nrErros;
                }
                else
                    indicadorErro = new Indicador
                    {
                        entidadeId = erro.entidadeId,
                        tipologiaId = indicadorId,
                        dataIndicador = dataIndicador,
                        publico = true,
                        descricao = string.Format("Número de eventos reportados com erro pela entidade {0} e pendentes de correção à data {1}.", erro.entidadeNome, dataIndicador.ToShortDateString()),
                        valor = erro.nrErros
                    };

                this.InsertOrUpdate(indicadorErro);
            }

            this.Save();
        }

        /// <summary>
        /// Processa indicador do número de erros identificados nos eventos submetidos pelas entidades seguradoras
        /// agregados por tipologia de erro
        /// </summary>
        public void processaIndicadorNrErrosTipo()
        {
            int mesIndicador = DateTime.Now.Month;
            DateTime dataIndicador = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            int indicadorId = context.ValoresSistema.Where(i => i.tipologia == "TIPO_INDICADOR" && i.valor == "NR_ERROS_EVENTOS_TIPO").Single().valorSistemaId;
   
            //-----------------------------------------------------------
            //NrErros - Numero de erros por seguradora e tipologia no mês corrente
            //-----------------------------------------------------------
           
            var queryErros =
               from en in
                   (from ent in context.Entidades
                    join v in context.ValoresSistema on "TIPO_ERRO" equals v.tipologia
                    where ent.ativo == true
                    select new { ent.entidadeId, ent.nome, tipologiaId = v.valorSistemaId, tipologia = v.descricao })
               join ind in
                   (from ev in
                        (from evento in context.EventosStagging
                         join erro in context.ErrosEventoStagging on evento.eventoStaggingId equals erro.eventoStaggingId
                         where SqlFunctions.DateDiff("DAY", evento.dataReporte, dataIndicador) == 0
                         select new { entidadeNome = evento.entidade.nome, evento.entidadeId, tipologiaId = erro.tipologiaId, tipologia = erro.tipologia.descricao, id = evento.eventoStaggingId })
                    group new { ev } by new { ev.entidadeNome, ev.entidadeId, ev.tipologiaId, ev.tipologia } into g
                    select new { entidadeNome = g.Key.entidadeNome, entidadeId = g.Key.entidadeId.Value, g.Key.tipologiaId, g.Key.tipologia, nrErros = g.Count() })
                on new { en.entidadeId, en.tipologiaId } equals new { ind.entidadeId, ind.tipologiaId } into res
               from x in res.DefaultIfEmpty()
               select new { entidadeNome = en.nome, en.entidadeId, en.tipologia, nrErros = (x.entidadeId == null ? 0 : x.nrErros) };

            string erroTipologia;
            foreach (var erro in queryErros.ToList())
            {
                //ERRO
                erroTipologia = erro.tipologia;
                Indicador indicadorErro;
                IQueryable<Indicador> indicadoresErro = context.Indicadores.Where(i => i.entidadeId == erro.entidadeId && i.tipologiaId == indicadorId && i.subTipo == erroTipologia && i.dataIndicador == dataIndicador);

                if (indicadoresErro != null && indicadoresErro.Count() != 0)
                {
                    indicadorErro = indicadoresErro.Single();
                    indicadorErro.valor = erro.nrErros;
                }
                else
                    indicadorErro = new Indicador
                    {
                        entidadeId = erro.entidadeId,
                        tipologiaId = indicadorId,
                        subTipo = erro.tipologia.ToString(),
                        dataIndicador = dataIndicador,
                        publico = true,
                        descricao = string.Format("Número de eventos reportados com erro do tipo {0}, pela entidade {1} para a data {2}.", erro.tipologia, erro.entidadeNome, dataIndicador.ToShortDateString()),
                        valor = erro.nrErros
                    };

                this.InsertOrUpdate(indicadorErro);
            }

            this.Save();
        }

        /// <summary>
        /// Processa indicador do número de eventos com avisos identificados nos eventos submetidos pelas entidades seguradoras
        /// </summary>
        public void processaIndicadorNrAvisos()
        {
            int mesIndicador = DateTime.Now.Month;
            DateTime dataIndicador = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            int indicadorId = context.ValoresSistema.Where(i => i.tipologia == "TIPO_INDICADOR" && i.valor == "NR_AVISOS_PERIODO_SEGURO").Single().valorSistemaId;
   

            //---------------------------------------------------------------------------
            //NrAvisos - Numero de avisos de reporte por seguradora e tipo no mês corrente
            //---------------------------------------------------------------------------
            var queryAvisos =
                from en in context.Entidades
                join ind in
                (from av in
                     (from apolice in context.Apolices
                     join aviso in context.Avisos on apolice.apoliceId equals aviso.apoliceId
                      where SqlFunctions.DateDiff("DAY", apolice.dataReporte, dataIndicador) == 0
                      select new { entidadeNome = apolice.entidade.nome, apolice.entidadeId, apolice.apoliceId }).Concat
                     (from apolice in context.ApolicesHistorico
                     join aviso in context.Avisos on apolice.apoliceId equals aviso.apoliceId
                      where SqlFunctions.DateDiff("DAY", apolice.dataReporte, dataIndicador) == 0
                      select new { entidadeNome = apolice.entidade.nome, apolice.entidadeId, apolice.apoliceId })
                 group new { av } by new { av.entidadeNome, av.entidadeId } into g
                 select new { entidadeNome = g.Key.entidadeNome, entidadeId = g.Key.entidadeId.Value, nrAvisos = g.Select(l => l.av.apoliceId).Distinct().Count() })
                on en.entidadeId equals ind.entidadeId into res
                from x in res.DefaultIfEmpty()
                where en.ativo == true
                select new { entidadeNome = en.nome, en.entidadeId, nrAvisos = (x.entidadeId == null ? 0 : x.nrAvisos) };


                               

            foreach (var aviso in queryAvisos.ToList())
            {
                Indicador indicador;
                IQueryable<Indicador> indicadores = context.Indicadores.Where(i => i.entidadeId == aviso.entidadeId && i.tipologiaId == indicadorId && i.dataIndicador == dataIndicador);

                if (indicadores != null && indicadores.Count() != 0)
                {
                    indicador = indicadores.Single();
                    indicador.valor = aviso.nrAvisos;
                }
                else
                    indicador = new Indicador
                    {
                        entidadeId = aviso.entidadeId,
                        tipologiaId = indicadorId,
                        dataIndicador = dataIndicador,
                        publico = true,
                        descricao = string.Format("Número de avisos de eventos reportados pela entidade {0} para a data {1}.", aviso.entidadeNome, dataIndicador.ToShortDateString()),
                        valor = aviso.nrAvisos
                    };

                this.InsertOrUpdate(indicador);
            }


            this.Save();
        }

        /// <summary>
        /// Processa indicador do número de avisos identificados nos eventos submetidos pelas entidades seguradoras
        /// agregados por tipologia de aviso
        /// </summary>
        public void processaIndicadorNrAvisosTipo()
        {
            int mesIndicador = DateTime.Now.Month;
            DateTime dataIndicador = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            int indicadorId = context.ValoresSistema.Where(i => i.tipologia == "TIPO_INDICADOR" && i.valor == "NR_AVISOS_PERIODO_SEGURO_TIPO").Single().valorSistemaId;
   
            //---------------------------------------------------------------------------
            //NrAvisos - Numero de avisos de reporte por seguradora e tipo no mês corrente
            //---------------------------------------------------------------------------
            var queryAvisos =
                from en in
                    (from ent in context.Entidades
                     join v in context.ValoresSistema on "TIPO_AVISO" equals v.tipologia
                     where ent.ativo == true
                     select new { ent.entidadeId, ent.nome, tipologiaId = v.valorSistemaId, tipologia = v.descricao })
                join ind in
                    (from av in
                         (from apolice in context.Apolices
                          join aviso in context.Avisos on apolice.apoliceId equals aviso.apoliceId
                          where SqlFunctions.DateDiff("DAY", apolice.dataReporte, dataIndicador) == 0
                          select new { entidadeNome = apolice.entidade.nome, apolice.entidadeId, tipologiaId = aviso.tipologiaId, tipologia = aviso.tipologia.descricao, apolice.apoliceId }).Concat
                          (from apolice in context.ApolicesHistorico
                           join aviso in context.Avisos on apolice.apoliceId equals aviso.apoliceId
                           where SqlFunctions.DateDiff("DAY", apolice.dataReporte, dataIndicador) == 0
                           select new { entidadeNome = apolice.entidade.nome, apolice.entidadeId, tipologiaId = aviso.tipologiaId, tipologia = aviso.tipologia.descricao, apolice.apoliceId })
                     group new { av } by new { av.entidadeNome, av.entidadeId, av.tipologiaId, av.tipologia } into g
                     select new { g.Key.entidadeNome, entidadeId = g.Key.entidadeId.Value, g.Key.tipologiaId, g.Key.tipologia, nrAvisos = g.Count() })
                 on new { en.entidadeId, en.tipologiaId } equals new { ind.entidadeId, ind.tipologiaId} into res
                from x in res.DefaultIfEmpty()
                select new { entidadeNome = en.nome, en.entidadeId, en.tipologia, nrAvisos = (x.entidadeId == null ? 0 : x.nrAvisos) };


            string avisoTipologia;
            foreach (var aviso in queryAvisos.ToList())
            {
                avisoTipologia = aviso.tipologia.ToString();
                Indicador indicador;
                IQueryable<Indicador> indicadores = context.Indicadores.Where(i => i.entidadeId == aviso.entidadeId && i.tipologiaId == indicadorId && i.subTipo == avisoTipologia && i.dataIndicador == dataIndicador);

                if (indicadores != null && indicadores.Count() != 0)
                {
                    indicador = indicadores.Single();
                    indicador.valor = aviso.nrAvisos;
                }
                else
                    indicador = new Indicador
                    {
                        entidadeId = aviso.entidadeId,
                        tipologiaId = indicadorId,
                        subTipo = aviso.tipologia.ToString(),
                        dataIndicador = dataIndicador,
                        publico = true,
                        descricao = string.Format("Número de avisos de eventos do tipo {0}, reportados pela entidade {1} para a data {2}.", aviso.tipologia, aviso.entidadeNome, dataIndicador.ToShortDateString()),
                        valor = aviso.nrAvisos
                    };

                this.InsertOrUpdate(indicador);
            }


            this.Save();
        }

        /// <summary>
        /// Processa o indicador do número de eventos de criação/alteração/anulação efectuados no sistema
        /// </summary>
        public void processaIndicadorNrEventosOperacao()
        {
            int mesIndicador = DateTime.Now.Month;
            DateTime dataIndicador = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            int indicadorId = context.ValoresSistema.Where(i => i.tipologia == "TIPO_INDICADOR" && i.valor == "NR_EVENTOS_OPERACAO").Single().valorSistemaId;
   
            //---------------------------------------------------------------------------
            //NrCriações - Numero de eventos de criação/alteração/anulação processados por seguradora e tipo no mês corrente
            //---------------------------------------------------------------------------
            var queryEventos =
                from en in
                    (from ent in context.Entidades
                     join v in context.ValoresSistema on "OPERACAO_EVENTO" equals v.tipologia
                     where ent.ativo == true
                     select new { ent.entidadeId, ent.nome, codigoOperacao = v.valor })
                join ind in
                (from evento in context.EventosHistorico
                 where SqlFunctions.DateDiff("DAY", evento.dataReporte, dataIndicador) == 0
                 group new { evento } by new { evento.entidade.nome, evento.entidadeId, codigoOperacao=evento.codigoOperacao.valor } into g
                 select new { entidadeNome = g.Key.nome, entidadeId=g.Key.entidadeId.Value, g.Key.codigoOperacao, nrEventos = g.Count() })
                  on new { en.entidadeId, codigoOperacao = en.codigoOperacao } equals new { ind.entidadeId, ind.codigoOperacao } into res
                from x in res.DefaultIfEmpty()
                select new { entidadeNome = en.nome, en.entidadeId, codigoOperacao = en.codigoOperacao, nrEventos = (x.entidadeId == null ? 0 : x.nrEventos) };


            string eventoCodOperacao;
            foreach (var evento in queryEventos.ToList())
            {
                eventoCodOperacao = evento.codigoOperacao;
                Indicador indicador;
                IQueryable<Indicador> indicadores = context.Indicadores.Where(i => i.entidadeId == evento.entidadeId && i.tipologiaId == indicadorId && i.subTipo == eventoCodOperacao && i.dataIndicador == dataIndicador);

                if (indicadores != null && indicadores.Count() != 0)
                {
                    indicador = indicadores.Single();
                    indicador.valor = evento.nrEventos;
                }
                else
                    indicador = new Indicador
                    {
                        entidadeId = evento.entidadeId,
                        tipologiaId = indicadorId,
                        subTipo = evento.codigoOperacao.ToString(),
                        dataIndicador = dataIndicador,
                        publico = true,
                        descricao = string.Format("Número de eventos do tipo '{0}', processado para a entidade {1} para a data {2}.", evento.codigoOperacao, evento.entidadeNome, dataIndicador.ToShortDateString()),
                        valor = evento.nrEventos
                    };

                this.InsertOrUpdate(indicador);
            }

            this.Save();
        }

        

        /// <summary>
        /// Processa o indicador do número matriculas reportadas fora do SLA
        /// </summary>
        public void processaIndicadorNrMatriculasForaSLA()
        {
            int mesIndicador = DateTime.Now.Month;
            DateTime dataIndicador = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            int indicadorId = context.ValoresSistema.Where(i => i.tipologia == "TIPO_INDICADOR" && i.valor == "NR_OPERACOES_FORA_SLA").Single().valorSistemaId;
   
            //---------------------------------------------------------------------------
            //NrMatriculas - Numero de matriculas (criação/anulação) reportadas fora do SLA
            //---------------------------------------------------------------------------
            //var queryEventos =
            //        from en in (from ent in context.Entidades
            //            join v in context.ValoresSistema on  "OPERACAO_EVENTO" equals v.tipologia
            //            where ent.ativo == true
            //            select new {ent.entidadeId, ent.nome, codigoOperacao = v.valor})
            //        join ind in
            //        (from evento in context.EventosHistorico
            //         join apolice in context.Apolices on evento.eventoHistoricoId equals apolice.eventoHistoricoId
            //         where SqlFunctions.DateDiff("DAY", evento.dataReporte, dataIndicador) == 0 && (apolice.SLAdays > 0 || apolice.SLAhours > new TimeSpan(0))
            //         group new { apolice, evento } by new { entidadeNome = apolice.entidade.nome, apolice.entidadeId, codigoOperacao = evento.codigoOperacao.valor } into g
            //         select new { g.Key.entidadeNome, entidadeId = g.Key.entidadeId.Value, g.Key.codigoOperacao, nrMatriculas = g.Count() })
            //        on new { en.entidadeId, codigoOperacao = en.codigoOperacao } equals new { ind.entidadeId, ind.codigoOperacao } into res
            //        from x in res.DefaultIfEmpty()
            //        select new { entidadeNome = en.nome, en.entidadeId, en.codigoOperacao, nrMatriculas = (x.entidadeId == null ? 0 : x.nrMatriculas) };


            var queryEventos =
                from en in (from ent in context.Entidades
                        join v in context.ValoresSistema on  "OPERACAO_EVENTO" equals v.tipologia
                        where ent.ativo == true
                        select new {ent.entidadeId, ent.nome, codigoOperacao = v.valor})
                    join ind in
                    (   
                        from ev in
                             (from evento in context.EventosHistorico
                             join apolice in context.Apolices on evento.eventoHistoricoId equals apolice.eventoHistoricoId
                             where SqlFunctions.DateDiff("DAY", evento.dataReporte, dataIndicador) == 0 && (apolice.SLAdays > 0 || apolice.SLAhours > new TimeSpan(0))
                             select new { entidadeNome = apolice.entidade.nome, entidadeId = apolice.entidadeId.Value, codigoOperacao = evento.codigoOperacao.valor } 
                             ).Concat
                             (from evento in context.EventosHistorico
                             join apolice in context.ApolicesHistorico on evento.eventoHistoricoId equals apolice.eventoHistoricoId
                             where SqlFunctions.DateDiff("DAY", evento.dataReporte, dataIndicador) == 0 && (apolice.SLAdays > 0 || apolice.SLAhours > new TimeSpan(0))
                             select new { entidadeNome = apolice.entidade.nome, entidadeId = apolice.entidadeId.Value, codigoOperacao = evento.codigoOperacao.valor })
                        group new { ev } by new { ev.entidadeNome, ev.entidadeId, ev.codigoOperacao} into g
                        select new { g.Key.entidadeNome, entidadeId = g.Key.entidadeId, g.Key.codigoOperacao, nrMatriculas = g.Count() }
                    ) on new { en.entidadeId, codigoOperacao = en.codigoOperacao } equals new { ind.entidadeId, ind.codigoOperacao } into res
                    from x in res.DefaultIfEmpty()
                    select new { entidadeNome = en.nome, en.entidadeId, en.codigoOperacao, nrMatriculas = (x.entidadeId == null ? 0 : x.nrMatriculas) }; 

            string eventoCodOperacao;
            foreach (var evento in queryEventos.ToList())
            {
                eventoCodOperacao = evento.codigoOperacao.ToString();
                Indicador indicador;
                IQueryable<Indicador> indicadores = context.Indicadores.Where(i => i.entidadeId == evento.entidadeId && i.tipologiaId == indicadorId && i.subTipo == eventoCodOperacao && i.dataIndicador == dataIndicador);


                if (indicadores != null && indicadores.Count() != 0)
                {
                    indicador = indicadores.Single();
                    indicador.valor = evento.nrMatriculas;
                }
                else
                    indicador = new Indicador
                    {
                        entidadeId = evento.entidadeId,
                        tipologiaId = indicadorId,
                        subTipo = evento.codigoOperacao.ToString(),
                        dataIndicador = dataIndicador,
                        publico = true,
                        descricao = string.Format("Número de eventos do tipo '{0}', processado fora do SLA para a entidade {1} para a data {2}.", evento.codigoOperacao, evento.entidadeNome, dataIndicador.ToShortDateString()),
                        valor = evento.nrMatriculas
                    };

                this.InsertOrUpdate(indicador);
            }


            this.Save();
        }

        /// <summary>
        /// Processa o indicador do número matriculas reportadas dentro do SLA
        /// </summary>
        public void processaIndicadorNrMatriculasDentroSLA()
        {
            int mesIndicador = DateTime.Now.Month;
            DateTime dataIndicador = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            int indicadorId = context.ValoresSistema.Where(i => i.tipologia == "TIPO_INDICADOR" && i.valor == "NR_OPERACOES_DENTRO_SLA").Single().valorSistemaId;
   
            //---------------------------------------------------------------------------
            //NrMatriculas - Numero de matriculas (criação/anulação) reportadas dentro do SLA
            //---------------------------------------------------------------------------
            //var queryEventos =
            //    from en in
            //        (from ent in context.Entidades
            //         join v in context.ValoresSistema on "OPERACAO_EVENTO" equals v.tipologia
            //         where ent.ativo == true
            //         select new { ent.entidadeId, ent.nome, codigoOperacao = v.valor })
            //    join ind in
            //        (from evento in context.EventosHistorico
            //         join apolice in context.Apolices on evento.eventoHistoricoId equals apolice.eventoHistoricoId
            //             where SqlFunctions.DateDiff("DAY", evento.dataReporte, dataIndicador) == 0 && apolice.SLAdays == 0 && apolice.SLAhours == new TimeSpan(0)
            //             group new { apolice, evento } by new { entidadeNome = apolice.entidade.nome, apolice.entidadeId, codigoOperacao = evento.codigoOperacao.valor } into g
            //         select new { g.Key.entidadeNome, entidadeId = g.Key.entidadeId.Value, g.Key.codigoOperacao, nrMatriculas = g.Count() })
            //    on new { en.entidadeId, codigoOperacao = en.codigoOperacao } equals new { ind.entidadeId, ind.codigoOperacao } into res
            //    from x in res.DefaultIfEmpty()
            //    select new { entidadeNome = en.nome, en.entidadeId, en.codigoOperacao, nrMatriculas = (x.entidadeId == null ? 0 : x.nrMatriculas) };

            var queryEventos =
                from en in
                    (from ent in context.Entidades
                     join v in context.ValoresSistema on "OPERACAO_EVENTO" equals v.tipologia
                     where ent.ativo == true
                     select new { ent.entidadeId, ent.nome, codigoOperacao = v.valor })
                join ind in
                    (
                        from ev in
                            (from evento in context.EventosHistorico
                             join apolice in context.Apolices on evento.eventoHistoricoId equals apolice.eventoHistoricoId
                             where SqlFunctions.DateDiff("DAY", evento.dataReporte, dataIndicador) == 0 && (apolice.SLAdays == 0 && apolice.SLAhours == new TimeSpan(0))
                             select new { entidadeNome = apolice.entidade.nome, entidadeId = apolice.entidadeId.Value, codigoOperacao = evento.codigoOperacao.valor }
                            ).Concat
                            (from evento in context.EventosHistorico
                             join apolice in context.ApolicesHistorico on evento.eventoHistoricoId equals apolice.eventoHistoricoId
                             where SqlFunctions.DateDiff("DAY", evento.dataReporte, dataIndicador) == 0 && (apolice.SLAdays == 0 && apolice.SLAhours == new TimeSpan(0))
                             select new { entidadeNome = apolice.entidade.nome, entidadeId = apolice.entidadeId.Value, codigoOperacao = evento.codigoOperacao.valor })
                        group new { ev } by new { ev.entidadeNome, ev.entidadeId, ev.codigoOperacao } into g
                        select new { g.Key.entidadeNome, entidadeId = g.Key.entidadeId, g.Key.codigoOperacao, nrMatriculas = g.Count() }
                    ) on new { en.entidadeId, codigoOperacao = en.codigoOperacao } equals new { ind.entidadeId, ind.codigoOperacao } into res
                from x in res.DefaultIfEmpty()
                select new { entidadeNome = en.nome, en.entidadeId, en.codigoOperacao, nrMatriculas = (x.entidadeId == null ? 0 : x.nrMatriculas) }; 

            
            string eventoCodOperacao;
            foreach (var evento in queryEventos.ToList())
            {
                eventoCodOperacao = evento.codigoOperacao.ToString();
                Indicador indicador;
                IQueryable<Indicador> indicadores = context.Indicadores.Where(i => i.entidadeId == evento.entidadeId && i.tipologiaId == indicadorId && i.subTipo == eventoCodOperacao && i.dataIndicador == dataIndicador);


                if (indicadores != null && indicadores.Count() != 0)
                {
                    indicador = indicadores.Single();
                    indicador.valor = evento.nrMatriculas;
                }
                else
                    indicador = new Indicador
                    {
                        entidadeId = evento.entidadeId,
                        tipologiaId = indicadorId,
                        subTipo = evento.codigoOperacao.ToString(),
                        dataIndicador = dataIndicador,
                        publico = true,
                        descricao = string.Format("Número de eventos do tipo '{0}', processado de acordo com o SLA para a entidade {1} para a data {2}.", evento.codigoOperacao, evento.entidadeNome, dataIndicador.ToShortDateString()),
                        valor = evento.nrMatriculas
                    };

                this.InsertOrUpdate(indicador);
            }


            this.Save();
        }


        /// <summary>
        /// Processa o indicador do número matriculas reportadas fora do SLA
        /// </summary>
        public void processaIndicadorMediaDesviosSLA()
        {
            int mesIndicador = DateTime.Now.Month;
            DateTime dataIndicador = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            int indicadorId = context.ValoresSistema.Where(i => i.tipologia == "TIPO_INDICADOR" && i.valor == "TEMPO_MEDIO_DESVIOS_SLA").Single().valorSistemaId;
   
            //---------------------------------------------------------------------------
            //NrMatriculas - Numero de matriculas (criação/anulação) reportadas fora do SLA
            //---------------------------------------------------------------------------
            //var queryEventos =
            //    from en in
            //        (from ent in context.Entidades
            //         join v in context.ValoresSistema on "OPERACAO_EVENTO" equals v.tipologia
            //         where ent.ativo == true
            //         select new { ent.entidadeId, ent.nome, codigoOperacao = v.valor })
            //    join ind in
            //        (from apolice in context.Apolices
            //         join evento in context.EventosHistorico on apolice.eventoHistoricoId equals evento.eventoHistoricoId
            //        where SqlFunctions.DateDiff("DAY", evento.dataReporte, dataIndicador) == 0  && (apolice.SLAdays > 0 || apolice.SLAhours > new TimeSpan(0))
            //        group new { apolice, evento } by new {entidadeNome = apolice.entidade.nome, apolice.entidadeId, evento.codigoOperacao } into g
            //         select new { g.Key.entidadeNome, entidadeId = g.Key.entidadeId.Value, codigoOperacao = g.Key.codigoOperacao.valor, mediaDesvio = g.Average(i => i.apolice.SLAdays*24*60 + i.apolice.SLAhours.Ticks / 10000000 / 60) })
            //    on new { en.entidadeId, codigoOperacao = en.codigoOperacao } equals new { ind.entidadeId, ind.codigoOperacao } into res
            //    from x in res.DefaultIfEmpty()
            //    select new { entidadeNome = en.nome, en.entidadeId, en.codigoOperacao, mediaDesvio = (x.entidadeId == null ? 0 : x.mediaDesvio) };


            var queryEventos =
                from en in
                    (from ent in context.Entidades
                     join v in context.ValoresSistema on "OPERACAO_EVENTO" equals v.tipologia
                     where ent.ativo == true
                     select new { ent.entidadeId, ent.nome, codigoOperacao = v.valor })
                join ind in
                    //from ev in
                    //    (from evento in context.EventosHistorico
                    //    join apolice in context.Apolices on evento.eventoHistoricoId equals apolice.eventoHistoricoId
                    //    where SqlFunctions.DateDiff("DAY", evento.dataReporte, dataIndicador) == 0 && (apolice.SLAdays > 0 || apolice.SLAhours > new TimeSpan(0))
                    //    select new { entidadeNome = apolice.entidade.nome, entidadeId = apolice.entidadeId.Value, codigoOperacao = evento.codigoOperacao.valor } 
                    //    ).Concat
                    //    (from evento in context.EventosHistorico
                    //    join apolice in context.ApolicesHistorico on evento.eventoHistoricoId equals apolice.eventoHistoricoId
                    //    where SqlFunctions.DateDiff("DAY", evento.dataReporte, dataIndicador) == 0 && (apolice.SLAdays > 0 || apolice.SLAhours > new TimeSpan(0))
                    //    select new { entidadeNome = apolice.entidade.nome, entidadeId = apolice.entidadeId.Value, codigoOperacao = evento.codigoOperacao.valor })
                
                    (from apolice in context.Apolices
                     join evento in context.EventosHistorico on apolice.eventoHistoricoId equals evento.eventoHistoricoId
                     where SqlFunctions.DateDiff("DAY", evento.dataReporte, dataIndicador) == 0 && (apolice.SLAdays > 0 || apolice.SLAhours > new TimeSpan(0))
                     select new { apolice.entidade.nome, entidadeId = apolice.entidadeId.Value, codigoOperacao = evento.codigoOperacao.valor, apolice.SLAdays, apolice.SLAhours }
                     ).Concat
                     (from apolice in context.ApolicesHistorico
                     join evento in context.EventosHistorico on apolice.eventoHistoricoId equals evento.eventoHistoricoId
                     where SqlFunctions.DateDiff("DAY", evento.dataReporte, dataIndicador) == 0 && (apolice.SLAdays > 0 || apolice.SLAhours > new TimeSpan(0))
                     select new { apolice.entidade.nome, entidadeId = apolice.entidadeId.Value, codigoOperacao = evento.codigoOperacao.valor, apolice.SLAdays, apolice.SLAhours })
                
                on new { en.entidadeId, codigoOperacao = en.codigoOperacao } equals new { ind.entidadeId, ind.codigoOperacao } into res
                from x in res.DefaultIfEmpty()
                select new { entidadeNome = en.nome, en.entidadeId, en.codigoOperacao, desvioDias = (x.entidadeId == null ? 0 : x.SLAdays), desvioHoras = (x.entidadeId == null ? new TimeSpan(0) : x.SLAhours)};


            var queryEventos2 = queryEventos.ToList().GroupBy(i => new { i.entidadeNome, i.entidadeId, i.codigoOperacao }).Select(x => new
                                                                                                                                            {
                                                                                                                                                MediaDias = x.Average(p => p.desvioDias),
                                                                                                                                                MediaHoras = x.Average(p => p.desvioHoras.TotalMinutes),
                                                                                                                                                entidadeId = x.Key.entidadeId,
                                                                                                                                                entidadeNome = x.Key.entidadeNome,
                                                                                                                                                codigoOperacao = x.Key.codigoOperacao
                                                                                                                                            });
               

            string eventoCodOperacao;
            foreach (var evento in queryEventos2.ToList())
            {
                eventoCodOperacao = evento.codigoOperacao.ToString();
                Indicador indicador;
                IQueryable<Indicador> indicadores = context.Indicadores.Where(i => i.entidadeId == evento.entidadeId && i.tipologiaId == indicadorId && i.subTipo == eventoCodOperacao && i.dataIndicador == dataIndicador);


                if (indicadores != null && indicadores.Count() != 0)
                {
                    indicador = indicadores.Single();
                    indicador.valor = evento.MediaDias*24*60+evento.MediaHoras;
                }
                else
                    indicador = new Indicador
                    {
                        entidadeId = evento.entidadeId,
                        tipologiaId = indicadorId,
                        subTipo = evento.codigoOperacao.ToString(),
                        dataIndicador = dataIndicador,
                        publico = true,
                        descricao = string.Format("Tempo médios de desvio de SLA, para sa operações e '{0}' da entidade {1} para a data {2}.", evento.codigoOperacao, evento.entidadeNome, dataIndicador.ToShortDateString()),
                        valor = evento.MediaDias*24*60+evento.MediaHoras
                    };

                this.InsertOrUpdate(indicador);
            }


            this.Save();
        }
    }
}

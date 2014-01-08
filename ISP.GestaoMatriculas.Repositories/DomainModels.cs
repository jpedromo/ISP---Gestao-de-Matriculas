using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using ISP.GestaoMatriculas.Model;
<<<<<<< HEAD
=======
using ISP.GestaoMatriculas.Model.Indicadores;
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
using System.Data.Entity.Core.Objects;
using System.Data.Common;

namespace ISP.GestaoMatriculas.Model
{
    public class DomainModels : UsersContext
    {

        public DbSet<Apolice> Apolices { get; set; }
        public DbSet<ApoliceHistorico> ApolicesHistorico { get; set; }
<<<<<<< HEAD
        public DbSet<Aviso> Avisos { get; set; }
=======
        public DbSet<AvisoApolice> AvisosApolice { get; set; }
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
        
        public DbSet<ActionLog> ActionLogs { get; set; }

        public DbSet<Veiculo> Veiculos { get; set; }
        public DbSet<Pessoa> Segurados { get; set; }

        public DbSet<Categoria> Categorias { get; set; }

        public DbSet<Concelho> Concelhos { get; set; }

        public DbSet<Entidade> Entidades { get; set; }

        public DbSet<Notificacao> Notificacoes { get; set; }

        public DbSet<Ficheiro> Ficheiros { get; set; }
        public DbSet<ErroFicheiro> ErrosFicheiro { get; set; }

        public DbSet<EventoStagging> EventosStagging { get; set; }
        public DbSet<EventoHistorico> EventosHistorico { get; set; }
        public DbSet<ErroEventoStagging> ErrosEventoStagging { get; set; }

<<<<<<< HEAD
        public DbSet<Indicador> Indicadores { get; set; }

        public DbSet<ApoliceIsento> ApolicesIsentos { get; set; }
        public DbSet<FicheiroIsentos> FicheirosIsentos { get; set; }

        public DbSet<ValorSistema> ValoresSistema { get; set; }
        
=======
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges(); // Important!

            ObjectContext ctx = ((IObjectContextAdapter)this).ObjectContext;

            List<ObjectStateEntry> objectStateEntryList =
                ctx.ObjectStateManager.GetObjectStateEntries(EntityState.Added
                                                           | EntityState.Modified
                                                           | EntityState.Deleted)
                .ToList();

            foreach (ObjectStateEntry entry in objectStateEntryList)
            {
                if (!entry.IsRelationship)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            // write log...
                            break;
                        case EntityState.Deleted:
                            // write log...
                            break;
                        case EntityState.Modified:
                            {
                                foreach (string propertyName in
                                             entry.GetModifiedProperties())
                                {
                                    DbDataRecord original = entry.OriginalValues;
                                    string oldValue = original.GetValue(
                                        original.GetOrdinal(propertyName))
                                        .ToString();

                                    CurrentValueRecord current = entry.CurrentValues;
                                    string newValue = current.GetValue(
                                        current.GetOrdinal(propertyName))
                                        .ToString();

                                    if (oldValue != newValue) // probably not necessary
                                    {
                                        //Log.WriteAudit(
                                        //    "Entry: {0} Original :{1} New: {2}",
                                        //    entry.Entity.GetType().Name,
                                        //    oldValue, newValue);
                                    }
                                }
                                break;
                            }
                    }
                }
            }
            return base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
<<<<<<< HEAD
            modelBuilder.Entity<Apolice>().ToTable("MAT_PERIODO_COBERTURA");
            modelBuilder.Entity<ApoliceHistorico>().ToTable("MAT_PERIODO_COBERTURA_HISTORICO");
            modelBuilder.Entity<Aviso>().ToTable("MAT_AVISO");

            modelBuilder.Entity<Veiculo>().ToTable("MAT_VEICULO");
            modelBuilder.Entity<Pessoa>().ToTable("MAT_TOMADOR_SEGURO");

            modelBuilder.Entity<Categoria>().ToTable("MAT_CATEGORIA");
            modelBuilder.Entity<Concelho>().ToTable("MAT_CONCELHO");
            modelBuilder.Entity<Entidade>().ToTable("MAT_ENTIDADE");
            modelBuilder.Entity<Notificacao>().ToTable("MAT_NOTIFICACAO");
            
            modelBuilder.Entity<Ficheiro>().ToTable("MAT_FICHEIRO");
            modelBuilder.Entity<ErroFicheiro>().ToTable("MAT_ERRO_FICHEIRO");

            modelBuilder.Entity<EventoStagging>().ToTable("MAT_EVENTO_STAGGING");
            modelBuilder.Entity<EventoHistorico>().ToTable("MAT_EVENTO_HISTORICO");
            modelBuilder.Entity<ErroEventoStagging>().ToTable("MAT_ERRO_EVENTO_STAGGING");

            modelBuilder.Entity<Indicador>().ToTable("MAT_INDICADOR");

            modelBuilder.Entity<ApoliceIsento>().ToTable("MAT_PERIODO_COBERTURA_ISENTO");
            modelBuilder.Entity<FicheiroIsentos>().ToTable("MAT_FICHEIRO_ISENTO");
            modelBuilder.Entity<ValorSistema>().ToTable("MAT_DESC_VALOR_SISTEMA");

            //TODO: mudar para required em vez de optional. Refazer controlador de Apolices para conter a mudança.
            modelBuilder.Entity<Entidade>().HasMany<Apolice>(e => e.apolices).WithOptional(a => a.entidade);
            modelBuilder.Entity<Entidade>().HasMany<ApoliceHistorico>(e => e.apolicesHistorico).WithOptional(a => a.entidade);

            modelBuilder.Entity<Entidade>().HasMany<Notificacao>(e => e.notificacoes).WithOptional(n => n.entidade);
            modelBuilder.Entity<Entidade>().HasMany<Indicador>(e => e.indicadores).WithOptional(i => i.entidade);
            modelBuilder.Entity<Entidade>().HasMany<EventoStagging>(e => e.eventosStagging).WithOptional(i => i.entidade);
            
            modelBuilder.Entity<Ficheiro>().HasMany<ErroFicheiro>(e => e.errosFicheiro).WithOptional(a => a.ficheiro);
            modelBuilder.Entity<Ficheiro>().HasMany<EventoStagging>(e => e.eventosStagging).WithOptional(a => a.ficheiro);

            modelBuilder.Entity<Apolice>().HasMany<Aviso>(e => e.avisosApolice).WithOptional(a => a.apolice);
            modelBuilder.Entity<ApoliceHistorico>().HasMany<Aviso>(e => e.avisosApoliceHistorico).WithOptional(a => a.apoliceHistorico);
            
            modelBuilder.Entity<EventoStagging>().HasMany<Aviso>(e => e.avisosEventoStagging).WithOptional(a => a.eventoStagging);
            modelBuilder.Entity<EventoStagging>().HasMany<ErroEventoStagging>(e => e.errosEventoStagging).WithOptional(a => a.eventoStagging);

            modelBuilder.Entity<FicheiroIsentos>().HasMany<ApoliceIsento>(e => e.apolicesIsentos).WithOptional(a => a.ficheiroIsentos);
=======
            modelBuilder.Entity<Apolice>().ToTable("Apolices");
            modelBuilder.Entity<AvisoApolice>().ToTable("AvisosApolice");

            modelBuilder.Entity<Veiculo>().ToTable("Veiculos");
            modelBuilder.Entity<Pessoa>().ToTable("Segurados");

            modelBuilder.Entity<Categoria>().ToTable("Categorias");
            modelBuilder.Entity<Concelho>().ToTable("Concelhos");
            modelBuilder.Entity<Entidade>().ToTable("Entidades");
            modelBuilder.Entity<Notificacao>().ToTable("Notificacoes");
            
            modelBuilder.Entity<Ficheiro>().ToTable("Ficheiros");
            modelBuilder.Entity<ErroFicheiro>().ToTable("ErrosFicheiro");

            modelBuilder.Entity<EventoStagging>().ToTable("EventosStagging");
            modelBuilder.Entity<EventoHistorico>().ToTable("EventosHistorico");
            modelBuilder.Entity<ErroEventoStagging>().ToTable("ErrosEventoStagging");

            modelBuilder.Entity<Indicador>().ToTable("Indicadores");

            modelBuilder.Entity<ApoliceHistorico>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable("ApolicesHistorico");
            });
 

            //TODO: mudar para required em vez de optional. Refazer controlador de Apolices para conter a mudança.
            //modelBuilder.Entity<Entidade>().HasMany<Apolice>(e => e.apolices).WithOptional(a => a.entidade);
            //modelBuilder.Entity<Entidade>().HasMany<ApoliceHistorico>(e => e.apolicesHistorico).WithOptional(a => a.entidade);
            
            //modelBuilder.Entity<Entidade>().HasMany<Notificacao>(e => e.notificacoes).WithOptional(n => n.entidade);
            //modelBuilder.Entity<Entidade>().HasMany<Indicador>(e => e.indicadores).WithOptional(i => i.entidade);
            //modelBuilder.Entity<Entidade>().HasMany<EventoStagging>(e => e.eventosStagging).WithRequired(i => i.entidade);

            //modelBuilder.Entity<Ficheiro>().HasMany<ErroFicheiro>(e => e.errosFicheiro).WithRequired(a => a.ficheiro);
            //modelBuilder.Entity<Ficheiro>().HasMany<EventoStagging>(e => e.eventosStagging).WithOptional(a => a.ficheiro);
            //modelBuilder.Entity<Apolice>().HasMany<AvisoApolice>(e => e.avisosApolice).WithRequired(a => a.apolice);
            //modelBuilder.Entity<EventoStagging>().HasMany<ErroEventoStagging>(e => e.errosEventoStagging).WithRequired(a => a.eventoStagging);
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c

            base.OnModelCreating(modelBuilder);

            
        }

    }
}
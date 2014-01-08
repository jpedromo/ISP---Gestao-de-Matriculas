namespace ISP.GestaoMatriculas.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActionLogs",
                c => new
                    {
                        ActionLogId = c.Int(nullable: false, identity: true),
                        Controller = c.String(),
                        Action = c.String(),
                        Message = c.String(),
                        IP = c.String(),
                        DateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.ActionLogId);
            
            CreateTable(
                "dbo.MAT_APOLICE",
                c => new
                    {
                        ApoliceId_PK = c.Int(nullable: false, identity: true),
                        EntidadeId_FK = c.Int(),
                        TomadorId_FK = c.Int(nullable: false),
                        VeiculoId_FK = c.Int(nullable: false),
                        ConcelhoCirculacaoId_FK = c.Int(nullable: false),
                        ValNumeroApolice = c.String(),
                        ValNumeroCertificadoProvisorio = c.String(),
                        DtInicio = c.DateTime(nullable: false),
                        DtFim = c.DateTime(nullable: false),
                        DtFimPlaneada = c.DateTime(nullable: false),
                        DtReporte = c.DateTime(nullable: false),
                        ValSLADias = c.Int(nullable: false),
                        ValSLAHoras = c.Time(nullable: false, precision: 7),
                        FlgAvisos = c.Boolean(nullable: false),
                        EventoHistoricoId_FK = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ApoliceId_PK)
                .ForeignKey("dbo.MAT_ENTIDADE", t => t.EntidadeId_FK)
                .ForeignKey("dbo.MAT_CONCELHO", t => t.ConcelhoCirculacaoId_FK, cascadeDelete: true)
                .ForeignKey("dbo.MAT_EVENTO_HISTORICO", t => t.EventoHistoricoId_FK, cascadeDelete: true)
                .ForeignKey("dbo.MAT_PESSOA", t => t.TomadorId_FK, cascadeDelete: true)
                .ForeignKey("dbo.MAT_VEICULO", t => t.VeiculoId_FK, cascadeDelete: true)
                .Index(t => t.EntidadeId_FK)
                .Index(t => t.ConcelhoCirculacaoId_FK)
                .Index(t => t.EventoHistoricoId_FK)
                .Index(t => t.TomadorId_FK)
                .Index(t => t.VeiculoId_FK);
            
            CreateTable(
                "dbo.MAT_AVISO",
                c => new
                    {
                        AvisoId_PK = c.Int(nullable: false, identity: true),
                        ApoliceId_FK = c.Int(),
                        ApoliceHistoricoId_FK = c.Int(),
                        EventoStagging_FK = c.Int(),
                        EventoHistoricoId_FK = c.Int(),
                        CodTipologia = c.Int(nullable: false),
                        Descricao = c.String(),
                        Campo = c.String(),
                    })
                .PrimaryKey(t => t.AvisoId_PK)
                .ForeignKey("dbo.MAT_APOLICE_HISTORICO", t => t.ApoliceHistoricoId_FK)
                .ForeignKey("dbo.MAT_EVENTO_STAGGING", t => t.EventoStagging_FK)
                .ForeignKey("dbo.MAT_EVENTO_HISTORICO", t => t.EventoHistoricoId_FK)
                .ForeignKey("dbo.MAT_APOLICE", t => t.ApoliceId_FK)
                .Index(t => t.ApoliceHistoricoId_FK)
                .Index(t => t.EventoStagging_FK)
                .Index(t => t.EventoHistoricoId_FK)
                .Index(t => t.ApoliceId_FK);
            
            CreateTable(
                "dbo.MAT_APOLICE_HISTORICO",
                c => new
                    {
                        ApoliceId_PK = c.Int(nullable: false, identity: true),
                        EntidadeId_FK = c.Int(),
                        TomadorId_FK = c.Int(nullable: false),
                        VeiculoId_KF = c.Int(nullable: false),
                        ConcelhoCirculacaoId_FK = c.Int(nullable: false),
                        ValNumeroApolice = c.String(),
                        ValNumeroCertificadoProvisorio = c.String(),
                        DtInicio = c.DateTime(nullable: false),
                        DtFim = c.DateTime(nullable: false),
                        DtFimPlaneada = c.DateTime(nullable: false),
                        DtReporte = c.DateTime(nullable: false),
                        ValSLADias = c.Int(nullable: false),
                        ValSLAHoras = c.Time(nullable: false, precision: 7),
                        EventoHistoricoId_FK = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ApoliceId_PK)
                .ForeignKey("dbo.MAT_CONCELHO", t => t.ConcelhoCirculacaoId_FK, cascadeDelete: true)
                .ForeignKey("dbo.MAT_ENTIDADE", t => t.EntidadeId_FK)
                .ForeignKey("dbo.MAT_EVENTO_HISTORICO", t => t.EventoHistoricoId_FK, cascadeDelete: true)
                .ForeignKey("dbo.MAT_PESSOA", t => t.TomadorId_FK, cascadeDelete: true)
                .ForeignKey("dbo.MAT_VEICULO", t => t.VeiculoId_KF, cascadeDelete: true)
                .Index(t => t.ConcelhoCirculacaoId_FK)
                .Index(t => t.EntidadeId_FK)
                .Index(t => t.EventoHistoricoId_FK)
                .Index(t => t.TomadorId_FK)
                .Index(t => t.VeiculoId_KF);
            
            CreateTable(
                "dbo.MAT_CONCELHO",
                c => new
                    {
                        ConcelhoId_PK = c.Int(nullable: false, identity: true),
                        CodConcelho = c.String(),
                        NomeConcelho = c.String(),
                    })
                .PrimaryKey(t => t.ConcelhoId_PK);
            
            CreateTable(
                "dbo.MAT_ENTIDADE",
                c => new
                    {
                        EntidadeId_PK = c.Int(nullable: false, identity: true),
                        RoleId_FK = c.Int(nullable: false),
                        Nome = c.String(),
                        CodEntidade = c.String(),
                        NomeResponsavel = c.String(),
                        EmailResponsavel = c.String(),
                        TelefoneResponsavel = c.String(),
                        ValScope = c.Int(nullable: false),
                        FlgAtivo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.EntidadeId_PK)
                .ForeignKey("dbo.webpages_Roles", t => t.RoleId_FK, cascadeDelete: true)
                .Index(t => t.RoleId_FK);
            
            CreateTable(
                "dbo.MAT_EVENTO_STAGGING",
                c => new
                    {
                        EventoStaggingId_PK = c.Int(nullable: false, identity: true),
                        ValIdOcorrencia = c.String(),
                        CodigoOperacao = c.Int(),
                        EntidadeId_FK = c.Int(nullable: false),
                        ValNumeroApolice = c.String(),
                        ValNumeroCertificadoProvisorio = c.String(),
                        ValDtInicio = c.String(),
                        ValHoraInicio = c.String(),
                        ValDtFim = c.String(),
                        ValHoraFim = c.String(),
                        ValMatricula = c.String(),
                        ValMarcar = c.String(),
                        ValModelo = c.String(),
                        ValAnoConstrucao = c.String(),
                        ValCodCategoriaVeiculo = c.String(),
                        ValCodConcelhoCirculacao = c.String(),
                        ValNomeTomadorSeguro = c.String(),
                        ValMoradaTomadorSeguro = c.String(),
                        ValCodPostalTomadorSeguro = c.String(),
                        ValNrIdentificacaoTomadorSeguro = c.String(),
                        ValNIFTomadorSeguro = c.String(),
                        FlgDeleted = c.Boolean(nullable: false),
                        VAlDtFimPlaneada = c.DateTime(),
                        TotErrosCumulativos = c.Int(nullable: false),
                        TotAvisosCumulativos = c.Int(nullable: false),
                        DtReporte = c.DateTime(nullable: false),
                        DtUltimaAlteracao = c.DateTime(nullable: false),
                        DtCorrecaoErro = c.DateTime(),
                        CodEstadoEvento = c.Int(nullable: false),
                        FicheiroId_FK = c.Int(),
                    })
                .PrimaryKey(t => t.EventoStaggingId_PK)
                .ForeignKey("dbo.MAT_FICHEIRO", t => t.FicheiroId_FK)
                .ForeignKey("dbo.MAT_ENTIDADE", t => t.EntidadeId_FK, cascadeDelete: true)
                .Index(t => t.FicheiroId_FK)
                .Index(t => t.EntidadeId_FK);
            
            CreateTable(
                "dbo.MAT_ERRO_EVENTO_STAGGING",
                c => new
                    {
                        ErroEventoStaggingId_PK = c.Int(nullable: false, identity: true),
                        EventoStaggingId_FK = c.Int(nullable: false),
                        CodTipologia = c.Int(nullable: false),
                        Descricao = c.String(),
                        Campo = c.String(),
                    })
                .PrimaryKey(t => t.ErroEventoStaggingId_PK)
                .ForeignKey("dbo.MAT_EVENTO_STAGGING", t => t.EventoStaggingId_FK, cascadeDelete: true)
                .Index(t => t.EventoStaggingId_FK);
            
            CreateTable(
                "dbo.MAT_FICHEIRO",
                c => new
                    {
                        EventoStaggingId_PK = c.Int(nullable: false, identity: true),
                        EntidadeId_FK = c.Int(nullable: false),
                        NomeFicheiro = c.String(),
                        ValLocalizacao = c.String(),
                        CodEstado = c.Int(nullable: false),
                        TotRegistos = c.Int(nullable: false),
                        TotEventosErro = c.Int(nullable: false),
                        TotEventosAviso = c.Int(nullable: false),
                        DtUpload = c.DateTime(nullable: false),
                        DtAlteracao = c.DateTime(nullable: false),
                        NomeUser = c.String(),
                    })
                .PrimaryKey(t => t.EventoStaggingId_PK)
                .ForeignKey("dbo.MAT_ENTIDADE", t => t.EntidadeId_FK, cascadeDelete: true)
                .Index(t => t.EntidadeId_FK);
            
            CreateTable(
                "dbo.MAT_ERRO_FICHEIRO",
                c => new
                    {
                        ErroFicheiroId_PK = c.Int(nullable: false, identity: true),
                        FicheiroId_FK = c.Int(nullable: false),
                        Descricao = c.String(),
                        DtValidacao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ErroFicheiroId_PK)
                .ForeignKey("dbo.MAT_FICHEIRO", t => t.FicheiroId_FK, cascadeDelete: true)
                .Index(t => t.FicheiroId_FK);
            
            CreateTable(
                "dbo.MAT_INDICADOR",
                c => new
                    {
                        IndicadorId_PK = c.Int(nullable: false, identity: true),
                        EntidadeId_FK = c.Int(nullable: false),
                        CodTipo = c.Int(nullable: false),
                        CodSubTipo = c.String(),
                        FlgPublico = c.Boolean(nullable: false),
                        Descricao = c.String(),
                        DtIndicador = c.DateTime(nullable: false),
                        ValValor = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.IndicadorId_PK)
                .ForeignKey("dbo.MAT_ENTIDADE", t => t.EntidadeId_FK, cascadeDelete: true)
                .Index(t => t.EntidadeId_FK);
            
            CreateTable(
                "dbo.MAT_NOTIFICACAO",
                c => new
                    {
                        NotificacaoId_PK = c.Int(nullable: false, identity: true),
                        DtCriacao = c.DateTime(nullable: false),
                        FlgLida = c.Boolean(nullable: false),
                        FlgEmail = c.Boolean(nullable: false),
                        FlgEnviadaEmail = c.Boolean(nullable: false),
                        ValMensagem = c.String(),
                        EntidadeId_FK = c.Int(nullable: false),
                        CodTipologia = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.NotificacaoId_PK)
                .ForeignKey("dbo.MAT_ENTIDADE", t => t.EntidadeId_FK, cascadeDelete: true)
                .Index(t => t.EntidadeId_FK);
            
            CreateTable(
                "dbo.webpages_Roles",
                c => new
                    {
                        RoleId = c.Int(nullable: false, identity: true),
                        RoleName = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.RoleId);
            
            CreateTable(
                "dbo.webpages_Membership",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        CreateDate = c.DateTime(),
                        ConfirmationToken = c.String(maxLength: 128),
                        IsConfirmed = c.Boolean(),
                        LastPasswordFailureDate = c.DateTime(),
                        PasswordFailuresSinceLastSuccess = c.Int(nullable: false),
                        Password = c.String(nullable: false, maxLength: 128),
                        PasswordChangedDate = c.DateTime(),
                        PasswordSalt = c.String(nullable: false, maxLength: 128),
                        PasswordVerificationToken = c.String(maxLength: 128),
                        PasswordVerificationTokenExpirationDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.webpages_OAuthMembership",
                c => new
                    {
                        Provider = c.String(nullable: false, maxLength: 30),
                        ProviderUserId = c.String(nullable: false, maxLength: 100),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Provider, t.ProviderUserId })
                .ForeignKey("dbo.webpages_Membership", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.MAT_EVENTO_HISTORICO",
                c => new
                    {
                        EventoHistoricoId_PK = c.Int(nullable: false, identity: true),
                        ValIdOcorrencia = c.String(),
                        CodOperacao = c.Int(nullable: false),
                        DtReporte = c.DateTime(nullable: false),
                        EntidadeId_FK = c.Int(),
                        NomeUser = c.String(),
                    })
                .PrimaryKey(t => t.EventoHistoricoId_PK)
                .ForeignKey("dbo.MAT_ENTIDADE", t => t.EntidadeId_FK)
                .Index(t => t.EntidadeId_FK);
            
            CreateTable(
                "dbo.MAT_PESSOA",
                c => new
                    {
                        PessoaId_PK = c.Int(nullable: false, identity: true),
                        Nome = c.String(),
                        NrIdentificacao = c.String(),
                        NIF = c.String(),
                        NomMorada = c.String(),
                        CodPostal = c.String(),
                    })
                .PrimaryKey(t => t.PessoaId_PK);
            
            CreateTable(
                "dbo.MAT_VEICULO",
                c => new
                    {
                        VeiculoId_PK = c.Int(nullable: false, identity: true),
                        CategoriaId_FK = c.Int(nullable: false),
                        ValAnoConstrucao = c.String(),
                        ValNumeroMatricula = c.String(),
                        ValMarcaVeiculo = c.String(),
                        ValModeloVeiculo = c.String(),
                    })
                .PrimaryKey(t => t.VeiculoId_PK)
                .ForeignKey("dbo.MAT_CATEGORIA", t => t.CategoriaId_FK, cascadeDelete: true)
                .Index(t => t.CategoriaId_FK);
            
            CreateTable(
                "dbo.MAT_CATEGORIA",
                c => new
                    {
                        CategoriaId_PK = c.Int(nullable: false, identity: true),
                        CodCategoriaVeiculo = c.String(),
                        Nome = c.String(),
                    })
                .PrimaryKey(t => t.CategoriaId_PK);
            
            CreateTable(
                "dbo.MAT_USER_PROFILE",
                c => new
                    {
                        UserID_PK = c.Int(nullable: false, identity: true),
                        EntidadeId_FK = c.Int(nullable: false),
                        UserName = c.String(),
                        Nome = c.String(),
                        ValEmail = c.String(),
                        ValTelefone = c.String(),
                        FlgAtivo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserID_PK)
                .ForeignKey("dbo.MAT_ENTIDADE", t => t.EntidadeId_FK, cascadeDelete: true)
                .Index(t => t.EntidadeId_FK);
            
            CreateTable(
                "dbo.webpages_UsersInRoles",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.webpages_Membership", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.webpages_Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MAT_USER_PROFILE", "EntidadeId_FK", "dbo.MAT_ENTIDADE");
            DropForeignKey("dbo.MAT_APOLICE", "VeiculoId_FK", "dbo.MAT_VEICULO");
            DropForeignKey("dbo.MAT_APOLICE", "TomadorId_FK", "dbo.MAT_PESSOA");
            DropForeignKey("dbo.MAT_APOLICE", "EventoHistoricoId_FK", "dbo.MAT_EVENTO_HISTORICO");
            DropForeignKey("dbo.MAT_APOLICE", "ConcelhoCirculacaoId_FK", "dbo.MAT_CONCELHO");
            DropForeignKey("dbo.MAT_AVISO", "ApoliceId_FK", "dbo.MAT_APOLICE");
            DropForeignKey("dbo.MAT_AVISO", "EventoHistoricoId_FK", "dbo.MAT_EVENTO_HISTORICO");
            DropForeignKey("dbo.MAT_APOLICE_HISTORICO", "VeiculoId_KF", "dbo.MAT_VEICULO");
            DropForeignKey("dbo.MAT_VEICULO", "CategoriaId_FK", "dbo.MAT_CATEGORIA");
            DropForeignKey("dbo.MAT_APOLICE_HISTORICO", "TomadorId_FK", "dbo.MAT_PESSOA");
            DropForeignKey("dbo.MAT_APOLICE_HISTORICO", "EventoHistoricoId_FK", "dbo.MAT_EVENTO_HISTORICO");
            DropForeignKey("dbo.MAT_EVENTO_HISTORICO", "EntidadeId_FK", "dbo.MAT_ENTIDADE");
            DropForeignKey("dbo.MAT_ENTIDADE", "RoleId_FK", "dbo.webpages_Roles");
            DropForeignKey("dbo.webpages_UsersInRoles", "RoleId", "dbo.webpages_Roles");
            DropForeignKey("dbo.webpages_UsersInRoles", "UserId", "dbo.webpages_Membership");
            DropForeignKey("dbo.webpages_OAuthMembership", "UserId", "dbo.webpages_Membership");
            DropForeignKey("dbo.MAT_NOTIFICACAO", "EntidadeId_FK", "dbo.MAT_ENTIDADE");
            DropForeignKey("dbo.MAT_INDICADOR", "EntidadeId_FK", "dbo.MAT_ENTIDADE");
            DropForeignKey("dbo.MAT_EVENTO_STAGGING", "EntidadeId_FK", "dbo.MAT_ENTIDADE");
            DropForeignKey("dbo.MAT_EVENTO_STAGGING", "FicheiroId_FK", "dbo.MAT_FICHEIRO");
            DropForeignKey("dbo.MAT_ERRO_FICHEIRO", "FicheiroId_FK", "dbo.MAT_FICHEIRO");
            DropForeignKey("dbo.MAT_FICHEIRO", "EntidadeId_FK", "dbo.MAT_ENTIDADE");
            DropForeignKey("dbo.MAT_ERRO_EVENTO_STAGGING", "EventoStaggingId_FK", "dbo.MAT_EVENTO_STAGGING");
            DropForeignKey("dbo.MAT_AVISO", "EventoStagging_FK", "dbo.MAT_EVENTO_STAGGING");
            DropForeignKey("dbo.MAT_APOLICE_HISTORICO", "EntidadeId_FK", "dbo.MAT_ENTIDADE");
            DropForeignKey("dbo.MAT_APOLICE", "EntidadeId_FK", "dbo.MAT_ENTIDADE");
            DropForeignKey("dbo.MAT_APOLICE_HISTORICO", "ConcelhoCirculacaoId_FK", "dbo.MAT_CONCELHO");
            DropForeignKey("dbo.MAT_AVISO", "ApoliceHistoricoId_FK", "dbo.MAT_APOLICE_HISTORICO");
            DropIndex("dbo.MAT_USER_PROFILE", new[] { "EntidadeId_FK" });
            DropIndex("dbo.MAT_APOLICE", new[] { "VeiculoId_FK" });
            DropIndex("dbo.MAT_APOLICE", new[] { "TomadorId_FK" });
            DropIndex("dbo.MAT_APOLICE", new[] { "EventoHistoricoId_FK" });
            DropIndex("dbo.MAT_APOLICE", new[] { "ConcelhoCirculacaoId_FK" });
            DropIndex("dbo.MAT_AVISO", new[] { "ApoliceId_FK" });
            DropIndex("dbo.MAT_AVISO", new[] { "EventoHistoricoId_FK" });
            DropIndex("dbo.MAT_APOLICE_HISTORICO", new[] { "VeiculoId_KF" });
            DropIndex("dbo.MAT_VEICULO", new[] { "CategoriaId_FK" });
            DropIndex("dbo.MAT_APOLICE_HISTORICO", new[] { "TomadorId_FK" });
            DropIndex("dbo.MAT_APOLICE_HISTORICO", new[] { "EventoHistoricoId_FK" });
            DropIndex("dbo.MAT_EVENTO_HISTORICO", new[] { "EntidadeId_FK" });
            DropIndex("dbo.MAT_ENTIDADE", new[] { "RoleId_FK" });
            DropIndex("dbo.webpages_UsersInRoles", new[] { "RoleId" });
            DropIndex("dbo.webpages_UsersInRoles", new[] { "UserId" });
            DropIndex("dbo.webpages_OAuthMembership", new[] { "UserId" });
            DropIndex("dbo.MAT_NOTIFICACAO", new[] { "EntidadeId_FK" });
            DropIndex("dbo.MAT_INDICADOR", new[] { "EntidadeId_FK" });
            DropIndex("dbo.MAT_EVENTO_STAGGING", new[] { "EntidadeId_FK" });
            DropIndex("dbo.MAT_EVENTO_STAGGING", new[] { "FicheiroId_FK" });
            DropIndex("dbo.MAT_ERRO_FICHEIRO", new[] { "FicheiroId_FK" });
            DropIndex("dbo.MAT_FICHEIRO", new[] { "EntidadeId_FK" });
            DropIndex("dbo.MAT_ERRO_EVENTO_STAGGING", new[] { "EventoStaggingId_FK" });
            DropIndex("dbo.MAT_AVISO", new[] { "EventoStagging_FK" });
            DropIndex("dbo.MAT_APOLICE_HISTORICO", new[] { "EntidadeId_FK" });
            DropIndex("dbo.MAT_APOLICE", new[] { "EntidadeId_FK" });
            DropIndex("dbo.MAT_APOLICE_HISTORICO", new[] { "ConcelhoCirculacaoId_FK" });
            DropIndex("dbo.MAT_AVISO", new[] { "ApoliceHistoricoId_FK" });
            DropTable("dbo.webpages_UsersInRoles");
            DropTable("dbo.MAT_USER_PROFILE");
            DropTable("dbo.MAT_CATEGORIA");
            DropTable("dbo.MAT_VEICULO");
            DropTable("dbo.MAT_PESSOA");
            DropTable("dbo.MAT_EVENTO_HISTORICO");
            DropTable("dbo.webpages_OAuthMembership");
            DropTable("dbo.webpages_Membership");
            DropTable("dbo.webpages_Roles");
            DropTable("dbo.MAT_NOTIFICACAO");
            DropTable("dbo.MAT_INDICADOR");
            DropTable("dbo.MAT_ERRO_FICHEIRO");
            DropTable("dbo.MAT_FICHEIRO");
            DropTable("dbo.MAT_ERRO_EVENTO_STAGGING");
            DropTable("dbo.MAT_EVENTO_STAGGING");
            DropTable("dbo.MAT_ENTIDADE");
            DropTable("dbo.MAT_CONCELHO");
            DropTable("dbo.MAT_APOLICE_HISTORICO");
            DropTable("dbo.MAT_AVISO");
            DropTable("dbo.MAT_APOLICE");
            DropTable("dbo.ActionLogs");
        }
    }
}

namespace ISP.GestaoMatriculas.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
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
                "dbo.Apolices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EntidadeId = c.Int(nullable: false),
                        TomadorId = c.Int(nullable: false),
                        VeiculoId = c.Int(nullable: false),
                        ConcelhoCirculacaoId = c.Int(nullable: false),
                        NumeroApolice = c.String(),
                        NumeroCertificadoProvisorio = c.String(),
                        DataInicio = c.DateTime(nullable: false),
                        DataFim = c.DateTime(nullable: false),
                        DataFimPlaneada = c.DateTime(nullable: false),
                        DataReporte = c.DateTime(nullable: false),
                        EventoHistoricoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Concelhos", t => t.ConcelhoCirculacaoId, cascadeDelete: true)
                .ForeignKey("dbo.Entidades", t => t.EntidadeId, cascadeDelete: true)
                .ForeignKey("dbo.EventosHistorico", t => t.EventoHistoricoId, cascadeDelete: true)
                .ForeignKey("dbo.Segurados", t => t.TomadorId, cascadeDelete: true)
                .ForeignKey("dbo.Veiculos", t => t.VeiculoId, cascadeDelete: true)
                .Index(t => t.ConcelhoCirculacaoId)
                .Index(t => t.EntidadeId)
                .Index(t => t.EventoHistoricoId)
                .Index(t => t.TomadorId)
                .Index(t => t.VeiculoId);
            
            CreateTable(
                "dbo.AvisosApolice",
                c => new
                    {
                        AvisoApoliceId = c.Int(nullable: false, identity: true),
                        apoliceId = c.Int(nullable: false),
                        tipologia = c.Int(nullable: false),
                        descricao = c.String(),
                        campo = c.String(),
                    })
                .PrimaryKey(t => t.AvisoApoliceId);
            
            CreateTable(
                "dbo.Concelhos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Codigo = c.String(),
                        Nome = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Entidades",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        roleId = c.Int(nullable: false),
                        Nome = c.String(),
                        codigoSeguradora = c.String(),
                        nomeResponsavel = c.String(),
                        emailResponsavel = c.String(),
                        telefoneResponsavel = c.String(),
                        scope = c.Int(nullable: false),
                        ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.webpages_Roles", t => t.roleId, cascadeDelete: true)
                .Index(t => t.roleId);
            
            CreateTable(
                "dbo.EventosStagging",
                c => new
                    {
                        eventoStaggingId = c.Int(nullable: false, identity: true),
                        idOcorrencia = c.String(),
                        CodigoOperacao = c.String(),
                        entidadId = c.Int(),
                        nrApolice = c.String(),
                        nrCertificadoProvisorio = c.String(),
                        dataInicioCobertura = c.String(),
                        dataFimCobertura = c.String(),
                        matricula = c.String(),
                        marca = c.String(),
                        modelo = c.String(),
                        anoConstrucao = c.String(),
                        categoriaVeiculo = c.String(),
                        concelhoCirculacao = c.String(),
                        nomeTomadorSeguro = c.String(),
                        moradaTomadorSeguro = c.String(),
                        codigoPostalTomador = c.String(),
                        nrIdentificacaoTomadorSeguro = c.String(),
                        estadoEvento = c.Int(nullable: false),
                        ficheiroID = c.Int(),
                    })
                .PrimaryKey(t => t.eventoStaggingId)
                .ForeignKey("dbo.Entidades", t => t.entidadId)
                .ForeignKey("dbo.Ficheiros", t => t.ficheiroID)
                .Index(t => t.entidadId)
                .Index(t => t.ficheiroID);
            
            CreateTable(
                "dbo.ErrosEventoStagging",
                c => new
                    {
                        ErroEventoStaggingId = c.Int(nullable: false, identity: true),
                        eventoStaggingId = c.Int(nullable: false),
                        tipologia = c.Int(nullable: false),
                        descricao = c.String(),
                        campo = c.String(),
                    })
                .PrimaryKey(t => t.ErroEventoStaggingId)
                .ForeignKey("dbo.EventosStagging", t => t.eventoStaggingId, cascadeDelete: true)
                .Index(t => t.eventoStaggingId);
            
            CreateTable(
                "dbo.Ficheiros",
                c => new
                    {
                        ficheiroId = c.Int(nullable: false, identity: true),
                        entidadeId = c.Int(nullable: false),
                        nomeFicheiro = c.String(),
                        localizacao = c.String(),
                        estado = c.Int(nullable: false),
                        erro = c.Boolean(nullable: false),
                        dataUpload = c.DateTime(nullable: false),
                        dataAlteracao = c.DateTime(nullable: false),
                        userName = c.String(),
                    })
                .PrimaryKey(t => t.ficheiroId)
                .ForeignKey("dbo.Entidades", t => t.entidadeId, cascadeDelete: true)
                .Index(t => t.entidadeId);
            
            CreateTable(
                "dbo.ErrosFicheiro",
                c => new
                    {
                        erroFicheiroId = c.Int(nullable: false, identity: true),
                        ficheiroId = c.Int(nullable: false),
                        descricao = c.String(),
                        dataValidacao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.erroFicheiroId)
                .ForeignKey("dbo.Ficheiros", t => t.ficheiroId, cascadeDelete: true)
                .Index(t => t.ficheiroId);
            
            CreateTable(
                "dbo.Indicadores",
                c => new
                    {
                        indicadorId = c.Int(nullable: false, identity: true),
                        entidadeId = c.Int(),
                        descricao = c.String(),
                        valor = c.Double(nullable: false),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.indicadorId)
                .ForeignKey("dbo.Entidades", t => t.entidadeId)
                .Index(t => t.entidadeId);
            
            CreateTable(
                "dbo.Notificacoes",
                c => new
                    {
                        notificacaoId = c.Int(nullable: false, identity: true),
                        dataCriacao = c.DateTime(nullable: false),
                        lida = c.Boolean(nullable: false),
                        mensagem = c.String(),
                        entidadeId = c.Int(),
                        tipoId = c.Int(nullable: false),
                        tipo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.notificacaoId)
                .ForeignKey("dbo.Entidades", t => t.entidadeId)
                .Index(t => t.entidadeId);
            
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
                "dbo.EventosHistorico",
                c => new
                    {
                        eventoHistoricoId = c.Int(nullable: false, identity: true),
                        idOcorrencia = c.String(),
                        codigoOperacao = c.String(),
                        seguradoraId = c.String(),
                    })
                .PrimaryKey(t => t.eventoHistoricoId);
            
            CreateTable(
                "dbo.Segurados",
                c => new
                    {
                        pessoaId = c.Int(nullable: false, identity: true),
                        nome = c.String(),
                        numeroIdentificacao = c.String(),
                        nif = c.String(),
                        morada = c.String(),
                        codigoPostal = c.String(),
                    })
                .PrimaryKey(t => t.pessoaId);
            
            CreateTable(
                "dbo.Veiculos",
                c => new
                    {
                        veiculoId = c.Int(nullable: false, identity: true),
                        categoriaId = c.Int(nullable: false),
                        anoConstrucao = c.String(),
                        numeroMatricula = c.String(),
                        marcaVeiculo = c.String(),
                        modeloVeiculo = c.String(),
                    })
                .PrimaryKey(t => t.veiculoId)
                .ForeignKey("dbo.Categorias", t => t.categoriaId, cascadeDelete: true)
                .Index(t => t.categoriaId);
            
            CreateTable(
                "dbo.Categorias",
                c => new
                    {
                        categoriaId = c.Int(nullable: false, identity: true),
                        codigoCategoriaVeiculo = c.String(),
                        nome = c.String(),
                    })
                .PrimaryKey(t => t.categoriaId);
            
            CreateTable(
                "dbo.UserProfile",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        entidadeId = c.Int(nullable: false),
                        UserName = c.String(),
                        nome = c.String(),
                        email = c.String(),
                        telefone = c.String(),
                        ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Entidades", t => t.entidadeId, cascadeDelete: true)
                .Index(t => t.entidadeId);
            
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
            
            CreateTable(
                "dbo.ApolicesHistorico",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Entidade_Id = c.Int(),
                        EntidadeId = c.Int(nullable: false),
                        TomadorId = c.Int(nullable: false),
                        VeiculoId = c.Int(nullable: false),
                        ConcelhoCirculacaoId = c.Int(nullable: false),
                        NumeroApolice = c.String(),
                        NumeroCertificadoProvisorio = c.String(),
                        DataInicio = c.DateTime(nullable: false),
                        DataFim = c.DateTime(nullable: false),
                        DataFimPlaneada = c.DateTime(nullable: false),
                        DataReporte = c.DateTime(nullable: false),
                        EventoHistoricoId = c.Int(nullable: false),
                        ApoliceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Entidades", t => t.Entidade_Id)
                .ForeignKey("dbo.Entidades", t => t.EntidadeId, cascadeDelete: true)
                .ForeignKey("dbo.Segurados", t => t.TomadorId, cascadeDelete: true)
                .ForeignKey("dbo.Veiculos", t => t.VeiculoId, cascadeDelete: true)
                .ForeignKey("dbo.Concelhos", t => t.ConcelhoCirculacaoId, cascadeDelete: true)
                .ForeignKey("dbo.EventosHistorico", t => t.EventoHistoricoId, cascadeDelete: true)
                .Index(t => t.Entidade_Id)
                .Index(t => t.EntidadeId)
                .Index(t => t.TomadorId)
                .Index(t => t.VeiculoId)
                .Index(t => t.ConcelhoCirculacaoId)
                .Index(t => t.EventoHistoricoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApolicesHistorico", "EventoHistoricoId", "dbo.EventosHistorico");
            DropForeignKey("dbo.ApolicesHistorico", "ConcelhoCirculacaoId", "dbo.Concelhos");
            DropForeignKey("dbo.ApolicesHistorico", "VeiculoId", "dbo.Veiculos");
            DropForeignKey("dbo.ApolicesHistorico", "TomadorId", "dbo.Segurados");
            DropForeignKey("dbo.ApolicesHistorico", "EntidadeId", "dbo.Entidades");
            DropForeignKey("dbo.ApolicesHistorico", "Entidade_Id", "dbo.Entidades");
            DropForeignKey("dbo.UserProfile", "entidadeId", "dbo.Entidades");
            DropForeignKey("dbo.Apolices", "VeiculoId", "dbo.Veiculos");
            DropForeignKey("dbo.Veiculos", "categoriaId", "dbo.Categorias");
            DropForeignKey("dbo.Apolices", "TomadorId", "dbo.Segurados");
            DropForeignKey("dbo.Apolices", "EventoHistoricoId", "dbo.EventosHistorico");
            DropForeignKey("dbo.Entidades", "roleId", "dbo.webpages_Roles");
            DropForeignKey("dbo.webpages_UsersInRoles", "RoleId", "dbo.webpages_Roles");
            DropForeignKey("dbo.webpages_UsersInRoles", "UserId", "dbo.webpages_Membership");
            DropForeignKey("dbo.webpages_OAuthMembership", "UserId", "dbo.webpages_Membership");
            DropForeignKey("dbo.Notificacoes", "entidadeId", "dbo.Entidades");
            DropForeignKey("dbo.Indicadores", "entidadeId", "dbo.Entidades");
            DropForeignKey("dbo.EventosStagging", "ficheiroID", "dbo.Ficheiros");
            DropForeignKey("dbo.ErrosFicheiro", "ficheiroId", "dbo.Ficheiros");
            DropForeignKey("dbo.Ficheiros", "entidadeId", "dbo.Entidades");
            DropForeignKey("dbo.ErrosEventoStagging", "eventoStaggingId", "dbo.EventosStagging");
            DropForeignKey("dbo.EventosStagging", "entidadId", "dbo.Entidades");
            DropForeignKey("dbo.Apolices", "EntidadeId", "dbo.Entidades");
            DropForeignKey("dbo.Apolices", "ConcelhoCirculacaoId", "dbo.Concelhos");
            DropIndex("dbo.ApolicesHistorico", new[] { "EventoHistoricoId" });
            DropIndex("dbo.ApolicesHistorico", new[] { "ConcelhoCirculacaoId" });
            DropIndex("dbo.ApolicesHistorico", new[] { "VeiculoId" });
            DropIndex("dbo.ApolicesHistorico", new[] { "TomadorId" });
            DropIndex("dbo.ApolicesHistorico", new[] { "EntidadeId" });
            DropIndex("dbo.ApolicesHistorico", new[] { "Entidade_Id" });
            DropIndex("dbo.UserProfile", new[] { "entidadeId" });
            DropIndex("dbo.Apolices", new[] { "VeiculoId" });
            DropIndex("dbo.Veiculos", new[] { "categoriaId" });
            DropIndex("dbo.Apolices", new[] { "TomadorId" });
            DropIndex("dbo.Apolices", new[] { "EventoHistoricoId" });
            DropIndex("dbo.Entidades", new[] { "roleId" });
            DropIndex("dbo.webpages_UsersInRoles", new[] { "RoleId" });
            DropIndex("dbo.webpages_UsersInRoles", new[] { "UserId" });
            DropIndex("dbo.webpages_OAuthMembership", new[] { "UserId" });
            DropIndex("dbo.Notificacoes", new[] { "entidadeId" });
            DropIndex("dbo.Indicadores", new[] { "entidadeId" });
            DropIndex("dbo.EventosStagging", new[] { "ficheiroID" });
            DropIndex("dbo.ErrosFicheiro", new[] { "ficheiroId" });
            DropIndex("dbo.Ficheiros", new[] { "entidadeId" });
            DropIndex("dbo.ErrosEventoStagging", new[] { "eventoStaggingId" });
            DropIndex("dbo.EventosStagging", new[] { "entidadId" });
            DropIndex("dbo.Apolices", new[] { "EntidadeId" });
            DropIndex("dbo.Apolices", new[] { "ConcelhoCirculacaoId" });
            DropTable("dbo.ApolicesHistorico");
            DropTable("dbo.webpages_UsersInRoles");
            DropTable("dbo.UserProfile");
            DropTable("dbo.Categorias");
            DropTable("dbo.Veiculos");
            DropTable("dbo.Segurados");
            DropTable("dbo.EventosHistorico");
            DropTable("dbo.webpages_OAuthMembership");
            DropTable("dbo.webpages_Membership");
            DropTable("dbo.webpages_Roles");
            DropTable("dbo.Notificacoes");
            DropTable("dbo.Indicadores");
            DropTable("dbo.ErrosFicheiro");
            DropTable("dbo.Ficheiros");
            DropTable("dbo.ErrosEventoStagging");
            DropTable("dbo.EventosStagging");
            DropTable("dbo.Entidades");
            DropTable("dbo.Concelhos");
            DropTable("dbo.AvisosApolice");
            DropTable("dbo.Apolices");
            DropTable("dbo.ActionLogs");
        }
    }
}

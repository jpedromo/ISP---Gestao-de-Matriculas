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
                "dbo.MAT_PERIODO_COBERTURA",
                c => new
                    {
                        CodPeriodoCoberturaId_PK = c.Int(nullable: false, identity: true),
                        CodEntidadeId_FK = c.Int(),
                        CodTomadorSeguroId_FK = c.Int(nullable: false),
                        CodVeiculoId_FK = c.Int(nullable: false),
                        CodConcelhoCirculacaoId_FK = c.Int(),
                        NrApolice = c.String(),
                        NrCertificadoProvisorio = c.String(),
                        DtInicio = c.DateTime(nullable: false),
                        DtFim = c.DateTime(nullable: false),
                        DtFimPlaneada = c.DateTime(nullable: false),
                        DtReporte = c.DateTime(nullable: false),
                        NomUtilizadorReporte = c.String(),
                        DtRegisto = c.DateTime(nullable: false),
                        ValSLADias = c.Int(nullable: false),
                        ValSLAHoras = c.Time(nullable: false, precision: 7),
                        FlgAvisos = c.Boolean(nullable: false),
                        CodEventoHistoricoId_FK = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CodPeriodoCoberturaId_PK)
                .ForeignKey("dbo.MAT_ENTIDADE", t => t.CodEntidadeId_FK)
                .ForeignKey("dbo.MAT_CONCELHO", t => t.CodConcelhoCirculacaoId_FK)
                .ForeignKey("dbo.MAT_EVENTO_HISTORICO", t => t.CodEventoHistoricoId_FK, cascadeDelete: true)
                .ForeignKey("dbo.MAT_TOMADOR_SEGURO", t => t.CodTomadorSeguroId_FK, cascadeDelete: true)
                .ForeignKey("dbo.MAT_VEICULO", t => t.CodVeiculoId_FK, cascadeDelete: true)
                .Index(t => t.CodEntidadeId_FK)
                .Index(t => t.CodConcelhoCirculacaoId_FK)
                .Index(t => t.CodEventoHistoricoId_FK)
                .Index(t => t.CodTomadorSeguroId_FK)
                .Index(t => t.CodVeiculoId_FK);
            
            CreateTable(
                "dbo.MAT_AVISO",
                c => new
                    {
                        CodAvisoId_PK = c.Int(nullable: false, identity: true),
                        CodPeriodoCoberturaId_FK = c.Int(),
                        CodPeriodoCoberturaHistoricoId_FK = c.Int(),
                        CodEventoStaggingId_FK = c.Int(),
                        CodEventoHistoricoId_FK = c.Int(),
                        CodTipologiaId_FK = c.Int(nullable: false),
                        NomDescricao = c.String(),
                        NomCampo = c.String(),
                    })
                .PrimaryKey(t => t.CodAvisoId_PK)
                .ForeignKey("dbo.MAT_PERIODO_COBERTURA_HISTORICO", t => t.CodPeriodoCoberturaHistoricoId_FK)
                .ForeignKey("dbo.MAT_EVENTO_STAGGING", t => t.CodEventoStaggingId_FK)
                .ForeignKey("dbo.MAT_EVENTO_HISTORICO", t => t.CodEventoHistoricoId_FK)
                .ForeignKey("dbo.MAT_DESC_VALOR_SISTEMA", t => t.CodTipologiaId_FK, cascadeDelete: true)
                .ForeignKey("dbo.MAT_PERIODO_COBERTURA", t => t.CodPeriodoCoberturaId_FK)
                .Index(t => t.CodPeriodoCoberturaHistoricoId_FK)
                .Index(t => t.CodEventoStaggingId_FK)
                .Index(t => t.CodEventoHistoricoId_FK)
                .Index(t => t.CodTipologiaId_FK)
                .Index(t => t.CodPeriodoCoberturaId_FK);
            
            CreateTable(
                "dbo.MAT_PERIODO_COBERTURA_HISTORICO",
                c => new
                    {
                        CodPeriodoCoberturaId_PK = c.Int(nullable: false, identity: true),
                        CodEntidadeId_FK = c.Int(),
                        CodTomadorId_FK = c.Int(nullable: false),
                        CodVeiculoId_FK = c.Int(nullable: false),
                        CodConcelhoCirculacaoId_FK = c.Int(),
                        NrApolice = c.String(),
                        NrCertificadoProvisorio = c.String(),
                        DtInicio = c.DateTime(nullable: false),
                        DtFim = c.DateTime(nullable: false),
                        DtFimPlaneada = c.DateTime(nullable: false),
                        DtReporte = c.DateTime(nullable: false),
                        NomUtilizadorReporte = c.String(),
                        DtArquivo = c.DateTime(nullable: false),
                        NomUtilizadorArquivo = c.String(),
                        ValSLADias = c.Int(nullable: false),
                        ValSLAHoras = c.Time(nullable: false, precision: 7),
                        CodEventoHistoricoId_FK = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CodPeriodoCoberturaId_PK)
                .ForeignKey("dbo.MAT_CONCELHO", t => t.CodConcelhoCirculacaoId_FK)
                .ForeignKey("dbo.MAT_ENTIDADE", t => t.CodEntidadeId_FK)
                .ForeignKey("dbo.MAT_EVENTO_HISTORICO", t => t.CodEventoHistoricoId_FK, cascadeDelete: true)
                .ForeignKey("dbo.MAT_TOMADOR_SEGURO", t => t.CodTomadorId_FK, cascadeDelete: true)
                .ForeignKey("dbo.MAT_VEICULO", t => t.CodVeiculoId_FK, cascadeDelete: true)
                .Index(t => t.CodConcelhoCirculacaoId_FK)
                .Index(t => t.CodEntidadeId_FK)
                .Index(t => t.CodEventoHistoricoId_FK)
                .Index(t => t.CodTomadorId_FK)
                .Index(t => t.CodVeiculoId_FK);
            
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
                        CodEntidadeId_PK = c.Int(nullable: false, identity: true),
                        CodRoleId_FK = c.Int(nullable: false),
                        NomeEntidade = c.String(),
                        CodEntidade = c.String(),
                        NomeResponsavel = c.String(),
                        XEmailResponsavel = c.String(),
                        NrTelefoneResponsavel = c.String(),
                        CodScopeId_FK = c.Int(nullable: false),
                        FlgAtivo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CodEntidadeId_PK)
                .ForeignKey("dbo.webpages_Roles", t => t.CodRoleId_FK, cascadeDelete: true)
                .ForeignKey("dbo.MAT_DESC_VALOR_SISTEMA", t => t.CodScopeId_FK, cascadeDelete: true)
                .Index(t => t.CodRoleId_FK)
                .Index(t => t.CodScopeId_FK);
            
            CreateTable(
                "dbo.MAT_EVENTO_STAGGING",
                c => new
                    {
                        CodEventoStaggingId_PK = c.Int(nullable: false, identity: true),
                        ValIdOcorrencia = c.String(),
                        codigoOperacao = c.String(),
                        CodEntidadeId_FK = c.Int(),
                        NrApolice = c.String(),
                        NrCertificadoProvisorio = c.String(),
                        DtInicio = c.String(),
                        DtHoraInicio = c.String(),
                        DtFim = c.String(),
                        DtHoraFim = c.String(),
                        XMatricula = c.String(),
                        NomMarca = c.String(),
                        NomModelo = c.String(),
                        DtAnoConstrucao = c.String(),
                        CodCategoriaVeiculo = c.String(),
                        CodConcelhoCirculacao = c.String(),
                        NomeTomadorSeguro = c.String(),
                        NomMoradaTomadorSeguro = c.String(),
                        CodPostalTomadorSeguro = c.String(),
                        NrIdentificacaoTomadorSeguro = c.String(),
                        NrNIFTomadorSeguro = c.String(),
                        FlgDeleted = c.Boolean(nullable: false),
                        DtFimPlaneada = c.DateTime(),
                        TotErrosCumulativos = c.Int(nullable: false),
                        TotAvisosCumulativos = c.Int(nullable: false),
                        DtReporte = c.DateTime(nullable: false),
                        NomUtilizadorReporte = c.String(),
                        DtRegisto = c.DateTime(nullable: false),
                        DtUltimaAlteracao = c.DateTime(nullable: false),
                        DtCorrecaoErro = c.DateTime(),
                        NomeUtilizadorCorrecao = c.String(),
                        DtArquivo = c.DateTime(),
                        UtilizadorArquivo = c.String(),
                        CodEstadoEventoId_FK = c.Int(nullable: false),
                        CodFicheiroId_FK = c.Int(),
                    })
                .PrimaryKey(t => t.CodEventoStaggingId_PK)
                .ForeignKey("dbo.MAT_DESC_VALOR_SISTEMA", t => t.CodEstadoEventoId_FK, cascadeDelete: true)
                .ForeignKey("dbo.MAT_FICHEIRO", t => t.CodFicheiroId_FK)
                .ForeignKey("dbo.MAT_ENTIDADE", t => t.CodEntidadeId_FK)
                .Index(t => t.CodEstadoEventoId_FK)
                .Index(t => t.CodFicheiroId_FK)
                .Index(t => t.CodEntidadeId_FK);
            
            CreateTable(
                "dbo.MAT_ERRO_EVENTO_STAGGING",
                c => new
                    {
                        CodErroEventoStaggingId_PK = c.Int(nullable: false, identity: true),
                        CodEventoStaggingId_FK = c.Int(),
                        CodTipologiaId_FK = c.Int(nullable: false),
                        NomDescricao = c.String(),
                        NomCampo = c.String(),
                    })
                .PrimaryKey(t => t.CodErroEventoStaggingId_PK)
                .ForeignKey("dbo.MAT_DESC_VALOR_SISTEMA", t => t.CodTipologiaId_FK, cascadeDelete: true)
                .ForeignKey("dbo.MAT_EVENTO_STAGGING", t => t.CodEventoStaggingId_FK)
                .Index(t => t.CodTipologiaId_FK)
                .Index(t => t.CodEventoStaggingId_FK);
            
            CreateTable(
                "dbo.MAT_DESC_VALOR_SISTEMA",
                c => new
                    {
                        CodValorSistemaId_PK = c.Int(nullable: false, identity: true),
                        CodTipologia = c.String(),
                        Valor = c.String(),
                        NomDescricao = c.String(),
                        NomDescricaoLonga = c.String(),
                        FlgEditavel = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CodValorSistemaId_PK);
            
            CreateTable(
                "dbo.MAT_FICHEIRO",
                c => new
                    {
                        CodFicheiroId_PK = c.Int(nullable: false, identity: true),
                        CodEntidadeId_FK = c.Int(nullable: false),
                        NomeFicheiro = c.String(),
                        XLocalizacao = c.String(),
                        CodEstadoId_FK = c.Int(),
                        TotRegistos = c.Int(nullable: false),
                        TotRegistosProcessados = c.Int(nullable: false),
                        TotEventosErro = c.Int(nullable: false),
                        TotEventosAviso = c.Int(nullable: false),
                        DtUpload = c.DateTime(nullable: false),
                        DtAlteracao = c.DateTime(nullable: false),
                        NomeUtilizador = c.String(),
                    })
                .PrimaryKey(t => t.CodFicheiroId_PK)
                .ForeignKey("dbo.MAT_ENTIDADE", t => t.CodEntidadeId_FK, cascadeDelete: true)
                .ForeignKey("dbo.MAT_DESC_VALOR_SISTEMA", t => t.CodEstadoId_FK)
                .Index(t => t.CodEntidadeId_FK)
                .Index(t => t.CodEstadoId_FK);
            
            CreateTable(
                "dbo.MAT_ERRO_FICHEIRO",
                c => new
                    {
                        CodErroFicheiroId_PK = c.Int(nullable: false, identity: true),
                        CodFicheiroId_FK = c.Int(),
                        CodTipologiaId_FK = c.Int(nullable: false),
                        NomDescricao = c.String(),
                        DtValidacao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CodErroFicheiroId_PK)
                .ForeignKey("dbo.MAT_DESC_VALOR_SISTEMA", t => t.CodTipologiaId_FK, cascadeDelete: true)
                .ForeignKey("dbo.MAT_FICHEIRO", t => t.CodFicheiroId_FK)
                .Index(t => t.CodTipologiaId_FK)
                .Index(t => t.CodFicheiroId_FK);
            
            CreateTable(
                "dbo.MAT_INDICADOR",
                c => new
                    {
                        CodIndicadorId_PK = c.Int(nullable: false, identity: true),
                        CodEntidadeId_FK = c.Int(),
                        CodTipologiaId_FK = c.Int(nullable: false),
                        CodSubTipo = c.String(),
                        FlgPublico = c.Boolean(nullable: false),
                        XDescricao = c.String(),
                        DtIndicador = c.DateTime(nullable: false),
                        ValValor = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.CodIndicadorId_PK)
                .ForeignKey("dbo.MAT_DESC_VALOR_SISTEMA", t => t.CodTipologiaId_FK, cascadeDelete: true)
                .ForeignKey("dbo.MAT_ENTIDADE", t => t.CodEntidadeId_FK)
                .Index(t => t.CodTipologiaId_FK)
                .Index(t => t.CodEntidadeId_FK);
            
            CreateTable(
                "dbo.MAT_NOTIFICACAO",
                c => new
                    {
                        CodNotificacaoId_PK = c.Int(nullable: false, identity: true),
                        DtCriacao = c.DateTime(nullable: false),
                        FlgLida = c.Boolean(nullable: false),
                        FlgEmail = c.Boolean(nullable: false),
                        FlgEnviadoEmail = c.Boolean(nullable: false),
                        dtEnvioEmail = c.DateTime(),
                        XMensagem = c.String(),
                        CodEntidadeId_FK = c.Int(),
                        CodTipologiaId_FK = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CodNotificacaoId_PK)
                .ForeignKey("dbo.MAT_DESC_VALOR_SISTEMA", t => t.CodTipologiaId_FK, cascadeDelete: true)
                .ForeignKey("dbo.MAT_ENTIDADE", t => t.CodEntidadeId_FK)
                .Index(t => t.CodTipologiaId_FK)
                .Index(t => t.CodEntidadeId_FK);
            
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
                        CodEventoHistoricoId_PK = c.Int(nullable: false, identity: true),
                        ValIdOcorrencia = c.String(),
                        CodOperacaoId_FK = c.Int(nullable: false),
                        DtReporte = c.DateTime(nullable: false),
                        CodEntidadeId_FK = c.Int(),
                    })
                .PrimaryKey(t => t.CodEventoHistoricoId_PK)
                .ForeignKey("dbo.MAT_DESC_VALOR_SISTEMA", t => t.CodOperacaoId_FK, cascadeDelete: true)
                .ForeignKey("dbo.MAT_ENTIDADE", t => t.CodEntidadeId_FK)
                .Index(t => t.CodOperacaoId_FK)
                .Index(t => t.CodEntidadeId_FK);
            
            CreateTable(
                "dbo.MAT_TOMADOR_SEGURO",
                c => new
                    {
                        CodTomadorSeguroId_PK = c.Int(nullable: false, identity: true),
                        Nome = c.String(),
                        NrIdentificacao = c.String(),
                        NrNIF = c.String(),
                        NomMorada = c.String(),
                        CodPostal = c.String(),
                    })
                .PrimaryKey(t => t.CodTomadorSeguroId_PK);
            
            CreateTable(
                "dbo.MAT_VEICULO",
                c => new
                    {
                        CodVeiculoId_PK = c.Int(nullable: false, identity: true),
                        CodCategoriaId_FK = c.Int(),
                        DtAnoConstrucao = c.String(),
                        XNumeroMatricula = c.String(),
                        XNumeroMatriculaCorrigido = c.String(),
                        NomMarcaVeiculo = c.String(),
                        NomModeloVeiculo = c.String(),
                    })
                .PrimaryKey(t => t.CodVeiculoId_PK)
                .ForeignKey("dbo.MAT_CATEGORIA", t => t.CodCategoriaId_FK)
                .Index(t => t.CodCategoriaId_FK);
            
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
                "dbo.MAT_PERIODO_COBERTURA_ISENTO",
                c => new
                    {
                        apoliceIsentoId = c.Int(nullable: false, identity: true),
                        CodEntidadeId_FK = c.Int(nullable: false),
                        XMatricula = c.String(),
                        XMatriculaCorrigida = c.String(),
                        NomEntidadeResponsavel = c.String(),
                        XMensagem = c.String(),
                        DtInicio = c.DateTime(nullable: false),
                        DtFim = c.DateTime(),
                        DtReporte = c.DateTime(nullable: false),
                        DtModificacao = c.DateTime(nullable: false),
                        FlgConfidencial = c.Boolean(nullable: false),
                        FlgOrigemFicheiro = c.Boolean(nullable: false),
                        FlgArquivo = c.Boolean(nullable: false),
                        CodFicheiroIsentosId_FK = c.Int(),
                    })
                .PrimaryKey(t => t.apoliceIsentoId)
                .ForeignKey("dbo.MAT_ENTIDADE", t => t.CodEntidadeId_FK, cascadeDelete: true)
                .ForeignKey("dbo.MAT_FICHEIRO_ISENTO", t => t.CodFicheiroIsentosId_FK)
                .Index(t => t.CodEntidadeId_FK)
                .Index(t => t.CodFicheiroIsentosId_FK);
            
            CreateTable(
                "dbo.MAT_FICHEIRO_ISENTO",
                c => new
                    {
                        CodFicheiroIsentosId_PK = c.Int(nullable: false, identity: true),
                        CodEntidadeId_FK = c.Int(),
                        NomFicheiro = c.String(),
                        XLocalizacao = c.String(),
                        CodEstadoId_FK = c.Int(nullable: false),
                        TotRegistos = c.Int(nullable: false),
                        TotEventosErro = c.Int(nullable: false),
                        TotEventosAviso = c.Int(nullable: false),
                        DtUpload = c.DateTime(nullable: false),
                        DtAlteracao = c.DateTime(nullable: false),
                        NomeUtilizador = c.String(),
                    })
                .PrimaryKey(t => t.CodFicheiroIsentosId_PK)
                .ForeignKey("dbo.MAT_ENTIDADE", t => t.CodEntidadeId_FK)
                .ForeignKey("dbo.MAT_DESC_VALOR_SISTEMA", t => t.CodEstadoId_FK, cascadeDelete: true)
                .Index(t => t.CodEntidadeId_FK)
                .Index(t => t.CodEstadoId_FK);
            
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
                        ValUtilizadorAD = c.String(),
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
            DropForeignKey("dbo.MAT_FICHEIRO_ISENTO", "CodEstadoId_FK", "dbo.MAT_DESC_VALOR_SISTEMA");
            DropForeignKey("dbo.MAT_FICHEIRO_ISENTO", "CodEntidadeId_FK", "dbo.MAT_ENTIDADE");
            DropForeignKey("dbo.MAT_PERIODO_COBERTURA_ISENTO", "CodFicheiroIsentosId_FK", "dbo.MAT_FICHEIRO_ISENTO");
            DropForeignKey("dbo.MAT_PERIODO_COBERTURA_ISENTO", "CodEntidadeId_FK", "dbo.MAT_ENTIDADE");
            DropForeignKey("dbo.MAT_PERIODO_COBERTURA", "CodVeiculoId_FK", "dbo.MAT_VEICULO");
            DropForeignKey("dbo.MAT_PERIODO_COBERTURA", "CodTomadorSeguroId_FK", "dbo.MAT_TOMADOR_SEGURO");
            DropForeignKey("dbo.MAT_PERIODO_COBERTURA", "CodEventoHistoricoId_FK", "dbo.MAT_EVENTO_HISTORICO");
            DropForeignKey("dbo.MAT_PERIODO_COBERTURA", "CodConcelhoCirculacaoId_FK", "dbo.MAT_CONCELHO");
            DropForeignKey("dbo.MAT_AVISO", "CodPeriodoCoberturaId_FK", "dbo.MAT_PERIODO_COBERTURA");
            DropForeignKey("dbo.MAT_AVISO", "CodTipologiaId_FK", "dbo.MAT_DESC_VALOR_SISTEMA");
            DropForeignKey("dbo.MAT_AVISO", "CodEventoHistoricoId_FK", "dbo.MAT_EVENTO_HISTORICO");
            DropForeignKey("dbo.MAT_PERIODO_COBERTURA_HISTORICO", "CodVeiculoId_FK", "dbo.MAT_VEICULO");
            DropForeignKey("dbo.MAT_VEICULO", "CodCategoriaId_FK", "dbo.MAT_CATEGORIA");
            DropForeignKey("dbo.MAT_PERIODO_COBERTURA_HISTORICO", "CodTomadorId_FK", "dbo.MAT_TOMADOR_SEGURO");
            DropForeignKey("dbo.MAT_PERIODO_COBERTURA_HISTORICO", "CodEventoHistoricoId_FK", "dbo.MAT_EVENTO_HISTORICO");
            DropForeignKey("dbo.MAT_EVENTO_HISTORICO", "CodEntidadeId_FK", "dbo.MAT_ENTIDADE");
            DropForeignKey("dbo.MAT_EVENTO_HISTORICO", "CodOperacaoId_FK", "dbo.MAT_DESC_VALOR_SISTEMA");
            DropForeignKey("dbo.MAT_ENTIDADE", "CodScopeId_FK", "dbo.MAT_DESC_VALOR_SISTEMA");
            DropForeignKey("dbo.MAT_ENTIDADE", "CodRoleId_FK", "dbo.webpages_Roles");
            DropForeignKey("dbo.webpages_UsersInRoles", "RoleId", "dbo.webpages_Roles");
            DropForeignKey("dbo.webpages_UsersInRoles", "UserId", "dbo.webpages_Membership");
            DropForeignKey("dbo.webpages_OAuthMembership", "UserId", "dbo.webpages_Membership");
            DropForeignKey("dbo.MAT_NOTIFICACAO", "CodEntidadeId_FK", "dbo.MAT_ENTIDADE");
            DropForeignKey("dbo.MAT_NOTIFICACAO", "CodTipologiaId_FK", "dbo.MAT_DESC_VALOR_SISTEMA");
            DropForeignKey("dbo.MAT_INDICADOR", "CodEntidadeId_FK", "dbo.MAT_ENTIDADE");
            DropForeignKey("dbo.MAT_INDICADOR", "CodTipologiaId_FK", "dbo.MAT_DESC_VALOR_SISTEMA");
            DropForeignKey("dbo.MAT_EVENTO_STAGGING", "CodEntidadeId_FK", "dbo.MAT_ENTIDADE");
            DropForeignKey("dbo.MAT_EVENTO_STAGGING", "CodFicheiroId_FK", "dbo.MAT_FICHEIRO");
            DropForeignKey("dbo.MAT_FICHEIRO", "CodEstadoId_FK", "dbo.MAT_DESC_VALOR_SISTEMA");
            DropForeignKey("dbo.MAT_ERRO_FICHEIRO", "CodFicheiroId_FK", "dbo.MAT_FICHEIRO");
            DropForeignKey("dbo.MAT_ERRO_FICHEIRO", "CodTipologiaId_FK", "dbo.MAT_DESC_VALOR_SISTEMA");
            DropForeignKey("dbo.MAT_FICHEIRO", "CodEntidadeId_FK", "dbo.MAT_ENTIDADE");
            DropForeignKey("dbo.MAT_EVENTO_STAGGING", "CodEstadoEventoId_FK", "dbo.MAT_DESC_VALOR_SISTEMA");
            DropForeignKey("dbo.MAT_ERRO_EVENTO_STAGGING", "CodEventoStaggingId_FK", "dbo.MAT_EVENTO_STAGGING");
            DropForeignKey("dbo.MAT_ERRO_EVENTO_STAGGING", "CodTipologiaId_FK", "dbo.MAT_DESC_VALOR_SISTEMA");
            DropForeignKey("dbo.MAT_AVISO", "CodEventoStaggingId_FK", "dbo.MAT_EVENTO_STAGGING");
            DropForeignKey("dbo.MAT_PERIODO_COBERTURA_HISTORICO", "CodEntidadeId_FK", "dbo.MAT_ENTIDADE");
            DropForeignKey("dbo.MAT_PERIODO_COBERTURA", "CodEntidadeId_FK", "dbo.MAT_ENTIDADE");
            DropForeignKey("dbo.MAT_PERIODO_COBERTURA_HISTORICO", "CodConcelhoCirculacaoId_FK", "dbo.MAT_CONCELHO");
            DropForeignKey("dbo.MAT_AVISO", "CodPeriodoCoberturaHistoricoId_FK", "dbo.MAT_PERIODO_COBERTURA_HISTORICO");
            DropIndex("dbo.MAT_USER_PROFILE", new[] { "EntidadeId_FK" });
            DropIndex("dbo.MAT_FICHEIRO_ISENTO", new[] { "CodEstadoId_FK" });
            DropIndex("dbo.MAT_FICHEIRO_ISENTO", new[] { "CodEntidadeId_FK" });
            DropIndex("dbo.MAT_PERIODO_COBERTURA_ISENTO", new[] { "CodFicheiroIsentosId_FK" });
            DropIndex("dbo.MAT_PERIODO_COBERTURA_ISENTO", new[] { "CodEntidadeId_FK" });
            DropIndex("dbo.MAT_PERIODO_COBERTURA", new[] { "CodVeiculoId_FK" });
            DropIndex("dbo.MAT_PERIODO_COBERTURA", new[] { "CodTomadorSeguroId_FK" });
            DropIndex("dbo.MAT_PERIODO_COBERTURA", new[] { "CodEventoHistoricoId_FK" });
            DropIndex("dbo.MAT_PERIODO_COBERTURA", new[] { "CodConcelhoCirculacaoId_FK" });
            DropIndex("dbo.MAT_AVISO", new[] { "CodPeriodoCoberturaId_FK" });
            DropIndex("dbo.MAT_AVISO", new[] { "CodTipologiaId_FK" });
            DropIndex("dbo.MAT_AVISO", new[] { "CodEventoHistoricoId_FK" });
            DropIndex("dbo.MAT_PERIODO_COBERTURA_HISTORICO", new[] { "CodVeiculoId_FK" });
            DropIndex("dbo.MAT_VEICULO", new[] { "CodCategoriaId_FK" });
            DropIndex("dbo.MAT_PERIODO_COBERTURA_HISTORICO", new[] { "CodTomadorId_FK" });
            DropIndex("dbo.MAT_PERIODO_COBERTURA_HISTORICO", new[] { "CodEventoHistoricoId_FK" });
            DropIndex("dbo.MAT_EVENTO_HISTORICO", new[] { "CodEntidadeId_FK" });
            DropIndex("dbo.MAT_EVENTO_HISTORICO", new[] { "CodOperacaoId_FK" });
            DropIndex("dbo.MAT_ENTIDADE", new[] { "CodScopeId_FK" });
            DropIndex("dbo.MAT_ENTIDADE", new[] { "CodRoleId_FK" });
            DropIndex("dbo.webpages_UsersInRoles", new[] { "RoleId" });
            DropIndex("dbo.webpages_UsersInRoles", new[] { "UserId" });
            DropIndex("dbo.webpages_OAuthMembership", new[] { "UserId" });
            DropIndex("dbo.MAT_NOTIFICACAO", new[] { "CodEntidadeId_FK" });
            DropIndex("dbo.MAT_NOTIFICACAO", new[] { "CodTipologiaId_FK" });
            DropIndex("dbo.MAT_INDICADOR", new[] { "CodEntidadeId_FK" });
            DropIndex("dbo.MAT_INDICADOR", new[] { "CodTipologiaId_FK" });
            DropIndex("dbo.MAT_EVENTO_STAGGING", new[] { "CodEntidadeId_FK" });
            DropIndex("dbo.MAT_EVENTO_STAGGING", new[] { "CodFicheiroId_FK" });
            DropIndex("dbo.MAT_FICHEIRO", new[] { "CodEstadoId_FK" });
            DropIndex("dbo.MAT_ERRO_FICHEIRO", new[] { "CodFicheiroId_FK" });
            DropIndex("dbo.MAT_ERRO_FICHEIRO", new[] { "CodTipologiaId_FK" });
            DropIndex("dbo.MAT_FICHEIRO", new[] { "CodEntidadeId_FK" });
            DropIndex("dbo.MAT_EVENTO_STAGGING", new[] { "CodEstadoEventoId_FK" });
            DropIndex("dbo.MAT_ERRO_EVENTO_STAGGING", new[] { "CodEventoStaggingId_FK" });
            DropIndex("dbo.MAT_ERRO_EVENTO_STAGGING", new[] { "CodTipologiaId_FK" });
            DropIndex("dbo.MAT_AVISO", new[] { "CodEventoStaggingId_FK" });
            DropIndex("dbo.MAT_PERIODO_COBERTURA_HISTORICO", new[] { "CodEntidadeId_FK" });
            DropIndex("dbo.MAT_PERIODO_COBERTURA", new[] { "CodEntidadeId_FK" });
            DropIndex("dbo.MAT_PERIODO_COBERTURA_HISTORICO", new[] { "CodConcelhoCirculacaoId_FK" });
            DropIndex("dbo.MAT_AVISO", new[] { "CodPeriodoCoberturaHistoricoId_FK" });
            DropTable("dbo.webpages_UsersInRoles");
            DropTable("dbo.MAT_USER_PROFILE");
            DropTable("dbo.MAT_FICHEIRO_ISENTO");
            DropTable("dbo.MAT_PERIODO_COBERTURA_ISENTO");
            DropTable("dbo.MAT_CATEGORIA");
            DropTable("dbo.MAT_VEICULO");
            DropTable("dbo.MAT_TOMADOR_SEGURO");
            DropTable("dbo.MAT_EVENTO_HISTORICO");
            DropTable("dbo.webpages_OAuthMembership");
            DropTable("dbo.webpages_Membership");
            DropTable("dbo.webpages_Roles");
            DropTable("dbo.MAT_NOTIFICACAO");
            DropTable("dbo.MAT_INDICADOR");
            DropTable("dbo.MAT_ERRO_FICHEIRO");
            DropTable("dbo.MAT_FICHEIRO");
            DropTable("dbo.MAT_DESC_VALOR_SISTEMA");
            DropTable("dbo.MAT_ERRO_EVENTO_STAGGING");
            DropTable("dbo.MAT_EVENTO_STAGGING");
            DropTable("dbo.MAT_ENTIDADE");
            DropTable("dbo.MAT_CONCELHO");
            DropTable("dbo.MAT_PERIODO_COBERTURA_HISTORICO");
            DropTable("dbo.MAT_AVISO");
            DropTable("dbo.MAT_PERIODO_COBERTURA");
            DropTable("dbo.ActionLogs");
        }
    }
}

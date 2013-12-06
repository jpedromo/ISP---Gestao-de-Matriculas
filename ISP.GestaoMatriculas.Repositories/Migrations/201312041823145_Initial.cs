namespace ISP.GestaoMatriculas.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ficheiros", "localizacao", c => c.String());
            AddColumn("dbo.Ficheiros", "dataAlteracao", c => c.DateTime(nullable: false));
            DropColumn("dbo.Apolices", "dataFimPlaneada");
            DropColumn("dbo.Apolices", "dataReporte");
            DropColumn("dbo.Ficheiros", "apagado");
            DropColumn("dbo.Ficheiros", "dataAlteração");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Ficheiros", "dataAlteração", c => c.DateTime(nullable: false));
            AddColumn("dbo.Ficheiros", "apagado", c => c.Boolean(nullable: false));
            AddColumn("dbo.Apolices", "dataReporte", c => c.DateTime(nullable: false));
            AddColumn("dbo.Apolices", "dataFimPlaneada", c => c.DateTime(nullable: false));
            DropColumn("dbo.Ficheiros", "dataAlteracao");
            DropColumn("dbo.Ficheiros", "localizacao");
        }
    }
}

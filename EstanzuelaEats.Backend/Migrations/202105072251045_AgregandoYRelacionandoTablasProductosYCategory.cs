namespace EstanzuelaEats.Backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AgregandoYRelacionandoTablasProductosYCategory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Productos", "CategoriaID", c => c.Int(nullable: false));
            AddColumn("dbo.Productos", "UserId", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Productos", "UserId");
            DropColumn("dbo.Productos", "CategoriaID");
        }
    }
}

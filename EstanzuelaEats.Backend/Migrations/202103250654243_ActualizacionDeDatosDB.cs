namespace EstanzuelaEats.Backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ActualizacionDeDatosDB : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Productos", "Existencias", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Productos", "Existencias");
        }
    }
}

namespace EstanzuelaEats.Backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class agregarCategorias : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoriaId = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 50),
                        ImagePath = c.String(),
                    })
                .PrimaryKey(t => t.CategoriaId);
            
            AddColumn("dbo.Productos", "IdCategoria", c => c.Int(nullable: false));
            AddColumn("dbo.Productos", "Category_CategoriaId", c => c.Int());
            CreateIndex("dbo.Productos", "Category_CategoriaId");
            AddForeignKey("dbo.Productos", "Category_CategoriaId", "dbo.Categories", "CategoriaId");
            DropColumn("dbo.Productos", "CategoriaID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Productos", "CategoriaID", c => c.Int(nullable: false));
            DropForeignKey("dbo.Productos", "Category_CategoriaId", "dbo.Categories");
            DropIndex("dbo.Productos", new[] { "Category_CategoriaId" });
            DropColumn("dbo.Productos", "Category_CategoriaId");
            DropColumn("dbo.Productos", "IdCategoria");
            DropTable("dbo.Categories");
        }
    }
}

namespace EstanzuelaEats.Backend.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Productos",
                c => new
                {
                    ProductoId = c.Int(nullable: false, identity: true),
                    NombreProducto = c.String(nullable: false, maxLength: 50),
                    PrecioProducto = c.Decimal(nullable: false, precision: 18, scale: 2),
                    DescripcionProducto = c.String(maxLength: 120),
                    PublicacionProducto = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.ProductoId);

        }

        public override void Down()
        {
            DropTable("dbo.Productos");
        }
    }
}

﻿namespace EstanzuelaEats.Backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AggImage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Productos", "ImagePath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Productos", "ImagePath");
        }
    }
}

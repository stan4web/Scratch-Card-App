namespace ScratchCardApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PinCodeModels",
                c => new
                    {
                        PinCodeModelId = c.Int(nullable: false, identity: true),
                        PinNumber = c.String(),
                        SerialNumber = c.String(),
                        Usage = c.Int(nullable: false),
                        EnrollmentId = c.Int(),
                        BatchNumber = c.String(),
                        StudentPin = c.String(),
                    })
                .PrimaryKey(t => t.PinCodeModelId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PinCodeModels");
        }
    }
}

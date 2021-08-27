namespace FPT_Learning_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removerelationshipbwcourseanduser : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserCourse", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserCourse", "CourseId", "dbo.Courses");
            DropIndex("dbo.UserCourse", new[] { "UserId" });
            DropIndex("dbo.UserCourse", new[] { "CourseId" });
            DropTable("dbo.UserCourse");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserCourse",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        CourseId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.CourseId });
            
            CreateIndex("dbo.UserCourse", "CourseId");
            CreateIndex("dbo.UserCourse", "UserId");
            AddForeignKey("dbo.UserCourse", "CourseId", "dbo.Courses", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserCourse", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}

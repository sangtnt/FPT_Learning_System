namespace FPT_Learning_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatecourse : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Courses", new[] { "CourseCategory_Id1" });
            DropColumn("dbo.Courses", "CourseCategory_Id");
            RenameColumn(table: "dbo.Courses", name: "CourseCategory_Id1", newName: "CourseCategory_Id");
            AddColumn("dbo.Courses", "CourseCategoryId", c => c.String());
            AlterColumn("dbo.Courses", "CourseCategory_Id", c => c.Guid());
            CreateIndex("dbo.Courses", "CourseCategory_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Courses", new[] { "CourseCategory_Id" });
            AlterColumn("dbo.Courses", "CourseCategory_Id", c => c.String());
            DropColumn("dbo.Courses", "CourseCategoryId");
            RenameColumn(table: "dbo.Courses", name: "CourseCategory_Id", newName: "CourseCategory_Id1");
            AddColumn("dbo.Courses", "CourseCategory_Id", c => c.String());
            CreateIndex("dbo.Courses", "CourseCategory_Id1");
        }
    }
}

namespace FPT_Learning_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatecoursefk : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Courses", "CourseCategory_Id", "dbo.CourseCategories");
            DropIndex("dbo.Courses", new[] { "CourseCategory_Id" });
            DropColumn("dbo.Courses", "CourseCategoryId");
            RenameColumn(table: "dbo.Courses", name: "CourseCategory_Id", newName: "CourseCategoryId");
            AlterColumn("dbo.Courses", "CourseCategoryId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Courses", "CourseCategoryId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Courses", "CourseCategoryId");
            AddForeignKey("dbo.Courses", "CourseCategoryId", "dbo.CourseCategories", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Courses", "CourseCategoryId", "dbo.CourseCategories");
            DropIndex("dbo.Courses", new[] { "CourseCategoryId" });
            AlterColumn("dbo.Courses", "CourseCategoryId", c => c.Guid());
            AlterColumn("dbo.Courses", "CourseCategoryId", c => c.String());
            RenameColumn(table: "dbo.Courses", name: "CourseCategoryId", newName: "CourseCategory_Id");
            AddColumn("dbo.Courses", "CourseCategoryId", c => c.String());
            CreateIndex("dbo.Courses", "CourseCategory_Id");
            AddForeignKey("dbo.Courses", "CourseCategory_Id", "dbo.CourseCategories", "Id");
        }
    }
}

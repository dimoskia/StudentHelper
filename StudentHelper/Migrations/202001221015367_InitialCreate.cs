namespace StudentHelper.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        Likes = c.Int(nullable: false),
                        Dislikes = c.Int(nullable: false),
                        PostId = c.Int(nullable: false),
                        UserDetails_UserDetailsId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Posts", t => t.PostId, cascadeDelete: true)
                .ForeignKey("dbo.UserDetails", t => t.UserDetails_UserDetailsId)
                .Index(t => t.PostId)
                .Index(t => t.UserDetails_UserDetailsId);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Content = c.String(),
                        Likes = c.Int(nullable: false),
                        Dislikes = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UserDetailsId = c.Int(nullable: false),
                        CourseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserDetails", t => t.UserDetailsId, cascadeDelete: true)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .Index(t => t.UserDetailsId)
                .Index(t => t.CourseId);
            
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Type = c.String(),
                        Year = c.Int(nullable: false),
                        Program = c.String(),
                        Semester = c.String(),
                        Description = c.String(),
                        DetailsUrl = c.String(),
                        ImageUrl = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Staffs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Title = c.String(),
                        DetailsUrl = c.String(),
                        ImageUrl = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        Role = c.String(),
                        Password = c.String(),
                        Confirmed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserDetails",
                c => new
                    {
                        UserDetailsId = c.Int(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        ImageUrl = c.String(),
                    })
                .PrimaryKey(t => t.UserDetailsId)
                .ForeignKey("dbo.Users", t => t.UserDetailsId)
                .Index(t => t.UserDetailsId);
            
            CreateTable(
                "dbo.Popularities",
                c => new
                    {
                        CourseId = c.Int(nullable: false),
                        UserDetailsId = c.Int(nullable: false),
                        Votes = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CourseId, t.UserDetailsId })
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .ForeignKey("dbo.UserDetails", t => t.UserDetailsId, cascadeDelete: true)
                .Index(t => t.CourseId)
                .Index(t => t.UserDetailsId);
            
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ImageData = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Assistants",
                c => new
                    {
                        CourseRefId = c.Int(nullable: false),
                        StaffRefId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CourseRefId, t.StaffRefId })
                .ForeignKey("dbo.Courses", t => t.CourseRefId, cascadeDelete: true)
                .ForeignKey("dbo.Staffs", t => t.StaffRefId, cascadeDelete: true)
                .Index(t => t.CourseRefId)
                .Index(t => t.StaffRefId);
            
            CreateTable(
                "dbo.UserCourses",
                c => new
                    {
                        User_Id = c.Int(nullable: false),
                        Course_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.Course_Id })
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.Courses", t => t.Course_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.Course_Id);
            
            CreateTable(
                "dbo.Professors",
                c => new
                    {
                        CourseRefId = c.Int(nullable: false),
                        StaffRefId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CourseRefId, t.StaffRefId })
                .ForeignKey("dbo.Courses", t => t.CourseRefId, cascadeDelete: true)
                .ForeignKey("dbo.Staffs", t => t.StaffRefId, cascadeDelete: true)
                .Index(t => t.CourseRefId)
                .Index(t => t.StaffRefId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Professors", "StaffRefId", "dbo.Staffs");
            DropForeignKey("dbo.Professors", "CourseRefId", "dbo.Courses");
            DropForeignKey("dbo.Posts", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.UserDetails", "UserDetailsId", "dbo.Users");
            DropForeignKey("dbo.Posts", "UserDetailsId", "dbo.UserDetails");
            DropForeignKey("dbo.Popularities", "UserDetailsId", "dbo.UserDetails");
            DropForeignKey("dbo.Popularities", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.Comments", "UserDetails_UserDetailsId", "dbo.UserDetails");
            DropForeignKey("dbo.UserCourses", "Course_Id", "dbo.Courses");
            DropForeignKey("dbo.UserCourses", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Assistants", "StaffRefId", "dbo.Staffs");
            DropForeignKey("dbo.Assistants", "CourseRefId", "dbo.Courses");
            DropForeignKey("dbo.Comments", "PostId", "dbo.Posts");
            DropIndex("dbo.Professors", new[] { "StaffRefId" });
            DropIndex("dbo.Professors", new[] { "CourseRefId" });
            DropIndex("dbo.UserCourses", new[] { "Course_Id" });
            DropIndex("dbo.UserCourses", new[] { "User_Id" });
            DropIndex("dbo.Assistants", new[] { "StaffRefId" });
            DropIndex("dbo.Assistants", new[] { "CourseRefId" });
            DropIndex("dbo.Popularities", new[] { "UserDetailsId" });
            DropIndex("dbo.Popularities", new[] { "CourseId" });
            DropIndex("dbo.UserDetails", new[] { "UserDetailsId" });
            DropIndex("dbo.Posts", new[] { "CourseId" });
            DropIndex("dbo.Posts", new[] { "UserDetailsId" });
            DropIndex("dbo.Comments", new[] { "UserDetails_UserDetailsId" });
            DropIndex("dbo.Comments", new[] { "PostId" });
            DropTable("dbo.Professors");
            DropTable("dbo.UserCourses");
            DropTable("dbo.Assistants");
            DropTable("dbo.Images");
            DropTable("dbo.Popularities");
            DropTable("dbo.UserDetails");
            DropTable("dbo.Users");
            DropTable("dbo.Staffs");
            DropTable("dbo.Courses");
            DropTable("dbo.Posts");
            DropTable("dbo.Comments");
        }
    }
}

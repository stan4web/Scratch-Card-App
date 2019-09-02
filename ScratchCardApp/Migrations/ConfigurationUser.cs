namespace ScratchCardApp.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Security;
    using WebMatrix.WebData;

    internal sealed class ConfigurationUser : DbMigrationsConfiguration<ScratchCardApp.Models.UsersContext>
    {
        public ConfigurationUser()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ScratchCardApp.Models.UsersContext context)
        {
            WebSecurity.InitializeDatabaseConnection(
              "ScratchcardDbContext",
              "UserProfile",
              "UserId",
              "UserName", autoCreateTables: true);

            if (!Roles.RoleExists("Admin"))
                Roles.CreateRole("Admin");

            if (!WebSecurity.UserExists("Ibestan"))
                WebSecurity.CreateUserAndAccount(
                    "Ibestan",
                    "Ibestan@123");
            if (!Roles.GetRolesForUser("Ibestan").Contains("Admin"))
                Roles.AddUsersToRoles(new[] { "Ibestan" }, new[] { "Admin" });
        }
    }
}

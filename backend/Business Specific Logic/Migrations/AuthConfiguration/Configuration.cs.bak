namespace BusinessSpecificLogic.Migrations.AuthConfiguration
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Reusable;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Reusable.Auth.AuthContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\AuthConfiguration";
        }

        protected override void Seed(Reusable.Auth.AuthContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //


            const string ADMIN = "Administrator";

            UserManager<IdentityUser> _userManager;
            _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(context));

            IdentityUser authUser = _userManager.FindByName(ADMIN);
            if (authUser == null)
            {
                authUser = new IdentityUser
                {
                    UserName = ADMIN
                };

                _userManager.Create(authUser, "admin1234");
                context.SaveChanges();
            }

            using (var ctx = new MainContext())
            {
                User user = ctx.Users.Where(u => u.UserName == ADMIN).FirstOrDefault();
                if (user == null)
                {
                    user = new User()
                    {
                        UserName = ADMIN,
                        sys_active = true,
                        Role = "Administrator",
                        Value = ADMIN
                    };
                    ctx.Users.Add(user);
                    ctx.SaveChanges();
                }
            }

        }
    }
}

namespace BusinessSpecificLogic.Migrations
{
    using EF;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Text;

    internal sealed class Configuration : DbMigrationsConfiguration<CQAContext>
    {
        public Configuration()
        {
            //if (System.Diagnostics.Debugger.IsAttached == false)
            //{
            //    System.Diagnostics.Debugger.Launch();
            //}
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(CQAContext context)
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

            context.cat_ProductLine.AddOrUpdate(
                new cat_ProductLine() { ProductLineKey = 2, Value = "Switches" },
                new cat_ProductLine() { ProductLineKey = 3, Value = "Solenoids" },
                new cat_ProductLine() { ProductLineKey = 4, Value = "Wiring" },
                new cat_ProductLine() { ProductLineKey = 5, Value = "Lighting" },
                new cat_ProductLine() { ProductLineKey = 6, Value = "Sensors" },
                new cat_ProductLine() { ProductLineKey = 7, Value = "Motors" },
                new cat_ProductLine() { ProductLineKey = 8, Value = "Actuators" }
           );

            context.cat_ConcernType.AddOrUpdate(
                new cat_ConcernType() { ConcernTypeKey = 2, Value = "Print Violation" },
                new cat_ConcernType() { ConcernTypeKey = 3, Value = "Possible Print Violation" },
                new cat_ConcernType() { ConcernTypeKey = 4, Value = "Dissatisfaction" }
            );

            try
            {
                //context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                throw new DbEntityValidationException(
                    "Entity Validation Failed - errors follow:\n" +
                    sb.ToString(), ex
                ); // Add the original exception as the innerException
            }
        }
    }
}

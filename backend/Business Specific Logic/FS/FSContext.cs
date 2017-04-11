namespace BusinessSpecificLogic.FS
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class FSContext : DbContext
    {
        public FSContext()
            : base("name=FSConn")
        {
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}

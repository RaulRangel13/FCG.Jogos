//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Infra.Catalogos
//{
//    public class CatalogoDBContext : DbContext
//    {
//        public CatalogoDBContext(DbContextOptions<CatalogoDBContext> options) : base(options) { }

//        public DbSet<Domain.Catalogos.Entities.Catalogo> Catalogo { get; set; }


//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {

//            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogoDBContext).Assembly);

//            base.OnModelCreating(modelBuilder);
//        }

//        //public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
//        //{
//        //    onBeforeSaveChanges();
//        //    return base.SaveChangesAsync(cancellationToken);
//        //}

//        //private void onBeforeSaveChanges()
//        //{
//        //}
//    }
//}

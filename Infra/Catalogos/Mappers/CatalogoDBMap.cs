//using Infra._Core;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders; // Add this using directive

//namespace Infra.Catalogos.Mappers
//{
//    public class CatalogoDBMap : BaseEntityMapper<Domain.Catalogos.Entities.Catalogo>
//    {
//        private const string tbName = "catalogo";

//        public override void setupEntity(EntityTypeBuilder<Domain.Catalogos.Entities.Catalogo> builder)
//        {
//            this.setupTable(builder);
//            this.setupFields(builder);
//            this.setupRelationships(builder);
//            this.setupIndexes(builder);
//        }

//        private void setupTable(EntityTypeBuilder<Domain.Catalogos.Entities.Catalogo> builder)
//        {
//            builder.ToTable(CatalogoDBMap.tbName);
//        }

//        private void setupFields(EntityTypeBuilder<Domain.Catalogos.Entities.Catalogo> builder)
//        {
//            builder.Property(x => x.created_at).HasColumnName("created_at").HasColumnType("timestamp without time zone");
//            builder.Property(x => x.alter_at).HasColumnName("alter_at").HasColumnType("timestamp without time zone");

//        }

//        private void setupRelationships(EntityTypeBuilder<Domain.Catalogos.Entities.Catalogo> builder)
//        {
//        }

//        private void setupIndexes(EntityTypeBuilder<Domain.Catalogos.Entities.Catalogo> builder)
//        {
//        }
//    }
//}

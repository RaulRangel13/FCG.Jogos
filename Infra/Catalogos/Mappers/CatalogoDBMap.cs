using Infra._Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders; // Add this using directive

namespace Infra.Catalogos.Mappers
{
    public class CatalogoDBMap : BaseEntityMapper<Domain.Catalogos.Entities.Catalogo>
    {
        private const string tbName = "catalogo";

        public override void setupEntity(EntityTypeBuilder<Domain.Catalogos.Entities.Catalogo> builder)
        {
            this.setupTable(builder);
            this.setupFields(builder);
            this.setupRelationships(builder);
            this.setupIndexes(builder);
        }

        private void setupTable(EntityTypeBuilder<Domain.Catalogos.Entities.Catalogo> builder)
        {
            builder.ToTable(CatalogoDBMap.tbName);
        }

        private void setupFields(EntityTypeBuilder<Domain.Catalogos.Entities.Catalogo> builder)
        {
        }

        private void setupRelationships(EntityTypeBuilder<Domain.Catalogos.Entities.Catalogo> builder)
        {
        }

        private void setupIndexes(EntityTypeBuilder<Domain.Catalogos.Entities.Catalogo> builder)
        {
        }
    }
}

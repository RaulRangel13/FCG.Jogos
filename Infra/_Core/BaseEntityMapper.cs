using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra._Core
{
    public abstract class BaseEntityMapper<TBase> : IEntityTypeConfiguration<TBase> where TBase : class
    {
        public void Configure(EntityTypeBuilder<TBase> builder)
        {

            setupEntity(builder);
        }

        public abstract void setupEntity(EntityTypeBuilder<TBase> builder);
    }
}

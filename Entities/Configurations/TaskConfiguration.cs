using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RoseAPI.Entities.Configurations
{
    public class TaskConfiguration : IEntityTypeConfiguration<Task>
    {
        public void Configure(EntityTypeBuilder<Task> builder)
        {
            builder.HasOne(c => c.Author)
                .WithMany(x => x.Tasks)
                .HasForeignKey(x => x.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}

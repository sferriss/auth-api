using Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Database.Mappings;

internal sealed class UserMapping : IEntityTypeConfiguration<User>
{
    private readonly string _schema;
    
    public UserMapping(string schema) =>
        _schema = schema;
    
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .ToTable("user", _schema);
        
        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasColumnName("id")
            .IsRequired()
            .HasDefaultValueSql("uuid_generate_v4()");
        
        builder
            .Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(50)
            .IsRequired();
        
        builder
            .Property(x => x.Email)
            .HasColumnName("email")
            .HasMaxLength(100)
            .IsRequired();
        
        builder
            .Property(x => x.Login)
            .HasColumnName("login")
            .HasMaxLength(50)
            .IsRequired();
        
        builder
            .Property(x => x.Password)
            .HasColumnName("password")
            .HasMaxLength(300)
            .IsRequired();
        
        builder
            .HasOne(x => x.Contact)
            .WithOne()
            .HasForeignKey<Contact>("user_id")
            .HasConstraintName("contact_user_id_fk");
    }
}
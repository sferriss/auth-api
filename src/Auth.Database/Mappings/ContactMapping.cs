using Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Database.Mappings;

internal sealed class ContactMapping : IEntityTypeConfiguration<Contact>
{
    private readonly string _schema;
    
    public ContactMapping(string schema) =>
        _schema = schema;
    
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder
            .ToTable("contact", _schema);
        
        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasColumnName("id")
            .IsRequired()
            .HasDefaultValueSql("uuid_generate_v4()");
        
        builder
            .Property(x => x.PhoneNumber)
            .HasColumnName("phone_number")
            .HasMaxLength(11)
            .IsRequired();
    }
}
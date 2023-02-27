using ACAI_API.Domain.Production.Basic.Person.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ACAI_API.Data.EF.Production.Basic.Person.Mapping
{
    public class PersonMapping : IEntityTypeConfiguration<PersonEntity>
    {
        public void Configure(EntityTypeBuilder<PersonEntity> builder)
        {
            builder.ToTable("ACAI_PERSON");

            builder.Property(e => e.Id)
                .HasColumnName("ID");

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("NAME");

            builder.Property(e => e.Gender)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("GENDER");

            builder.Property(e => e.Phone)
                .HasMaxLength(200)
                .HasColumnName("PHONE");

            builder.Property(e => e.CPFourCNPJ)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("CFPOURCNPJ");

            builder.Property(e => e.RGourRegistrationState)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("RGOURREGISTRATIONSTATE");

            builder.Property(e => e.Address)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("ADDRESS");

            builder.Property(e => e.Complement)
                .HasMaxLength(200)
                .HasColumnName("COMPLEMENT");

            builder.Property(e => e.Number)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("NUMBER");

            builder.Property(e => e.Cep)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("CEP");

            builder.Property(e => e.FantasyName)
                .HasMaxLength(200)
                .HasColumnName("FANTASYNAME");

            builder.Property(e => e.Situation)
                .HasColumnName("SITUATION");

            builder.Property(e => e.City)
                .IsRequired()
                .HasColumnName("CITY");

            builder.Property(e => e.BirthDate)
                .IsRequired()
                .HasColumnName("BIRTHDATE");

            builder.Property(e => e.Insercion_Date)
                .IsRequired()
                .HasColumnName("INSERCIONDATE");
        }
    }
}

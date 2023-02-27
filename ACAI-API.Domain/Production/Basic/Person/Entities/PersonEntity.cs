using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACAI_API.Domain.Production.Basic.Person.Entities
{
    public class PersonEntity : BaseEntity
    {
        public virtual string? Name { get; set; }

        public virtual string? Gender { get; set; }

        public virtual string? Phone { get; set; }

        public virtual string? CPFourCNPJ { get; set; }

        public virtual string? RGourRegistrationState { get; set; }

        public virtual string? Address { get; set; }

        public virtual string? Complement { get; set; }

        public virtual string? Number { get; set; }

        public virtual string? Cep { get; set; }

        public virtual string? FantasyName { get; set; }

        public virtual Boolean? Situation { get; set; }

        public virtual string? City { get; set; }

        public virtual DateTime? BirthDate { get; set; }
    }
}

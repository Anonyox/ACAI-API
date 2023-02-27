using ACAI_API.Domain.Production.Basic.Person.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACAI_API.Domain.Production.ConnectionTables.Employee.Entities
{
    public class EmployeeEntity : BaseEntity
    {
        public virtual string? Username { get; set; }

        public virtual string? Password { get; set; }

        public virtual string? Email { get; set; }

        public Guid? Person_Id { get; set; }

        public PersonEntity? Person { get; set; }
    }
}

using ACAI_API.Domain.Production.Basic.Person.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACAI_API.Domain.Production.ConnectionTables.Employee.DTOs
{
    public class EmployeeDto : BaseDto
    {
        public string? Username { get; set; }

        public string? Password { get; set; }

        public string? Email { get; set; }

        public Guid? Person_Id { get; set; }
    }

    public class EmployeeDetailDto : EmployeeDto
    {
        public PersonDetailDto? Person { get; set; }
    }
}

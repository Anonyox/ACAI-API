using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACAI_API.Domain.Production.ConnectionTables.Employee.Commands
{
    public class EmployeeCommand : BaseCommand
    {
        public string? Username { get; set; }

        public string? Password { get; set; }

        public string? Email { get; set; }

        public Guid? Person_Id { get; set; }


    }
}

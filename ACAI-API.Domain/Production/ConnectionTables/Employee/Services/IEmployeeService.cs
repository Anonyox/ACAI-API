using ACAI_API.Domain.Production.ConnectionTables.Employee.Entities;
using ACAI_API.Domain.Util.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACAI_API.Domain.Production.ConnectionTables.Employee.Services
{
	public interface IEmployeeService : IBaseService<EmployeeEntity>
	{
		Task<ValidateableResponse<EmployeeEntity>> AddEmployee<TCommand>(TCommand t);

		Task<ValidateableResponse<EmployeeEntity>> UpdateEmployee<TCommand>(Guid key, TCommand t);
	}
}

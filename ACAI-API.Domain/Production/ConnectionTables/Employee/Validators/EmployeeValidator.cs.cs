using ACAI_API.Domain.Production.ConnectionTables.Employee.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACAI_API.Domain.Production.ConnectionTables.Employee.Validators
{
	public class EmployeeValidator : BaseValidator<EmployeeEntity>
	{
		public EmployeeValidator()
		{
			/*RuleFor(r => r.FirstName)
				.NotEmpty()
				.WithMessage("O nome deve ser informado.")
				.MaximumLength(200)
				.WithMessage("O nome deve conter até 200 caracteres.");

			RuleFor(r => r.LastName)
				.NotEmpty()
				.MaximumLength(200);
			*/
		}
	}
}

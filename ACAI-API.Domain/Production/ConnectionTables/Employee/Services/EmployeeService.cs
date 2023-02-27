using ACAI_API.Domain.Production.ConnectionTables.Employee.Entities;
using ACAI_API.Domain.Production.ConnectionTables.Employee.Repositories;
using ACAI_API.Domain.Util.Validation;
using AutoMapper;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACAI_API.Domain.Production.ConnectionTables.Employee.Services
{
	public class EmployeeService : BaseService<EmployeeEntity>, IEmployeeService
	{
		public EmployeeService(IMapper mapper, IValidator<EmployeeEntity> validator, IEmployeeRepository repository)
			: base(mapper, validator, repository)
		{
		}

		public async Task<ValidateableResponse<EmployeeEntity>> AddEmployee<TCommand>(TCommand t)
		{
			var entity = Mapper.Map<EmployeeEntity>(t);

			var validationResult = await Validator.ValidateAsync(entity);
			if (!validationResult.IsValid)
				return new ValidateableResponse<EmployeeEntity>(validationResult);

			await Repository.AddAsync(entity);
			await Repository.ApplyChanges();

			return new ValidateableResponse<EmployeeEntity>(entity);
		}

		public async Task<ValidateableResponse<EmployeeEntity>> UpdateEmployee<TCommand>(Guid id, TCommand t)
		{
			var entity = await Repository.GetAsync(id);
			if (entity == null)
				return null;


			Mapper.Map(t, entity);

			var validationResult = await Validator.ValidateAsync(entity);
			if (!validationResult.IsValid)
				return new ValidateableResponse<EmployeeEntity>(validationResult);

			await Repository.ApplyChanges();

			return new ValidateableResponse<EmployeeEntity>(entity);
		}
	}
}

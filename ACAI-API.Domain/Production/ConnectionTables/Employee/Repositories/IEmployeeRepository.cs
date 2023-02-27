using ACAI_API.Domain.Production.ConnectionTables.Employee.DTOs;
using ACAI_API.Domain.Production.ConnectionTables.Employee.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACAI_API.Domain.Production.ConnectionTables.Employee.Repositories
{
    public interface IEmployeeRepository : IBaseRepository<EmployeeEntity>
    {
        Task<EmployeeDetailDto> GetEmployee(Guid id);

        Task<ICollection<EmployeeDetailDto>> GetEmployees();

        Task<ICollection<EmployeeDetailDto>> GetSearch(string search);




        /// <summary>
        /// Obtém um funcionário ativo atraves do <paramref name="username"/> e <paramref name="password"/>.
        /// </summary>
        /// <param name="username">Username.</param>
        /// <param name="password">Senha</param>
        /// <returns>Uma instância de uma entidade <see cref="EmployeeEntity">.</returns>
        Task<EmployeeEntity> GetEmployeeForAuthentication(string username, string password);

        Task<EmployeeDetailDto> GetEmployeeName(string name);

    }
}

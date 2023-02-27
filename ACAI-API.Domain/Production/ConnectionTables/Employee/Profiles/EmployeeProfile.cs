using ACAI_API.Domain.Production.ConnectionTables.Employee.Commands;
using ACAI_API.Domain.Production.ConnectionTables.Employee.DTOs;
using ACAI_API.Domain.Production.ConnectionTables.Employee.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACAI_API.Domain.Production.ConnectionTables.Employee.Profiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile() 
        {
            CreateMap<EmployeeCommand, EmployeeEntity>();

            CreateMap<EmployeeEntity, EmployeeDto>();

            CreateMap<EmployeeEntity, EmployeeDetailDto>();

        }
    }
}

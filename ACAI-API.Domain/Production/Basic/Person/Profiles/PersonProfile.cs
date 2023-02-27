using ACAI_API.Domain.Production.Basic.Person.Commands;
using ACAI_API.Domain.Production.Basic.Person.DTOs;
using ACAI_API.Domain.Production.Basic.Person.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACAI_API.Domain.Production.Basic.Person.Profiles
{
    public class PersonProfile : Profile
    {
        public PersonProfile()
        {
            CreateMap<PersonCommand, PersonEntity>();

            CreateMap<PersonEntity, PersonDto>();

            CreateMap<PersonEntity, PersonDetailDto>();
        }
    }
}

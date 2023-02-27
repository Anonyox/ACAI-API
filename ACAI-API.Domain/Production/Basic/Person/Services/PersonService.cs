using ACAI_API.Domain.Production.Basic.Person.Entities;
using ACAI_API.Domain.Production.Basic.Person.Repositories;
using AutoMapper;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACAI_API.Domain.Production.Basic.Person.Services
{
    public class PersonService : BaseService<PersonEntity>, IPersonService
    {
        public PersonService(IMapper mapper, IValidator<PersonEntity> validator, IPersonRepository repository)
            : base(mapper, validator, repository)
        {

        }
    }
}

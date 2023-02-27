using ACAI_API.Domain.Production.Basic.Person.DTOs;
using ACAI_API.Domain.Production.Basic.Person.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACAI_API.Domain.Production.Basic.Person.Repositories
{
    public  interface IPersonRepository : IBaseRepository<PersonEntity>
    {
        Task<PersonDetailDto> GetPerson(Guid id);

        //Obter todas as pessoa.
        Task<ICollection<PersonDetailDto>> GetPeople();
    }
}

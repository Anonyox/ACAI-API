using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ACAI_API.Data.EF;
using ACAI_API.Domain.Authorization.Model;
using ACAI_API.Domain.Production.Basic.Person.DTOs;
using ACAI_API.Domain.Production.Basic.Person.Entities;
using ACAI_API.Domain.Production.Basic.Person.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACAI_API.Data.Basic.Person.Repositories
{
	public class PersonRepositoryEntityFramework : BaseRepositoryEntityFramework<PersonEntity>, IPersonRepository
	{
		private readonly IMapper _mapper = null;
		private readonly UserPrincipal _userPrincipal;

		public PersonRepositoryEntityFramework(DbContext context, IMapper mapper, UserPrincipal userPrincipal)
			: base(context)
		{
			_mapper = mapper;
			_userPrincipal = userPrincipal;
		}
		
		//Implementação do metodo para verificar se ja existe o valor.
		public async Task<bool> ExistsCodeAsync(string value, Guid id)
		{
			return await DbSet.AnyAsync(x => x.Name == value && x.Id != id);
		}

		//Implementação do metodo de obter pessoa pelo id.
		public async Task<PersonDetailDto> GetPerson(Guid id)
		{
			var people = await DbSet
				
				
				.Include(x => x.City)
				.FirstOrDefaultAsync(x => x.Id == id);

			if (people == null)
				return null;

			var result = _mapper.Map<PersonDetailDto>(people);

			return result;
		}

		//Implementação do metodo de obter todas as pessoa.
		public async Task<ICollection<PersonDetailDto>> GetPeople()
		{
		
			var peoples = await DbSet
				.Include(x => x.City)
			
			
				.Where(x => x.Situation == true)
				.OrderBy(x => x.Name)
				.Take(50)
				.ToListAsync();

			if (peoples == null)
				return null;

			var result = _mapper.Map<ICollection<PersonDetailDto>>(peoples);

			return result;
		}



		public async Task<PersonDetailDto> GetPersonDocument(string document)
		{
			var people = await DbSet
				
			
			
				.Where(x => x.CPFourCNPJ == document)
				.FirstOrDefaultAsync();

			if (people == null)
				return null;

			var result = _mapper.Map<PersonDetailDto>(people);

			return result;
		}

        public async Task<PersonDetailDto> GetPersonName(string name)
        {
			var people = await DbSet
				.Where(x => x.Name == name)
				.FirstOrDefaultAsync();

			if (people == null)
				return null;

			var result = _mapper.Map<PersonDetailDto>(people);

			return result;
		}

    }
}

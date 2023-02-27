using System.Linq;
using AutoMapper;
using ACAI_API.Domain;
//using ACAI_API.Domain.Basic.Person.Entities;

namespace ACAI_API.Api.Util.AutoMapper
{
	public class GlobalMapping : Profile
	{
		public GlobalMapping()
		{
			/*
			var domainTypes = typeof(PersonEntity).Assembly
										.GetTypes()
										.Where(x => x.IsSubclassOf(typeof(BaseEntity)) && !x.IsAbstract)
										.ToList();

			foreach (var t in domainTypes)
			{
				CreateMap(typeof(long), t).ConvertUsing(typeof(EntityConverter<>).MakeGenericType(t));
				CreateMap(typeof(long?), t).ConvertUsing(typeof(EntityConverter<>).MakeGenericType(t));
			}
			*/
			
		}
	}
}
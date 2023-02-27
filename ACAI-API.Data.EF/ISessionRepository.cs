using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACAI_API.Data.EF
{
	public interface ISessionRepository
	{
		Task<T> LoadAsync<T>(object id);

		Task<T> GetAsync<T>(object id);
	}
}

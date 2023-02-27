using System.Data;

namespace ACAI_API.Domain.Util.Data
{
    public interface IUnitOfWork
	{
		bool IsActive { get; }

		void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

		Task RollbackAsync();

		Task CommitAsync();
	}
}

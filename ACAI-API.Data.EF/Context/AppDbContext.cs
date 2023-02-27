using ACAI_API.Domain.Util.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace ACAI_API.Data.EF.Context
{
    public class AppDbContext : DbContext, IUnitOfWork
	{
		private IDbContextTransaction? transaction = null;

		public AppDbContext()
		{
		}

		public AppDbContext(DbContextOptions options)
		   : base(options)
		{
		}

		public bool IsActive => transaction != null;

		public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
		{
			transaction = this.Database.BeginTransaction(isolationLevel);
		}

		public async Task CommitAsync()
		{
			await transaction.CommitAsync().ConfigureAwait(false);
		}

		public async Task RollbackAsync()
		{
			await transaction.RollbackAsync().ConfigureAwait(false);
			await transaction.DisposeAsync().ConfigureAwait(false);
			transaction = null;
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			SeedData.Seed(modelBuilder);

			var assembly = typeof(AppDbContext).Assembly;

			modelBuilder.ApplyConfigurationsFromAssembly(assembly);
		}
	}
}

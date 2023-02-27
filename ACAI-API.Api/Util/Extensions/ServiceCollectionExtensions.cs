using FluentValidation;
using ACAI_API.Api.Util.AutoMapper;
using ACAI_API.Api.Util.Filters;
using ACAI_API.Data;
using ACAI_API.Data.EF;
using ACAI_API.Data.EF.Context;
using ACAI_API.Domain;
using ACAI_API.Domain.Authorization.Model;
using ACAI_API.Domain.Authorization.Services;
using ACAI_API.Domain.Util.Caching;
using ACAI_API.Domain.Util.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Security.Principal;
using System.Text;


namespace ACAI_API.Api.Util.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddAutoMapper(this IServiceCollection services)
			=> services.AddAutoMapper(new[]
			   {
				   typeof(GlobalMapping).Assembly,
				   typeof(BaseEntity).Assembly
			   });

		/// <summary>
		/// Adiciona os objetos do NHibernate a instância de IServiceCollection
		/// </summary>
		/// <param name="services"></param>
		/// <param name="connectionString"></param>
		/// <param name="provider"></param>
		/// <returns></returns>
		/*public static IServiceCollection AddNHibernate(this IServiceCollection services, IConfiguration configuration)
		{
			var connectionString = configuration.GetConnectionString("Default");

			var sessionFactory = NHibernateInit.Init(connectionString, DatabaseProvider.SqlServer);

			services.AddSingleton(sessionFactory);
			services.AddScoped((provider) => sessionFactory.OpenSession());
			services.AddScoped<IUnitOfWork, NHibernateUnitOfWork>();
			services.AddScoped<AsyncTransactionActionFilter>();

			return services;
		}
		*/


		public static IServiceCollection AddEntityFramework(this IServiceCollection services, IConfiguration configuration)
		{
			var connectionString = configuration.GetConnectionString("Default");

			services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

			services.AddScoped<DbContext>(s => s.GetService<AppDbContext>());
			services.AddScoped<IUnitOfWork>(s => s.GetService<AppDbContext>());
			services.AddScoped<AsyncTransactionActionFilter>();

#if DEBUG
			HibernatingRhinos.Profiler.Appender.EntityFramework.EntityFrameworkProfiler.Initialize();
#endif

			return services;
		}

		/// <summary>
		/// Adiciona os repositórios a instância de IServiceCollection.
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public static IServiceCollection AddEntityFrameworkRepositories(this IServiceCollection services)
		{
			var abstractBaseRepository = typeof(IBaseRepository<>);
			var concretBaseRepository = typeof(BaseRepositoryEntityFramework<>);

			services.AddScoped(abstractBaseRepository, concretBaseRepository);

			var repositoryTypes = from a in abstractBaseRepository.Assembly.GetTypes()
								  from c in concretBaseRepository.Assembly.GetTypes()
								  where a.IsInterface && a.GetInterface(abstractBaseRepository.Name) != null
								  where c.IsClass && a.IsAssignableFrom(c)
								  select new
								  {
									  AbstractRepository = a,
									  ConcretRepository = c
								  };

			foreach (var repositoryType in repositoryTypes)
			{
				services.AddScoped(repositoryType.AbstractRepository, repositoryType.ConcretRepository);
			}

			return services;
		}

		/// <summary>
		/// Adiciona os repositórios a instância de IServiceCollection.
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		/*public static IServiceCollection AddNHibernateRepositories(this IServiceCollection services)
		{
			var abstractBaseRepository = typeof(IBaseRepository<>);
			var concretBaseRepository = typeof(BaseRepositoryEntityFramework<>);

			services.AddScoped(abstractBaseRepository, concretBaseRepository);
			services.AddScoped(typeof(ISessionRepository), typeof(SessionRepository));

			var repositoryTypes = from a in abstractBaseRepository.Assembly.GetTypes()
								  from c in concretBaseRepository.Assembly.GetTypes()
								  where a.IsInterface && a.GetInterface(abstractBaseRepository.Name) != null
								  where c.IsClass && a.IsAssignableFrom(c)
								  select new
								  {
									  AbstractRepository = a,
									  ConcretRepository = c
								  };

			foreach (var repositoryType in repositoryTypes)
			{
				services.AddScoped(repositoryType.AbstractRepository, repositoryType.ConcretRepository);
			}

			return services;
		}
		*/

		/// <summary>
		/// Adiciona os objetos de validação a instância de IServiceCollection.
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public static IServiceCollection AddValidators(this IServiceCollection services)
		{
			var baseEntity = typeof(BaseEntity);
			var types = baseEntity.Assembly.GetTypes();

			var validatorTypes = from e in types
								 from v in types
								 where e.IsClass && !e.IsAbstract && baseEntity.IsAssignableFrom(e)
								 where v.IsClass && typeof(BaseValidator<>).MakeGenericType(e).IsAssignableFrom(v)
								 select new
								 {
									 AbstractValidator = typeof(IValidator<>).MakeGenericType(e),
									 ConcretValidator = v
								 };

			foreach (var validatorType in validatorTypes)
			{
				services.AddScoped(validatorType.AbstractValidator, validatorType.ConcretValidator);
			}

			return services;
		}

		/// <summary>
		/// Adiciona os servicos a instância de ISeviceCollection.
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public static IServiceCollection AddServices(this IServiceCollection services)
		{
			var abstractBaseService = typeof(IBaseService<>);

			var assemblyServiceTypes = abstractBaseService.Assembly.GetTypes();

			var serviceTypes = from a in assemblyServiceTypes
							   from c in assemblyServiceTypes
							   where a.IsInterface && a.GetInterface(abstractBaseService.Name) != null
							   where c.IsClass && a.IsAssignableFrom(c)
							   select new
							   {
								   AbstractService = a,
								   ConcretService = c
							   };

			foreach (var serviceType in serviceTypes)
			{
				services.AddScoped(serviceType.AbstractService, serviceType.ConcretService);
			}

			//services.AddScoped<IDriverService, DriverService>();
			//services.AddScoped<IShiftService, ShiftService>();
			//services.AddScoped<IWorkFrontService, WorkFrontService>();
			//
			return services;
		}

		public static IServiceCollection AddCache(this IServiceCollection services)
		{
			services.AddScoped<ICache, DefaultCache>();

			return services;
		}

		/// <summary>
		/// Adiciona os serviços de autenticação a instância de IServiceCollection.
		/// </summary>
		/// <param name="services"></param>
		/// <param name="configuration"></param>
		/// <returns></returns>
		public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
		{
            #region AUTHETICATION EMPLOYEE
            // auth settings
            var authSettingsSection = configuration.GetSection("AuthSettings");
			var authSettingsClientSection = configuration.GetSection("AuthClientContactSupportResponseSettings");
			services.Configure<AuthSettings>(authSettingsSection);
		

			// recupera authSettings
			var authSettings = authSettingsSection.Get<AuthSettings>();
			

			// Configuracao Jwt
			var key = Encoding.ASCII.GetBytes(authSettings.Secret);
			

			// Adiciona a configuração de autenticação
			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				// força o http
				options.RequireHttpsMetadata = true;
				// guarda o token na instance atual da aplicação
				options.SaveToken = true;
				// parametros de validação
				options.TokenValidationParameters = new TokenValidationParameters()
				{
					ValidateIssuerSigningKey = true, // validar o emissor com a assinatura
					IssuerSigningKey = new SymmetricSecurityKey(key), // algoritmo de criptografia no token
					ValidateIssuer = true, // valida o issuer - para aceitar apenas tokens gerados pela propria api
					ValidateAudience = true, // quais dominios sao validos 
											 // ValidAudiences = use essa propriedade caso precise liberar para outras audiencias
					ValidAudience = authSettings.ValidAudiance,
					ValidIssuer = authSettings.ValidIssuer,
				};
			});

			services.AddHttpContextAccessor();
			services.AddTransient<IPrincipal>(provider => provider.GetService<IHttpContextAccessor>().HttpContext.User);
			services.AddScoped<IAuthService, AuthService>();
			

			services.AddScoped<UserPrincipal>(serviceProvider =>
			{
				var principal = serviceProvider.GetService<IPrincipal>();
				return serviceProvider.GetService<IAuthService>().GetUser(principal);
			});

			
		
		

			#endregion



			return services;

		}

		
	}




}


//services.AddScoped<IClientContactSupportResponseAuthService, ClientContactSupportResponseService>();
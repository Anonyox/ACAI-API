using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using ACAI_API.Api.Util.Extensions;
using Microsoft.IdentityModel.Logging;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Net;
using System.Reflection;
using System.IO;
using System;
using Microsoft.Extensions.PlatformAbstractions;

namespace ACAI_API.Api
{
	public class Startup
	{
		private readonly IConfiguration _configuration;

		public Startup(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers()
				.AddJsonOptions(jsonOptions =>
				{
                    // remove campos nulos no json
#pragma warning disable SYSLIB0020 // O tipo ou membro é obsoleto
					jsonOptions.JsonSerializerOptions.IgnoreNullValues = true;
#pragma warning restore SYSLIB0020 // O tipo ou membro é obsoleto

					// jsonOptions.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
				});

			services.AddCors();

			services.AddSwaggerGen(c => {
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "ACAI.Api", Version = "v1" });
			});

			services.AddAutoMapper();
			services.AddEntityFramework(_configuration);
			services.AddEntityFrameworkRepositories();
			services.AddValidators();
			services.AddServices();
			services.AddCache();

			services.AddHttpsRedirection(options =>
			{
				options.RedirectStatusCode = (int)HttpStatusCode.TemporaryRedirect;
				options.HttpsPort = 5001;
			});

			


			services.AddAuthentication(_configuration);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				IdentityModelEventSource.ShowPII = true; // for localhost issuer

				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LSGestor.Api v1"));
			}
			app.UseCors(x => x
				.AllowAnyOrigin()
				.AllowAnyHeader()
				.AllowAnyMethod()
			);
			//app.UseHttpsRedirection();
			app.UseRouting();

			app.UseRequestLocalization("en-US", "en", "pt-BR", "pt", "es");

			app.UseTokenValidation();
			app.UseAuthentication();
			app.UseAuthorization();

			app.UseCustomExceptionHandler();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
using Asp.Versioning;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http.Connections;
using Newtonsoft.Json.Converters;
using SVG.API.Application.Behaviors;
using SVG.API.Infrastructure.Filters;
using SVG.API.Infrastructure.Middleware;
using SVG.API.Infrastructure.Options;
using System.Reflection;
using SVG.API.Infrastructure.AutoMapperExtensions;

namespace SVG.API
{
    public class Startup
    {
        internal IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var mvcCoreBuilder = services.AddMvcCore();

            services.AddApiVersioning(v =>
            {
                v.AssumeDefaultVersionWhenUnspecified = true;
                v.DefaultApiVersion = new Asp.Versioning.ApiVersion(1.0);
                v.ReportApiVersions = true;
                v.ApiVersionReader = ApiVersionReader.Combine(
                    new QueryStringApiVersionReader("api-version"),
                    new HeaderApiVersionReader("x-version"),
                    new MediaTypeApiVersionReader("ver"));
            }).AddApiExplorer(v =>
            {
                v.GroupNameFormat = "'v'vvv";
                v.SubstituteApiVersionInUrl = true;
            });

            var mvcBuilder = services.AddMvc(option =>
            {
                //option.OutputFormatters.Insert(0, new PowerAppOutputFormater());

                option.EnableEndpointRouting = false;

                option.Filters.Add<CommandValidationExceptionFilter>();
            });

            mvcBuilder.AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
            });

            services.AddAutoMapper();

            services.AddOptions();
            services.Configure<FileStorageOption>(Configuration.GetSection(FileStorageOption.SectionName));

            services.AddCors(o =>
            {
                o.AddPolicy("EnableCors", x => x.SetIsOriginAllowed(origin => true)
                                                .AllowAnyMethod()
                                                .AllowAnyHeader()
                                                .AllowCredentials());
            });

            AddMediator(services);
        }

        private void AddMediator(IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseCors("EnableCors");
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
            });
        }
    }
}
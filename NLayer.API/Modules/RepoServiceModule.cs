using Autofac;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Repository;
using NLayer.Repository.Repositories;
using NLayer.Repository.UnitOfWorks;
using NLayer.Service.Mapper;
using NLayer.Service.Services;
using System.Reflection;
using Module = Autofac.Module;

namespace NLayer.API.Modules
{
    // Program.cs içinde builder.Host.ConfigureContainer ile register edilmeli her Module
    public class RepoServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(Service<>)).As(typeof(IService<>)).InstancePerLifetimeScope();

            builder.RegisterType(typeof(UnitOfWork)).As(typeof(IUnitOfWork)).InstancePerLifetimeScope();


            var apiAssembly = Assembly.GetExecutingAssembly();
            var repoAssembly = Assembly.GetAssembly(typeof(AppDbContext));
            var serviceAssembly = Assembly.GetAssembly(typeof(MapProfile));

            builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, serviceAssembly).Where(x => x.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, serviceAssembly).Where(x => x.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerLifetimeScope();

            // Autofac.InstancePerLifetimeScope > .NET.AddScoped
            // Autofac.InstancePerDependency > .NET.AddTransient

            // Scoped : 1 request başlangıçtan bitene kadar aynı instance kullanısn 
            // Transient : Herhangi bir classın ctorunda o interface nerede geçildiyse her seferinde yeni bir instance oluşturuyor

        }
    }
}

using Autofac;
using Microsoft.Extensions.Configuration;
using Zeus.Infrastructure.Configuration.Mappers;
using Zeus.Infrastructure.Configuration.Modules;

namespace Zeus.Infrastructure.Configuration
{
   public sealed class ZeusModule : Module
   {
      private readonly IConfiguration _configuration;

      public ZeusModule(IConfiguration configuration)
      {
         _configuration = configuration;
      }

      protected override void Load(ContainerBuilder builder)
      {
         RegisterMappers(builder);
         RegisterModules(builder);
      }

      private static void RegisterMappers(ContainerBuilder builder)
      {
         builder
            .RegisterInstance(MapsterConfig.Initialize())
            .SingleInstance();
      }

      private void RegisterModules(ContainerBuilder builder)
      {
         builder.RegisterModule<RepositoryModule>();
         builder.RegisterModule<MediatorModule>();
         builder.RegisterModule<ReportModule>();
         builder.RegisterModule(new SettingsModule(_configuration));
      }
   }
}

using Autofac;
using Zeus.Enums.Plcs;
using Zeus.Enums.Reports;
using Zeus.Enums.System;
using Zeus.Infrastructure.Reports.Language;
using Zeus.Infrastructure.Reports.Language.Base;
using Zeus.Infrastructure.Reports.Plcs;
using Zeus.Infrastructure.Reports.Plcs.Base;
using Zeus.Infrastructure.Reports.Types;
using Zeus.Infrastructure.Reports.Types.Base;

namespace Zeus.Infrastructure.Configuration.Modules
{
   internal sealed class ReportModule : Module
   {
      protected override void Load(ContainerBuilder builder)
      {
         RegisterLanguageProcessors(builder);
         RegisterPlcProcessors(builder);
         RegisterTypeProcessors(builder);
      }

      private static void RegisterLanguageProcessors(ContainerBuilder builder)
      {
         builder
            .RegisterType<EnglishLanguageProcessor>()
            .Keyed<ILanguageProcessor>(Language.English);

         builder
            .RegisterType<PolishLanguageProcessor>()
            .Keyed<ILanguageProcessor>(Language.Polish);
      }

      private static void RegisterPlcProcessors(ContainerBuilder builder)
      {
         builder
            .RegisterType<MeterPlcProcessor>()
            .Keyed<IPlcProcessor>(PlcType.Meter);

         builder
            .RegisterType<Rvd145PlcProcessor>()
            .Keyed<IPlcProcessor>(PlcType.Rvd145);
      }

      private static void RegisterTypeProcessors(ContainerBuilder builder)
      {
         builder
            .RegisterType<DayReportProcessor>()
            .Keyed<IReportProcessor>(ReportType.Day);

         builder
            .RegisterType<MonthReportProcessor>()
            .Keyed<IReportProcessor>(ReportType.Month);

         builder
            .RegisterType<YearReportProcessor>()
            .Keyed<IReportProcessor>(ReportType.Year);

         builder
            .RegisterType<DayOfYearReportProcessor>()
            .Keyed<IReportProcessor>(ReportType.DayOfYear);
      }
   }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mapster;
using OfficeOpenXml;
using Zeus.Domain.Plcs.Rvds;
using Zeus.Infrastructure.Reports.Plcs.Base;
using Zeus.Infrastructure.Reports.Types.Base;
using Zeus.Infrastructure.Repositories;
using Zeus.Models.Devices.Dto;
using Zeus.Models.Locations.Dto;
using Zeus.Models.Plcs.Rvds.Dto;
using Zeus.Utilities.Extensions;

namespace Zeus.Infrastructure.Reports.Plcs
{
   internal sealed class Rvd145PlcProcessor : BasePlcProcessor, IPlcProcessor
   {
      public async Task FillDataAsync(UnitOfWork uow, ExcelWorksheets sheets, DateOnly date, IReadOnlyCollection<LocationReportDto> locations, IReadOnlyCollection<DeviceReportDto> devices, IReportProcessor reportProcessor, TypeAdapterConfig mapper, CancellationToken cancellationToken)
      {
         IReadOnlyDictionary<int, Rvd145ReportDto[]> plcData = await GetPlcDataAsync<Rvd145, Rvd145ReportDto>(uow.Rvd145, date, reportProcessor, mapper, cancellationToken);
         if (plcData.Count == 0)
         {
            return;
         }

         foreach (LocationReportDto location in locations)
         {
            foreach (DeviceReportDto device in devices.Where(x => x.LocationId == location.Id))
            {
               if (!plcData.ContainsKey(device.Id))
               {
                  continue;
               }

               FillSheet(sheets[location.Name], device, plcData[device.Id], reportProcessor);
            }
         }
      }

      private static void FillSheet(ExcelWorksheet sheet, DeviceReportDto device, IReadOnlyCollection<Rvd145ReportDto> data, IReportProcessor reportProcessor)
      {
         foreach (Rvd145ReportDto rvd in data)
         {
            int rowIndex = reportProcessor.StartingPoints.Row + reportProcessor.GetDatePart(rvd.Date);

            sheet.Cells[rowIndex, reportProcessor.StartingPoints.PlcColumn + 0].Value = rvd.OutsideTempAvg.Round();
            sheet.Cells[rowIndex, reportProcessor.StartingPoints.PlcColumn + 1].Value = rvd.OutsideTempMin.Round();
            sheet.Cells[rowIndex, reportProcessor.StartingPoints.PlcColumn + 2].Value = rvd.OutsideTempMax.Round();

            sheet.Cells[rowIndex, reportProcessor.StartingPoints.PlcColumn + 3].Value = rvd.CoHighInletPresureAvg.Round();
            sheet.Cells[rowIndex, reportProcessor.StartingPoints.PlcColumn + 4].Value = rvd.CoHighInletPresureMin.Round();
            sheet.Cells[rowIndex, reportProcessor.StartingPoints.PlcColumn + 5].Value = rvd.CoHighInletPresureMax.Round();

            if (device.IsCo1)
            {
               sheet.Cells[rowIndex, reportProcessor.StartingPoints.PlcColumn + 9].Value = rvd.Co1LowInletTempAvg.Round();
               sheet.Cells[rowIndex, reportProcessor.StartingPoints.PlcColumn + 10].Value = rvd.Co1LowInletTempMin.Round();
               sheet.Cells[rowIndex, reportProcessor.StartingPoints.PlcColumn + 11].Value = rvd.Co1LowInletTempMax.Round();

               sheet.Cells[rowIndex, reportProcessor.StartingPoints.PlcColumn + 15].Value = rvd.Co1LowOutletPresureAvg.Round();
               sheet.Cells[rowIndex, reportProcessor.StartingPoints.PlcColumn + 16].Value = rvd.Co1LowOutletPresureMin.Round();
               sheet.Cells[rowIndex, reportProcessor.StartingPoints.PlcColumn + 17].Value = rvd.Co1LowOutletPresureMax.Round();
            }

            if (device.IsCwu)
            {
               sheet.Cells[rowIndex, reportProcessor.StartingPoints.PlcColumn + 27].Value = rvd.CwuTempAvg.Round();
               sheet.Cells[rowIndex, reportProcessor.StartingPoints.PlcColumn + 28].Value = rvd.CwuTempMin.Round();
               sheet.Cells[rowIndex, reportProcessor.StartingPoints.PlcColumn + 29].Value = rvd.CwuTempMax.Round();

               sheet.Cells[rowIndex, reportProcessor.StartingPoints.PlcColumn + 30].Value = rvd.CwuCirculationTempAvg.Round();
               sheet.Cells[rowIndex, reportProcessor.StartingPoints.PlcColumn + 31].Value = rvd.CwuCirculationTempMin.Round();
               sheet.Cells[rowIndex, reportProcessor.StartingPoints.PlcColumn + 32].Value = rvd.CwuCirculationTempMax.Round();
            }
         }

         int summaryRowIndex = reportProcessor.StartingPoints.Row + reportProcessor.SummaryRowOffset;

         sheet.Cells[summaryRowIndex, reportProcessor.StartingPoints.PlcColumn + 0].Value = data.Average(x => x.OutsideTempAvg).Round();
         sheet.Cells[summaryRowIndex, reportProcessor.StartingPoints.PlcColumn + 1].Value = data.Min(x => x.OutsideTempMin).Round();
         sheet.Cells[summaryRowIndex, reportProcessor.StartingPoints.PlcColumn + 2].Value = data.Max(x => x.OutsideTempMax).Round();

         sheet.Cells[summaryRowIndex, reportProcessor.StartingPoints.PlcColumn + 3].Value = data.Average(x => x.CoHighInletPresureAvg).Round();
         sheet.Cells[summaryRowIndex, reportProcessor.StartingPoints.PlcColumn + 4].Value = data.Min(x => x.CoHighInletPresureMin).Round();
         sheet.Cells[summaryRowIndex, reportProcessor.StartingPoints.PlcColumn + 5].Value = data.Max(x => x.CoHighInletPresureMax).Round();

         if (device.IsCo1)
         {
            sheet.Cells[summaryRowIndex, reportProcessor.StartingPoints.PlcColumn + 9].Value = data.Average(x => x.Co1LowInletTempAvg).Round();
            sheet.Cells[summaryRowIndex, reportProcessor.StartingPoints.PlcColumn + 10].Value = data.Min(x => x.Co1LowInletTempMin).Round();
            sheet.Cells[summaryRowIndex, reportProcessor.StartingPoints.PlcColumn + 11].Value = data.Max(x => x.Co1LowInletTempMax).Round();

            sheet.Cells[summaryRowIndex, reportProcessor.StartingPoints.PlcColumn + 15].Value = data.Average(x => x.Co1LowOutletPresureAvg).Round();
            sheet.Cells[summaryRowIndex, reportProcessor.StartingPoints.PlcColumn + 16].Value = data.Min(x => x.Co1LowOutletPresureMin).Round();
            sheet.Cells[summaryRowIndex, reportProcessor.StartingPoints.PlcColumn + 17].Value = data.Max(x => x.Co1LowOutletPresureMax).Round();
         }

         if (device.IsCwu)
         {
            sheet.Cells[summaryRowIndex, reportProcessor.StartingPoints.PlcColumn + 27].Value = data.Average(x => x.CwuTempAvg).Round();
            sheet.Cells[summaryRowIndex, reportProcessor.StartingPoints.PlcColumn + 28].Value = data.Min(x => x.CwuTempMin).Round();
            sheet.Cells[summaryRowIndex, reportProcessor.StartingPoints.PlcColumn + 29].Value = data.Max(x => x.CwuTempMax).Round();

            sheet.Cells[summaryRowIndex, reportProcessor.StartingPoints.PlcColumn + 30].Value = data.Average(x => x.CwuCirculationTempAvg).Round();
            sheet.Cells[summaryRowIndex, reportProcessor.StartingPoints.PlcColumn + 31].Value = data.Min(x => x.CwuCirculationTempMin).Round();
            sheet.Cells[summaryRowIndex, reportProcessor.StartingPoints.PlcColumn + 32].Value = data.Max(x => x.CwuCirculationTempMax).Round();
         }
      }
   }
}

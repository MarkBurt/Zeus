using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using OfficeOpenXml;
using Zeus.Infrastructure.Reports.Types.Base;
using Zeus.Infrastructure.Repositories;
using Zeus.Models.Devices.Dto;

namespace Zeus.Infrastructure.Reports.Plcs.Base
{
   internal interface IPlcProcessor
   {
      Task FillDataAsync(UnitOfWork uow, ExcelWorksheet sheet, DateOnly date, IReadOnlyCollection<DeviceReportDto> devices, IReportProcessor reportProcessor, IMapper mapper, CancellationToken cancellationToken);
   }
}

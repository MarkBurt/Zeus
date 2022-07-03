using System;

namespace Zeus.Models.Base.Dto
{
   public abstract class BasePlcDto
   {
      public int DeviceId { get; init; }
      public DateTime Date { get; init; }
      public string Name { get; init; }

      public bool IsDelayed => (DateTime.Now - Date).TotalMinutes > 15;

      public BasePlcDto()
      {
         Name = string.Empty;
      }
   }
}

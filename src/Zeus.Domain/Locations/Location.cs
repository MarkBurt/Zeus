using Zeus.Domain.Base;
using Zeus.Utilities.Helpers;

namespace Zeus.Domain.Locations
{
   public sealed class Location : BaseDomain
   {
      public string Name { get; private set; }
      public string MacAddress { get; private set; }
      public string Hostname { get; private set; }
      public string ClientVersion { get; private set; }
      public bool IncludeReport { get; private set; }

      public Location(string name, string macAddress, bool includeReport, bool isActive) : base(isActive)
      {
         Hostname = string.Empty;
         ClientVersion = string.Empty;

         Name = name;
         MacAddress = macAddress;
         IncludeReport = includeReport;
      }

      public void Update(string name, string macAddress, bool includeReport, bool isActive)
      {
         Version = RandomHelper.CreateShort();

         Name = name;
         MacAddress = macAddress;
         IncludeReport = includeReport;
         IsActive = isActive;
      }

      public void Update(string hostname, string clientVersion)
      {
         Version = RandomHelper.CreateShort();

         Hostname = hostname;
         ClientVersion = clientVersion;
      }
   }
}

using System;

namespace CPK.SharedModule.Config
{
    public static class SharedConfig
    {
        public static readonly string EnvironmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        public static bool IsProduction => EnvironmentName == "Production";

        public static EnvironmentNameEnum EnvironmentNameEnum
        {
            get
            {
                return EnvironmentName switch
                {
                    "Production" => EnvironmentNameEnum.Production,
                    "Staging" => EnvironmentNameEnum.Staging,
                    "Development" => EnvironmentNameEnum.Development,
                    _ => EnvironmentNameEnum.Development
                };
            }
        }
    }
    
    public enum EnvironmentNameEnum
    {
        Production,
        Staging,
        Development
    }
}
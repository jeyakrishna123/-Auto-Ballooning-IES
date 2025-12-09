using Microsoft.Extensions.Hosting;
using System;

namespace BubbleDrawingAutomationWeb.Models.Configuration
{
    public class AppSettings
    {
        public AppSettings()
        {
            ENVIRONMENT = "development";
            MPMConnStr = "";
            IsCrypto = "No";
            SessionTimeout = 10;
            ExtractImageFilePath = "";
            GetDrawingServiceUrlList = "";
        }

        public string ENVIRONMENT { get; set; }
        public string MPMConnStr { get; set; }
        public string IsCrypto { get; set; }
        public int SessionTimeout { get; set; } =  10;
        public string ExtractImageFilePath { get; set; }
        public string GetDrawingServiceUrlList { get; set; }
    }

    [System.AttributeUsage(System.AttributeTargets.Assembly, Inherited = false, AllowMultiple = false)]
    sealed class SpaRootAttribute : System.Attribute
    {
        public string SpaRoot { get; }
        public SpaRootAttribute(string SpaRoot)
        {
            this.SpaRoot = SpaRoot;
        }
    }
}

using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Configuration;
using BubbleDrawing.Cryptography;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using BubbleDrawingAutomationWeb.Models.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
//using ManagementFeesCalculation;
//using Microsoft.Dexterity.Applications;
/*
****************************************************************************
*
' Name      :   MsSqlConnectionInfo.cs
' Type      :   C# File
' Screen Id : 
' Arguments :
'------------------------------------------------------------------------------
' Name              Type        Description
' ----              ----        -----------
'------------------------------------------------------------------------------
' Return value :
' Called by    :
' Description  :
' Modification History :
'------------------------------------------------------------------------------
' Date                  Version             By              Reason
' ----                  -------             ---				------
' 29-Mar-2013           V1.0                Ibrahim          New
'--------------------------------------------------------------------------------
*/

namespace WebTools.DbConnectivity
{

     /// <summary>
    /// Extends Base Connectivity Info
    /// Provides information for MsSql Connection.
    /// </summary>
    public class MsSqlConnectionInfo : BaseConnectionInfo
    {
        int _timeout = 0;

        public MsSqlConnectionInfo()
            : base()
        {
            // Default MS SQL port
            if (Port == 0)
                Port = 1433;
        }
        public MsSqlConnectionInfo(int timeout)
            : base()
        {
            if (Port == 0)
                Port = 1433;
            _timeout = timeout;
        }
        public string WorkstationID
        {
            get
            {
                return workstationID;
            }
            set
            {
                workstationID = value;
            }
        }

        //public string GetConnectionString()
        //{

        //    string dada = Dynamics.Globals.SqlDataSourceName.Value;
        //    string datasourcename = DatabaseConnection.Server(dada, HKEY.LocalMachine);
        //    string connectionString = @"Data Source=" + datasourcename + ";Initial Catalog=" + Dynamics.Globals.IntercompanyId.Value + ";integrated security=true;packet size=4096;user id=sa;password=idt@123;";
        //    return connectionString;


        //    //string connectionString = ConfigurationManager.AppSettings["MyConnection"].ToString();
        //    //return connectionString;
        //}

        public string GetConnectionString()
        {
            string connectionString = string.Empty;
            string _IsCrypto = "No";
            var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", true, true);
            var config = builder.Build();
            string MPMConnStr = config.GetSection("AppSettings:MPMConnStr").Value;

            string _MPMConnStr = MPMConnStr;
            //// string IsCrypto = Convert.ToString(ConfigurationManager.AppSettings["IsCrypto"]);
            string IsCrypto = Convert.ToString(_IsCrypto);
            if (string.IsNullOrEmpty(IsCrypto) == true)
                IsCrypto = "";
            if (IsCrypto.ToLower() == "yes")
            {
                CCryptography decryptor = new CCryptFactory().GetDecryptor();
                string str = (string)null;
                //IConfiguration sToCrypt =  ConfigurationManager.AppSettings["MPMConnStr"];
                string sToCrypt = _MPMConnStr;
                ref string local = ref str;
                decryptor.Crypt(sToCrypt, out local);
                connectionString = str;
            }
            else
            {
               //// connectionString = ConfigurationManager.AppSettings["MPMConnStr"];
                connectionString = _MPMConnStr;
            }
            
            return connectionString;
        }
        public string GetConnectionString1()
        {
            StringBuilder connString = new StringBuilder();
            connString.Append("Data Source=").Append(this.HostName);
            connString.Append(";initial catalog=").Append(this.DatabaseName);
            //connString.Append(";Integrated Security=True");
            connString.Append(";user id=").Append(this.UserName);
            connString.Append(";password=").Append(this.Password);
            return connString.ToString();
        }

        private string workstationID = Environment.MachineName;
    }
}

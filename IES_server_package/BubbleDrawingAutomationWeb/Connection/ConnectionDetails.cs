using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebTools.DbConnectivity;
using Microsoft.Win32;



namespace BubbleDrawing.Connection
{
    public class ConnectionDetails
    {
        public static MsSqlConnectionInfo GetConnection()
        {
            MsSqlConnectionInfo info = new MsSqlConnectionInfo();
            return info;
        }
    }
}
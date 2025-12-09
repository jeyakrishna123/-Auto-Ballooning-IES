using System;
using System.Runtime.InteropServices;
/*
****************************************************************************
*
' Name      :   IConnectionInfo.cs
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
	/// Base class for implementing connection information
	/// </summary>
	public interface IConnectionInfo
	{

		string HostName { get; set; }
		int Port { get; set; }
		string UserName { get; set; }
		string Password { get; set; }
		string DatabaseName { get; set; }
	}
}

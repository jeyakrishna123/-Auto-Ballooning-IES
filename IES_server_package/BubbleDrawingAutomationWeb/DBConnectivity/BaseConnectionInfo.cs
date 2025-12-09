using System;
using System.Runtime.InteropServices;
/*
****************************************************************************
*
' Name      :   BaseConnectionInfo.cs
' Type      :   Class File
' Screen Id : 
' Arguments :
'------------------------------------------------------------------------------
' Name								Type			Description
' ----								----			 -----------
' BooleanColumn.cs		Class File		BooleanColumn
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
	/// Base class for for database connectivity
	/// </summary>
	// TODO: make it abstract
	public abstract class BaseConnectionInfo : IConnectionInfo
	{
		protected BaseConnectionInfo()
		{
		}

		#region IConnectionInfo Members

		public string HostName
		{
			get
			{
				return hostName;
			}
			set
			{
				hostName = value;
			}
		}

		public int Port
		{
			get
			{
				return port;
			}
			set
			{
				port = value;
			}
		}

		public string UserName
		{
			get
			{
				return userName;
			}
			set
			{
				userName = value;
			}
		}

		public string Password
		{
			get
			{
				return password;
			}
			set
			{
				password = value;
			}
		}

		public string DatabaseName
		{
			get
			{
				return databaseName;
			}
			set
			{
				databaseName = value;
			}
		}

		#endregion

		#region Private data

		private string hostName;
		private int port;
		private string userName;
		private string password;
		private string databaseName;

		#endregion
	}
}

using System;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
/*
****************************************************************************
*
' Name      :   ParameterCollection.cs
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
	/// Data type for storing name-value pairs for passing to a SQL query or a stored procedure
	/// </summary>
	[Serializable]
	public class ParameterCollection : NameObjectCollectionBase
	{	
		public ParameterCollection()
		{
		}

		/// <summary>
		/// Get the value at a specific index
		/// </summary>
		public SqlParameter this[int index]
		{	get
			{	return (SqlParameter)BaseGet(index);
			}
		}

		/// <summary>
		/// Gets or sets the value associated with the specified key.
		/// </summary>
 		public SqlParameter this[String parameterName]
		{
			get
			{
				return (SqlParameter)BaseGet(parameterName);
			}
			set
			{
				BaseSet(parameterName, value );
			}
		}

		/// <summary>
		/// Gets a String array that contains all the keys in the collection.
		/// </summary>
		public String[] AllKeys  
		{	get
			{	return( BaseGetAllKeys() );
			}
		}

		/// <summary>
		/// Gets an Object array that contains all the values in the collection.
		/// </summary>
		public Array AllValues
		{	get
			{	return( BaseGetAllValues() );
			}
		}

		/// <summary>
		/// Adds an entry to the collection.
		/// </summary>
		/// <param name="parameterName">The key</param>
		/// <param name="parameterValue">The value</param>
		public void Add(string parameterName, object parameterValue)
		{
			SqlParameter result = new SqlParameter(parameterName, parameterValue);
            //24092014---K.Velu--- Adding NULL value parameter
            if (parameterValue == null)
                result.Value = DBNull.Value;
			this.BaseAdd(parameterName, result);
		}

		/// <summary>
		/// Adds a parameter to the collection.
		/// </summary>
		/// <param name="parameterName">The key</param>
		/// <param name="dbType">The value</param>
		/// <param name="size"></param>
		/// <param name="parameterValue"></param>
		public void Add(string parameterName, SqlDbType dbType, int size, object parameterValue)
		{
			SqlParameter result = new SqlParameter(parameterName, dbType , size);
			result.Value = parameterValue;
			this.BaseAdd(parameterName, result);
		}
		

		/// <summary>
		/// Removes an entry with the specified key from the collection.
		/// </summary>
		/// <param name="parameterName">The key</param>
		public void Remove( String parameterName )  
		{
			this.BaseRemove( parameterName );
		}

		/// <summary>
		/// Removes an entry in the specified index from the collection.
		/// </summary>
		/// <param name="index">Index</param>
		public void Remove( int index )  {
			this.BaseRemoveAt( index );
		}

		/// <summary>
		/// Clears all the elements in the collection.
		/// </summary>
		public void Clear()  {
			this.BaseClear();
		}

	}
}

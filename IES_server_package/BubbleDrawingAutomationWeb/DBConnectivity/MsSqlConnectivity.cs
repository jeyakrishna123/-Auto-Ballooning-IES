using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Text;
using System.Data.OleDb;
using System.Security.Cryptography;
using System.Collections;


/*
****************************************************************************
*
' Name      :   MsSqlConnectivity.cs
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
    /// Provides access to MS SQL database and tables
    /// </summary>
    public class MsSqlConnectivity : BaseConnectivity
    {

        public MsSqlConnectivity(MsSqlConnectionInfo connectionInfo)
            : base(connectionInfo)
        {
        }

        public override void Connect()
        {
            if (IsConnected())
                return;
            MsSqlConnectionInfo connInfo = (MsSqlConnectionInfo)ConnectionInfo;
            connection = new SqlConnection(connInfo.GetConnectionString());
            connection.Open();
        }

        #region "Declarations"

        private SqlConnection conDB;
        private SqlCommand cmdDB;
        private SqlDataAdapter dapDB;
        private DataSet dstDB;
        private SqlDataReader drdDB;
        private SqlTransaction tranDB;
        private SqlParameter[] SP_OledbParameter;
        private ArrayList arlSample = new ArrayList();
        public TripleDESCryptoServiceProvider TripleDes = new TripleDESCryptoServiceProvider();
        private string strReturn;
        OleDbCommand cmdHrm;
        OleDbDataAdapter dapHrm;
        OleDbDataReader drdHrm;
        OleDbTransaction tranHrm;
        DataSet dstHrm;
        #endregion

        #region Private data

        #endregion

        public override DataSet RunQueryDataSet(string query, string tableName)
        {
            try
            {
                Connect();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = query;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds, tableName);
               
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public override DataSet RunQueryDataSet(string query, ParameterCollection parameters, string tableName)
        {
            try
            {
            Connect();
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = query;
            foreach (SqlParameter parameter in parameters.AllValues)
                cmd.Parameters.Add(parameter);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            DataSet ds = new DataSet();
            da.Fill(ds, tableName);
            Dispose();
            return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public override IDataReader RunQueryReader(string query)
        {
            try
            {
            Connect();
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = query;
            return cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public override IDataReader RunQueryReader(string query, ParameterCollection parameters)
        {
            try
            {
            Connect();
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = query;
            foreach (SqlParameter parameter in parameters.AllValues)
                cmd.Parameters.Add(parameter);
            return cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }
        public override IDataReader RunQueryReader(string query, SqlTransaction ObjTransation)  //DH:21-Sep-2006 Transaction is included
        {
            try{

            Connect();
            SqlCommand cmd = connection.CreateCommand();
            cmd.Transaction = ObjTransation;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = query;
            return cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public override IDataReader RunQueryReader(string query, ParameterCollection parameters, SqlTransaction ObjTransation)//DH:21-Sep-2006 Transaction is included
        {
            try{
            Connect();
            SqlCommand cmd = connection.CreateCommand();
            cmd.Transaction = ObjTransation;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = query;
            foreach (SqlParameter parameter in parameters.AllValues)
                cmd.Parameters.Add(parameter);
            return cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public override IDataReader RunStoredProcedureReader(string procedureName, ParameterCollection parameters)
        {
            try{
            Connect();
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = procedureName;
            cmd.CommandType = CommandType.StoredProcedure;
            foreach (SqlParameter parameter in parameters.AllValues)
                cmd.Parameters.Add(parameter);
            return cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public override IDataReader RunStoredProcedureReader(string procedureName, ParameterCollection parameters, SqlTransaction ObjTransation) //KK:13-Sep-2006
        {
            try{
            Connect();
            SqlCommand cmd = connection.CreateCommand();
            cmd.Transaction = ObjTransation;
            cmd.CommandText = procedureName;
            cmd.CommandType = CommandType.StoredProcedure;
            foreach (SqlParameter parameter in parameters.AllValues)
                cmd.Parameters.Add(parameter);
            return cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }
        public override object RunStoredProcedureDataSet(string procedureName, ParameterCollection parameters)
        {try{
            Connect();
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = procedureName;
            cmd.CommandType = CommandType.StoredProcedure;
            foreach (SqlParameter parameter in parameters.AllValues)
                cmd.Parameters.Add(parameter);
            return cmd.ExecuteScalar();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            Dispose();
        }
        }
        public override object RunStoredProcedureScalar(string procedureName, ParameterCollection parameters, SqlTransaction ObjTransation)
        {try{
            Connect();
            SqlCommand cmd = connection.CreateCommand();
            cmd.Transaction = ObjTransation;
            cmd.CommandText = procedureName;
            cmd.CommandType = CommandType.StoredProcedure;
            foreach (SqlParameter parameter in parameters.AllValues)
                cmd.Parameters.Add(parameter);
            return cmd.ExecuteScalar();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            Dispose();
        }
        }


        public override DataSet RunStoredProcedureDataSet(string procedureName, ParameterCollection parameters, string tableName)
        {
            try{
            Connect();
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = procedureName;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            foreach (SqlParameter parameter in parameters.AllValues)
                cmd.Parameters.Add(parameter);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            DataSet ds = new DataSet();
            da.Fill(ds, tableName);
            Dispose();
            return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }


        public override DataTable RunStoredProcedureDataTable(string procedureName, ParameterCollection parameters, string tableName)
        {
            try{
            Connect();
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = procedureName;
            cmd.CommandType = CommandType.StoredProcedure;
            foreach (SqlParameter parameter in parameters.AllValues)
                cmd.Parameters.Add(parameter);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            DataTable dt = new DataTable();
            da.Fill(dt);
            Dispose();
            return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }


        public override int RunNonQuery(string query)
        {
            try
            {
            Connect();
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = query;
            return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public override int RunNonQuery(string query, ParameterCollection parameters)
        {
            try
            {
            Connect();
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = query;
            foreach (SqlParameter parameter in parameters.AllValues)
                cmd.Parameters.Add(parameter);
            return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public override int RunNonQuery(string query, SqlTransaction ObjTransation)	//KK:13-Sep-2006
        {
            try
            {
            Connect();
            SqlCommand cmd = connection.CreateCommand();
            cmd.Transaction = ObjTransation;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = query;
            return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public override int RunNonQuery(string query, ParameterCollection parameters, SqlTransaction ObjTransation) //KK:13-Sep-2006
        {
            try
            {
            Connect();
            SqlCommand cmd = connection.CreateCommand();
            cmd.Transaction = ObjTransation;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = query;
            foreach (SqlParameter parameter in parameters.AllValues)
                cmd.Parameters.Add(parameter);
            return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }
        public override object RunQueryScalar(string query)
        {
            try
            {
            Connect();
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = query;
            return cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public override object RunQueryScalar(string query, ParameterCollection parameters)
        {
            try
            {
            Connect();
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = query;
            foreach (SqlParameter parameter in parameters.AllValues)
                cmd.Parameters.Add(parameter);
            return cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public override void Fill(string query, DataTable dataTable)
        {
            try
            {
                Connect();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = query;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(dataTable);
                Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public override void Fill(string query, ParameterCollection parameters, DataTable dataTable)
        {
            try
            {
            Connect();
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = query;
            foreach (SqlParameter parameter in parameters.AllValues)
                cmd.Parameters.Add(parameter);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dataTable);
            Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public override void FillStoredProcedure(string procedureName, DataTable dataTable)
        {
            try
            {
            Connect();
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = procedureName;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dataTable);
            Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public override void FillStoredProcedure(string procedureName, ParameterCollection parameters, DataTable dataTable)
        {
            try
            {
            Connect();
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = procedureName;
            foreach (SqlParameter parameter in parameters.AllValues)
                cmd.Parameters.Add(parameter);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dataTable);
            Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public void StartTransaction()
        {
            try
            {
            Connect();
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "BEGIN TRAN";
            cmd.ExecuteNonQuery();
            Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public void CommitTransaction()
        {
            try
            {
            Connect();
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "COMMIT TRAN";
            cmd.ExecuteNonQuery();
            Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public void RollbackTransaction()
        {
            try
            {
            Connect();
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "ROLLBACK TRAN";
            cmd.ExecuteNonQuery();
            Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }


        public override bool TableExists(string tableName)
        {
            bool result = false;
            string sql = "SELECT * FROM dbo.sysobjects WHERE id = object_id(@TableName)";
            ParameterCollection parameters = new ParameterCollection();
            parameters.Add("@TableName", tableName);
            DataSet ds = RunQueryDataSet(sql, parameters, "SysObjects");
            if (ds.Tables["SysObjects"].Rows.Count > 0)
                result = true;
            return result;
        }


        public override string[] GetTables()
        {
            string sql = "SELECT * FROM dbo.sysobjects WHERE OBJECTPROPERTY(id, 'IsUserTable') = 1 AND OBJECTPROPERTY(id, 'IsMSShipped') = 0";
            DataSet dsTables = RunQueryDataSet(sql, "Tables");
            DataTable dtTables = dsTables.Tables["Tables"];
            string[] result = new string[dtTables.Rows.Count];
            for (int i = 0; i < dtTables.Rows.Count; i++)
                result[i] = Convert.ToString(dtTables.Rows[i]["name"]);
            return result;
        }

        public bool IsConnected()
        {
            if (connection == null)
                return false;
            if (connection.State != ConnectionState.Open)
                return false;
            return true;
        }

        public override void Disconnect()
        {
            if (connection == null)
                return;
            try
            {
                connection.Close();
            }
            catch (SqlException)
            {	// already closed, ignoring
            }
        }

        public override IDbConnection Connection
        {
            get
            {
                Connect();
                return connection;
            }
        }

        private SqlConnection connection;

        public override void Dispose()
        {
            Disconnect();
            if (connection != null)
                connection.Dispose();
        }





        public void OutputParameter(string ParameterName, int FieldSize, object ParameterType)
        {

            if (SP_OledbParameter == null)
                Array.Resize<SqlParameter>(ref SP_OledbParameter, 1);
            else
                Array.Resize<SqlParameter>(ref SP_OledbParameter, SP_OledbParameter.Length + 1);

            SP_OledbParameter[SP_OledbParameter.Length - 1] = new SqlParameter();
            SP_OledbParameter[SP_OledbParameter.Length - 1].ParameterName = ParameterName;
            SP_OledbParameter[SP_OledbParameter.Length - 1].Size = FieldSize;
            SP_OledbParameter[SP_OledbParameter.Length - 1].SqlDbType = (SqlDbType)ParameterType;
            if (CommandType.Text.ToString().ToUpper() == "FUNCTION")
                SP_OledbParameter[SP_OledbParameter.Length - 1].Direction = ParameterDirection.ReturnValue;
            else
                SP_OledbParameter[SP_OledbParameter.Length - 1].Direction = ParameterDirection.Output;
        }
        public DataSet RunStoredProcedureDataSetSearchByStatus(string procedureName, string status, string fromDate, string toDate, string tableName)
        {
            try{
            Connect();
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = procedureName;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Status", status);
            cmd.Parameters.AddWithValue("@StartDate", fromDate);
            cmd.Parameters.AddWithValue("@EndDate", toDate);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            DataSet ds = new DataSet();
            da.Fill(ds, tableName);
            Dispose();
            return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public DataSet RunStoredProcedureDataSetSearch(string procedureName, string companyName, string fromDate, string toDate, string tableName)
        {
            try
            {
            Connect();
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = procedureName;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Text", companyName);
            cmd.Parameters.AddWithValue("@StartDate", fromDate);
            cmd.Parameters.AddWithValue("@EndDate", toDate);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            DataSet ds = new DataSet();
            da.Fill(ds, tableName);
            Dispose();
            return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }
        public DataSet RunStoredProcedureDataSetSearchDate(string procedureName, Int32 companyId, string fromDate, string toDate, string tableName)
        {
            try
            {
            Connect();
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = procedureName;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CompanyID", companyId);
            cmd.Parameters.AddWithValue("@StartDate", fromDate);
            cmd.Parameters.AddWithValue("@EndDate", toDate);
            SqlDataAdapter daa = new SqlDataAdapter();
            daa.SelectCommand = cmd;
            DataSet dss = new DataSet();
            daa.Fill(dss, tableName);
            Dispose();
            return dss;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }


        public DataSet RunStoredProcedureDataSetSearchDateWithStatus(string procedureName, string status, Int32 companyId, string fromDate, string toDate, string tableName)
        {
            try
            {
            Connect();
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = procedureName;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@status", status);
            cmd.Parameters.AddWithValue("@CompanyID", companyId);
            cmd.Parameters.AddWithValue("@StartDate", fromDate);
            cmd.Parameters.AddWithValue("@EndDate", toDate);
            SqlDataAdapter daa = new SqlDataAdapter();
            daa.SelectCommand = cmd;
            DataSet dss = new DataSet();
            daa.Fill(dss, tableName);
            Dispose();
            return dss;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }
    }
}

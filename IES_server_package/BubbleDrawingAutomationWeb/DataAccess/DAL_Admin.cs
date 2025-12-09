using BubbleDrawing.BusinessEntity;
using BubbleDrawing.Connection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebTools.DbConnectivity;

namespace BubbleDrawing.DataAccess
{
   public class DAL_Admin
    {
        MsSqlConnectivity MsSql = new MsSqlConnectivity(ConnectionDetails.GetConnection());
        
        public DataSet Insert_Update_UserAccess_Save(BEL_Admin objBEL_Admin)
        {
            ParameterCollection param = new ParameterCollection();
            try
            {
                param.Add("@UserID", objBEL_Admin.UserID);
                param.Add("@UserName", objBEL_Admin.UserName);
                param.Add("@WorkCenter", objBEL_Admin.WorkCenter);
                param.Add("@Planner", objBEL_Admin.Planner);
                param.Add("@Operator", objBEL_Admin.Operator);
                param.Add("@Quality", objBEL_Admin.Quality);
                param.Add("@Admin", objBEL_Admin.Admin);
                return MsSql.RunStoredProcedureDataSet("[dbo].[usp_Insert_Update_UserAccess]", param, "UserAccess_INS_UPD");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                MsSql.Dispose();
            }
        }
        public DataSet Fetch_GetUsers(string UseID)
        {
            try
            {
                ParameterCollection param = new ParameterCollection();
                param.Add("@USERID", UseID);
                return MsSql.RunStoredProcedureDataSet("usp_GetUsers", param, "GetUserInfo");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                MsSql.Dispose();
            }
        }
        public DataSet Fetch_GetDomainUsers(string USERID)
        {
            try
            {
                ParameterCollection param = new ParameterCollection();
                param.Add("@USERID", USERID);
                return MsSql.RunStoredProcedureDataSet("usp_GetDomainUsers", param, "GetUserDomainInfo");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                MsSql.Dispose();
            }
        }
        public DataSet Fetch_ScreenDetails(string UserID)
        {
            try
            {
                ParameterCollection param = new ParameterCollection();
                param.Add("@USERID", UserID);
                return MsSql.RunStoredProcedureDataSet("usp_GetUserScreenDetails", param, "GetUserScreenDetails");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                MsSql.Dispose();
            }
        }
        public DataSet Fetch_DeleteUsers(string UserID)
        {

            try
            {
                ParameterCollection param = new ParameterCollection();
                param.Add("@USERID", UserID);
                return MsSql.RunStoredProcedureDataSet("usp_DeleteUsers", param, "DeleteUsers");

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                MsSql.Dispose();
            }
        }
        public DataSet Fetch_LoginCredentialCheck(string UserID, string Password)
        {
            try
            {
                ParameterCollection param = new ParameterCollection();
                param.Add("@USERID", UserID);
                //param.Add("@PASSWORD", Password);
                return MsSql.RunStoredProcedureDataSet("usp_LoginCredentialCheck", param, "LoginCredentialCheck");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                MsSql.Dispose();
            }
        }

        public DataSet Fetch_ConfigutationDetails()
        {
            try
            {
                ParameterCollection param = new ParameterCollection();
                return MsSql.RunStoredProcedureDataSet("usp_FETCH_ConfigurationDetails", param, "ConfigDetails");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                MsSql.Dispose();
            }
        }

        public DataSet Fetch_WorkCenter()
        {

            try
            {
                ParameterCollection param = new ParameterCollection();

                return MsSql.RunStoredProcedureDataSet("usp_GetWorkCenter", param, "Fetch_WorkCenter");

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                MsSql.Dispose();
            }
        }

        public DataSet Check_WorkCenter(string WorkCenter)
        {

            try
            {
                ParameterCollection param = new ParameterCollection();
                param.Add("@Work_Center", WorkCenter);
                return MsSql.RunStoredProcedureDataSet("usp_WorkCenterCheck", param, "Check_WorkCenter");

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                MsSql.Dispose();
            }
        }

        public DataSet Insert_Update_SettingsConfig(string CurrentUser,DataTable dt)
        {
            ParameterCollection param = new ParameterCollection();
            try
            {
                param.Add("@ModifiedBy", CurrentUser);
                param.Add("@udt_Configuration", dt);
              
                return MsSql.RunStoredProcedureDataSet("[dbo].[usp_INS_UPD_SettingsConfig]", param, "SettingsConfig");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                MsSql.Dispose();
            }
        }

        public DataSet Insert_Update_DBConfig(string CurrentUser, DataTable dt,DataTable dtConfiguration)
        {
            ParameterCollection param = new ParameterCollection();
            try
            {
                param.Add("@ModifiedBy", CurrentUser);
                param.Add("@udt_DBConfiguration", dt);
                param.Add("@udt_Configuration", dtConfiguration);
                return MsSql.RunStoredProcedureDataSet("[dbo].[usp_INS_UPD_DBConfig]", param, "DBConfig");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                MsSql.Dispose();
            }
        }

        public DataSet Fetch_LastWorkCenter()
        {
            ParameterCollection param = new ParameterCollection();
            try
            {              
                return MsSql.RunStoredProcedureDataSet("[dbo].[usp_FETCH_LastWorkCenter]", param, "LastWorkCenter");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                MsSql.Dispose();
            }
        }

        public DataSet UPDATE_LastWorkCenter(string LastWorkCenter)
        {
            ParameterCollection param = new ParameterCollection();
            try
            {
                param.Add("@LastWorkCenter", LastWorkCenter);
                return MsSql.RunStoredProcedureDataSet("[dbo].[usp_UPDATE_LastWorkCenter]", param, "LastWorkCenter");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                MsSql.Dispose();
            }
        }

        public DataSet INSERT_UPDATE_NEWUSERS(string UserID,string UserName)
        {
            ParameterCollection param = new ParameterCollection();
            try
            {
                param.Add("@UserID", UserID);
                param.Add("@UserName", UserName);
                return MsSql.RunStoredProcedureDataSet("[dbo].[usp_INS_UPD_CreateNewUsers]", param, "CreateNewUsers");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                MsSql.Dispose();
            }
        }

        public DataSet DELETE_NEWUSERS(string UserID)
        {
            ParameterCollection param = new ParameterCollection();
            try
            {
                param.Add("@UserID", UserID);
               
                return MsSql.RunStoredProcedureDataSet("[dbo].[usp_DELETE_User]", param, "DELETE_User");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                MsSql.Dispose();
            }
        }
    }
}

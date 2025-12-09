using BubbleDrawing.BusinessEntity;
using BubbleDrawing.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleDrawing.BusinessLogic
{
 public class BLL_Admin
    {
        DAL_Admin objDAL_Admin = new DAL_Admin();

        public void Insert_Update_UserAccess_Save(BEL_Admin objBEL_Admin)
        {
            objDAL_Admin.Insert_Update_UserAccess_Save(objBEL_Admin);
        }
        public DataSet Fetch_GetUsers(string UserID)
        {
            return objDAL_Admin.Fetch_GetUsers(UserID);
        }
        public DataSet Fetch_GetDomainUsers(string USERID)
        {
            return objDAL_Admin.Fetch_GetDomainUsers(USERID);
        }
        public DataSet Fetch_ScreenDetails(string UserID)
        {
            return objDAL_Admin.Fetch_ScreenDetails(UserID);
        }
        public DataSet Fetch_DeleteUsers(string UserID)
        {
            return objDAL_Admin.Fetch_DeleteUsers(UserID);
        }
        public DataSet Fetch_LoginCredentialCheck(string UserID, string Password)
        {
            return objDAL_Admin.Fetch_LoginCredentialCheck(UserID, Password);
        }

        public DataSet Fetch_ConfigutationDetails( )
        {
            return objDAL_Admin.Fetch_ConfigutationDetails();
        }

        public DataSet Fetch_WorkCenter()
        {
            return objDAL_Admin.Fetch_WorkCenter();
        }

        public DataSet Check_WorkCenter(string WorkCenter)
        {
            return objDAL_Admin.Check_WorkCenter(WorkCenter);
        }

        public DataSet Insert_Update_SettingsConfig(string CurrentUserID,DataTable dt)
        {
            return objDAL_Admin.Insert_Update_SettingsConfig(CurrentUserID,dt);
        }

        public DataSet Insert_Update_DBConfig(string CurrentUserID, DataTable dt,DataTable dtConfiguration)
        {
            return objDAL_Admin.Insert_Update_DBConfig(CurrentUserID, dt, dtConfiguration);
        }

        public DataSet Fetch_LastWorkCenter()
        {
            return objDAL_Admin.Fetch_LastWorkCenter();
        }

        public DataSet UPDATE_LastWorkCenter(string LastWorkCenter)
        {
            return objDAL_Admin.UPDATE_LastWorkCenter(LastWorkCenter);
        }

        public DataSet INSERT_UPDATE_NEWUSERS(string UserID, string UserName)
        {
            return objDAL_Admin.INSERT_UPDATE_NEWUSERS(UserID, UserName);
        }

        public DataSet DELETE_NEWUSERS(string UserID)
        {
            return objDAL_Admin.DELETE_NEWUSERS(UserID);
        }
    }
}

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
  public  class DAL_Summary
    {
        MsSqlConnectivity MsSql = new MsSqlConnectivity(ConnectionDetails.GetConnection());
        

        public DataSet Fetch_GetPendingReviewList(string ProductionOrderNumber, string Part, string SerialNo,string Status)
        {
            ParameterCollection param = new ParameterCollection();
            try
            {
                param.Add("@ProductionOrderNumber", ProductionOrderNumber);
                param.Add("@Part", Part);
                param.Add("@SerialNo", SerialNo);
                param.Add("@Status", Status);
                return MsSql.RunStoredProcedureDataSet("usp_GetPendingReviewList", param, "GetPendingReviewList");
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


        public DataSet Fetch_InspectionApprovalReport(string DrawNum,string ProdordID,string Revision,string SerialNo,string Createdby)
        {
            ParameterCollection param = new ParameterCollection();
            try
            {
                param.Add("@DrawingNumber", DrawNum);
                param.Add("@ProductionOrderNumber", ProdordID);
                param.Add("@Revision", Revision);
                param.Add("@SerialNo", SerialNo);
                param.Add("@CreatedBy", Createdby);
                return MsSql.RunStoredProcedureDataSet("usp_report_InspectionApprovalReport", param, "GetInspectionApprovalReport");
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

        public DataSet ApprovalInspectionReport(string DrawNum, string ProdordID, string Revision, string SerialNo, string Createdby,string Folder_PDFReport,string Folder_DWNGReport,string Folder_EXCELReport,int APPREJ)
        {
            ParameterCollection param = new ParameterCollection();
            try
            {
                param.Add("@DrawingNumber", DrawNum);
                param.Add("@ProductionOrderNumber", ProdordID);
                param.Add("@Revision", Revision);
                param.Add("@SerialNo", SerialNo);
                param.Add("@CreatedBy", Createdby);
                param.Add("@Folder_PDFReport", Folder_PDFReport);
                param.Add("@Folder_DWNGReport", Folder_DWNGReport);
                param.Add("@Folder_EXCELReport", Folder_EXCELReport);
                param.Add("@APP_REJ", APPREJ);
                return MsSql.RunStoredProcedureDataSet("usp_report_ApprovalInspectionReport", param, "ApprovalInspectionReport");
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

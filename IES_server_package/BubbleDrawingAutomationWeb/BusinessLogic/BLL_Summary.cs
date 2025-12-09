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
   public class BLL_Summary
    {
        DAL_Summary objDAL_Summary = new DAL_Summary();
        public DataSet Fetch_GetPendingReviewList(string ProductionOrderNumber, string Part, string SerialNo,string Status)
        {
            try
            {
                return objDAL_Summary.Fetch_GetPendingReviewList(ProductionOrderNumber,  Part,  SerialNo, Status);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public DataSet Fetch_InspectionApprovalReport(string DrawNum, string ProdordID, string Revision, string SerialNo, string Createdby)
        {
            try
            {
                return objDAL_Summary.Fetch_InspectionApprovalReport(DrawNum, ProdordID, Revision, SerialNo, Createdby);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public DataSet ApprovalInspectionReport(string DrawNum, string ProdordID, string Revision, string SerialNo, string Createdby,string Folder_PDFReport, string Folder_DWNGReport, string Folder_EXCELReport,int APPREJ)
        {
            try
            {
                return objDAL_Summary.ApprovalInspectionReport(DrawNum, ProdordID, Revision, SerialNo, Createdby, Folder_PDFReport, Folder_DWNGReport, Folder_EXCELReport, APPREJ);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using BubbleDrawing.BusinessEntity;
using WebTools.DbConnectivity;
using BubbleDrawing.Connection;

namespace BubbleDrawing.DataAccess
{
   public class DAL_DimensionInput
    {
        MsSqlConnectivity MsSql = new MsSqlConnectivity(ConnectionDetails.GetConnection());

        public DataSet Insert_Update_DimensionInputHeaderandLiner_Save(BEL_DimensionInputHeader objBEL_DimensionInputHeader, BEL_DimensionInputLinear objBEL_DimensionInputLinear, DataTable dtBaloon_Lnr_Dimension)
        {
            ParameterCollection param = new ParameterCollection();
            try
            {
                param.Add("@DrawingNumber", objBEL_DimensionInputHeader.DrawingNumber);
                param.Add("@Total_Page_No", objBEL_DimensionInputHeader.Total_Page_No);
                param.Add("@Page_No", objBEL_DimensionInputLinear.Page_No);
                param.Add("@SerialNo", objBEL_DimensionInputLinear.SerialNo);
                param.Add("@ProductionOrderNumber", objBEL_DimensionInputLinear.ProductionOrderNumber);
                param.Add("@Revision", objBEL_DimensionInputLinear.Revision);
                param.Add("@Part", objBEL_DimensionInputHeader.Part);
                param.Add("@Batch", objBEL_DimensionInputHeader.Batch);
                param.Add("@Quantity", objBEL_DimensionInputHeader.Quantity);
                param.Add("@ConfirmationNo", objBEL_DimensionInputHeader.ConfirmationNo);
                param.Add("@Part_Revision", objBEL_DimensionInputHeader.Part_Revision);
                param.Add("@OperationNo", objBEL_DimensionInputHeader.OperationNo);
                param.Add("@WorkCenter", objBEL_DimensionInputLinear.WorkCenter);
                param.Add("@CreatedBy", objBEL_DimensionInputLinear.CreatedBy);
                param.Add("@dt_Dimension_Input_Image_Details", dtBaloon_Lnr_Dimension);
                return MsSql.RunStoredProcedureDataSet("[dbo].[usp_Dimension_Input_Header_INS_UPD]", param, "DimensionInput_Header_INS_UPD");
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
        public byte[] Fetch_DrawingBalloonImage(BEL_DimensionInputLinear objBEL_DimensionInputLinear)
        {
            try
            {
                ParameterCollection param = new ParameterCollection();
                param.Add("@BaloonDrwFileID", objBEL_DimensionInputLinear.BaloonDrwFileID);
                param.Add("@DrawingNumber", objBEL_DimensionInputLinear.DrawingNumber);
                param.Add("@Revision", objBEL_DimensionInputLinear.Revision);
                param.Add("@Page_No", objBEL_DimensionInputLinear.Page_No);
                return (byte[])MsSql.RunStoredProcedureDataSet("usp_Get_Drawing_Balloon_Image", param);
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
        public string Check_SerialNumber(BEL_DimensionInputLinear objBEL_DimensionInputLinear)
        {
            try
            {
                ParameterCollection param = new ParameterCollection();
                param.Add("@BaloonDrwFileID", objBEL_DimensionInputLinear.BaloonDrwFileID);
                param.Add("@DrawingNumber", objBEL_DimensionInputLinear.DrawingNumber);
                param.Add("@ProductionOrderNumber", objBEL_DimensionInputLinear.ProductionOrderNumber);
                param.Add("@SerialNo", objBEL_DimensionInputLinear.SerialNo);
                return Convert.ToString(MsSql.RunStoredProcedureDataSet("usp_CheckSerialNumber", param));
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
        public DataSet Fetch_DimensionInputImageDetails(BEL_DimensionInputLinear objBEL_DimensionInputLinear)
        {
            try
            {
                ParameterCollection param = new ParameterCollection();

                param.Add("@BaloonDrwFileID", objBEL_DimensionInputLinear.BaloonDrwFileID);
                param.Add("@DrawingNumber", objBEL_DimensionInputLinear.DrawingNumber);
                param.Add("@ProductionOrderNumber", objBEL_DimensionInputLinear.ProductionOrderNumber);
                param.Add("@SerialNo", objBEL_DimensionInputLinear.SerialNo);
                param.Add("@Revision", objBEL_DimensionInputLinear.Revision);
                param.Add("@Page_No", objBEL_DimensionInputLinear.Page_No);

                return MsSql.RunStoredProcedureDataSet("usp_Get_Dimension_Input_ImageDetails", param, "GetDimesionInputImage");
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
        public DataSet Fetch_CheckDimensionInput(BEL_DimensionInputLinear objBEL_DimensionInputLinear)
        {
            try
            {
                ParameterCollection param = new ParameterCollection();
                param.Add("@DrawingNumber", objBEL_DimensionInputLinear.DrawingNumber);
                param.Add("@ProductionOrderNumber", objBEL_DimensionInputLinear.ProductionOrderNumber);
                return MsSql.RunStoredProcedureDataSet("usp_Check_Dimension_Input_Page_Revision", param, "GetDimesionInputPageRevision");
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
        public DataSet Fetch_ReportInspectionReport(BEL_DimensionInputLinear objBEL_DimensionInputLinear)
        {
            try
            {
                ParameterCollection param = new ParameterCollection();
                param.Add("@DrawingNumber", objBEL_DimensionInputLinear.DrawingNumber);
                param.Add("@ProductionOrderNumber", objBEL_DimensionInputLinear.ProductionOrderNumber);
                param.Add("@Revision", objBEL_DimensionInputLinear.Revision);
                param.Add("@SerialNo", objBEL_DimensionInputLinear.SerialNo);
                return MsSql.RunStoredProcedureDataSet("usp_report_InspectionReport", param, "GetReportInspectionReport");
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

        public DataSet FetchDimensionDetails_ConfirmationNo(string connstr, string selectquery)
        {
            System.Data.DataSet ds = new DataSet();
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(connstr);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(selectquery, con);
                System.Data.SqlClient.SqlDataAdapter adap = new System.Data.SqlClient.SqlDataAdapter(cmd);
                adap.Fill(ds);
                con.Close();
                return ds;
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

        public DataSet Fetch_Comments( )
        {
            try
            {
                ParameterCollection param = new ParameterCollection();
              
                return MsSql.RunStoredProcedureDataSet("usp_FETCH_CommentsDetails", param, "CommentsDetails");
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

        public DataSet Delete_BalloonSublineItem(long DrawLineID)
        {
            try
            {
                ParameterCollection param = new ParameterCollection();

                param.Add("@DrawLineID", DrawLineID);
                return MsSql.RunStoredProcedureDataSet("[dbo].[usp_Delete_Balloon_SublineItem]", param, "Delete_BalloonSublineItem");
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

        public DataSet Add_BalloonSublineItem(DataTable dtBalloonSublineItem, string Batch, int Quantity, string OperationNo, string CurrentSerialNo)
        {
            try
            {
                ParameterCollection param = new ParameterCollection();
                param.Add("@Batch", Batch);
                param.Add("@Quantity", Quantity);
                param.Add("@OperationNo", OperationNo);
                param.Add("@CurrentSerialNo", CurrentSerialNo);
                param.Add("@dt_Dimension_Input_Image_Details", dtBalloonSublineItem);
                return MsSql.RunStoredProcedureDataSet("[dbo].[usp_Add_Balloon_SublineItem]", param, "Add_BalloonSublineItem");
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


        public DataSet Check_IsDimensionInputEntered(BEL_DimensionInputLinear objBEL_DimensionInputLinear)
        {
            try
            {
                ParameterCollection param = new ParameterCollection();

                param.Add("@BaloonDrwFileID", objBEL_DimensionInputLinear.BaloonDrwFileID);
                param.Add("@DrawingNumber", objBEL_DimensionInputLinear.DrawingNumber);
                param.Add("@ProductionOrderNumber", objBEL_DimensionInputLinear.ProductionOrderNumber);
                param.Add("@Revision", objBEL_DimensionInputLinear.Revision);
                param.Add("@Page_No", objBEL_DimensionInputLinear.Page_No);
                param.Add("@Balloon", objBEL_DimensionInputLinear.Balloon);
                return MsSql.RunStoredProcedureDataSet("[dbo].[usp_Check_IsDimensionInputEntered]", param, "Check_IsDimensionInputEntered");
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

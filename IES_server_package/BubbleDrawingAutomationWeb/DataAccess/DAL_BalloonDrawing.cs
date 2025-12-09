using BubbleDrawing.BusinessEntity;
using BubbleDrawing.Connection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebTools.DbConnectivity;
using System.Data.SqlClient;

namespace BubbleDrawing.DataAccess
{
   public class DAL_BalloonDrawing
    {
        MsSqlConnectivity MsSql = new MsSqlConnectivity(ConnectionDetails.GetConnection());
        public DataSet Insert_Update_Ballon_Axis(BEL_BalloonDrawingHeader objBalloonDrawingHeader, DataTable dtBaloon_Lnr_Draw,Int64 balloonno,string action)
        {
            ParameterCollection param = new ParameterCollection();
            try
            {
                param.Add("@DrawingNo", objBalloonDrawingHeader.DrawingNumber);
                param.Add("@RevNo", objBalloonDrawingHeader.Revision);
                param.Add("@Total_Page_No", objBalloonDrawingHeader.Total_Page_No);
                param.Add("@Page_no", objBalloonDrawingHeader.PageNo);
                param.Add("@CreatedBy", objBalloonDrawingHeader.CreatedBy);
                param.Add("@Rotate", objBalloonDrawingHeader.Rotate_properties);
                param.Add("@BallonNo", balloonno);
                param.Add("@Action", action);
                param.Add("@dt_Tbl_Balloon_Lnr", dtBaloon_Lnr_Draw);

                return MsSql.RunStoredProcedureDataSet("[dbo].[usp_INSERT_UPDATE_BalloonDetails]", param, "Tbl_Balloon_Lnr");
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
        public DataSet Fetch_DrawingBalloon(BEL_BalloonDrawingLinear objBEL_BalloonDrawingLinear)
        {
            try
            {
                ParameterCollection param = new ParameterCollection();
                param.Add("@DrawingNumber", objBEL_BalloonDrawingLinear.DrawingNumber);
                param.Add("@Revision", objBEL_BalloonDrawingLinear.Revision);
                return MsSql.RunStoredProcedureDataSet("usp_Get_Balloon_Details", param, "Balloon_Details");
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
        public DataSet Insert_Update_DrawBaloonHeaderandLiner_Save(BEL_BalloonDrawingHeader objBalloonDrawingHeader, DataTable dtBaloon_Lnr_Draw)
        {
            ParameterCollection param = new ParameterCollection();
            try
            {
                param.Add("@ProductionOrderNumber", objBalloonDrawingHeader.ProductionOrderNo);
                param.Add("@Part_Revision", objBalloonDrawingHeader.Part_Revision);
                param.Add("@PartNo", objBalloonDrawingHeader.PartNo);
                param.Add("@Total_Page_No",  objBalloonDrawingHeader.Total_Page_No);
                 param.Add("@DrawingNumber",  objBalloonDrawingHeader.DrawingNumber);
                 param.Add("@Revision",       objBalloonDrawingHeader.Revision);
                param.Add("@Page_no", objBalloonDrawingHeader.PageNo);
                param.Add("@dt_Drawing_Balloon_Image_Details", dtBaloon_Lnr_Draw);

                 return MsSql.RunStoredProcedureDataSet("[dbo].[usp_DrawBaloon_Header_INS_UPD]", param, "DrawBaloon_Header_INS_UPD");
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
        public DataSet Fetch_DrawingBalloonImage(BEL_OriginalBalloonImage objBEL_OriginalBalloonImage)
        {
            try
            {
                ParameterCollection param = new ParameterCollection();
                param.Add("@BaloonDrwFileID", objBEL_OriginalBalloonImage.BaloonDrwFileID);
                param.Add("@DrawingNumber", objBEL_OriginalBalloonImage.DrawingNumber);
                param.Add("@Revision", objBEL_OriginalBalloonImage.Revision);
                param.Add("@Page_No", objBEL_OriginalBalloonImage.Page_No);
                return MsSql.RunStoredProcedureDataSet("usp_Get_Drawing_Balloon_Image", param,"Image");
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

        public DataSet Fetch_MeasureType( )
        {
            try
            {
                ParameterCollection param = new ParameterCollection();                
                return MsSql.RunStoredProcedureDataSet("usp_FETCH_MeasureType", param, "Type");
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

        public DataSet Fetch_MeasureSubType()
        {
            try
            {
                ParameterCollection param = new ParameterCollection();
                return MsSql.RunStoredProcedureDataSet("usp_FETCH_MeasureSubType", param, "SubType");
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

        public DataSet Fetch_UOM()
        {
            try
            {
                ParameterCollection param = new ParameterCollection();
                return MsSql.RunStoredProcedureDataSet("usp_FETCH_UOM", param, "UOM");
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
        public string Fetch_GetMaxBalloonNoPagewise(BEL_BalloonDrawingLinear objBEL_BalloonDrawingLinear)
        {
            try
            {
                ParameterCollection param = new ParameterCollection();
                param.Add("@DrawingNumber", objBEL_BalloonDrawingLinear.DrawingNumber);
                param.Add("@Revision", objBEL_BalloonDrawingLinear.Revision);
                param.Add("@Page_No", objBEL_BalloonDrawingLinear.Page_No);
                return Convert.ToString(MsSql.RunStoredProcedureDataSet("usp_Get_Max_Balloon_No_Pagewise", param));
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
        
        public DataSet Fetch_DrawBalloonImageDetail(BEL_BalloonDrawingLinear objBEL_BalloonDrawingLinear)
        {
            try
            {
                ParameterCollection param = new ParameterCollection();
                param.Add("@BaloonDrwFileID", objBEL_BalloonDrawingLinear.BaloonDrwFileID);
                param.Add("@DrawingNumber", objBEL_BalloonDrawingLinear.DrawingNumber);
                param.Add("@Revision", objBEL_BalloonDrawingLinear.Revision);
                param.Add("@Page_No", objBEL_BalloonDrawingLinear.Page_No);
                return MsSql.RunStoredProcedureDataSet("usp_Get_Draw_Balloon_ImageDetails", param,"GetDrawBalloonImage");
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

        public DataSet Delete_BalloonDrawing(BEL_BalloonDrawingLinear objBEL_BalloonDrawingLinear)
        {
            try
            {
                ParameterCollection param = new ParameterCollection();
                param.Add("@BaloonDrwFileID", objBEL_BalloonDrawingLinear.BaloonDrwFileID);
                param.Add("@DrawLineID"     , objBEL_BalloonDrawingLinear.DrawLineID);
                return MsSql.RunStoredProcedureDataSet("[dbo].[usp_Delete_Balloon]", param, "Delete_Balloon");
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
        public DataSet FetchDimensionDetails_ProductionOrderNo(string connstr,string selectquery)
        {
            System.Data.DataSet ds = new DataSet();
            try
            {
               SqlConnection con = new System.Data.SqlClient.SqlConnection(connstr);
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

        public DataSet Check_IsDimensionCreateded(BEL_DimensionInputLinear objBEL_DimensionInputLinear)
        {
            try
            {
                ParameterCollection param = new ParameterCollection();

                param.Add("@BaloonDrwFileID", objBEL_DimensionInputLinear.BaloonDrwFileID);
                param.Add("@DrawingNumber", objBEL_DimensionInputLinear.DrawingNumber);
                param.Add("@ProductionOrderNumber", objBEL_DimensionInputLinear.ProductionOrderNumber);
                param.Add("@Revision", objBEL_DimensionInputLinear.Revision);
                param.Add("@Page_No", objBEL_DimensionInputLinear.Page_No);
                return MsSql.RunStoredProcedureDataSet("[dbo].[usp_Check_IsDimensionCreated]", param, "Check_IsDimensionCreateded");
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


        public DataSet Delete_BalloonDetails(BEL_DimensionInputLinear objBEL_DimensionInputLinear)
        {
            try
            {
                ParameterCollection param = new ParameterCollection();

                param.Add("@BaloonDrwFileID", objBEL_DimensionInputLinear.BaloonDrwFileID);
                param.Add("@DrawingNumber", objBEL_DimensionInputLinear.DrawingNumber);
                param.Add("@ProductionOrderNumber", objBEL_DimensionInputLinear.ProductionOrderNumber);
                param.Add("@Revision", objBEL_DimensionInputLinear.Revision);
                param.Add("@Page_No", objBEL_DimensionInputLinear.Page_No);
                return MsSql.RunStoredProcedureDataSet("[dbo].[usp_Delete_BalloonDetails]", param, "Delete_BalloonDetails");
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

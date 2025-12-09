using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using BubbleDrawing.BusinessEntity;
using BubbleDrawing.DataAccess;

namespace BubbleDrawing.BusinessLogic
{
   public class BLL_DimensionInput
    {
        DAL_DimensionInput objDAL_DimensionInput = new DAL_DimensionInput();
        public DataSet Insert_Update_DimensionInputHeaderandLiner_Save(BEL_DimensionInputHeader objBEL_DimensionInputHeader, BEL_DimensionInputLinear objBEL_DimensionInputLinear, DataTable dtBaloon_Lnr_Dimension)
        {
            try
            {
                return objDAL_DimensionInput.Insert_Update_DimensionInputHeaderandLiner_Save(objBEL_DimensionInputHeader, objBEL_DimensionInputLinear, dtBaloon_Lnr_Dimension);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Check_SerialNumber(BEL_DimensionInputLinear objBEL_DimensionInputLinear)
        {
            try
            {
                return objDAL_DimensionInput.Check_SerialNumber(objBEL_DimensionInputLinear);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public DataSet Fetch_DimensionInputImageDetails(BEL_DimensionInputLinear objBEL_DimensionInputLinear)
        {
            try
            {
                return objDAL_DimensionInput.Fetch_DimensionInputImageDetails(objBEL_DimensionInputLinear);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public byte[] Fetch_DrawingBalloonImage(BEL_DimensionInputLinear objBEL_DimensionInputLinear)
        {
            try
            {
                return objDAL_DimensionInput.Fetch_DrawingBalloonImage(objBEL_DimensionInputLinear);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public DataSet Fetch_CheckDimensionInput(BEL_DimensionInputLinear objBEL_DimensionInputLinear)
        {
            try
            {
                return objDAL_DimensionInput.Fetch_CheckDimensionInput(objBEL_DimensionInputLinear);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public DataSet Fetch_ReportInspectionReport(BEL_DimensionInputLinear objBEL_DimensionInputLinear)
        {
            try
            {
                return objDAL_DimensionInput.Fetch_ReportInspectionReport(objBEL_DimensionInputLinear);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public DataSet FetchDimensionDetails_ConfirmationNo(string connstr,string sqlquery )
        {
            try
            {
                return objDAL_DimensionInput.FetchDimensionDetails_ConfirmationNo(connstr,sqlquery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public DataSet Fetch_Comments( )
        {
            try
            {
                return objDAL_DimensionInput.Fetch_Comments();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }



        public DataSet Delete_BalloonSublineItem(long DrawLineID)
        {
            try
            {
                return objDAL_DimensionInput.Delete_BalloonSublineItem(DrawLineID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public DataSet Add_BalloonSublineItem(DataTable dtBalloonSublineItem, string Batch, int Quantity, string OperationNo, string CurrentSerialNo)
        {
            try
            {
                return objDAL_DimensionInput.Add_BalloonSublineItem(dtBalloonSublineItem, Batch, Quantity, OperationNo, CurrentSerialNo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public DataSet Check_IsDimensionInputEntered(BEL_DimensionInputLinear objBEL_DimensionInputLinear)
        {
            try
            {
                return objDAL_DimensionInput.Check_IsDimensionInputEntered(objBEL_DimensionInputLinear);
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

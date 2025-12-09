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
    public class BLL_BalloonDrawing
    {
        DAL_BalloonDrawing objDAL_BalloonDrawing = new DAL_BalloonDrawing();
        public DataSet Insert_Update_Ballon_Axis(BEL_BalloonDrawingHeader objBalloonDrawingHeader, DataTable dtBaloon_Lnr_Draw,Int64 ballonno,string action)
        {
           return objDAL_BalloonDrawing.Insert_Update_Ballon_Axis(objBalloonDrawingHeader, dtBaloon_Lnr_Draw,ballonno,action);
        }
        public DataSet Fetch_DrawingBalloon(BEL_BalloonDrawingLinear objBEL_BalloonDrawingLinear)
        {
            try
            {
                return objDAL_BalloonDrawing.Fetch_DrawingBalloon(objBEL_BalloonDrawingLinear);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public void Insert_Update_DrawBaloonHeaderandLiner_Save(BEL_BalloonDrawingHeader objBalloonDrawingHeader, DataTable dtBaloon_Lnr_Draw)
        {
            objDAL_BalloonDrawing.Insert_Update_DrawBaloonHeaderandLiner_Save(objBalloonDrawingHeader, dtBaloon_Lnr_Draw);
        }
        public DataSet Fetch_DrawingBalloonImage(BEL_OriginalBalloonImage objBEL_OriginalBalloonImage)
        {
            try
            {
                return objDAL_BalloonDrawing.Fetch_DrawingBalloonImage(objBEL_OriginalBalloonImage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public DataSet Fetch_MeasureType()
        {
            try
            {
                return objDAL_BalloonDrawing.Fetch_MeasureType();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public DataSet Fetch_MeasureSubType()
        {
            try
            {
                return objDAL_BalloonDrawing.Fetch_MeasureSubType();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public DataSet Fetch_UOM()
        {
            try
            {
                return objDAL_BalloonDrawing.Fetch_UOM();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public string Fetch_GetMaxBalloonNoPagewise(BEL_BalloonDrawingLinear objBEL_BalloonDrawingLinear)
        {
            try
            {
                return objDAL_BalloonDrawing.Fetch_GetMaxBalloonNoPagewise(objBEL_BalloonDrawingLinear);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public DataSet Fetch_DrawBalloonImageDetail(BEL_BalloonDrawingLinear objBEL_BalloonDrawingLinear)
        {
            try
            {
                return objDAL_BalloonDrawing.Fetch_DrawBalloonImageDetail(objBEL_BalloonDrawingLinear);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public DataSet Delete_BalloonDrawing(BEL_BalloonDrawingLinear objBEL_BalloonDrawingLinear)
        {
            try
            {
                return objDAL_BalloonDrawing.Delete_BalloonDrawing(objBEL_BalloonDrawingLinear);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public DataSet FetchDimensionDetails_ProductionOrderNo(string connstr,string selectquery )
        {
            try
            {
                return objDAL_BalloonDrawing.FetchDimensionDetails_ProductionOrderNo(connstr, selectquery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public DataSet Check_IsDimensionCreateded(BEL_DimensionInputLinear objBEL_DimensionInputLinear)
        {
            try
            {
                return objDAL_BalloonDrawing.Check_IsDimensionCreateded(objBEL_DimensionInputLinear);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public DataSet Delete_BalloonDetails(BEL_DimensionInputLinear objBEL_DimensionInputLinear)
        {
            try
            {
                return objDAL_BalloonDrawing.Delete_BalloonDetails(objBEL_DimensionInputLinear);
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

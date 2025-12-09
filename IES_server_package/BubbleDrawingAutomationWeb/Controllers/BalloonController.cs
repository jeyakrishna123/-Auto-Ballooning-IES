using ImageMagick;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Build.BuildEngine;
using static Azure.Core.HttpHeader;
using Tesseract;
using System.IO;
using OpenCvSharp.XImgProc;
using System.Xml.Linq;
using System.Reflection;
using BubbleDrawing.BusinessEntity;
using BubbleDrawing.BusinessLogic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using BubbleDrawingAutomationWeb.Models;
using Microsoft.EntityFrameworkCore;
using BubbleDrawing.Common;
using static log4net.Appender.ColoredConsoleAppender;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV;

namespace BubbleDrawingAutomationWeb.Controllers
{
    #region Class

    #region CreateBalloon
    public class CreateBalloon
    {
        public string drawingNo { get; set; }
        public string revNo { get; set; }
        public int pageNo { get; set; }
        public int totalPage { get; set; }
        public List<OCRResults> ballonDetails { get; set; }
        public string rotate { get; set; }
    }
    #endregion

    #region DeleteBalloon
    public class DeleteBalloon
    {
        public string drawingNo { get; set; }
        public string revNo { get; set; }
        public string pageNo { get; set; }
        public string totalPage { get; set; }
        public List<Int64> deleteItem { get; set; }
    }
    #endregion

    #endregion

    [Route("api/[controller]")]
    [ApiController]
    public class BalloonController : ControllerBase
    {
        #region Data Transform
        public static IEnumerable<object> GetValues<T>(IEnumerable<T> items, string propertyName)
        {
            Type type = typeof(T);
            var prop = type.GetProperty(propertyName);
            foreach (var item in items)
                yield return prop.GetValue(item, null);
        }
        public List<T> ConvertToList<T>(DataTable dt)
        {
            var columnNames = dt.Columns.Cast<DataColumn>()
                    .Select(c => c.ColumnName)
                    .ToList();
            var properties = typeof(T).GetProperties();
            return dt.AsEnumerable().Select(row =>
            {
                var objT = Activator.CreateInstance<T>();
                foreach (var pro in properties)
                {
                    if (columnNames.Contains(pro.Name))
                    {
                        PropertyInfo pI = objT.GetType().GetProperty(pro.Name);
                        pro.SetValue(objT, row[pro.Name] == DBNull.Value ? null : Convert.ChangeType(row[pro.Name], pI.PropertyType));
                    }
                }
                return objT;
            }).ToList();
        }
        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
        #endregion

        private readonly DimTestContext _dbcontext;
        string temp = string.Empty;
        string username = "Admin";

        public BalloonController(DimTestContext dbcontext) {
            _dbcontext = dbcontext;
        }

        #region create data
        [HttpPost]
        [Route("create")]
        public IEnumerable<object> create(CreateBalloon searchForm)
        {
            List<OCRResults> results = new List<OCRResults>();
            var obj = new List<object>();
            if (searchForm != null)
            {
                var hdr = _dbcontext.TblBaloonDrawingHeaders.Where(w => w.DrawingNumber == searchForm.drawingNo.ToString() && w.Revision == searchForm.revNo.ToString()).FirstOrDefault();

                if (hdr == null)
                {
                    var hdrtable = _dbcontext.TblBaloonDrawingHeaders;
                    TblBaloonDrawingHeader tblhdr = new TblBaloonDrawingHeader();
                    tblhdr.DrawingNumber = searchForm.drawingNo;
                    tblhdr.Revision = searchForm.revNo;
                    tblhdr.ProductionOrderNumber = "N/A";
                    tblhdr.Part_Revision = "N/A";
                    tblhdr.Total_Page_No = searchForm.totalPage;
                    //tblhdr.RotateProperties = searchForm.rotate;
                    tblhdr.CreatedDate = DateTime.Now;
                    tblhdr.ModifiedDate = DateTime.Now;
                    tblhdr.CreatedBy = username;
                    tblhdr.ModifiedBy = username;
                    hdrtable.Add(tblhdr);
                    _dbcontext.SaveChanges();
                }
                else
                {
                    var lnritems = _dbcontext.TblBaloonDrawingLiners.Where(w => w.DrawingNumber == searchForm.drawingNo.ToString() && w.Revision == searchForm.revNo.ToString()).Count();
                    if (lnritems > 0)
                    {
                        List<TblBaloonDrawingLiner> rList = _dbcontext.TblBaloonDrawingLiners.Where(w => w.DrawingNumber == searchForm.drawingNo.ToString() && w.Revision == searchForm.revNo.ToString()).ToList();
                        _dbcontext.TblBaloonDrawingLiners.RemoveRange(rList);
                        _dbcontext.SaveChanges();
                    }
                    hdr.RotateProperties = null;// searchForm.rotate;
                    _dbcontext.SaveChanges();
                    
                }
                var hdrnew = _dbcontext.TblBaloonDrawingHeaders.Where(w => w.DrawingNumber == searchForm.drawingNo.ToString() && w.Revision == searchForm.revNo.ToString()).FirstOrDefault();
                byte[] imgbyt = new byte[] { 0x20 };
                List<TblBaloonDrawingLiner> lnr = new List<TblBaloonDrawingLiner>();
                List<OCRResults> lstoCRResults = new List<OCRResults>();
                lstoCRResults = searchForm.ballonDetails;
                foreach (var i in lstoCRResults)
                {
                    Int64 hdrid = hdrnew.BaloonDrwID;
                    lnr.Add(new TblBaloonDrawingLiner
                    {
                        BaloonDrwID = hdrid,
                        BaloonDrwFileID = i.BaloonDrwFileID,
                        ProductionOrderNumber = i.ProductionOrderNumber,
                        Part_Revision = i.Part_Revision,
                        Page_No = i.Page_No,
                        DrawingNumber = i.DrawingNumber,
                        Revision = i.Revision,
                        Balloon = i.Balloon,
                        Spec = i.Spec,
                        Nominal = i.Nominal,
                        Minimum = i.Minimum,
                        Maximum = i.Maximum,
                        MeasuredBy = i.MeasuredBy,
                        MeasuredOn = i.MeasuredOn,
                        Circle_X_Axis = i.Circle_X_Axis,
                        Circle_Y_Axis = i.Circle_Y_Axis,
                        Circle_Width = i.Circle_Width,
                        Circle_Height = i.Circle_Height,
                        Balloon_Thickness = i.Balloon_Thickness,
                        Balloon_Text_FontSize = i.Balloon_Text_FontSize,
                        ZoomFactor = i.ZoomFactor,
                        Crop_X_Axis = i.Crop_X_Axis,
                        Crop_Y_Axis = i.Crop_Y_Axis,
                        Crop_Width = i.Crop_Width,
                        Crop_Height = i.Crop_Height,
                        Type = i.Type,
                        SubType = i.SubType,
                        Unit = i.Unit,
                        Quantity = i.Quantity,
                        ToleranceType = i.ToleranceType,
                        PlusTolerance = i.PlusTolerance,
                        MinusTolerance = i.MinusTolerance,
                        MinTolerance = i.MinTolerance,
                        MaxTolerance = i.MaxTolerance,
                        CropImage = imgbyt,
                        CreatedBy = i.CreatedBy,
                        CreatedDate = i.CreatedDate,
                        ModifiedBy = username,
                        ModifiedDate = DateTime.Now,
                        IsCritical = i.IsCritical
                    });

                }
                _dbcontext.TblBaloonDrawingLiners.AddRange(lnr);
                _dbcontext.SaveChanges();
                var result = _dbcontext.TblBaloonDrawingLiners
                      .Where(w => w.DrawingNumber == searchForm.drawingNo.ToString() && w.Revision == searchForm.revNo.ToString())
                      .OrderBy(f => f.DrawLineID)
                      .ToList();

                return result;

            }
            return obj;
        }
        #endregion

        #region unspecified 
        [HttpPut]
        [Route("update")]
        public IEnumerable<object> update(CreateBalloon searchForm)
        {
            var hdrnew = _dbcontext.TblBaloonDrawingHeaders.Where(w => w.DrawingNumber == searchForm.drawingNo.ToString() && w.Revision == searchForm.revNo.ToString()).FirstOrDefault();
            byte[] imgbyt = new byte[] { 0x20 };

            List<OCRResults> lstoCRResults = new List<OCRResults>();
            lstoCRResults = searchForm.ballonDetails;
            foreach (var i in lstoCRResults)
            {
                string Min, Max, Nominal, Type, SubType, Unit, ToleranceType, PlusTolerance, MinusTolerance;
                BubbleDrawing.CommonMethods cmt = new BubbleDrawing.CommonMethods();
                cmt.GetMinMaxValues(i.Spec.Trim(), out Min, out Max, out Nominal, out Type, out SubType, out Unit, out ToleranceType, out PlusTolerance, out MinusTolerance);
                int Num_Qty = 1;
                if (i.Spec.Contains("X"))
                {
                    string qty = i.Spec.Substring(0, i.Spec.IndexOf("X")).Replace(" ", "");
                    int value;
                    if (int.TryParse(qty, out value))
                        Num_Qty = Convert.ToInt16(qty);
                }
                TblBaloonDrawingLiner lnrup = _dbcontext.TblBaloonDrawingLiners.Where(f => f.DrawingNumber == i.DrawingNumber && f.Balloon == i.Balloon && f.Revision == i.Revision).FirstOrDefault();
                if (lnrup == null) throw new Exception("");
                lnrup.Spec = i.Spec;
                lnrup.Nominal = Nominal;
                lnrup.Minimum = Min;
                lnrup.Maximum = Max;
                lnrup.Type = Type;
                lnrup.SubType = SubType;
                lnrup.Unit = Unit;
                lnrup.Quantity = Num_Qty;
                lnrup.ToleranceType = ToleranceType;
                lnrup.PlusTolerance = PlusTolerance;
                lnrup.MinusTolerance = MinusTolerance;
                lnrup.MinTolerance = i.MinTolerance;
                lnrup.MaxTolerance = i.MaxTolerance;
                lnrup.ModifiedBy = i.ModifiedBy;
                lnrup.ModifiedDate = i.ModifiedDate;
                lnrup.IsCritical = i.IsCritical;
                _dbcontext.SaveChanges();
            }

            var result = _dbcontext.TblBaloonDrawingLiners
                  .Where(w => w.DrawingNumber == searchForm.drawingNo.ToString() && w.Revision == searchForm.revNo.ToString())
                  .ToList();

            return result;
        }

        [HttpDelete]
        [Route("delete")]
        public IEnumerable<object> delete(DeleteBalloon searchForm)
        {
            IEnumerable<object> obj1 = new List<object>();

            List<OCRResults> results = new List<OCRResults>();
            if (searchForm != null)
            {
                var newli = searchForm.deleteItem;
                List<string> delList = new List<string>();
                foreach (var item in newli)
                {
                    var intItem = Convert.ToInt64(item);
                    var strItem = Convert.ToString(item);
                   // delList.Add(Convert.ToString(item));
                    var ck = _dbcontext.TblBaloonDrawingLiners
                                   .Where(p => p.DrawingNumber == searchForm.drawingNo.ToString())
                                   .Where(p => p.Revision == searchForm.revNo.ToString())
                                   .Where(p => p.Balloon.Contains(intItem  + "."))
                                   .OrderBy(f => f.DrawLineID)
                                   .ToList();
                    if (ck.Count() > 0)
                    {
                        _dbcontext.TblBaloonDrawingLiners.RemoveRange(ck);
                        _dbcontext.SaveChanges();
                    }
                    else
                    {
                        var del = _dbcontext.TblBaloonDrawingLiners
                            .Where(p => p.DrawingNumber == searchForm.drawingNo.ToString())
                            .Where(p => p.Revision == searchForm.revNo.ToString())
                            .Where(d => d.Balloon == strItem).First();
                        _dbcontext.TblBaloonDrawingLiners.Remove(del);
                        _dbcontext.SaveChanges();
                    }

                }
                var oquery = _dbcontext.TblBaloonDrawingLiners
              .Where(p => p.DrawingNumber == searchForm.drawingNo.ToString() && p.Revision == searchForm.revNo.ToString()).ToList();
                var test = oquery.OrderBy(f => f.DrawLineID).Select(e => new { sl = e.Balloon.Contains(".") ? Convert.ToInt64(e.Balloon.Substring(0, e.Balloon.IndexOf("."))) : Convert.ToInt64(e.Balloon), DrawLineID = e.DrawLineID }).DistinctBy(i => i.sl).ToList();

                var oquery1 = _dbcontext.TblBaloonDrawingLiners.Where(w => w.DrawingNumber == searchForm.drawingNo.ToString()  && w.Revision == searchForm.revNo.ToString()).OrderBy(f => f.Page_No).ToList();
                if (oquery1.Count() > 0)
                {
                    Int64 j = 1;
                    foreach (var i in test.OrderBy(f => f.DrawLineID).ToList())
                    {
                        var ck = _dbcontext.TblBaloonDrawingLiners
                            .Where(p => p.DrawingNumber == searchForm.drawingNo.ToString())
                            .Where(p => p.Revision == searchForm.revNo.ToString())
                            .Where(p => p.DrawLineID == i.DrawLineID)
                            .Where(p => p.Balloon.Contains(i.sl + "."))
                            .OrderBy(f => f.DrawLineID)
                            .ToList();

                        if (ck.Count() > 0)
                        {
                            var inner = _dbcontext.TblBaloonDrawingLiners
                            .Where(p => p.DrawingNumber == searchForm.drawingNo.ToString())
                            .Where(p => p.Revision == searchForm.revNo.ToString())
                            .Where(p => p.Balloon.Contains(i.sl + "."))
                            .OrderBy(f => f.DrawLineID)
                            .ToList();
                            Int64 k = 1;
                            foreach (var o in inner)
                            {
                                TblBaloonDrawingLiner liner = _dbcontext.TblBaloonDrawingLiners.Where(f => f.DrawLineID == o.DrawLineID).FirstOrDefault();
                                if (liner == null) throw new Exception("");
                                liner.Balloon = j.ToString() + "." + k.ToString();
                                k++;
                                _dbcontext.SaveChanges();

                            }
                        }
                        else
                        {
                            TblBaloonDrawingLiner liner = _dbcontext.TblBaloonDrawingLiners.Where(f => f.DrawLineID == i.DrawLineID).FirstOrDefault();
                            if (liner == null) throw new Exception("");
                            liner.Balloon = j.ToString();
                        }
                        j++;

                        _dbcontext.SaveChanges();

                    }
                   
                }
                /*
                  var searchIds = delList;
                  var result = _dbcontext.TblBaloonDrawingLiners.Where(p => searchIds.Contains(p.Balloon));
                var oquery = _dbcontext.TblBaloonDrawingLiners.Where(w => w.DrawingNumber == searchForm.drawingNo.ToString() && w.Revision == searchForm.revNo.ToString());
                if (oquery.Count() > 0)
                {
                    Int64 j = 1;
                    foreach (var i in oquery.OrderBy(f => f.Page_No).ToList())
                    {
                        TblBaloonDrawingLiner liner = _dbcontext.TblBaloonDrawingLiners.Where(f => f.Balloon == i.Balloon).FirstOrDefault();
                        if (liner == null) throw new Exception("");
                        liner.Balloon = j.ToString();
                        _dbcontext.SaveChanges();
                        j++;
                    }
                }
               */            }
            obj1 = get(searchForm.drawingNo, searchForm.revNo);
            return obj1;
        }

        [HttpGet]
        [Route("get")]
        public IEnumerable<object> get(string drawno,string revno)
        {
            List<OCRResults> results = new List<OCRResults>();
            if (drawno != "" && revno!="")
            {
                var result = _dbcontext.TblBaloonDrawingLiners
                   .Where(w => w.DrawingNumber == drawno.ToString() && w.Revision == revno.ToString())
                   .ToList();

                return result;
            }
            return results;
        }

        [HttpPut]
        [Route("reOrder")]
        public IEnumerable<object> reOrder(ResetBalloon searchForm)
        {
            IEnumerable<object> obj1 = new List<object>();
            var query = _dbcontext.TblBaloonDrawingLiners.Where(w => w.DrawingNumber == searchForm.CdrawingNo.ToString() && w.Page_No == searchForm.pageNo && w.Revision == searchForm.CrevNo.ToString()).Count();
            if (query > 0)
            {
                List<TblBaloonDrawingLiner> rList = _dbcontext.TblBaloonDrawingLiners.Where(w => w.DrawingNumber == searchForm.CdrawingNo.ToString() && w.Page_No == searchForm.pageNo && w.Revision == searchForm.CrevNo.ToString()).ToList();
                _dbcontext.TblBaloonDrawingLiners.RemoveRange(rList);
                _dbcontext.SaveChanges();

            }
            var hdrnew = _dbcontext.TblBaloonDrawingHeaders.Where(w => w.DrawingNumber == searchForm.CdrawingNo.ToString() && w.Revision == searchForm.CrevNo.ToString()).FirstOrDefault();
            byte[] imgbyt = new byte[] { 0x20 };
            List<TblBaloonDrawingLiner> lnr = new List<TblBaloonDrawingLiner>();
            List<OCRResults> lstoCRResults = new List<OCRResults>();
            lstoCRResults = searchForm.originalRegions;
            foreach (var i in lstoCRResults)
            {
                Int64 hdrid = hdrnew.BaloonDrwID;
                lnr.Add(new TblBaloonDrawingLiner
                {
                    BaloonDrwID = hdrid,
                    BaloonDrwFileID = i.BaloonDrwFileID,
                    ProductionOrderNumber = i.ProductionOrderNumber,
                    Part_Revision = i.Part_Revision,
                    Page_No = i.Page_No,
                    DrawingNumber = i.DrawingNumber,
                    Revision = i.Revision,
                    Balloon = i.Balloon,
                    Spec = i.Spec,
                    Nominal = i.Nominal,
                    Minimum = i.Minimum,
                    Maximum = i.Maximum,
                    MeasuredBy = i.MeasuredBy,
                    MeasuredOn = i.MeasuredOn,
                    Circle_X_Axis = i.Circle_X_Axis,
                    Circle_Y_Axis = i.Crop_Y_Axis,
                    Circle_Width = i.Circle_Width,
                    Circle_Height = i.Circle_Height,
                    Balloon_Thickness = i.Balloon_Thickness,
                    Balloon_Text_FontSize = i.Balloon_Text_FontSize,
                    ZoomFactor = i.ZoomFactor,
                    Crop_X_Axis = i.Crop_X_Axis,
                    Crop_Y_Axis = i.Crop_Y_Axis,
                    Crop_Width = i.Crop_Width,
                    Crop_Height = i.Crop_Height,
                    Type = i.Type,
                    SubType = i.SubType,
                    Unit = i.Unit,
                    Quantity = i.Quantity,
                    ToleranceType = i.ToleranceType,
                    PlusTolerance = i.PlusTolerance,
                    MinusTolerance = i.MinusTolerance,
                    MinTolerance = i.MinTolerance,
                    MaxTolerance = i.MaxTolerance,
                    CropImage = imgbyt,
                    CreatedBy = i.CreatedBy,
                    CreatedDate = i.CreatedDate,
                    ModifiedBy = i.ModifiedBy,
                    ModifiedDate = i.ModifiedDate,
                    IsCritical = i.IsCritical
                });

            }
            _dbcontext.TblBaloonDrawingLiners.AddRange(lnr);
            _dbcontext.SaveChanges();

            obj1 = get(searchForm.CdrawingNo, searchForm.CrevNo);
            return obj1;
        }
        #endregion
    }
}



 

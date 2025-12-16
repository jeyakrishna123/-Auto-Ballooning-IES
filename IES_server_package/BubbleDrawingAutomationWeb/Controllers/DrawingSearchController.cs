using BubbleDrawing.BusinessLogic;
using BubbleDrawing.Cryptography;
using BubbleDrawingAutomationWeb.Models;
using BubbleDrawingAutomationWeb.Models.Configuration;
using Emgu.CV;
using Emgu.CV.CvEnum;
using ImageMagick;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenCvSharp;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Tesseract;
using static System.Net.Mime.MediaTypeNames;
using BubbleCommon = BubbleDrawing.Common;

namespace BubbleDrawingAutomationWeb.Controllers
{
    #region class 

    #region KeyValuesClass
    public class KeyValuesClass
    {
        private string a_key;
        private string a_value;
        public KeyValuesClass(string a_key, string a_value)
        {
            this.a_key = a_key;
            this.a_value = a_value;
        }
        public string Key
        {
            get { return a_key; }
            set { a_key = value; }
        }
        public string Value
        {
            get { return a_value; }
            set { a_value = value; }
        }
    }
    #endregion

    #region SearchForm
    public class SearchForm
    {
        public string drawingNo { get; set; }
        public string revNo { get; set; }
        public string baseUrl { get; set; }
        public string sessionUserId { get; set; }
    }
    #endregion

    #region AutoBalloon
    public class AutoBalloon
    {
        public string CdrawingNo { get; set; }
        public double aspectRatio { get; set; }
        public double bgImgW { get; set; }
        public double bgImgH { get; set; }
        public double bgImgX { get; set; }
        public double bgImgY { get; set; }
        public string selectedRegion { get; set; }
        public List<OCRResults> drawingRegions { get; set; }
        public List<OCRResults> balloonRegions { get; set; }
        public List<OCRResults> originalRegions { get; set; }
        public List<OCRResults> annotation { get; set; }
        public string rotate { get; set; }
        public int bgImgRotation { get; set; }
        public string drawingDetails { get; set; }
        public int ItemView { get; set; }
        public string CrevNo { get; set; }
        public int pageNo { get; set; }
        public int totalPage { get; set; }
        public List<Origin> origin { get; set; }
    }
    public class Origin
    {
        public int count { get; set; }
        public int fullHeight { get; set; }
        public int fullWidth { get; set; }
        public int item { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int height { get; set; }
        public int width { get; set; }
        public string src { get; set; }
        public float scale { get; set; }
    }
    #endregion

    #region ResetBalloon
    public class ResetBalloon
    {
        public string CdrawingNo { get; set; }
        public string CrevNo { get; set; }
        public int pageNo { get; set; }
        public int totalPage { get; set; }
        public List<OCRResults> originalRegions { get; set; }
    }
    #endregion

    #region RotateBalloon
    public class RotateBalloon
    {
        public string drawingNo { get; set; }
        public string revNo { get; set; }
        public string pageNo { get; set; }
        public string totalPage { get; set; }
        public string drawingDetails { get; set; }
        public int ItemView { get; set; }
        public int rotation { get; set; }
        public string sessionUserId { get; set; }
    }
    #endregion

    #region OCRResults
    public class OCRResults
    {
        public Int64 BaloonDrwID { get; set; }
        public string BaloonDrwFileID { get; set; }
        public string ProductionOrderNumber { get; set; }
        public string Part_Revision { get; set; }
        public int Page_No { get; set; }
        public string DrawingNumber { get; set; }
        public string Revision { get; set; }
        public string Balloon { get; set; }
        public string Spec { get; set; }
        public string Nominal { get; set; }
        public string Minimum { get; set; }
        public string Maximum { get; set; }
        public string MeasuredBy { get; set; }
        [MaybeNull]
        public DateTime MeasuredOn { get; set; }
        public int Circle_X_Axis { get; set; }
        public int Circle_Y_Axis { get; set; }
        public int Circle_Width { get; set; }
        public int Circle_Height { get; set; }
        [MaybeNull]
        public int Balloon_Thickness { get; set; }
        [MaybeNull]
        public int Balloon_Text_FontSize { get; set; }
        [MaybeNull]
        public decimal ZoomFactor { get; set; }
        public int Crop_X_Axis { get; set; }
        public int Crop_Y_Axis { get; set; }
        public int Crop_Width { get; set; }
        public int Crop_Height { get; set; }
        public string Type { get; set; }
        public string SubType { get; set; }
        public string Unit { get; set; }
        public int Quantity { get; set; }
        public string ToleranceType { get; set; }
        public string PlusTolerance { get; set; }
        public string MinusTolerance { get; set; }
        public string MaxTolerance { get; set; }
        public string MinTolerance { get; set; }
        public byte[] CropImage { get; set; }
        public string CreatedBy { get; set; }
        [MaybeNull]
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        [MaybeNull]
        public DateTime ModifiedDate { get; set; }
        [MaybeNull]
        public bool? IsCritical { get; set; }
        public string id { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string selectedRegion { get; set; }
        public bool isballooned { get; set; }
    }
    #endregion

    #region Specification
    public class Specification
    {
        public string spec { get; set; }
        public List<OCRResults> originalRegions { get; set; }

        public string plusTolerance { get; set; }
        public string toleranceType { get; set; }

        public string minusTolerance { get; set; }
        public string maximum { get; set; }
        public string minimum { get; set; }
    }
    #endregion

    #region PartialImage
    public class PartialImage
    {
        public int item { get; set; }
        public int count { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int width { get; set; }
        public int fullWidth { get; set; }
        public int height { get; set; }
        public int fullHeight { get; set; }
        public string src { get; set; }
        public float scale { get; set; }
    }
    #endregion

    #region Circle_AutoBalloon
    public class Circle_AutoBalloon
    {
        public System.Drawing.RectangleF Bounds { get; set; }
    }
    #endregion

    #region AutoBalloon_OCR
    public class AutoBalloon_OCR
    {
        public int X_Axis { get; set; }
        public int Y_Axis { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Ocr_Text { get; set; }
        public int Qty { get; set; }
        public int No { get; set; }
        public int parent { get; set; }
        public bool subballoon { get; set; }
    }
    #endregion

    #region AG_OCR
    public class AG_OCR
    {

        public int GroupID { get; set; }
        public int cx { get; set; }
        public int nx { get; set; }
        public int cy { get; set; }
        public string text { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int w { get; set; }
        public int h { get; set; }

    }
    public class AGF_OCR
    {

        public int GroupID { get; set; }
        public int parentID { get; set; }
        public string text { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int w { get; set; }
        public int h { get; set; }

    }
    #endregion

    #region WordInfo
    class WordInfo
    {
        public string Text { get; set; }
        public OpenCvSharp.Rect BoundingBox { get; set; }
        public int Id { get; set; }
    }
    #endregion

    #endregion

    [Route("api/[controller]")]
    [ApiController]
    public class DrawingSearchController : ControllerBase
    {
        private readonly DimTestContext _dbcontext;
        private readonly AppSettings _appSettings;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _options;
        private readonly IHttpContextAccessor _httpcontext;
        DataSet dsconfig = new DataSet();
        BubbleCommon.Settings objSettings = new BubbleCommon.Settings();
        BubbleCommon.ErrorLog objerr = new BubbleCommon.ErrorLog();
        string envcpath = Environment.CurrentDirectory;
        System.Data.DataTable dtFiles_Production = new DataTable("Production_Files");
        System.Data.DataTable dtFiles_Header = new DataTable("Drawing_Header");
        protected IList<System.Drawing.Image> imageList_Crop;
        List<Circle_AutoBalloon> lstCircle = new List<Circle_AutoBalloon>();
        List<PartialImage> partial_image = new List<PartialImage>();
        int drawing_page_no = 0;
        int drawing_total_page_no = 0;
        string drawing_no = string.Empty;
        string drawing_revision_no = string.Empty;
        bool iscontainsiphen = false;
        string temp = string.Empty;
        string username = "Admin";
        private Emgu.CV.OCR.Tesseract _ocr;

        #region LoadConfigDetails
        private void LoadConfigDetails()
        {
            try
            {
                BLL_Admin objBLL_Admin = new BLL_Admin();
                dsconfig = objBLL_Admin.Fetch_ConfigutationDetails();
                string IsCrypto = this._appSettings.IsCrypto;
                string MPMConnStr = this._appSettings.MPMConnStr;
                if (dsconfig.Tables[0].Rows.Count > 0)
                {
                    DataTable dtMPM = dsconfig.Tables[0].AsEnumerable()
                             .Where(r => r.Field<string>("Environment") == "MPM")
                             .CopyToDataTable();
                    if (dtMPM.Rows.Count > 0)
                    {
                        objSettings.MPMEnvironment = Convert.ToString(dtMPM.Rows[0]["Environment"]);
                        objSettings.MPMServer = Convert.ToString(dtMPM.Rows[0]["Datasource"]);
                        objSettings.MPMDatabase = Convert.ToString(dtMPM.Rows[0]["DBName"]);
                        if (Convert.ToString(dtMPM.Rows[0]["Authendication"]) == "SQL Server")
                        {
                            objSettings.MPMAuthendication = Convert.ToString(dtMPM.Rows[0]["Authendication"]);
                            objSettings.MPMUserID = Convert.ToString(dtMPM.Rows[0]["UserID"]);

                            if (string.IsNullOrEmpty(IsCrypto) != true)
                            {
                                if (IsCrypto.ToLower() == "yes")
                                {
                                    objSettings.MPMPassword = Decrypt(Convert.ToString(dtMPM.Rows[0]["Password"]));
                                }
                                else
                                {
                                    objSettings.MPMPassword = (Convert.ToString(dtMPM.Rows[0]["Password"]));
                                }
                            }
                            else
                            {
                                objSettings.MPMPassword = (Convert.ToString(dtMPM.Rows[0]["Password"]));
                            }
                        }
                        else
                        {
                            objSettings.MPMAuthendication = Convert.ToString(dtMPM.Rows[0]["Authendication"]);
                            objSettings.MPMUserID = string.Empty;
                            objSettings.MPMPassword = string.Empty;
                        }
                    }
                    DataTable dtQDMS = dsconfig.Tables[0].AsEnumerable()
                             .Where(r => r.Field<string>("Environment") == "QDMS")
                             .CopyToDataTable();
                    if (dtQDMS.Rows.Count > 0)
                    {
                        objSettings.QDMSEnvironment = Convert.ToString(dtQDMS.Rows[0]["Environment"]);
                        objSettings.QDMSServer = Convert.ToString(dtQDMS.Rows[0]["Datasource"]);
                        objSettings.QDMSDatabase = Convert.ToString(dtQDMS.Rows[0]["DBName"]);
                        if (Convert.ToString(dtQDMS.Rows[0]["Authendication"]) == "SQL Server")
                        {
                            objSettings.QDMSAuthendication = Convert.ToString(dtQDMS.Rows[0]["Authendication"]);
                            objSettings.QDMSUserID = Convert.ToString(dtQDMS.Rows[0]["UserID"]);
                            if (string.IsNullOrEmpty(IsCrypto) != true)
                            {
                                if (IsCrypto.ToLower() == "yes")
                                {
                                    objSettings.QDMSPassword = Decrypt(Convert.ToString(dtQDMS.Rows[0]["Password"]));
                                }
                                else
                                {
                                    objSettings.QDMSPassword = (Convert.ToString(dtQDMS.Rows[0]["Password"]));
                                }
                            }
                            else
                            {
                                objSettings.QDMSPassword = (Convert.ToString(dtQDMS.Rows[0]["Password"]));
                            }
                        }
                        else
                        {
                            objSettings.QDMSAuthendication = Convert.ToString(dtQDMS.Rows[0]["Authendication"]);
                            objSettings.QDMSUserID = string.Empty;
                            objSettings.QDMSPassword = string.Empty;
                        }
                    }
                    DataTable dtCWI = dsconfig.Tables[0].AsEnumerable()
                             .Where(r => r.Field<string>("Environment") == "CWI")
                             .CopyToDataTable();
                    if (dtCWI.Rows.Count > 0)
                    {
                        objSettings.CWIEnvironment = Convert.ToString(dtCWI.Rows[0]["Environment"]);
                        objSettings.CWIServer = Convert.ToString(dtCWI.Rows[0]["Datasource"]);
                        objSettings.Database = Convert.ToString(dtCWI.Rows[0]["DBName"]);
                        if (Convert.ToString(dtCWI.Rows[0]["Authendication"]) == "SQL Server")
                        {
                            objSettings.CWIAuthendication = Convert.ToString(dtCWI.Rows[0]["Authendication"]);
                            objSettings.CWIUserID = Convert.ToString(dtCWI.Rows[0]["UserID"]);
                            if (string.IsNullOrEmpty(IsCrypto) != true)
                            {
                                if (IsCrypto.ToLower() == "yes")
                                {
                                    objSettings.CWIPassword = Decrypt(Convert.ToString(dtCWI.Rows[0]["Password"]));
                                }
                                else
                                {
                                    objSettings.CWIPassword = (Convert.ToString(dtCWI.Rows[0]["Password"]));
                                }
                            }
                            else
                            {
                                objSettings.CWIPassword = (Convert.ToString(dtCWI.Rows[0]["Password"]));
                            }
                        }
                        else
                        {
                            objSettings.CWIAuthendication = Convert.ToString(dtCWI.Rows[0]["Authendication"]);
                            objSettings.CWIUserID = string.Empty;
                            objSettings.CWIPassword = string.Empty;
                        }
                    }
                    if (dsconfig.Tables[1].Rows.Count > 0)
                    {
                        DataView view = new DataView(dsconfig.Tables[1]);
                        DataTable dt = view.ToTable(false, "Key", "Value");
                        List<KeyValuePair<String, string>> test = new List<KeyValuePair<string, string>>();
                        foreach (DataRow dr in dt.Rows)
                        {
                            test.Add(new KeyValuePair<string, string>(dr["Key"].ToString(), Convert.ToString(dr["Value"])));
                        }
                        foreach (var element in test)
                        {
                            if (element.Key == "DwngByProductionOrder")
                            {
                                objSettings.DwngByProductionOrder = element.Value;
                            }
                            else if (element.Key == "DwngByConfirmationNo")
                            {
                                objSettings.DwngByConfirmationNo = element.Value;
                            }
                            else if (element.Key == "Drawing_Folder")
                            {
                                objSettings.DrawingFolder = element.Value;
                            }
                            else if (element.Key == "Baloon_Drawing")
                            {
                                objSettings.BalloonedFolder = element.Value;
                            }
                            else if (element.Key == "ErrorLog_Folder")
                            {
                                objSettings.ErrorLogFolder = element.Value;
                            }
                            else if (element.Key == "InspectionReportPDF_Folder")
                            {
                                objSettings.InspectionReportPDF_Folder = element.Value;
                            }
                            else if (element.Key == "InspectionImagePDF_Folder")
                            {
                                objSettings.InspectionImagePDF_Folder = element.Value;
                            }
                            else if (element.Key == "InspectionExcelReport_Folder")
                            {
                                objSettings.InspectionExcelReport_Folder = element.Value;
                            }
                            else if (element.Key == "BalloonWidth")
                            {
                                objSettings.BalloonWidth = Convert.ToInt32(element.Value);
                            }
                            else if (element.Key == "BalloonHeight")
                            {
                                objSettings.BalloonHeight = Convert.ToInt32(element.Value);
                            }
                            else if (element.Key == "BalloonColor")
                            {
                                string[] splitBalloonColor = element.Value.Split('-');
                                objSettings.BalloonColor = element.Value;
                            }
                            else if (element.Key == "BalloonTextColor")
                            {
                                string[] splitBalloonTextColor = element.Value.Split('-');
                                objSettings.BalloonTextColor = element.Value;
                            }
                            else if (element.Key == "BalloonFontSize")
                            {
                                objSettings.BalloonFontSize = Convert.ToInt32(element.Value);
                            }
                            else if (element.Key == "BallonNumberFontSize")
                            {
                                objSettings.BallonNumberFontSize = Convert.ToInt32(element.Value);
                            }
                            else if (element.Key == "MinMaxOneDigit")
                            {
                                objSettings.MinMaxOneDigit = Convert.ToDecimal(element.Value);
                            }
                            else if (element.Key == "MinMaxTwoDigit")
                            {
                                objSettings.MinMaxTwoDigit = Convert.ToDecimal(element.Value);
                            }
                            else if (element.Key == "MinMaxThreeDigit")
                            {
                                objSettings.MinMaxThreeDigit = Convert.ToDecimal(element.Value);
                            }
                            else if (element.Key == "MinMaxFourDigit")
                            {
                                objSettings.MinMaxFourDigit = Convert.ToDecimal(element.Value);
                            }
                            else if (element.Key == "MinMaxAngles")
                            {
                                objSettings.MinMaxAngles = Convert.ToString(element.Value);
                            }
                            else if (element.Key == "DimensionUpdateBalloonColor")
                            {
                                string[] splitDimensionUpdateBalloonTextColor = element.Value.Split('-');
                                objSettings.DimensionUpdateBalloonColor = element.Value;
                            }
                            else if (element.Key == "LoginSessionTimeOut")
                            {
                                objSettings.LoginSessionTimeout = element.Value;
                            }
                            else if (element.Key == "LastWorkCenter")
                            {
                                objSettings.LastWorkCenter = element.Value;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objerr.WriteErrorToText(ex);
            }
        }
        #endregion

        #region Cryptography
        private string Encrypt(string strString)
        {
            string str = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(strString) != true)
                {
                    string text = strString;
                    CCryptography encryptor = new CCryptFactory().GetEncryptor();

                    string sToCrypt = text;
                    string local = str;
                    encryptor.Crypt(sToCrypt, out local);
                }

            }
            catch (Exception ex)
            {
                throw new Exception();

            }
            return str;
        }
        private string Decrypt(string strEncString)
        {
            string str = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(strEncString) != true)
                {
                    string text = strEncString;
                    CCryptography decryptor = new CCryptFactory().GetDecryptor();

                    string sToCrypt = text;
                    string local = str;
                    decryptor.Crypt(sToCrypt, out local);
                }

            }
            catch (Exception ex)
            {
                throw new Exception();
            }
            return str;
        }
        #endregion

        #region Page_Revision_Details

        private void Page_Revision_Details(string FileID)
        {
            iscontainsiphen = false;
            try
            {
                if (FileID.Contains("of") && FileID.Contains("-"))
                {
                    drawing_total_page_no = Convert.ToInt16(FileID.Substring(FileID.IndexOf("of") + 2).Replace(".tiff", "").Replace(".TIFF", "").Replace(".tif", "").Replace(".TIF", "").Replace(".PNG", "").Replace(".Png", "").Replace(".png", ""));
                    string stringBeforeChar = FileID.Substring(0, FileID.IndexOf("of"));
                    stringBeforeChar = stringBeforeChar.Substring(stringBeforeChar.Length - 5).Replace(".", "");
                    if (stringBeforeChar.Contains("-"))
                    {
                        iscontainsiphen = true;
                    }
                    else
                    {
                        iscontainsiphen = false;
                    }
                    stringBeforeChar = stringBeforeChar.Substring(0, 2);
                    drawing_page_no = Convert.ToInt16(stringBeforeChar);
                    string[] rev = FileID.Split('.');
                    drawing_no = rev[0];
                    drawing_revision_no = rev[1];
                }
                else if (FileID.Contains("of"))
                {
                    drawing_total_page_no = Convert.ToInt16(FileID.Substring(FileID.IndexOf("of") + 2).Replace(".tiff", "").Replace(".TIFF", "").Replace(".tif", "").Replace(".TIF", "").Replace(".PNG", "").Replace(".Png", "").Replace(".png", ""));
                    string stringBeforeChar = FileID.Substring(0, FileID.IndexOf("of"));
                    stringBeforeChar = stringBeforeChar.Substring(stringBeforeChar.Length - 4);
                    stringBeforeChar = stringBeforeChar.Substring(0, 2);
                    drawing_page_no = Convert.ToInt16(stringBeforeChar);
                    string[] rev = FileID.Split('.');
                    drawing_no = rev[0];
                    drawing_revision_no = rev[1];
                }
                else if (FileID.Contains("-"))
                {
                    drawing_total_page_no = Convert.ToInt16(FileID.Substring(FileID.LastIndexOf("-") + 2).Replace(".tiff", "").Replace(".TIFF", "").Replace(".tif", "").Replace(".TIF", "").Replace(".PNG", "").Replace(".Png", "").Replace(".png", ""));
                    string stringBeforeChar = FileID.Substring(0, FileID.LastIndexOf("-"));
                    stringBeforeChar = stringBeforeChar.Substring(stringBeforeChar.Length - 2);
                    drawing_page_no = Convert.ToInt16(stringBeforeChar);
                    string[] rev = FileID.Split('.');
                    drawing_no = rev[0];
                    drawing_revision_no = rev[1];
                }
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
        #endregion

        #region Image Process
        public MemoryStream imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            try
            {
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            }
            catch (Exception ex)
            {

            }
            return ms;
        }
        private void RemoveOpaqueColormapFrom1BPP(string dtFiles)
        {
            // Load the 1 bpp PNG image
            using (MagickImage image = new MagickImage(dtFiles))
            {
                // Convert to grayscale
                image.ColorType = ColorType.Grayscale;
                // Ensure that it's 1 bpp
                image.Depth = 1;
                // Save the modified image
                MemoryStream outputStream = new MemoryStream();
                image.Write(outputStream, MagickFormat.Png);
                byte[] outputBytes = outputStream.ToArray();
                FileInfo fi = new FileInfo(dtFiles);
                string Fname = fi.Name.Replace(fi.Extension, "");
                // You can return the modified image bytes as a response
                temp = SaveBytesToFile(outputBytes, Fname);
                outputStream.Dispose();
            }
        }
        public static string SaveBytesToFile(byte[] bytes, string Fname)
        {
            string regionImageFile = System.IO.Path.Combine(System.IO.Path.GetTempPath(), Guid.NewGuid().ToString() + $"{Fname}.png");
            try
            {
                System.IO.File.WriteAllBytes(regionImageFile, bytes);
                return regionImageFile;
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during file writing
                Console.WriteLine($"Error saving file: {ex.Message}");
                return null;
            }
        }
        public void RotateImagefile(string ImageFile, int angle)
        {
            if (angle != 0)
            {
                System.Drawing.Image orimage = System.Drawing.Image.FromFile(ImageFile);
                RotateFlipType r;
                switch (angle)
                {
                    case 90:
                        r = RotateFlipType.Rotate90FlipNone;
                        orimage.RotateFlip(r);

                        break;
                    case 180:
                        r = RotateFlipType.Rotate180FlipNone;
                        orimage.RotateFlip(r);
                        break;
                    case 270:
                        r = RotateFlipType.Rotate270FlipNone;
                        orimage.RotateFlip(r);
                        break;
                }
                orimage.Save(ImageFile);
                orimage.Dispose();
            }
        }
        public string rotateproperties(string drawingNo, string revNo, DataTable dtFiles_Production)
        {
            string rotate = string.Empty;
            try
            {
                var hdr = _dbcontext.TblBaloonDrawingHeaders.Where(w => w.DrawingNumber == drawingNo.ToString() && w.Revision == revNo.ToString()).FirstOrDefault();
               
                if (hdr != null)
                {
                    rotate = hdr.RotateProperties;
                    if (rotate == null)
                    {
                        rotate = "[";
                        for (int rt = 0; rt < dtFiles_Production.Rows.Count; rt++)
                        {
                            rotate += "0,";
                        }
                        rotate = rotate.Remove(rotate.Length - 1, 1);
                        rotate += "]";
                    }
                }
                else
                {
                    rotate = "[";
                    for (int rt = 0; rt < dtFiles_Production.Rows.Count; rt++)
                    {
                        rotate += "0,";
                    }
                    rotate = rotate.Remove(rotate.Length - 1, 1);
                    rotate += "]";
                }
                return rotate;
            }
            catch (Exception ex)
            {
                objerr.WriteErrorLog(ex.ToString());
                return rotate;
            }
            return rotate;
        }
        private string scaleImage(string s, int itemview, bool isFlag)
        {
            int maximum = 32767;
            string resize = "false";
            try
            {
                using (FileStream inputStream = System.IO.File.OpenRead(s))
                {
                    using (SixLabors.ImageSharp.Image image = SixLabors.ImageSharp.Image.Load(s))
                    {
                        int originalWidth = image.Width;
                        int originalHeight = image.Height;
                        int newWidth, newHeight;
                        if (image.Width > maximum || image.Height > maximum)
                        {
                            resize = "true";
                            if (originalWidth > originalHeight)
                            {
                                // Landscape orientation
                                newWidth = maximum;
                                newHeight = (int)((double)originalHeight / originalWidth * maximum);
                            }
                            else
                            {
                                // Portrait or square orientation
                                newHeight = maximum;
                                newWidth = (int)((double)originalWidth / originalHeight * maximum);
                            }
                            using (Image<Rgba32> rgbaImage = image.CloneAs<Rgba32>())
                            {
                                // Calculate the scaling factor for resizing
                                float widthScale = (float)newWidth / originalWidth;
                                float heightScale = (float)newHeight / originalHeight;
                                float scale = Math.Min(widthScale, heightScale);
                                int scaledWidth = (int)(originalWidth * scale);
                                int scaledHeight = (int)(originalHeight * scale);
                                FileInfo fi = new FileInfo(s);
                                string fileName = fi.Name;//.Replace(fi.Extension, "") + ".png";
                                if (isFlag)
                                {
                                    partial_image.Add(new PartialImage
                                    {
                                        x = 0,
                                        y = 0,
                                        width = newWidth,
                                        height = newHeight,
                                        src = Convert.ToString(fileName),
                                        scale = scale,
                                        fullWidth = originalWidth,
                                        fullHeight = originalHeight,
                                        item = itemview,
                                        count = partial_image.Count()
                                    });
                                }
                                // Resize the image
                                using (Image<Rgba32> resizedImage = rgbaImage.Clone(x => x.Resize(scaledWidth, scaledHeight)))
                                {
                                    // Save the resized image to the output path
                                    image.Dispose();
                                    inputStream.Dispose();
                                    resizedImage.Save(s); // Change the format if needed
                                }
                            }
                        }
                        else
                        {
                            if (isFlag)
                            {
                                FileInfo fi = new FileInfo(s);
                                string fileName = fi.Name.Replace(fi.Extension, "") + ".png";
                                partial_image.Add(new PartialImage
                                {
                                    x = 0,
                                    y = 0,
                                    width = originalWidth,
                                    height = originalHeight,
                                    src = Convert.ToString(fileName),
                                    scale = 1,
                                    fullWidth = originalWidth,
                                    fullHeight = originalHeight,
                                    item = itemview,
                                    count = partial_image.Count()
                                });
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                objerr.WriteErrorToText(ex);
            }
            return resize;
        }
        static Bitmap CropImage(Bitmap img, System.Drawing.Rectangle cropArea)
        {
            Bitmap croppedImage = img.Clone(cropArea, img.PixelFormat);
            return croppedImage;
        }
        static Bitmap ChangeResolution(Bitmap img, float newDpi)
        {
            Bitmap newImage = new Bitmap(img.Width, img.Height);
            newImage.SetResolution(newDpi, newDpi);
            using (Graphics g = Graphics.FromImage(newImage))
            {
                g.DrawImage(img, 0, 0);
            }
            return newImage;
        }
        #endregion

        #region Session
        public sessionobj getsession()
        {
            var session = HttpContext.Session;
            sessionobj obj = session.GetObjectFromJson<sessionobj>("sessionobj");
            return obj;
        }
        public sessionobj setsession()
        {
            var session = HttpContext.Session;
            var sessionInfo = new sessionobj();
            sessionobj sessionobj = session.GetObjectFromJson<sessionobj>("sessionobj");
            if (sessionobj == null)
            {
                string sessionName = "Current User";
                string sessionId = Guid.NewGuid().ToString();
                sessionInfo.sessionUserName = sessionName;
                sessionInfo.sessionUserId = sessionId;
                sessionInfo.expire = TimeSpan.FromMinutes(10);
                session.SetObjectAsJson("sessionobj", sessionInfo);
            }
            else
            {
                sessionobj.expire = TimeSpan.FromMinutes(10);
                session.SetObjectAsJson("sessionobj", sessionobj);
            }
            sessionobj obj = session.GetObjectFromJson<sessionobj>("sessionobj");
            return obj;
        }
        #endregion

        #region Predefined data

        public DataTable Load_MeasureType()
        {
            DataTable dtType = new DataTable();
            try
            {
                BLL_BalloonDrawing objBLL_BalloonDrawing = new BLL_BalloonDrawing();
                dtType = objBLL_BalloonDrawing.Fetch_MeasureType().Tables[0];
                if (dtType.Rows.Count > 0)
                {
                    return dtType;
                }
                return dtType;
            }
            catch (Exception ex)
            {
                objerr.WriteErrorToText(ex);
              //  throw new Exception();
                return dtType;
            }
        }
        public DataTable Load_MeasureSubType()
        {
            DataTable dtSubType = new DataTable();
            try
            {
                BLL_BalloonDrawing objBLL_BalloonDrawing = new BLL_BalloonDrawing();
                dtSubType = objBLL_BalloonDrawing.Fetch_MeasureSubType().Tables[0];
                if (dtSubType.Rows.Count > 0)
                {
                    return dtSubType;
                }
                return dtSubType;
            }
            catch (Exception ex)
            {
                objerr.WriteErrorToText(ex);
                // throw new Exception();
                return dtSubType;
            }
        }
        public DataTable Load_UOM()
        {

            DataTable dtUOM = new DataTable();
            try
            {
                BLL_BalloonDrawing objBLL_BalloonDrawing = new BLL_BalloonDrawing();
                dtUOM = objBLL_BalloonDrawing.Fetch_UOM().Tables[0];
                if (dtUOM.Rows.Count > 0)
                {
                    return dtUOM;
                }
                return dtUOM;
            }
            catch (Exception ex)
            {
                objerr.WriteErrorToText(ex);
                // throw new Exception();
                return dtUOM;
            }
        }
        #endregion

        #region Data Transform
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
        private string OcrTextOptimization(string regionText, int x, int y, int w, int h, int imagewidth, int imageheight, AutoBalloon searchForm)
        {
            string retval = "";
            try
            {
                if (string.IsNullOrWhiteSpace(regionText) || regionText == "" || regionText == "  " || regionText == " " || regionText == null)
                {
                    return retval;
                }
                if (regionText == "2180°")
                {
                    regionText = "2X180°";
                }
                if (regionText == "00")
                {
                    regionText = "30°";
                }
                if (regionText == "çë")
                {
                    regionText = "ç";
                }
                if (regionText == "63")
                {
                    regionText = "»";
                }
                if (regionText == "X5°" || regionText == "45°(" || regionText == "45Z" || regionText == "452" || regionText == "45" || regionText == "45O" || regionText == "450" || regionText == "42")
                {
                    regionText = "45°";
                }
                if (regionText == "32")
                {
                    regionText = "´";
                }
                if (regionText == "38" || regionText == "30" || regionText == "396" || regionText == "380" || regionText == "390" || regionText == "300" || regionText == "3905" || regionText == "398" || regionText == "30O")
                {
                    regionText = "30°";
                }
                if (regionText == "150" || regionText == "15")
                {
                    regionText = "15°";
                }
                if (regionText == "100" || regionText == "10")
                {
                    regionText = "10°";
                }
                if (regionText == "250" || regionText == "25")
                {
                    regionText = "25°";
                }
                if (regionText == "200" || regionText == "29«")
                {
                    regionText = "20°";
                }

                if (regionText == "900")
                {
                    regionText = "90°";
                }
                if (regionText == "70" || regionText == "7")
                {
                    regionText = "7°";
                }
                if (regionText == "50")
                {
                    regionText = "5°";
                }
                if (regionText == "û")
                {
                    regionText = "";
                }
                if (regionText == "Rù6")
                {
                    regionText = "R.6";
                }
                if (regionText == "R.î3" || regionText == "R.îñ")
                {
                    regionText = "R.03";
                }
                if (regionText == "R.00")
                {
                    regionText = "R.005";
                }

                if (regionText == ".îîóë")
                {
                    regionText = ".005";
                }
                if (regionText == "125")
                {
                    regionText = "«";
                }
                Dictionary<string, string> replacements = new Dictionary<string, string>{
                        // { "ë", "I" },
                        { "î.", "O" },
                        { "çç", "" },
                        { "═", "" },
                        { "══", "" },
                        //{ "(","" },
                        //{ ")","" },
                        //{ " ", "" },
                        //  { "ù", "." },
                        { "EB", "─" },
                        {"XX",""},
                        {"##","" },
                        {"..","" },
                        {":","" },
                        {"«ç","" },
                        //{"─","" },
                        // {"H","" }
                        {"çë","ç" },
                        {"±F","OF" },
                        {"°F","OF" },
                        {"-³","" },
                       // {"³","" },
                        {".³","" },
                        {"³-","" },
                        {".ë","" },
                        {"-³ ",""},
                        { "-═",""},
                        { "-----",""},
                        { "ð","2"},
                        { " ±","±"},
                        { "à","¡"},
                        { "±-","±."},
                        { "|","" },
                        { "-X","" },
                        { "APART","" }
                                      };
                foreach (var replacement in replacements)
                {
                    regionText = regionText.Replace(replacement.Key, replacement.Value);
                }
                if (regionText.Contains("(") && regionText.Contains(")") && regionText.Contains("-"))
                {
                    regionText = regionText.Replace("-", ".");
                }
                if (regionText.Trim().StartsWith("I"))
                {
                    regionText = regionText.Replace("I", "");
                }
                if (regionText.Length == 1 && (regionText == "«" || regionText == "´" || regionText == "Ú" || regionText == "Û" || regionText == "»"))
                {
                    return regionText + "," + x.ToString() + "," + y.ToString() + "," + w.ToString() + "," + h.ToString();
                }
                if (Regex.IsMatch(regionText, @"\(([^[\]]*)\)"))
                {
                    return retval;
                }
                regionText = Regex.Replace(regionText, @"\r\n?|\n", "");
                regionText = Regex.Replace(regionText, @"(\(\/\)|\([0-9]{1}\))", "");
                string brackets = @"[\(\)]";
                regionText = Regex.Replace(regionText, brackets, "");
                try
                {
                    if (regionText.Contains("+") && regionText.Contains(")"))
                    {
                        regionText = regionText.Substring(0, regionText.IndexOf(")", regionText.IndexOf(")") + 1));
                    }
                }
                catch (Exception ex)
                {
                    objerr.WriteErrorToText(ex);
                }
                if (Regex.IsMatch(regionText, @"^(((?:ç)|(?:─)|)(?:\s)?(?:¡)?(?:\s)?(?:\.\d+)(?:\/)?(?:\d+?\.\d+)?(?:\s)?(?:[A-Z]{1})?(?:Þ)?(?:\s)?(?:[A-Z]{1})?(?:Þ)?(?:\s)?(?:[A-Z]{1})?(?:Þ)?)$"))
                {

                    string mainValuePattern = @"^-?(([(?:¡)|(?:ç)|(?:─)|(?:\s+)]+)?([(?:\.\d+)|(?:/)|(?:\d+\.\d+)]+))?";
                    if (regionText.Contains("─"))
                    {
                        string[] spltxt = regionText.Split("─");
                        if (spltxt[1].Length > 0 && !spltxt[1].StartsWith(" "))
                        {
                            if (spltxt[0] == "") { regionText = "─" + " " + spltxt[1]; }
                            else { regionText = spltxt[0] + "─ " + spltxt[1]; }
                        }
                    }

                    if (regionText.Contains("ç"))
                    {
                        string[] spltxt = regionText.Split("ç");
                        if (spltxt[1].Length > 0 && !spltxt[1].StartsWith(" "))
                        {
                            if (spltxt[0] == "") { regionText = "ç" + " " + spltxt[1]; }
                            else { regionText = spltxt[0] + "ç " + spltxt[1]; }

                        }
                    }
                    return regionText + "," + x.ToString() + "," + y.ToString() + "," + w.ToString() + "," + h.ToString();
                }
                if (regionText.Contains("ç") && regionText.Contains("°"))
                {
                    return retval;
                }
                if ((regionText.EndsWith("X.") || regionText.EndsWith("X-")) && regionText.Length > 3)
                {
                    regionText = regionText.Replace("X.", "").Replace("X-", "");
                }

                if (regionText.Contains("X"))
                {
                    string[] hastext = regionText.Split("X");
                    if (hastext[0].Trim().Length > 0 && hastext[1].Trim().Length > 0 && Regex.IsMatch(regionText, @"^((\d+(\.\d+))|(\d+)|(\.\d+)(?:\s|))X((?:\s|)(\d+))$"))
                    {
                        var deg = hastext[1].Trim();
                        if (hastext[1].Trim().Length == 3 && hastext[1].Trim().EndsWith("0"))
                        {
                            hastext[1] = hastext[1].Substring(0, hastext[1].Length - 1) + "°";
                        }
                        regionText = hastext[0] + "X" + hastext[1];
                        return regionText + "," + x.ToString() + "," + y.ToString() + "," + w.ToString() + "," + h.ToString();
                    }
                }
                if (regionText.Contains("X") && regionText.Contains("±"))
                {
                    string[] hastext = regionText.Split("X");
                    if (hastext[0].Trim().Length > 0 && hastext[1].Trim().Length > 0)
                    {
                        regionText = hastext[0] + "X" + hastext[1];
                        return regionText + "," + x.ToString() + "," + y.ToString() + "," + w.ToString() + "," + h.ToString();
                    }
                }

                if (Regex.IsMatch(regionText, @"^¡((\d+(\.\d+))|(\d+)|(\.\d+)(?:\s|))±((\d+(\.\d+))|(\d+)|(\.\d+)(?:\s|))((?:V\-)|(?:»))((\d+(\.\d+))|(\d+)|(\.\d+)(?:\s|))+$"))
                {
                    string arrowpattern = @"^¡(\d+(\.\d+)|(\.\d+))±(\d+(\.\d+)|(\.\d+))((?:V\-)|(?:»))(\d+(\.\d+)|(\.\d+))+$";
                    Regex arrowregex = new Regex(arrowpattern);
                    Match match = arrowregex.Match(regionText);

                    if (match.Success)
                    {
                        regionText = regionText.Replace("1V-", "¨").Replace("»", "¨");
                        return regionText + "," + x.ToString() + "," + y.ToString() + "," + w.ToString() + "," + h.ToString();
                    }
                }

                if (Regex.IsMatch(regionText, @"^¡((\d+(\.\d+))|(\d+)|(\.\d+)(?:\s+|))((?:V\-)|(?:»))(?:\s+|)?((\d+(\.\d+))|(\d+)|(\.\d+)(?:\s|))([A-Z]+)"))
                {
                    string arrowpattern = @"^¡((\d+(\.\d+))|(\d+)|(\.\d+)(?:\s+|))((?:V\-)|(?:»))(?:\s+|)?((\d+(\.\d+))|(\d+)|(\.\d+)(?:\s|))([A-Z]+)";
                    Regex arrowregex = new Regex(arrowpattern);
                    Match match = arrowregex.Match(regionText);

                    if (match.Success)
                    {
                        regionText = regionText.Replace("1V-", "¨").Replace("»", "¨");
                        return regionText + "," + x.ToString() + "," + y.ToString() + "," + w.ToString() + "," + h.ToString();
                    }
                }

                if (Regex.IsMatch(regionText, @"^(?:¡)?(\d+\.\d+)((\.\:\d+)|(\.\d+))$"))
                {
                    string pattern_non_plmin = @"^(?:¡)?(\d+\.\d+)((\.\:\d+)|(\.\d+))$";
                    Regex non_plmin_regex = new Regex(pattern_non_plmin);
                    Match match_non_plmin = non_plmin_regex.Match(regionText);
                    if (match_non_plmin.Success)
                    {
                        string non_plmin_suffix = match_non_plmin.Groups[2].Value;
                        non_plmin_suffix = non_plmin_suffix.Replace(":", "");
                        string non_plmin_prefix = regionText.Substring(0, regionText.Length - non_plmin_suffix.Length);
                        regionText = non_plmin_prefix + " -" + non_plmin_suffix;
                        return regionText + "," + x.ToString() + "," + y.ToString() + "," + w.ToString() + "," + h.ToString();
                    }
                }
                if (searchForm.selectedRegion == "Full Image")
                {
                    string[] stringArray = { "unless", "PRODUCTS" };
                    if ((stringArray.Any(s => regionText.ToLower().Contains(s.ToLower()))))
                    {
                        return retval;
                    }
                }
                if (string.IsNullOrWhiteSpace(regionText) || regionText == "  " || regionText == " " || regionText == null)
                {
                    return retval;
                }
                //bool isletonly= Regex.IsMatch(regionText, @"^[a-zA-Z]+$");
                bool Is_Need_Ocr = true;
                bool isDigitPresent1 = regionText.Any(c => char.IsDigit(c));
                if (regionText.Contains("X") && (isDigitPresent1 == false && !regionText.Contains("°")) && regionText != "SPCL" && regionText != "TOP" && regionText != "BOX" && regionText != "PIN")
                {
                    Is_Need_Ocr = false;
                }
                if (regionText.Contains("çç") || regionText.Contains("7J") || regionText.Contains("4J"))
                {
                    Is_Need_Ocr = false;
                }

                string[] skipwords = { "DRAWING", "Nî.", "FRAME", "SHEET", "VëEW", "REVISIîN", "SECTION", "D-D", "C-C", "B-B", "A-A", "DETAII.", "LINE", "WITH", "WIDTH", "SLOT", "SLOTS", "LEAD", "HAND", "I.EFT", "RIGHT", "SEE", "LINEWITH", "IN", "E-", "(COAT", "PER", "BOM", "FLATS", "CONFIGURATION", "FLAT", "EB", searchForm.CdrawingNo.ToLower(), searchForm.CdrawingNo.ToUpper(), searchForm.CdrawingNo };
                string result = skipwords.FirstOrDefault(x => x == regionText);
                if (result != null)// || isletonly==true)
                {
                    Is_Need_Ocr = false;
                }
                string pattern = @"^\d+0F\d+$";
                Regex regex = new Regex(pattern);
                MatchCollection matches = regex.Matches(regionText);
                if (matches.Count() > 0)
                {
                    return retval;
                }

                if (regionText.StartsWith("R") && Regex.IsMatch(regionText.Trim(), @"^(?:R(\d+|(?:\.d+)|)(?:\.\d+))$"))
                {
                    return regionText + "," + x.ToString() + "," + y.ToString() + "," + w.ToString() + "," + h.ToString();
                }
                if (regionText.StartsWith("V ") && Regex.IsMatch(regionText.Trim(), @"^(?:V)(?:\s)?((\d+\.\d+)|(\d+)|(\.\d+))(?:\s)?X(?:\s)?(\d+)?°"))
                {
                    regionText = regionText.Replace("V", "§");
                    return regionText + "," + x.ToString() + "," + y.ToString() + "," + w.ToString() + "," + h.ToString();
                }

                if (regionText.Contains("±"))
                {
                    if (regionText.Contains('O'))
                    {
                        regionText = regionText.Replace("O", "0");
                    }
                    string[] hastext = regionText.Split("±");

                    if (hastext[0].Trim().Length > 0 && hastext[1].Trim().Length > 0 && Regex.IsMatch(regionText, @"^((\d+)(?:\s|))±((?:\s|)(\d+)?°)$"))
                    {
                        var deg = hastext[0].Trim();
                        if (hastext[0].Trim().Length == 3 && hastext[0].Trim().EndsWith("0"))
                        {
                            hastext[0] = hastext[0].Substring(0, hastext[0].Length - 1) + "°";
                        }
                        regionText = hastext[0] + "±" + hastext[1];
                    }

                    if (hastext[0].Length > 0 && hastext[1].Length > 0 && Regex.IsMatch(regionText.Trim(), @"^((\d+|(?:\.d+)|)(?:\.\d+)?°)±((\d+|(?:\.d+)|)(?:\.\d+)?°)$"))
                    {
                        regionText = hastext[0] + "±" + hastext[1];

                        // regionText = hastext[0] + " -."+ hastext[1];
                        return regionText + "," + x.ToString() + "," + y.ToString() + "," + w.ToString() + "," + h.ToString();
                    }
                }
                if (regionText.Contains('¡') && regionText.Length == 1)
                {
                    return regionText + "," + x.ToString() + "," + y.ToString() + "," + w.ToString() + "," + h.ToString();
                }
                if (regionText.IndexOf('¡') != regionText.LastIndexOf('¡'))
                {
                    return retval;
                }
                if (regionText.StartsWith("ë³") && regionText.Length == 2)
                {
                    return regionText + "," + x.ToString() + "," + y.ToString() + "," + w.ToString() + "," + h.ToString();
                }
                if (regionText.Contains("³") && regionText.Length > 1)
                {
                    regionText = regionText.Replace("³", "");
                }
                if (regionText.Length == 1 && Regex.IsMatch(regionText, @"^[a-zA-Z!@#$%^&*()-_=+\[\]{};:'"",.<>/?]+$"))
                {
                    return retval;
                }
                if (Regex.IsMatch(regionText, @"^[^0-9]*$"))
                {
                    return retval;
                }

                if (Regex.IsMatch(regionText, @"^[XxWMI]"))
                {
                    regionText = regex.Replace(regionText, "");
                }
                if (regionText.Contains("-.") && regionText.Contains("-") && !regionText.Contains("+"))
                {
                    regionText = regionText.Replace("-.", "±.");
                    return regionText + "," + x.ToString() + "," + y.ToString() + "," + w.ToString() + "," + h.ToString();
                }
                // contain + - 
                if (regionText.Contains("+") && regionText.Contains("-"))
                {
                    string mainValuePattern = @"^-?((?:¡|)(\d+|(?:\.d+)|)(?:\.\d+))?";
                    string positiveValuePattern = @"\+((\d+|(?:\.d+)|)(?:\.\d+))?";
                    string negativeValuePattern = @"-((\d+|(?:\.d+)|)(?:\.\d+))?";

                    Match mainValueMatch = Regex.Match(regionText, mainValuePattern);
                    MatchCollection positiveValueMatches = Regex.Matches(regionText, positiveValuePattern);
                    MatchCollection negativeValueMatches = Regex.Matches(regionText, negativeValuePattern);


                    string mainValue = mainValueMatch.Value;
                    string positive = string.Empty;
                    string negative = string.Empty;
                    foreach (Match match in positiveValueMatches)
                    {
                        positive = match.Value;
                    }
                    foreach (Match match in negativeValueMatches)
                    {
                        negative = match.Value;
                    }
                    return regionText + "," + x.ToString() + "," + y.ToString() + "," + w.ToString() + "," + h.ToString();
                }

                if (regionText.Contains("TURNED"))
                {
                    regionText = regionText.Replace("TURNED", "RMS");
                    return regionText + "," + x.ToString() + "," + y.ToString() + "," + w.ToString() + "," + h.ToString();
                }
                // UN thread

                if (Regex.IsMatch(regionText, @"^(((\d+)(?:\s)(\d+)|(\d+)-(\d+))/(\d+)-(\d+)(?:\s)UN((?:-)|(?:\s))[0-9]+[A-Z])"))
                {
                    return regionText + "," + x.ToString() + "," + y.ToString() + "," + w.ToString() + "," + h.ToString();
                }
                if (regionText.Contains("UNF"))
                {
                    return regionText + "," + x.ToString() + "," + y.ToString() + "," + w.ToString() + "," + h.ToString();
                }
                if (regionText.Contains("HIF"))
                {
                    return regionText + "," + x.ToString() + "," + y.ToString() + "," + w.ToString() + "," + h.ToString();
                }
                //UNS thread
                if (Regex.IsMatch(regionText, @"^((?:L)(\d+)/(\d+)-(\d+)(?:\s)UNS-[0-9]+[A-Z])"))
                {
                    if (regionText.StartsWith("L"))
                    {
                        return regionText.Substring(1) + "," + x.ToString() + "," + y.ToString() + "," + w.ToString() + "," + h.ToString();
                    }
                    return regionText + "," + x.ToString() + "," + y.ToString() + "," + w.ToString() + "," + h.ToString();
                }
                // STUB ACME
                if (regionText.Contains("STUB ACME"))
                {
                    if (regionText.StartsWith("L") || regionText.StartsWith("ç"))
                    {
                        return regionText.Substring(1) + "," + x.ToString() + "," + y.ToString() + "," + w.ToString() + "," + h.ToString();
                    }
                    return regionText + "," + x.ToString() + "," + y.ToString() + "," + w.ToString() + "," + h.ToString();
                }
                // HST-DS
                if (regionText.Contains("HST-DS"))
                {
                    if (regionText.StartsWith("L") || regionText.StartsWith("ç"))
                    {
                        return regionText.Substring(1) + "," + x.ToString() + "," + y.ToString() + "," + w.ToString() + "," + h.ToString();
                    }
                    return regionText + "," + x.ToString() + "," + y.ToString() + "," + w.ToString() + "," + h.ToString();
                }
                // HST
                if (regionText.Contains("HST"))
                {
                    if (regionText.StartsWith("L") || regionText.StartsWith("ç"))
                    {
                        return regionText.Substring(1) + "," + x.ToString() + "," + y.ToString() + "," + w.ToString() + "," + h.ToString();
                    }
                    return regionText + "," + x.ToString() + "," + y.ToString() + "," + w.ToString() + "," + h.ToString();
                }

                if (Regex.IsMatch(regionText, @"^[0-9.#]+$") && !regionText.StartsWith("#") && !regionText.EndsWith("#") && new Regex(Regex.Escape("#")).Matches(regionText).Count == 1)
                {
                    string[] hastext = regionText.Split("#");
                    if (hastext[0].Length > 0 && hastext[1].Length > 0 && Regex.IsMatch(hastext[0], @"^([0-9]+|(?:\.[0-9]+))?$") && Regex.IsMatch(hastext[1], @"^([0-9]+|(?:\.[0-9]+))?$"))
                    {
                        // regionText = hastext[0] + " -."+ hastext[1];
                        //  return regionText + "," + x.ToString() + "," + y.ToString() + "," + w.ToString() + "," + h.ToString();
                    }
                }
                if (regionText.Contains("VAM") || regionText.Contains("TOP") || regionText.Contains("SPCL"))
                {
                    return regionText + "," + x.ToString() + "," + y.ToString() + "," + w.ToString() + "," + h.ToString();
                }
                // surface finish 
                if (Regex.IsMatch(regionText, @"^[0-9.#]+$"))
                {

                }

                if (Regex.IsMatch(regionText, @"^(?:¡)?((?:\d+\-\d{2,})|(?:\d+\s\d{2,}))(?:.)?((?:\d+\-\d{2,})|(?:\d+\s\d{2,}))?"))
                {
                    string pattern_non_dot = @"^(?:¡)?((?:\d+\-\d{2,})|(?:\d+\s\d{2,}))(?:.)?((?:\d+\-\d{2,})|(?:\d+\s\d{2,}))?";
                    Regex non_dot_regex = new Regex(pattern_non_dot);
                    Match match_non_plmin = non_dot_regex.Match(regionText);
                    if (match_non_plmin.Success)
                    {
                        GroupCollection groups = match_non_plmin.Groups;
                        string g1 = groups[1].Value.ToString();
                        string g2 = groups[2].Value.ToString();
                        string g11 = string.Empty;
                        string g22 = string.Empty;
                        if (g1 != "" && (g1.Contains("-") || g1.Contains(" ")))
                        {
                            g11 = g1.Replace(" ", ".").Replace("-", ".");
                            regionText = regionText.Replace(g1, g11);
                        }
                        if (g2 != "" && (g2.Contains("-") || g2.Contains(" ")))
                        {
                            g22 = g2.Replace(" ", ".").Replace("-", ".");
                            regionText = regionText.Replace(g2, g22);
                        }
                    }
                }
                if (regionText.EndsWith("+") && regionText.Length > 1)
                {
                    regionText = regionText.Replace("+", "");
                }

                if (regionText.Contains(".") && !regionText.Contains("¡"))
                {

                    var res1 = regionText.Split('.').Last();
                    if (Regex.IsMatch(res1, @"^[a-zA-Z]+$"))
                    {
                        Is_Need_Ocr = false;
                    }
                    if (res1.Length == 0)
                    {
                        Is_Need_Ocr = false;
                    }
                    string[] res2 = regionText.Split('.');
                    if (res2[0] == "»" || res2[0] == "«")
                    {
                        Is_Need_Ocr = false;
                    }
                }
                if ((regionText.Contains(".") || regionText.Contains("/")) && regionText.Contains("°") && !regionText.Contains("X"))
                {
                    regionText = regionText.Replace("/", "").Replace(".", "").Replace("-", "");
                }
                if (regionText.Contains("T") || regionText.Contains("#") || regionText.Contains("ZZ") || regionText.Contains("//") || regionText.Contains("/") || regionText.Contains("--") || regionText.Contains(",,") || regionText.Contains("C") || regionText.Contains("D") || regionText.Contains(":") || regionText.Contains("î") || regionText.Contains("K") || regionText.Contains("VV") || regionText.Contains("SS") || regionText.Contains("HF") || regionText.Contains("çV") || regionText.Contains("7V") || regionText.Contains("I") || regionText.Contains("W«") || regionText.Contains("Mç") || (regionText.Contains("22") && !regionText.Contains(".")) && !regionText.Contains("PIN"))
                {
                    if (regionText.Length > 1 || regionText.Contains("-"))
                    {
                        if (regionText.Contains("-"))
                        {
                            string[] minussym = regionText.Split("-");
                            if ((Regex.IsMatch(minussym[0], @"^[0-9]+$") || Regex.IsMatch(minussym[1], @"^[0-9]+$") || isDigitPresent1) && !regionText.Contains("#"))
                            {
                                regionText = regionText.Replace("-", ".");
                            }
                        }
                        else if (regionText != "PIN")
                        {
                            Is_Need_Ocr = false;
                        }

                    }
                    else if (regionText.Length > 0)
                    {
                        Is_Need_Ocr = false;
                    }
                }
                int finalyaxis = imageheight - 200;
                int finalxaxis = imagewidth - 200;
                if (regionText.Contains("±") || regionText.Contains("û"))
                {
                    regionText = Regex.Replace(regionText, "[A-Za-z ]", "");
                    regionText = regionText.Replace("û", "");
                }
                if (regionText.Contains(".."))
                {
                    var res11 = regionText.Split("..");
                    if (res11.Length > 0)
                    {
                        if (res11[1].Length > 0)
                        {
                            regionText = regionText.Replace("..", "");
                        }
                    }
                }
                if (regionText.Contains("¡"))
                {
                    var res11 = regionText.Split('¡');
                    if (res11.Length > 0)
                    {
                        if (res11[1].Any(c => char.IsDigit(c)) == false)
                        {
                            return retval;
                        }
                    }
                }

                if (regionText.Contains("-") && !regionText.Contains("#"))
                {
                    var res1 = regionText.Split('-');
                    if (res1.Length > 0)
                    {
                        if ((res1[0].Any(c => char.IsDigit(c))))
                        {
                            regionText = regionText.Replace("-", ".");
                            if (regionText.Contains(".."))
                            {
                                var res11 = regionText.Split("..");
                                if (res11.Length > 0)
                                {
                                    if (res11[1].Length == 0)
                                    {
                                        regionText = regionText.Replace("..", "");
                                    }
                                }
                            }
                        }
                        else if (regionText.Contains("X"))
                        {
                            regionText = regionText.Replace("-", "");
                            int count = Regex.Matches(regionText, "X").Count;
                            if (count > 1)
                            {
                                string[] cntt = regionText.Split("X");
                                if (cntt[0] == "")
                                {
                                    regionText = regionText.Remove(0, 1);

                                }
                            }
                        }
                        else if (!regionText.Contains("+-"))
                        {
                            Is_Need_Ocr = false;
                        }
                    }
                }
                if (regionText != "" && Is_Need_Ocr == true)
                {
                    retval = regionText + "," + x.ToString() + "," + y.ToString() + "," + w.ToString() + "," + h.ToString();
                }
                return retval;
            }
            catch (Exception ex)
            {
                objerr.WriteErrorToText(ex);
                return retval;
            }
        }
        static List<List<Item>> GroupItemsByX(List<Item> items, int thresholdX)
        {
            List<List<Item>> groups = new List<List<Item>>();
            List<Item> currentGroup = null;

            // Sort items by X coordinate
            items.Sort((a, b) => a.Y.CompareTo(b.Y));
            var sortedItems = items.OrderBy(item => item.X).ThenBy(item => item.Y);


            // Group items based on X coordinate
            foreach (var item in sortedItems)
            {
                if (currentGroup == null || Math.Abs(currentGroup.Last().Y - item.Y + item.H) > thresholdX)
                {
                    // Start a new group
                    currentGroup = new List<Item>();
                    groups.Add(currentGroup);
                }

                currentGroup.Add(item);
            }

            return groups;
        }
        class Item
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int W { get; set; }
            public int H { get; set; }
            public string Text { get; set; }
            public bool isBallooned { get; set; }
        }
        class ActiveItems
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int W { get; set; }
            public int H { get; set; }
            public int NH { get; set; }
            public int GroupID { get; set; }
            public string Text { get; set; }
            public bool isBallooned { get; set; }
        }


        private OCRResults balloonProcess(
            string ImageFile,
            string drawingNo,
            string revNo,
            int pageNo,
            string desFile,
            string Balloon,
            string ocrtext,
            string Nominal,
            string Min,
            string Max,
            AutoBalloon searchForm,
            Int64 ocr_X,
            Int64 ocr_Y,
            Int64 ocr_W,
            Int64 ocr_H,
            decimal s_x,
            decimal s_y,
            decimal s_w,
            decimal s_h,
            string Type,
            string SubType,
            string Unit,
            int Num_Qty,
            string ToleranceType,
            string PlusTolerance,
            string MinusTolerance,
            bool isplmin,
            string isplmin_mintol,
            string isplmin_pltol,
            string isplmin_spec

            )
        {
            OCRResults oCRResults = new OCRResults();
            oCRResults.BaloonDrwID = 0;
            oCRResults.DrawingNumber = drawingNo;
            oCRResults.Revision = revNo.ToUpper();
            oCRResults.Page_No = pageNo;
            oCRResults.BaloonDrwFileID = desFile;
            oCRResults.ProductionOrderNumber = "N/A";
            oCRResults.Part_Revision = "N/A";
            oCRResults.Balloon = Convert.ToString(Balloon);
            oCRResults.Spec = ocrtext.ToString();
            oCRResults.Nominal = Nominal;
            oCRResults.Minimum = Min;
            oCRResults.Maximum = Max;
            oCRResults.MeasuredBy = username;
            oCRResults.MeasuredOn = DateTime.Now;
            if (searchForm.selectedRegion == "Selected Region")
            {

                oCRResults.Crop_X_Axis = (int)ocr_X  ;
                oCRResults.Crop_Y_Axis = (int)ocr_Y ;
                oCRResults.Crop_Width = (int)ocr_W ;
                oCRResults.Crop_Height = (int)ocr_H ;
                oCRResults.Circle_X_Axis = (int)ocr_X  ;
                oCRResults.Circle_Y_Axis = (int)ocr_Y  ;
                oCRResults.Circle_Width = 28;
                oCRResults.Circle_Height = 28;
            }
            else
            {
                if (ocr_X < 28)
                {
                    oCRResults.Crop_X_Axis = (int)ocr_X + 29;
                    oCRResults.Circle_X_Axis = (int)ocr_X + 29;
                }
                else
                {
                    oCRResults.Crop_X_Axis = (int)ocr_X;
                    oCRResults.Circle_X_Axis = (int)ocr_X;
                }
                oCRResults.Crop_Y_Axis = (int)ocr_Y;
                oCRResults.Crop_Width = (int)ocr_W;
                oCRResults.Crop_Height = (int)ocr_H;
                oCRResults.Circle_Y_Axis = (int)ocr_Y;
                oCRResults.Circle_Width = 28;
                oCRResults.Circle_Height = 28;
            }
            oCRResults.Type = Type;
            oCRResults.SubType = SubType;
            oCRResults.Unit = Unit;
            oCRResults.Quantity = (int)Num_Qty;

            if (Min != "" && Max != "")
            {
                oCRResults.ToleranceType = ToleranceType;
            }
            else
            {
                oCRResults.ToleranceType = "Default";
            }
            if (ocrtext.Contains("R."))
            {
                oCRResults.ToleranceType = "Linear";
            }
            if (PlusTolerance != "")
            {
                oCRResults.PlusTolerance = "+" + PlusTolerance;
            }
            else
            {
                oCRResults.PlusTolerance = "0";
            }
            if (MinusTolerance != "")
            {
                oCRResults.MinusTolerance = "-" + MinusTolerance;
            }
            else
            {
                oCRResults.MinusTolerance = "0";
            }
            oCRResults.MaxTolerance = "";
            oCRResults.MinTolerance = "";
            oCRResults.CreatedBy = username;
            oCRResults.CreatedDate = DateTime.Now;
            oCRResults.ModifiedBy = "";
            oCRResults.ModifiedDate = DateTime.Now;
            oCRResults.x = (int)ocr_X;
            oCRResults.y = (int)ocr_Y;
            oCRResults.width = (int)ocr_W;
            oCRResults.height = (int)ocr_H;
            oCRResults.id = "";
            oCRResults.selectedRegion = "Full Image";
            if (isplmin && isplmin_mintol != "" && isplmin_pltol != "" && isplmin_spec != "")
            {
                oCRResults.Spec = isplmin_spec;
                oCRResults.Nominal = isplmin_spec;
                try
                {
                    oCRResults.Minimum = Convert.ToString(Convert.ToDecimal(isplmin_spec.Replace("¡", "")) - Convert.ToDecimal(isplmin_mintol));
                    oCRResults.Maximum = Convert.ToString(Convert.ToDecimal(isplmin_spec.Replace("¡", "")) + Convert.ToDecimal(isplmin_pltol));
                    oCRResults.MinusTolerance = "-" + Convert.ToString(isplmin_mintol);
                    oCRResults.PlusTolerance = "+" + Convert.ToString(isplmin_pltol);
                }
                catch (Exception)
                {

                }
                oCRResults.ToleranceType = "Linear";
                oCRResults.Type = "Dimension";
                oCRResults.Unit = "Inches";
                oCRResults.SubType = "Circularity";
            }
            if (ocrtext.Contains("BOX") || ocrtext.Contains("PIN"))
            {
                oCRResults.ToleranceType = "Linear";
                oCRResults.Unit = "";
                oCRResults.Type = "";
                oCRResults.SubType = "";
                oCRResults.Minimum = "0";
                oCRResults.Maximum = "0";
                oCRResults.PlusTolerance = "0";
                oCRResults.MinusTolerance = "0";
            }
            if (Type == "Surface Finish")
            {
                oCRResults.ToleranceType = "Linear";
            }
            byte[] imgbyt = new byte[] { 0x20 };
            oCRResults.CropImage = imgbyt;
            return oCRResults;
        }

        public string ocrTextTransform(string ocrtext)
        {
            try
            {
                if (ocrtext == "2180°")
                {
                    ocrtext = "2X180°";
                }
                if (ocrtext == "63" || ocrtext == "ç63" || ocrtext == "63(")
                {
                    ocrtext = "»";
                }
                if (ocrtext == "X5°" || ocrtext == "45Z" || ocrtext == "45" || ocrtext == "45O" || ocrtext == "42" || ocrtext == "45ç")
                {
                    ocrtext = "45°";
                }
                if (ocrtext == "32")
                {
                    ocrtext = "´";
                }
                if (ocrtext == "38" || ocrtext == "35" || ocrtext == "30" || ocrtext == "396"  || ocrtext == "380" || ocrtext == "390" || ocrtext == "300" || ocrtext == "3905" || ocrtext == "398" || ocrtext == "30O")
                {
                    ocrtext = "30°";
                }
                if (ocrtext == "150" || ocrtext == "15")
                {
                    ocrtext = "15°";
                }
                if (ocrtext == "100" || ocrtext == "10")
                {
                    ocrtext = "10°";
                }
                if (ocrtext == "250" || ocrtext == "25")
                {
                    ocrtext = "25°";
                }
                if (ocrtext == "200" || ocrtext == "29«")
                {
                    ocrtext = "20°";
                }
                if (ocrtext == "900")
                {
                    ocrtext = "90°";
                }
                if (ocrtext == "70")
                {
                    ocrtext = "7°";
                }
                if (ocrtext == "û")
                {
                    ocrtext = "";
                }
                if (ocrtext == "Rù6")
                {
                    ocrtext = "R.6";
                }
                if (ocrtext == "R.î3" || ocrtext == "R.îñ")
                {
                    ocrtext = "R.03";
                }
                if (ocrtext == ".îîóë")
                {
                    ocrtext = ".005";
                }
                if (ocrtext.Contains("#") && !(ocrtext.Contains("VAM") || ocrtext.Contains("PIN") || ocrtext.Contains("BOX") || ocrtext.Contains("SPCL")))
                {
                    string[] spltxt = ocrtext.Split("#");
                    bool isDigit1 = spltxt[0].Any(c => char.IsDigit(c));
                    bool isDigit2 = spltxt[1].Any(c => char.IsDigit(c));
                    if (!isDigit1 || !isDigit2)
                    {
                        ocrtext = "";
                    }
                }
                if (!Regex.IsMatch(ocrtext, @"^(((?:\d+\.\d+)|(\.\d+))(?:\s)?(?:[A-Z])+)$") && !Regex.IsMatch(ocrtext, @"^((?:¡)?(?:\d+)?(\.\d+))$") && Regex.IsMatch(ocrtext, @"^(((?:ç)|(?:─)|)(?:\s)?(?:¡)?(?:\s)?(?:\.\d+)(?:\/)?(?:\d+?\.\d+)?(?:\s)?(?:[A-Z]{1})?(?:Þ)?(?:\s)?(?:[A-Z]{1})?(?:Þ)?(?:\s)?(?:[A-Z]{1})?(?:Þ)?)$"))
                {

                    string mainValuePattern = @"^-?(([(?:¡)|(?:ç)|(?:─)|(?:\s+)]+)?([(?:\.\d+)|(?:/)|(?:\d+\.\d+)]+))?";
                    if (ocrtext.Contains("─"))
                    {
                        string[] spltxt = ocrtext.Split("─");
                        if (spltxt[1].Length > 0 && !spltxt[1].StartsWith(" "))
                        {
                            if (spltxt[0] == "") { ocrtext = "─" + " " + spltxt[1]; }
                            else { ocrtext = spltxt[0] + "─ " + spltxt[1]; }
                        }
                    }

                    if (ocrtext.Contains("ç"))
                    {
                        string[] spltxt = ocrtext.Split("ç");
                        if (spltxt[1].Length > 0 && !spltxt[1].StartsWith(" "))
                        {
                            if (spltxt[0] == "") { ocrtext = "ç" + " " + spltxt[1]; }
                            else { ocrtext = spltxt[0] + "ç " + spltxt[1]; }

                        }
                    }


                    Match mainValueMatch = Regex.Match(ocrtext, mainValuePattern);
                    string mainValue = mainValueMatch.Value;
                    string[] mspltxt = ocrtext.Split(mainValue);
                    if (mspltxt[1].Length > 0)
                    {
                        if (Regex.IsMatch(mspltxt[1], @"^[A-Z]")) { ocrtext = mainValue + " " + mspltxt[1]; }

                        
                    }
                    Dictionary<string, string> surface_replace = new Dictionary<string, string>{
                                {".","ù"},
                                {"0","î"},
                                {"1","ï"},
                                {"2","ð"},
                                {"3","ñ"},
                                {"4", "ò"},
                                {"5","ó"},
                                {"6","ô"},
                                {"7", "õ"},
                                {"8","ö" },
                                {"9","÷" },

                                {"─","û" },
                                {"¡","à" },
                                {" ","ì" },

                                {"A","À" },
                                {"B","Á" },
                                {"C","Â" },
                                {"D","Ã" },
                                {"E","Ä" },
                                {"F","Å" },
                                {"G","Æ" },
                                {"H","Ç" },
                                {"I","È" },
                                {"J","É" },
                                {"K","Ê" },
                                {"L","Ë" },
                                {"M","Ì" },
                                {"N","Í" },
                                {"O","Î" }



                               };

                    foreach (var replacement in surface_replace)
                    {
                        ocrtext = ocrtext.Replace(replacement.Key, replacement.Value);
                    }
                    ocrtext = "ë" + ocrtext + "í";

                }
                if (Regex.IsMatch(ocrtext, @"\b(MIN|MAX)\b"))
                {
                    ocrtext = ocrtext.Replace("MIN", "").Replace("MAX", "");
                }
                return ocrtext;
            }
            catch (Exception ex)
            {
                return ocrtext;
            }
        }

        public void checkedPluseMinuse(string ocrtext, out object isplmin)
        {
            try
            {
                if (ocrtext.Contains("+") && ocrtext.Contains("-"))
                {
                    string mainValuePattern = @"^-?((?:¡|)(\d+|(?:\.d+)|)(?:\.\d+))?";
                    string positiveValuePattern = @"\+((\d+|(?:\.d+)|)(?:\.\d+))?";
                    string negativeValuePattern = @"-((\d+|(?:\.d+)|)(?:\.\d+))?";

                    Match mainValueMatch = Regex.Match(ocrtext, mainValuePattern);
                    MatchCollection positiveValueMatches = Regex.Matches(ocrtext, positiveValuePattern);
                    MatchCollection negativeValueMatches = Regex.Matches(ocrtext, negativeValuePattern);


                    string mainValue = mainValueMatch.Value;
                    string positive = string.Empty;
                    string negative = string.Empty;
                    foreach (Match match in positiveValueMatches)
                    {
                        positive = match.Value;
                    }
                    foreach (Match match in negativeValueMatches)
                    {
                        negative = match.Value;
                    }
                    isplmin = new { isplmin = true, isplmin_spec = mainValue, isplmin_pltol = positive.Replace("+", ""), isplmin_mintol = negative.Replace("-", "") };

                }
                else
                {
                    isplmin = new { isplmin = false, isplmin_spec = "", isplmin_pltol = "", isplmin_mintol = "" };

                }
            }
            catch (Exception ex)
            {
                isplmin = new { isplmin = false, isplmin_spec = "", isplmin_pltol = "", isplmin_mintol = "" };

            }
        }
        public void getQty(string ocrtext, out int Num_Qty)
        {
            try
            {


                bool isDigitPresent = ocrtext.Any(c => char.IsDigit(c));
                string qty = "";
                if (!ocrtext.Contains("BOX") && ocrtext.Contains("X") && isDigitPresent)
                {
                    if (ocrtext.Length > 2)
                    {
                        int count = Regex.Matches(ocrtext, "X").Count;
                        if (count > 1)
                        {
                            string[] result4 = ocrtext.Split('X');
                            if (Char.IsNumber(result4[0], 0))
                            {
                                qty = result4[0];
                            }
                            else if (Char.IsNumber(result4[1], 0))
                            {
                                qty = result4[1];
                            }
                        }
                        else
                        {
                            qty = ocrtext.Substring(0, ocrtext.IndexOf("X")).Replace(" ", "");
                        }
                    }
                    else
                    {
                        qty = ocrtext.Substring(0, ocrtext.IndexOf("X")).Replace(" ", "");
                    }
                }
                int value;
                if (int.TryParse(qty, out value)) { Num_Qty = Convert.ToInt16(qty); }
                else { Num_Qty = 1; }


            }
            catch (Exception ex)
            {
                Num_Qty = 1;
            }

        }
        #endregion

        #region Data Connection
        public string getConnectionStr(DataTable dtMPM)
        {
            string connstr = string.Empty;
            string IsCrypto = this._appSettings.IsCrypto;
            if (string.IsNullOrEmpty(Convert.ToString(dtMPM.Rows[0]["Authendication"])) != true)
            {
                if (Convert.ToString(dtMPM.Rows[0]["Authendication"]) == "SQL Server")
                {
                    if (string.IsNullOrEmpty(IsCrypto) != true)
                    {
                        if (IsCrypto.ToLower() == "yes")
                        {
                            connstr = "Data Source=" + Convert.ToString(dtMPM.Rows[0]["Datasource"]) + ";Initial Catalog=" + Convert.ToString(dtMPM.Rows[0]["DBName"]) + ";User ID=" + Convert.ToString(dtMPM.Rows[0]["UserID"]) + ";Password=" + Decrypt(Convert.ToString(dtMPM.Rows[0]["Password"])) + ";TrustServerCertificate=True;Encrypt=False;Connection Timeout=30;";
                        }
                        else
                        {
                            connstr = "Data Source=" + Convert.ToString(dtMPM.Rows[0]["Datasource"]) + ";Initial Catalog=" + Convert.ToString(dtMPM.Rows[0]["DBName"]) + ";User ID=" + Convert.ToString(dtMPM.Rows[0]["UserID"]) + ";Password=" + (Convert.ToString(dtMPM.Rows[0]["Password"])) + ";TrustServerCertificate=True;Encrypt=False;Connection Timeout=30;";
                        }
                    }
                    else
                    {
                        connstr = "Data Source=" + Convert.ToString(dtMPM.Rows[0]["Datasource"]) + ";Initial Catalog=" + Convert.ToString(dtMPM.Rows[0]["DBName"]) + ";User ID=" + Convert.ToString(dtMPM.Rows[0]["UserID"]) + ";Password=" + (Convert.ToString(dtMPM.Rows[0]["Password"])) + ";TrustServerCertificate=True;Encrypt=False;Connection Timeout=30;";
                    }
                }
                else
                {
                    connstr = @"Data Source=" + Convert.ToString(dtMPM.Rows[0]["Datasource"]) + ";Initial Catalog=" + Convert.ToString(dtMPM.Rows[0]["DBName"]) + ";Integrated Security=True;TrustServerCertificate=True;Encrypt=False;Connection Timeout=30;";
                }
            }
            return connstr;
        }
        #endregion

        #region Collection
        public DataTable RequestHeader(DataTable dtFiles_Header)
        {
            List<string> header = new List<string> { "DrawingNo", "Part_No", "Revision_No", "PRevisionNo", "sessionId" };
            for (var i = 0; i < header.Count; i++)
            {
                dtFiles_Header.Columns.Add(header[i].ToString(), typeof(string));
                dtFiles_Header.Columns[i].ColumnName = header[i].ToString();
            }
            return dtFiles_Header;
        }
        public DataTable RequestProduct(DataTable dtFiles_Production)
        {
            List<string> header = new List<string> { "FileName", "FilePath", "Annotation", "Drawing", "CurrentPage", "TotalPage", "partial", "resize" }; //"rotation",
            for (var i = 0; i < header.Count; i++)
            {
                dtFiles_Production.Columns.Add(header[i].ToString(), typeof(string));
                dtFiles_Production.Columns[i].ColumnName = header[i].ToString();
            }
            return dtFiles_Production;
        }
        #endregion

        #region Traits

        class XCoordinateComparer : IComparer<int>
        {
            public int Compare(int x1, int x2)
            {
                // Check if the x-coordinates are within 300 units
                if ((Math.Sign(x1 - x2) == -1 && Math.Abs(x1 - x2) <= 300) || Math.Abs(x1 - x2) <= 300)
                //if (Math.Abs(x1 - x2) < 300)
                {
                    // If within 300 units, consider them equal
                    return 0;
                }
                else
                {
                    // Otherwise, compare based on x-coordinate
                    return x1.CompareTo(x2);
                }
            }
        }

        class GroupCoordinateComparer : IComparer<int>
        {
            public int Compare(int y1, int y2)
            {
                return y1.CompareTo(y2);
            }
        }
        #endregion

        #region Misc
        private Dictionary<int, List<AutoBalloon_OCR>> GroupItemsByProximity(List<AutoBalloon_OCR> items, int threshold)
        {
            var groupedItems = new Dictionary<int, List<AutoBalloon_OCR>>();
            int groupCount = 1;

            foreach (var item in items)
            {
                bool addedToGroup = false;

                // Check if the item is close to any existing group
                foreach (var group in groupedItems.Values)
                {
                    var closestItem = group.OrderBy(i => CalculateDistance(item, i)).First();

                    if (CalculateDistance(item, closestItem) <= threshold)
                    {
                        group.Add(item);
                        group.Sort((a, b) => a.X_Axis.CompareTo(b.X_Axis));
                        addedToGroup = true;
                        break;
                    }
                }

                // If not close to any existing group, create a new group
                if (!addedToGroup)
                {
                    groupedItems.Add(groupCount, new List<AutoBalloon_OCR> { item });
                    groupCount++;
                }
            }

            return groupedItems;
        }
        private double CalculateDistance(AutoBalloon_OCR item1, AutoBalloon_OCR item2)
        {
            return Math.Sqrt(Math.Pow(item1.X_Axis - item2.X_Axis, 2) + Math.Pow(item1.Y_Axis - item2.Y_Axis, 2));
        }
        public bool IsStringNumeric(string input)
        {
            return int.TryParse(input, out _);
        }
        public int CountDigits(string input)
        {
            int count = 0;

            foreach (char c in input)
            {
                if (char.IsDigit(c))
                {
                    count++;
                }
            }

            return count;
        }
        public int CountAlphabetChars(string input)
        {
            int count = 0;
            foreach (char c in input)
            {
                if (char.IsLetter(c))
                {
                    count++;
                }
            }
            return count;
        }
        private double CalculateDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }
        private List<AutoBalloon_OCR> OrderOCR(IList<AutoBalloon_OCR> regions)
        {
            var sort = regions.ToList();
            sort.Sort((l, r) =>
            {
                if (Math.Abs(l.X_Axis - r.Y_Axis) < 15)
                    return l.X_Axis.CompareTo(r.X_Axis);
                return l.Y_Axis.CompareTo(r.Y_Axis);
            });

            return sort;
        }
        #endregion

        #region Emgu library
        private string OcrImage(Emgu.CV.OCR.Tesseract ocr, Emgu.CV.Mat image, Emgu.CV.Mat imageColor)
        {
            try
            {
                Emgu.CV.Structure.Bgr drawCharColor = new Emgu.CV.Structure.Bgr(System.Drawing.Color.Red);
                if (image.NumberOfChannels == 1)
                    Emgu.CV.CvInvoke.CvtColor(image, imageColor, Emgu.CV.CvEnum.ColorConversion.Gray2Bgr);
                else
                    image.CopyTo(imageColor);
                ocr.SetImage(imageColor);
                if (ocr.Recognize() != 0)
                    throw new Exception("Failed to recognizer image");
                Emgu.CV.OCR.Tesseract.Character[] characters = ocr.GetCharacters();

                # region Tryout
                /*
                foreach (Emgu.CV.OCR.Tesseract.Character character in characters)
                {
                    // Retrieve text
                    string text = character.Text;

                    // Retrieve position (bounding box)
                    System.Drawing.Rectangle boundingBox = character.Region;

                    // Display text and position
                    Console.WriteLine("Text: " + text);
                    Console.WriteLine("Position: " + boundingBox);
                }
                */
                #endregion

                if (characters.Length == 0)
                {
                    Emgu.CV.Mat imgGrey = new Emgu.CV.Mat();
                    Emgu.CV.CvInvoke.CvtColor(image, imgGrey, ColorConversion.Bgr2Gray);
                    Emgu.CV.Mat imgThresholded = new Emgu.CV.Mat();
                    Emgu.CV.CvInvoke.Threshold(imgGrey, imgThresholded, 65, 255, ThresholdType.Binary);
                    GC.Collect();
                    _ocr.SetImage(imgThresholded);
                    imageColor = imgThresholded;
                    if (characters.Length == 0)
                    {
                        Emgu.CV.CvInvoke.Threshold(image, imgThresholded, 190, 255, ThresholdType.Binary);
                        ocr.SetImage(imgThresholded);
                        imageColor = imgThresholded;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return ocr.GetUTF8Text();
        }
        #endregion
        public DrawingSearchController(DimTestContext dbcontext, IOptions<AppSettings> options, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpcontext)
        {
            _appSettings = options.Value;
            _dbcontext = dbcontext;
            LoadConfigDetails();
            _httpClientFactory = httpClientFactory;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            _httpcontext = httpcontext;
        }

        #region API

        #region Get Drawing Images
        [HttpPost]
        [Route("GetDrawingByNumber")]
        public async Task<ActionResult<SearchForm>> GetDrawingByNumber(SearchForm searchForm)
        {
            try
            {
                sessionobj sessionData = getsession();
                if (sessionData == null)
                {
                    sessionData = setsession();
                }
                string connstr = string.Empty;
                dtFiles_Header = RequestHeader(dtFiles_Header);
                dtFiles_Production = RequestProduct(dtFiles_Production);
                List<KeyValuesClass> response = new List<KeyValuesClass>();
                if (_dbcontext.TblConfigurations == null)
                {
                    return NotFound();
                }
                else
                {
                    string env = this._appSettings.ENVIRONMENT;
                    objerr.WriteErrorLog(" enc  " + env);
                    if (dsconfig.Tables.Count > 0)
                    {
                        string clientPath = envcpath + "\\ClientApp\\src\\drawing\\";
                        string externalApiUrl = this._appSettings.GetDrawingServiceUrlList;
                        string endPoint = "/" + searchForm.drawingNo.ToString() + "/" + searchForm.revNo.ToString();
                        string sessionUserId = string.Empty;
                        string OrgPath = clientPath + "drawing\\";
                        if (!Directory.Exists(OrgPath))
                        {
                            Directory.CreateDirectory(OrgPath);
                        }
                        if (searchForm.sessionUserId == "")
                        {
                            sessionUserId = sessionData.sessionUserId;
                        }
                        else
                        {
                            sessionUserId = searchForm.sessionUserId;
                        }
                        if (env != "development")
                        {
                            clientPath = clientPath   + sessionUserId + "\\";
                            if (!Directory.Exists(clientPath))
                            {
                                Directory.CreateDirectory(clientPath);
                            }
                            OrgPath = clientPath + "drawing\\";
                            if (!Directory.Exists(OrgPath))
                            {
                                Directory.CreateDirectory(OrgPath);
                            }
                        }
                        if (env != "development")
                        {
                            try
                            {
                                System.IO.DirectoryInfo deletablepreImage = new System.IO.DirectoryInfo(clientPath);
                                foreach (System.IO.FileInfo f in deletablepreImage.GetFiles())
                                {
                                    f.Delete();
                                }
                                System.IO.DirectoryInfo deletableorgImage = new System.IO.DirectoryInfo(OrgPath);
                                foreach (System.IO.FileInfo f in deletableorgImage.GetFiles())
                                {
                                    f.Delete();
                                }
                                objerr.WriteErrorLog(" externalApiUrl  " + externalApiUrl);
                                objerr.WriteErrorLog(" endPoint  " + endPoint);
                                using (HttpClient httpClient = new HttpClient())
                                {
                                    HttpResponseMessage res = httpClient.GetAsync(externalApiUrl + endPoint, HttpCompletionOption.ResponseContentRead).Result;
                                    var responseBody = await res.Content.ReadAsStringAsync();
                                    objerr.WriteErrorLog(" responseBody  " + Convert.ToString(responseBody));
                                    var json = JsonConvert.DeserializeObject<JToken>(Convert.ToString(responseBody));
                                    var data = json.Value<JToken>("dataList");
                                    var errorMessage = json["errorMessage"].Value<string>();
                                    var errorCode = json["errorCode"].Value<int>();
                                    if (errorMessage == "Successful")
                                    {
                                        DataRow dtHrow;
                                        DataRow dtFrow;
                                        dtHrow = dtFiles_Header.NewRow();
                                        dtHrow["DrawingNo"] = searchForm.drawingNo.ToString();
                                        dtHrow["Part_No"] = "";
                                        dtHrow["Revision_No"] = searchForm.revNo.ToString();
                                        dtHrow["PRevisionNo"] = "";
                                        dtHrow["sessionId"] = sessionUserId;
                                        dtFiles_Header.Rows.Add(dtHrow);
                                        int citem = 1;
                                        var total_page = data.Count();
                                        foreach (var item in data)
                                        {
                                            var curs = citem++;
                                            dtFrow = dtFiles_Production.NewRow();
                                            string Url = Convert.ToString(item.Value<JToken>("srvC_URL"));
                                            string prefix = searchForm.drawingNo.ToString() + "." + searchForm.revNo.ToString() + ".";
                                            prefix = prefix.ToUpper();
                                            var cur = "";
                                            if (curs.ToString().Length < 2)
                                            {
                                                cur = "0" + curs.ToString();
                                            }
                                            else
                                            {
                                                cur = curs.ToString();
                                            }
                                            var tot = "";
                                            if (total_page.ToString().Length < 2)
                                            {
                                                tot = "0" + total_page.ToString();
                                            }
                                            else
                                            {
                                                tot = total_page.ToString();
                                            }
                                            //var fileName = prefix + cur + "-" + tot + "of" + tot + ".png";
                                            string fileName = Convert.ToString(item.Value<JToken>("fileNames"));
                                            var filePath = clientPath + fileName;
                                            dtFrow["FileName"] = fileName;
                                            dtFrow["FilePath"] = filePath;
                                          //  dtFrow["rotation"] = 0;
                                            dtFrow["Annotation"] = filePath;
                                            dtFrow["Drawing"] = fileName;
                                            dtFrow["CurrentPage"] = curs - 1;
                                            dtFrow["TotalPage"] = total_page;
                                            objerr.WriteErrorLog(" Image source  " + curs + ":" + Url);
                                            objerr.WriteErrorLog(" Image Target  " + curs + ":" + filePath);
                                            // SaveExternalImage(Url, filePath, curs);
                                            using (WebClient client = new WebClient()) // WebClient class inherits IDisposable
                                            {
                                                client.UseDefaultCredentials = true;
                                                byte[] a = client.DownloadData(Url);
                                                string s = string.Empty;

                                                System.IO.File.WriteAllBytes(filePath, a);
                                                System.IO.File.WriteAllBytes(OrgPath + fileName, a);
                                                dtFrow["resize"] = scaleImage(filePath, citem - 2, true);
                                               // scaleImage(OrgPath + fileName, citem - 2, false);
                                            }
                                            dtFiles_Production.Rows.Add(dtFrow);
                                        }
                                        System.IO.DirectoryInfo deletableImage = new System.IO.DirectoryInfo(clientPath);
                                        foreach (System.IO.FileInfo f in deletableImage.GetFiles())
                                        {
                                            f.Delete();
                                        }
                                       // string rotate = rotateproperties(searchForm.drawingNo, searchForm.revNo, dtFiles_Production);
                                        // OrgPath = "";
                                        try
                                        {
                                            for (int k = 0; k < dtFiles_Production.Rows.Count; k++)
                                            {
                                                string tifImg = dtFiles_Production.Rows[k]["FilePath"].ToString();
                                                string desFile = dtFiles_Production.Rows[k]["FileName"].ToString();
                                                string resize = dtFiles_Production.Rows[k]["resize"].ToString();
                                                string Source = OrgPath + "\\" + desFile;

                                               // var matches = Regex.Matches(rotate, @"\d+");
                                               // int[] numbers = matches.Cast<Match>().Select(m => int.Parse(m.Value)).ToArray();
                                                string ImageFile = System.IO.Path.Combine(System.IO.Path.GetTempPath(), Guid.NewGuid().ToString() + desFile);
                                                System.IO.File.Copy(Source, ImageFile, true);
                                                /*
                                                if (numbers.Length > 0)
                                                {
                                                    try
                                                    {
                                                        dtFiles_Production.Rows[k]["rotation"] = numbers[k];
                                                        RotateImagefile(ImageFile, numbers[k]);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        dtFiles_Production.Rows[k]["rotation"] = 0;
                                                    }
                                                }
                                                */
                                                System.IO.File.Copy(ImageFile, tifImg, true);
                                                System.IO.File.Delete(ImageFile);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            objerr.WriteErrorToText(ex);
                                            string table = dtFiles_Production.ToString();
                                            objerr.WriteErrorLog(table);
                                        }
                                        IEnumerable<object> results = new List<object>();
                                        if (dtFiles_Production.Rows.Count > 0)
                                        {
                                            results = _dbcontext.TblBaloonDrawingLiners.Where(w => w.DrawingNumber == searchForm.drawingNo.ToString() && w.Revision == searchForm.revNo.ToString()).OrderBy(f => f.DrawLineID).ToList();
                                        }
                                        var returnObject = new List<object>();
                                        returnObject.Add(dtFiles_Production);
                                        returnObject.Add(dtFiles_Header);
                                        returnObject.Add(results);
                                        var lmtype = Load_MeasureType();
                                        var lmsubtype = Load_MeasureSubType();
                                        //var units = Load_UOM();
                                        returnObject.Add(lmtype);
                                        returnObject.Add(lmsubtype);
                                        returnObject.Add(new object[] { "Inches" }); // units 
                                        returnObject.Add(new object[] { "Linear", "Default" }); // Tolerance
                                        returnObject.Add(partial_image);
                                        return StatusCode(StatusCodes.Status200OK, returnObject);
                                    }
                                    else
                                    {
                                        return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status204NoContent, "Drawing does not exists against your Search.");
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                objerr.WriteErrorToText(ex);
                                return BadRequest("catch:Please Enter Valid Inputs.");
                            }
                        }
                        var dtM = dsconfig.Tables[0].AsEnumerable().Where(r => r.Field<string>("Environment") == "MPM");
                        DataTable dtMPM = dtM.CopyToDataTable();
                        if (dtMPM.Rows.Count > 0)
                        {
                            connstr = getConnectionStr(dtMPM);
                            string squery = "select ProductionOrderNo as [Production_Order_No],Drawing_No as [Drawing_No] ,DRevNo as [Revision_No],PartNo as [Part_No],PRevNo as [BOM_REV] from Tbl_BalloonDrawing WHERE Drawing_No = @Drawing_No   and DRevNo = @DRevNo";
                            string query = squery.Replace("@Drawing_No", "'" + searchForm.drawingNo.ToString() + "'").Replace("@DRevNo", "'" + searchForm.revNo.ToString() + "'");
                            BLL_BalloonDrawing objBLL_BalloonDrawing = new BLL_BalloonDrawing();
                            DataTable dtvalue = objBLL_BalloonDrawing.FetchDimensionDetails_ProductionOrderNo(connstr, query).Tables[0];
                            if (dtvalue.Rows.Count > 0)
                            {
                                var ext = new List<string> { ".png", ".PNG", ".Png" };
                                List<string> files = new List<string>();
                                for (int k = 0; k < dtvalue.Rows.Count; k++)
                                {
                                    DataRow dtHrow;
                                    dtHrow = dtFiles_Header.NewRow();
                                    dtHrow["DrawingNo"] = Convert.ToString(dtvalue.Rows[k]["Drawing_No"]);
                                    dtHrow["Part_No"] = Convert.ToString(dtvalue.Rows[k]["Part_No"]);
                                    dtHrow["Revision_No"] = Convert.ToString(dtvalue.Rows[k]["Revision_No"]);
                                    dtHrow["PRevisionNo"] = Convert.ToString(dtvalue.Rows[k]["BOM_REV"]);
                                    dtHrow["sessionId"] = sessionUserId;
                                    dtFiles_Header.Rows.Add(dtHrow);
                                    string search = Convert.ToString(dtvalue.Rows[k]["Drawing_No"]);
                                    var myFiles = Directory.GetFiles(objSettings.DrawingFolder, search.Trim() + ".*", SearchOption.TopDirectoryOnly)
                                         .Where(s => ext.Contains(System.IO.Path.GetExtension(s)));
                                    foreach (string s in myFiles)
                                    {
                                        files.Add(s);
                                    }
                                }
                                List<string> filess = new List<string>();
                                for (int k = 0; k < dtvalue.Rows.Count; k++)
                                {
                                    string search = Convert.ToString(dtvalue.Rows[k]["Drawing_No"]);
                                    var myFiles = Directory.GetFiles(objSettings.DrawingFolder, search.Trim() + ".*", SearchOption.TopDirectoryOnly)
                                         .Where(s => ext.Contains(System.IO.Path.GetExtension(s)));
                                    foreach (string s in myFiles)
                                    {
                                        filess.Add(s);
                                    }
                                }
                                int sc = 0;
                                foreach (string s in filess)
                                {
                                    DataRow dtrow;
                                    dtrow = dtFiles_Production.NewRow();
                                    dtrow["FileName"] = s.Replace(objSettings.DrawingFolder, "").Replace("\\", "");
                                    dtrow["FilePath"] = s;
                                    dtrow["resize"] = scaleImage(s, sc, true);
                                    dtFiles_Production.Rows.Add(dtrow);
                                    sc++;
                                }
                                if (dtFiles_Production.Rows.Count > 0)
                                {
                                    string clientLocalPath = "";
                                    System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(clientPath);
                                    foreach (System.IO.FileInfo file in di.GetFiles())
                                    {
                                        file.Delete();
                                    }
                                   // string rotate = rotateproperties(searchForm.drawingNo, searchForm.revNo, dtFiles_Production);
                                    for (int k = 0; k < dtFiles_Production.Rows.Count; k++)
                                    {
                                        string tifImg = dtFiles_Production.Rows[k]["FilePath"].ToString();
                                        FileInfo fi = new FileInfo(tifImg);
                                        string desFile = fi.Name;
                                        string pngImg = fi.DirectoryName + "\\" + desFile;
                                        string clientImg = clientPath + desFile;
                                        string clientLocalPathImg = clientLocalPath + desFile;
                                        if (String.Equals(fi.Extension.ToLower(), ".tif", StringComparison.OrdinalIgnoreCase) || String.Equals(fi.Extension.ToLower(), ".tiff", StringComparison.OrdinalIgnoreCase))
                                        {
                                            System.Drawing.Bitmap.FromFile(tifImg).Save(pngImg, System.Drawing.Imaging.ImageFormat.Png);
                                        }
                                      //  var matches = Regex.Matches(rotate, @"\d+");
                                      //  int[] numbers = matches.Cast<Match>().Select(m => int.Parse(m.Value)).ToArray();
                                        string ImageFile = System.IO.Path.Combine(System.IO.Path.GetTempPath(), Guid.NewGuid().ToString() + desFile);
                                        System.IO.File.Copy(tifImg, ImageFile, true);
                                        /*
                                        if (numbers.Length > 0)
                                        {
                                            try
                                            {
                                                dtFiles_Production.Rows[k]["rotation"] = numbers[k];
                                                RotateImagefile(ImageFile, numbers[k]);
                                            }
                                            catch (Exception ex)
                                            {
                                                dtFiles_Production.Rows[k]["rotation"] = 0;
                                            }
                                        }
                                        else
                                        {
                                            dtFiles_Production.Rows[k]["rotation"] = 0;
                                        }
                                        */
                                        System.IO.File.Copy(ImageFile, clientImg, true);
                                        System.IO.File.Delete(ImageFile);
                                        dtFiles_Production.Rows[k]["Annotation"] = fi.DirectoryName + "\\" + fi.Name;
                                        dtFiles_Production.Rows[k]["Drawing"] = clientLocalPathImg;
                                        Page_Revision_Details(desFile);
                                        dtFiles_Production.Rows[k]["CurrentPage"] = drawing_page_no;
                                        dtFiles_Production.Rows[k]["TotalPage"] = drawing_total_page_no;
                                    }
                                    IEnumerable<object> results = new List<object>();

                                    if (dtFiles_Production.Rows.Count > 0)
                                    {
                                        for (int k = 1; k <= dtFiles_Production.Rows.Count; k++)
                                        {
                                            dtFiles_Production.Rows[k - 1]["CurrentPage"] = k;
                                            dtFiles_Production.Rows[k - 1]["TotalPage"] = dtFiles_Production.Rows.Count;
                                        }
                                        results = _dbcontext.TblBaloonDrawingLiners.Where(w => w.DrawingNumber == searchForm.drawingNo.ToString() && w.Revision == searchForm.revNo.ToString()).OrderBy(f => f.DrawLineID).ToList();
                                    }

                                    var returnObject = new List<object>();
                                    returnObject.Add(dtFiles_Production);
                                    returnObject.Add(dtFiles_Header);
                                    returnObject.Add(results);
                                    var lmtype = Load_MeasureType();
                                    var lmsubtype = Load_MeasureSubType();
                                    var units = Load_UOM();
                                    returnObject.Add(lmtype);
                                    returnObject.Add(lmsubtype);
                                    returnObject.Add(new object[] { "Inches" }); // units 
                                    returnObject.Add(new object[] { "Linear", "Default" }); // Tolerance
                                    returnObject.Add(partial_image);


                                    return StatusCode(StatusCodes.Status200OK, returnObject);
                                }
                                else
                                {
                                    return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status204NoContent, "Drawing does not exists against your Search.");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objerr.WriteErrorToText(ex);
                return BadRequest("Oops!.. Something Went wrong.Please Try later.");
            }
            return BadRequest("Please Enter Valid Inputs.");
        }
        #endregion

        #region Manual Balloon
        [HttpPost]
        [Route("SplBalloon")]
        public async Task<ActionResult<AutoBalloon>> SplBalloon(AutoBalloon searchForm)
        {
            IEnumerable<object> returnObject = new List<object>();
            BubbleDrawingAutomationWeb.Controllers.BalloonController balcon = new BubbleDrawingAutomationWeb.Controllers.BalloonController(_dbcontext);
            BubbleDrawingAutomationWeb.Controllers.CreateBalloon objbaldet = new BubbleDrawingAutomationWeb.Controllers.CreateBalloon();
            if (_dbcontext.TblConfigurations == null)
            {
                return NotFound();
            }
            else
            {
                List<OCRResults> lstoCRResults = searchForm.originalRegions.Where(x1 => x1.isballooned == true).ToList();
                List<OCRResults> previous = searchForm.originalRegions.Where(x1 => x1.isballooned == true).ToList();
                try
                {
                    decimal s_x = 0;
                    decimal s_y = 0;
                    decimal s_w = 0;
                    decimal s_h = 0;
                    string dtFiles = searchForm.drawingDetails;
                    FileInfo fi = new FileInfo(dtFiles);
                    string desFile = fi.Name;
                    string OrgPath = dtFiles;
                    string env = this._appSettings.ENVIRONMENT;
                    if (env != "development")
                    {
                        OrgPath = dtFiles.Replace(desFile, "") + "\\drawing\\" + desFile;
                    }
                    string SelImageFile = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "SelImageFile_" + Guid.NewGuid().ToString() + desFile);
                    string Fname = fi.Name;
                    temp = System.IO.Path.Combine(System.IO.Path.GetTempPath(), Guid.NewGuid().ToString() + $"{Fname}");
                    System.IO.File.Copy(OrgPath, temp, true);
                    //RemoveOpaqueColormapFrom1BPP(OrgPath);
                    if (searchForm.bgImgRotation != 0)
                    {
                     //   RotateImagefile(temp, searchForm.bgImgRotation);
                    }
                    System.IO.File.Copy(temp, SelImageFile, true);
                    if (searchForm.selectedRegion == "Spl")
                    {
                        List<OCRResults> request = searchForm.originalRegions.Where(x1 => x1.isballooned == false).ToList();
                        foreach (var obj in request)
                        {
                            s_x = obj.x;
                            s_y = obj.y;
                            s_w = obj.width;
                            s_h = obj.height;

                            System.Drawing.RectangleF rectElipse = new System.Drawing.RectangleF((float)obj.x, (float)obj.y, (float)obj.width, (float)obj.height);
                            lstCircle.Add(new Circle_AutoBalloon { Bounds = rectElipse });
                            string OriginalImage = OrgPath;
                            System.Drawing.Rectangle rectElipse1 = new System.Drawing.Rectangle((int)obj.x, (int)obj.y, (int)obj.width, (int)obj.height);
                            Bitmap originalImage = new Bitmap(SelImageFile);
                            Bitmap croppedImage = CropImage(originalImage, rectElipse1);
                            // Create a new image with a specific resolution (e.g., 300 DPI)
                            Bitmap newImage = ChangeResolution(croppedImage, 300.0f);
                            string cropname = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "cropname_" + Guid.NewGuid().ToString() + $"text_region.png");
                            // Save or use the new image
                            newImage.Save(cropname);
                            temp = cropname;
                            System.Drawing.Image img = System.Drawing.Image.FromFile(temp);
                            byte[] bytes = null;
                            byte[] debugBytes = null;
                            bytes = imageToByteArray(img).ToArray();
                            int filesize = bytes.Length;
                            String ocredText = string.Empty;
                            Emgu.CV.Mat resul = new Emgu.CV.Mat();
                            Emgu.CV.Mat source = new Emgu.CV.Mat(temp);
                            string customLanguagePath = (new System.IO.DirectoryInfo(Environment.CurrentDirectory).FullName) + @"\tessdata";
                            _ocr = new Emgu.CV.OCR.Tesseract(customLanguagePath, "IMSsym1", Emgu.CV.OCR.OcrEngineMode.Default);
                            string ocrtext = OcrImage(_ocr, source, resul);
                            objerr.WriteErrorLog(" Words " + ocrtext);
                            objerr.WriteErrorLog(" cropped " + temp);
                            bool isplmin = false;
                            string isplmin_spec = "";
                            string isplmin_pltol = "";
                            string isplmin_mintol = "";
                            string[] linesArray = ocrtext.Split(new string[] { "\n", Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                            linesArray = linesArray.Where(o => o != "ë ." && o != "/ ." && o != "Z-J:" && o != "-." && o != "ë" && o != "û" && o != "Z ë :").ToArray();
                            if (linesArray.Length > 1)
                            {
                                if (linesArray[0].Length == 1 && Regex.IsMatch(linesArray[0], @"^[$A-Za-z]+$"))
                                {
                                    linesArray = linesArray.Where(item => item != linesArray[0]).ToArray();
                                }
                                string[] resultArray = linesArray.Select(s => Regex.Replace(s, "³", string.Empty)).ToArray();
                                linesArray = resultArray;
                            }
                            if (linesArray.Length > 1)
                            {
                                if (linesArray[0].Contains("X"))
                                {
                                    string[] hastext = linesArray[0].Split("X");
                                    if (hastext[0].Length > 1)
                                    {
                                        hastext[0] = Regex.Replace(hastext[0], @"[^\d.]", "");
                                        linesArray[0] = hastext[0] + "X" + hastext[1];
                                    }
                                    if (hastext[0].Trim().Length > 0 && hastext[1].Trim().Length > 0 && Regex.IsMatch(linesArray[0], @"^((\d+(\.\d+))|(\d+)|(\.\d+))X((?:\s|)(\d+))$"))
                                    {
                                        var deg = hastext[1].Trim();
                                        if (hastext[1].Trim().Length == 3 && hastext[1].Trim().EndsWith("0"))
                                        {
                                            hastext[1] = hastext[1].Substring(0, hastext[1].Length - 1) + "°";
                                        }
                                        linesArray[0] = hastext[0] + "X" + hastext[1];
                                    }
                                }
                            }
                            for (int i = 0; i < linesArray.Length; i++)
                            {
                                if (linesArray[i].Contains("»") && linesArray[i].Length > 2)
                                    linesArray[i] = linesArray[i].Replace("»", "¨");
                                if (linesArray[i].Contains(":") && linesArray[i].Contains("±"))
                                    linesArray[i] = linesArray[i].Replace(":", ".");
                                if (linesArray[i].StartsWith("L"))
                                    linesArray[i] = linesArray[i].Substring(1);

                                if (linesArray[i].StartsWith("I"))
                                    linesArray[i] = linesArray[i].Replace("I", "");
                                if (linesArray[i].EndsWith("I"))
                                    linesArray[i] = linesArray[i].Replace("I", "");
                                if (linesArray[i].StartsWith("X"))
                                    linesArray[i] = linesArray[i].Substring(1);
                                if (linesArray[i].Contains("#"))
                                    linesArray[i] = "";
                                linesArray[i] = Regex.Replace(linesArray[i], @"\r\n?|\n", "");
                                if (Regex.IsMatch(linesArray[i], @"^((:?═)+(\d+\.\d+|\.\d+))$"))
                                    linesArray[i] = Regex.Replace(linesArray[i], @"^═", "");
                                linesArray[i] = linesArray[i]
                                            .Replace("î", "0")
                                            .Replace("ï", "1")
                                            .Replace("ð", "2")
                                            .Replace("ñ", "3")
                                            .Replace("ò", "4")
                                            .Replace("ó", "5")
                                            .Replace("ô", "6")
                                            .Replace("õ", "7")
                                            .Replace("ö", "8")
                                            .Replace("÷", "9")
      
                                            ;
                                linesArray[i] = linesArray[i].Replace("(2)", "¡")
                                            .Replace("(Z)", "¡")
                                            .Replace("(/)", "¡");
                            }
                            
                            linesArray = linesArray.Where(o => o != "").ToArray();


                            if (ocrtext != "" && ocrtext != null)
                            {
                                if (linesArray.Length > 1)
                                {
                                    if (linesArray[0].Length > 1)
                                    {
                                        if (linesArray[0].Contains(".") || linesArray[0].Contains("X") || linesArray[0].Any(c => char.IsDigit(c)) || linesArray[0].Contains("°"))
                                        {
                                            ocrtext = linesArray[0].TrimEnd('.');
                                        }
                                    }
                                    else if (linesArray[1].Length > 1)
                                    {
                                        if (linesArray[1].Contains(".") || linesArray[1].Contains("X") || linesArray[1].Any(c => char.IsDigit(c)) || linesArray[1].Contains("°"))
                                        {
                                            ocrtext = linesArray[1].TrimEnd('.');
                                        }
                                    }
                                    if (linesArray[0].Length > 0 && linesArray[1].Length > 0)
                                    {
                                        if (linesArray[0].Contains("X") && (linesArray[1].Any(c => char.IsDigit(c)) || linesArray[1].Contains(".") || linesArray[1].Contains("°") || linesArray[1].Contains("±")))
                                        {
                                            if (linesArray[1].Contains("±") && linesArray[1].Contains("°"))
                                            {
                                                string[] spstr = linesArray[1].Split("±");
                                                if (spstr[1].Length > 0)
                                                {
                                                    if (spstr[1].Contains("°"))
                                                    {
                                                        string newvall = spstr[0].Remove(spstr[0].LastIndexOf("0"));
                                                        ocrtext = linesArray[0].TrimEnd('.') + newvall.TrimEnd('.') + "°" + "±" + spstr[1].TrimEnd('.');
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                ocrtext = linesArray[0].TrimEnd('.') + linesArray[1].TrimEnd('.');
                                            }
                                        }
                                    }
                                    if (linesArray[0].Contains("+"))
                                    {
                                        isplmin = true;
                                        string substr = linesArray[0].Substring(ocrtext.IndexOf("+") + 1).Replace(" ", "");
                                        isplmin_pltol = substr.Replace("+", "").Replace("═", "").Replace("-", "").Replace("O10", "010").TrimEnd('.');
                                    }
                                    if (linesArray[1].Contains("..") || linesArray[1].Contains("-"))
                                    {
                                        if (linesArray[1].Contains(".."))
                                        {
                                            isplmin_spec = linesArray[1].Split("..")[0].Replace("(/)", "¡").Replace("(2)", "¡").Replace("à", "¡").TrimEnd('.');
                                            ocrtext = isplmin_spec.TrimEnd('.');
                                            isplmin_mintol = linesArray[1].Split("..")[1].Replace("Oîî", "000").TrimEnd('.');
                                            if (!isplmin_mintol.Contains("."))
                                            {
                                                isplmin_mintol = "." + isplmin_mintol.Replace("I", "").Replace("Oîï", "001").Replace("Oîó", "005").Replace("îîó", "005").Trim().TrimEnd('.');
                                            }
                                            if (isplmin_spec.Contains("X"))
                                            {
                                                ocrtext = isplmin_spec.TrimEnd('.');
                                            }
                                        }
                                        if (linesArray[1].Contains("-"))
                                        {
                                            int count = Regex.Matches(linesArray[1], "-").Count;
                                            if (count > 1)
                                            {
                                                int index = linesArray[1].IndexOf("-", StringComparison.OrdinalIgnoreCase);

                                                if (index != -1)
                                                {
                                                    linesArray[1] = linesArray[1].Substring(0, index) + "." + linesArray[1].Substring(index + 1);
                                                }
                                            }
                                            isplmin_spec = linesArray[1].Split("-")[0].TrimEnd('.').Replace("ë", "").Replace("P", "0");
                                            if (Regex.IsMatch(isplmin_spec, @"[^$a-zA-Z.]+$"))
                                            {
                                                isplmin_spec = Regex.Replace(isplmin_spec, @"^[$a-zA-Z.]+$", "");
                                            }
                                            ocrtext = isplmin_spec.TrimEnd('.').Replace("ë", "");
                                            if (isplmin_spec.Contains("X"))
                                            {
                                                ocrtext = isplmin_spec.TrimEnd('.');
                                                isplmin_spec = isplmin_spec.Split("X")[1].TrimEnd('.');
                                            }
                                            isplmin_mintol = linesArray[1].Split("-")[1].TrimEnd('.');
                                            if (Regex.IsMatch(isplmin_mintol, @"[^$0-9.]+$"))
                                            {
                                                isplmin_mintol = Regex.Replace(isplmin_mintol, @"^[$0-9.]+$", "");
                                            }
                                        }
                                    }
                                }
                                string mainItem = string.Empty;
                                mainItem = linesArray.FirstOrDefault(s => s.Contains("¡"));
                                if (mainItem != null && linesArray.Length > 1 && !ocrtext.Contains("¡"))
                                {
                                    var result = linesArray.Select((s, i) => new { Item = s, IsMain = s.Contains("¡"), index = i }).ToArray();
                                    if (result.Length > 0)
                                    {
                                        var mainFound = result.FirstOrDefault(s => s.IsMain);
                                        var subFound = result.FirstOrDefault(s => !s.IsMain);
                                        if (mainFound != null)
                                        {
                                            linesArray[1] = mainFound.Item;
                                            linesArray[0] = subFound.Item;
                                            if (linesArray[0].Contains("-"))
                                            {
                                                isplmin = true;
                                                string substr = linesArray[0].Substring(ocrtext.IndexOf("-") + 1).Replace(" ", "");
                                                isplmin_mintol = substr.Replace("-", "").Replace("═", "").Replace("+", "").Replace("O10", "010").TrimEnd('.');
                                            }
                                            if (linesArray[1].Contains("+"))
                                            {
                                                isplmin = true;
                                                isplmin_spec = linesArray[1].Split("+")[0];
                                                ocrtext = isplmin_spec.TrimEnd('.').Replace("ë", "");
                                                isplmin_pltol = linesArray[1].Split("+")[1].Replace("Oîî", "000").Replace("Oîï", "001").Replace("Oîó", "005").Replace("îîó", "005").Trim().TrimEnd('.');
                                                if (Regex.IsMatch(isplmin_pltol, @"[^$0-9.]+$"))
                                                {
                                                    isplmin_pltol = Regex.Replace(isplmin_pltol, @"^[$0-9.]+$", "");
                                                }
                                                if (!isplmin_pltol.Contains("."))
                                                {
                                                    isplmin_pltol = "." + isplmin_pltol.Replace("I", "").Replace("Oîï", "001").Replace("Oîó", "005").Replace("îîó", "005").Trim().TrimEnd('.');
                                                }
                                            }

                                        }
                                    }
                                }
                                if (Regex.IsMatch(isplmin_pltol, @"[^$0-9.]+$"))
                                {
                                    isplmin_pltol = Regex.Replace(isplmin_pltol, @"^[$0-9.]+$", "");
                                }
                                if (Regex.IsMatch(isplmin_mintol, @"[^$0-9.]+$"))
                                {
                                    isplmin_mintol = Regex.Replace(isplmin_mintol, @"^[$0-9.]+$", "");
                                }
                                if (Regex.IsMatch(isplmin_spec, @"[^$0-9.]+$"))
                                {
                                    isplmin_spec = Regex.Replace(isplmin_spec, @"^[$0-9.]+$", "");
                                }
                                ocrtext = Regex.Replace(ocrtext, @"\r\n?|\n", "");
                                ocrtext = Regex.Replace(ocrtext, @"(\(\/\)|\([0-9]{1}\))", "");
                                string brackets = @"[\(\)]";
                                ocrtext = Regex.Replace(ocrtext, brackets, "");
                                if (ocrtext != "")
                                {

                                }
                                isplmin_pltol.Replace("I", "");
                                Dictionary<string, string> replacements = new Dictionary<string, string>{
                                    // { "ë", "I" },
                        { "î.", "O" },
                        { "çç", "" },
                        { "═", "" },
                        //{ "(","" },
                        //{ ")","" },
                        //{ " ", "" },
                        //  { "ù", "." },
                        { "EB", "─" },
                        {"XX",""},
                        {"##","" },
                        {"..","" },
                        {":","" },
                        {"«ç","" },
                        {"─","" },
                        // {"H","" }
                        {"çë","ç" },
                        {"±F","OF" },
                        {"°F","OF" },
                        {"-³","" },
                       // {"³","" },
                        {".³","" },
                        {"³-","" },
                        {".ë","" },
                        {"|","" }
                                //{"ë", "I"}, {"î.", "O"},{"çç", ""},{"═", ""},{"(",""},{")",""},{" ", ""},{"ù", "."},{"EB", "─"},{"XX",""},{"³",""},
                                //{"##",""},{"#",""},{"..",""},{":",""},{"«ç",""},{"─",""},{"H",""}
                                };
                                foreach (var replacement in replacements)
                                {
                                    ocrtext = ocrtext.Replace(replacement.Key, replacement.Value).TrimEnd('.');
                                }
                                if (Regex.IsMatch(ocrtext, @"^((?:\s|:?[/\\;:'"",.]|:?«|:?»|)(\d+)(?:\s|:?[/\\;:'"",.]|:?«|:?»|))$") && !ocrtext.Contains("±"))
                                {
                                    ocrtext = ocrtext.Replace("«", "").Replace("»", "").Replace("/", "")
                                                     .Replace(";", "")
                                                     .Replace(":", "")
                                                     .Replace("'", "")
                                                     .Replace("\"", "")
                                                     .Replace(",", "")
                                                     .Replace("\\", "");
                                }
                                if (Regex.IsMatch(ocrtext, @"^((?:\s|)(?:([A-Z])|[/\\;:'"",.]|)(\d+)?°(?:\s|)(?:([A-Z])|[/\\;:'"",.]|))$") && !ocrtext.Contains("±"))
                                {

                                    ocrtext = ocrtext.Replace("/", "")
                                                     .Replace(";", "")
                                                     .Replace(":", "")
                                                     .Replace("'", "")
                                                     .Replace("\"", "")
                                                     .Replace(",", "")
                                                     .Replace("\\", "");
                                    string numericString = new string(ocrtext.Where(char.IsDigit).ToArray());
                                    ocrtext = numericString + "°";
                                }
                                string regionText = ocrtext;
                                if (regionText == "2180°")
                                {
                                    regionText = "2X180°";
                                }
                                if (regionText == "00")
                                {
                                    regionText = "30°";
                                }
                                if (regionText == "çë")
                                {
                                    regionText = "ç";
                                }
                                if (regionText == "63" || regionText == "ç63")
                                {
                                    regionText = "»";
                                }
                                if (regionText == "X5°" || regionText == "45°(" || regionText == "45Z" || regionText == "45" || regionText == "45O" || regionText == "450" || regionText == "42")
                                {
                                    regionText = "45°";
                                }
                                if (regionText == "32")
                                {
                                    regionText = "´";
                                }
                                if (regionText == "38" || regionText == "30ç" || regionText == "ç30" || regionText == "30" || regionText == "396" || regionText == "390" || regionText == "300" || regionText == "3905" || regionText == "398" || regionText == "30O")
                                {
                                    regionText = "30°";
                                }
                                if (regionText == "150" || regionText == "15")
                                {
                                    regionText = "15°";
                                }
                                if (regionText == "100" || regionText == "10")
                                {
                                    regionText = "10°";
                                }
                                if (regionText == "250")
                                {
                                    regionText = "25°";
                                }
                                if (regionText == "200")
                                {
                                    regionText = "20°";
                                }
                                if (regionText == "900")
                                {
                                    regionText = "90°";
                                }
                                if (regionText == "70")
                                {
                                    regionText = "7°";
                                }
                                if (regionText == "û")
                                {
                                    regionText = "";
                                }
                                if (regionText == "Rù6")
                                {
                                    regionText = "R.6";
                                }
                                if (regionText == "R.î3" || regionText == "R.îñ")
                                {
                                    regionText = "R.03";
                                }
                                if (regionText == ".îîóë")
                                {
                                    regionText = ".005";
                                }
                                if (regionText == "125" || regionText == "ç125")
                                {
                                    regionText = "«";
                                }
                                ocrtext = regionText;
                                if (ocrtext.Contains("X") || ocrtext.Contains("R"))
                                {
                                    ocrtext = ocrtext.Replace("îñ", "03").Replace("I", "").TrimEnd('.');
                                }

                                if (ocrtext.Contains("±"))
                                {
                                    string[] hastext = ocrtext.Split("±");
                                    if (ocrtext.Contains('O'))
                                    {
                                        ocrtext = ocrtext.Replace("O", "0");
                                    }

                                    if (hastext[0].Trim().Length > 0 && hastext[1].Trim().Length > 0 && Regex.IsMatch(ocrtext, @"^((\d+)(?:\s|))±((?:\s|)(\d+)?°)$"))
                                    {
                                        var deg = hastext[0].Trim();
                                        if (hastext[0].Trim().Length == 3 && hastext[0].Trim().EndsWith("0"))
                                        {
                                            hastext[0] = hastext[0].Substring(0, hastext[0].Length - 1) + "°";
                                        }
                                        ocrtext = hastext[0] + "±" + hastext[1];

                                    }
                                    hastext = ocrtext.Split("±");
                                    if (hastext[0].Trim().Length > 0 && hastext[1].Trim().Length > 0 && Regex.IsMatch(ocrtext, @"^((\d+)?°±((?:\s|)(\d+)))$"))
                                    {
                                        var deg = hastext[1].Trim();
                                        if (hastext[1].Trim().Length == 3 && hastext[1].Trim().EndsWith("0"))
                                        {
                                            hastext[1] = hastext[1].Substring(0, hastext[1].Length - 1) + "°";
                                        }
                                        ocrtext = hastext[0] + "±" + hastext[1];

                                    }
                                    hastext = ocrtext.Split("±");
                                    if (hastext[0].Length > 0 && hastext[1].Length > 0 && Regex.IsMatch(ocrtext, @"^((\d+|(?:\.d+)|)(?:\.\d+))±((\d+|(?:\.d+)|)(?:\.\d+))$"))
                                    {
                                        ocrtext = hastext[0] + "±" + hastext[1];
                                    }
                                }

                                string Min, Max, Nominal, Type, SubType, Unit, ToleranceType, PlusTolerance, MinusTolerance;
                                BubbleDrawing.CommonMethods cmt = new BubbleDrawing.CommonMethods();
                                cmt.GetMinMaxValues(ocrtext.Trim(), out Min, out Max, out Nominal, out Type, out SubType, out Unit, out ToleranceType, out PlusTolerance, out MinusTolerance);
                                string qty = "1";
                                int Num_Qty = 1;
                                int beforeqty = 1;
                                bool isDigitPresent = ocrtext.Any(c => char.IsDigit(c));
                                if (ocrtext.Contains("X") && (isDigitPresent || ocrtext.Contains("°")))
                                {
                                    if (ocrtext.Length > 2)
                                    {
                                        int count = Regex.Matches(ocrtext, "X").Count;
                                        if (count > 1)
                                        {
                                            string[] result4 = ocrtext.Split('X');
                                            if (Char.IsNumber(result4[0], 0))
                                            {
                                                qty = result4[0];
                                            }
                                            else if (Char.IsNumber(result4[1], 0))
                                            {
                                                qty = result4[1];
                                            }
                                        }
                                        else
                                        {
                                            qty = ocrtext.Substring(0, ocrtext.IndexOf("X")).Replace(" ", "");
                                            if (qty.Contains("."))
                                            {
                                                qty = "1";
                                                beforeqty = 1;
                                                if (ocrtext.Contains("450"))
                                                {
                                                    ocrtext = ocrtext.Replace("450", "45°");
                                                }
                                            }
                                            else
                                            {
                                                ocrtext = ocrtext.Replace(qty + "X", "");
                                            }
                                            cmt.GetMinMaxValues(ocrtext.Trim(), out Min, out Max, out Nominal, out Type, out SubType, out Unit, out ToleranceType, out PlusTolerance, out MinusTolerance);
                                            if (isplmin_spec.Contains("X"))
                                            {
                                                isplmin_spec = isplmin_spec.Replace(qty + "X", "");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        qty = ocrtext.Substring(0, ocrtext.IndexOf("X")).Replace(" ", "");
                                    }
                                    int value;
                                    if (int.TryParse(qty, out value))
                                        Num_Qty = Convert.ToInt16(qty);
                                    beforeqty = value;
                                }
                                if (qty.Contains("."))
                                {
                                    qty = "1";
                                    beforeqty = 1;
                                }

                                if (Regex.IsMatch(isplmin_pltol, @"[^$0-9.]+$"))
                                {
                                    isplmin_pltol = Regex.Replace(isplmin_pltol, @"^[$0-9.]+$", "");
                                }
                                if (Regex.IsMatch(isplmin_mintol, @"[^$0-9.]+$"))
                                {
                                    isplmin_mintol = Regex.Replace(isplmin_mintol, @"^[$0-9.]+$", "");
                                }
                                if (Regex.IsMatch(isplmin_spec, @"[^$0-9.]+$"))
                                {
                                    isplmin_spec = Regex.Replace(isplmin_spec, @"^[$0-9.]+$", "");
                                }
                                Int64 balincid = 1;
                                for (int k = 1; k <= beforeqty; k++)
                                {
                                    if (lstoCRResults.Count > 0 && k == 1)
                                    {
                                        balincid = lstoCRResults.Where(r => r.Balloon != null).Max(r => Convert.ToInt64(r.Balloon.Substring(0, r.Balloon.IndexOf('.') > 0 ? r.Balloon.IndexOf('.') : r.Balloon.Length))) + 1;
                                    }
                                    OCRResults oCRResults1 = new OCRResults();
                                    string cnt = k.ToString();
                                    oCRResults1.BaloonDrwID = 0;
                                    oCRResults1.DrawingNumber = searchForm.CdrawingNo;
                                    oCRResults1.Revision = searchForm.CrevNo.ToUpper();
                                    oCRResults1.Page_No = searchForm.pageNo;
                                    oCRResults1.BaloonDrwFileID = desFile;
                                    oCRResults1.ProductionOrderNumber = "N/A";
                                    oCRResults1.Part_Revision = "N/A";
                                    if (beforeqty > 1)
                                    {
                                        oCRResults1.Balloon = Convert.ToString(string.Join(".", Convert.ToInt16(balincid), cnt));
                                    }
                                    else
                                    {
                                        oCRResults1.Balloon = Convert.ToString(balincid);
                                    }
                                    oCRResults1.Spec = ocrtext.Trim().TrimEnd('.').ToString();
                                    oCRResults1.Nominal = Nominal;
                                    oCRResults1.Minimum = Min;
                                    oCRResults1.Maximum = Max;
                                    oCRResults1.MeasuredBy = username;
                                    oCRResults1.MeasuredOn = DateTime.Now;
                                    oCRResults1.Crop_X_Axis = (int)s_x;
                                    oCRResults1.Crop_Y_Axis = (int)s_y;
                                    oCRResults1.Crop_Width = (int)s_w;
                                    oCRResults1.Crop_Height = (int)s_h;
                                    oCRResults1.Circle_X_Axis = (int)s_x;
                                    oCRResults1.Circle_Y_Axis = (int)s_y;
                                    oCRResults1.Circle_Width = 28;
                                    oCRResults1.Circle_Height = 28;
                                    oCRResults1.Type = Type;
                                    oCRResults1.SubType = SubType;
                                    oCRResults1.Unit = Unit;
                                    oCRResults1.Quantity = (int)beforeqty;
                                    if (Min != "" && Max != "")
                                    {
                                        oCRResults1.ToleranceType = ToleranceType;
                                    }
                                    else
                                    {
                                        oCRResults1.ToleranceType = "Default";
                                    }
                                    if (ocrtext.Contains("R."))
                                    {
                                        oCRResults1.ToleranceType = "Linear";
                                    }
                                    if (PlusTolerance != "")
                                    {
                                        oCRResults1.PlusTolerance = "+" + PlusTolerance;
                                    }
                                    else
                                    {
                                        oCRResults1.PlusTolerance = "0";
                                    }
                                    if (MinusTolerance != "")
                                    {
                                        oCRResults1.MinusTolerance = "-" + MinusTolerance;
                                    }
                                    else
                                    {
                                        oCRResults1.MinusTolerance = "0";
                                    }
                                    oCRResults1.MaxTolerance = "";
                                    oCRResults1.MinTolerance = "";
                                    oCRResults1.CreatedBy = username;
                                    oCRResults1.CreatedDate = DateTime.Now;
                                    oCRResults1.ModifiedBy = "";
                                    oCRResults1.ModifiedDate = DateTime.Now;
                                    oCRResults1.x = (int)s_x;
                                    oCRResults1.y = (int)s_y;
                                    oCRResults1.width = (int)s_w;
                                    oCRResults1.height = (int)s_h;
                                    oCRResults1.id = "";
                                    oCRResults1.selectedRegion = "Spl";
                                    if (isplmin && isplmin_mintol != "" && isplmin_pltol != "" && isplmin_spec != "")
                                    {
                                        oCRResults1.Spec = isplmin_spec;
                                        oCRResults1.Nominal = isplmin_spec;
                                        try
                                        {
                                            oCRResults1.Minimum = Convert.ToString(Convert.ToDecimal(isplmin_spec.Replace("¡", "")) - Convert.ToDecimal(isplmin_mintol));
                                            oCRResults1.Maximum = Convert.ToString(Convert.ToDecimal(isplmin_spec.Replace("¡", "")) + Convert.ToDecimal(isplmin_pltol));
                                            oCRResults1.MinusTolerance = "-" + Convert.ToString(isplmin_mintol).TrimEnd('.');
                                            oCRResults1.PlusTolerance = "+" + Convert.ToString(isplmin_pltol).TrimEnd('.');
                                        }
                                        catch (Exception ex)
                                        {
                                            objerr.WriteErrorToText(ex);
                                            objerr.WriteErrorLog("isplmin_spec-->" + isplmin_spec + "-->isplmin_mintol-->" + isplmin_mintol + "-->isplmin_pltol-->" + isplmin_pltol);
                                        }
                                        oCRResults1.ToleranceType = "Linear";
                                        oCRResults1.Type = "Dimension";
                                        oCRResults1.Unit = "Inches";
                                        oCRResults1.SubType = "Circularity";
                                    }
                                    if (ocrtext.Contains("X") && ocrtext.Contains("°"))
                                    {
                                        oCRResults1.Minimum = "";
                                        oCRResults1.Maximum = "";
                                        oCRResults1.MinusTolerance = "0";
                                        oCRResults1.PlusTolerance = "0";
                                        oCRResults1.ToleranceType = "Linear";
                                        oCRResults1.Type = "";
                                        oCRResults1.Unit = "";
                                        oCRResults1.SubType = "";
                                    }
                                    lstoCRResults.Add(oCRResults1);
                                }
                            }
                        }
                        /*
                        objbaldet.drawingNo = searchForm.CdrawingNo;
                        objbaldet.revNo = searchForm.CrevNo;
                        objbaldet.totalPage = searchForm.totalPage;
                        objbaldet.pageNo = searchForm.pageNo;
                        objbaldet.rotate = searchForm.rotate;
                        objbaldet.ballonDetails = lstoCRResults;
                        returnObject = balcon.create(objbaldet);
                        */
                        returnObject = lstoCRResults;
                    }
                    var precount = previous.Count();
                    var nxtcount = lstoCRResults.Count();
                    if (precount == nxtcount)
                    {
                        Int64 dBalloonid = 1;
                        OCRResults dummy = new OCRResults();
                        if (nxtcount > 0)
                        {
                            dBalloonid = lstoCRResults.Where(r => r.Balloon != null).Max(r => Convert.ToInt64(r.Balloon.Substring(0, r.Balloon.IndexOf('.') > 0 ? r.Balloon.IndexOf('.') : r.Balloon.Length))) + 1;
                        }

                        dummy.Balloon = Convert.ToString(dBalloonid);
                        dummy.BaloonDrwID = 0;
                        dummy.DrawingNumber = searchForm.CdrawingNo;
                        dummy.Revision = searchForm.CrevNo.ToUpper();
                        dummy.Page_No = searchForm.pageNo;
                        dummy.BaloonDrwFileID = desFile;
                        dummy.ProductionOrderNumber = "N/A";
                        dummy.Part_Revision = "N/A";
                        dummy.Spec = "";
                        dummy.Nominal = "";
                        dummy.Minimum = "";
                        dummy.Maximum = "";
                        dummy.MeasuredBy = username;
                        dummy.MeasuredOn = DateTime.Now;
                        dummy.Crop_X_Axis = (int)s_x;
                        dummy.Crop_Y_Axis = (int)s_y;
                        dummy.Crop_Width = (int)s_w;
                        dummy.Crop_Height = (int)s_h;
                        dummy.Circle_X_Axis = (int)s_x;
                        dummy.Circle_Y_Axis = (int)s_y;
                        dummy.Circle_Width = 28;
                        dummy.Circle_Height = 28;
                        dummy.Type = "";
                        dummy.SubType = "";
                        dummy.Unit = "";
                        dummy.Quantity = 1;
                        dummy.ToleranceType = "Default";
                        dummy.PlusTolerance = "0";
                        dummy.MinusTolerance = "0";
                        dummy.MaxTolerance = "";
                        dummy.MinTolerance = "";
                        dummy.CreatedBy = username;
                        dummy.CreatedDate = DateTime.Now;
                        dummy.ModifiedBy = "";
                        dummy.ModifiedDate = DateTime.Now;
                        dummy.x = (int)s_x;
                        dummy.y = (int)s_y;
                        dummy.width = (int)s_w;
                        dummy.height = (int)s_h;
                        dummy.id = "";
                        dummy.selectedRegion = "";
                        dummy.isballooned = true;
                        lstoCRResults.Add(dummy);
                        returnObject = lstoCRResults;
                        return StatusCode(StatusCodes.Status201Created, returnObject);
                    }

                    return StatusCode(StatusCodes.Status200OK, returnObject);
                }
                catch (Exception ex)
                {
                    objerr.WriteErrorToText(ex);
                    returnObject = lstoCRResults;// balcon.get(searchForm.CdrawingNo, searchForm.CrevNo);
                }
                return StatusCode(StatusCodes.Status200OK, returnObject);
            }
        }
        #endregion

        #region Auto Balloon Process
        [HttpPost]
        [Route("AutoBalloon")]
        public async Task<ActionResult<AutoBalloon>> AutoBalloon(AutoBalloon searchForm)
        {
            if (_dbcontext.TblConfigurations == null)
            {
                return NotFound();
            }
            else
            {
                BubbleDrawingAutomationWeb.Controllers.BalloonController balcon = new BubbleDrawingAutomationWeb.Controllers.BalloonController(_dbcontext);
                BubbleDrawingAutomationWeb.Controllers.CreateBalloon objbaldet = new BubbleDrawingAutomationWeb.Controllers.CreateBalloon();
                IEnumerable<object> returnObject = new List<object>();
                List<OCRResults> lstoCRResults = new List<OCRResults>();
                try
                {
                    #region Auto balloon pre-process logic
                    if (searchForm.selectedRegion == "Full Image")
                    {
                        var hdr = _dbcontext.TblBaloonDrawingLiners.Where(w => w.DrawingNumber == searchForm.CdrawingNo.ToString() && w.Revision == searchForm.CrevNo.ToString() && w.Page_No == searchForm.pageNo).FirstOrDefault();
                        if (hdr != null)
                        {
                            ResetBalloon objreset = new ResetBalloon();
                            objreset.pageNo = searchForm.pageNo;
                            objreset.CrevNo = searchForm.CrevNo;
                            objreset.CdrawingNo = searchForm.CdrawingNo;
                            // resetBalloons(objreset);
                        }
                    }
                    string[] skipwords = { "DRAWING", "Nî.", "FRAME", "SHEET", "REVISIîN", "SECTION", "D-D", "C-C", "B-B", "A-A", "DETAII.", "LINE", "WITH", "WIDTH", "SLOT", "SLOTS", "LEAD", "HAND", "I.EFT", "RIGHT", "SEE", "LINEWITH", "IN", "E-", "(COAT", "PER", "BOM", "FLATS", "CONFIGURATION", "FLAT", "EB", searchForm.CdrawingNo.ToLower(), searchForm.CdrawingNo.ToUpper(), searchForm.CdrawingNo, searchForm.CrevNo, searchForm.CrevNo.ToLower(), searchForm.CrevNo.ToUpper() };
                    string drawingNo = searchForm.CdrawingNo;
                    string revNo = searchForm.CrevNo;
                    string dtFiles = searchForm.drawingDetails;
                    double _aspectRatio = searchForm.aspectRatio;
                    double bgImgW = searchForm.bgImgW;
                    double bgImgH = searchForm.bgImgH;
                    string selectedRegion = searchForm.selectedRegion;
                    int bgImgRotation = searchForm.bgImgRotation;
                    int pageNo = searchForm.pageNo;
                    int totalPage = searchForm.totalPage;
                    string env = this._appSettings.ENVIRONMENT;
                    string finame = string.Empty;
                    FileInfo fi = new FileInfo(dtFiles);
                    finame = dtFiles;
                    string desFile = fi.Name;
                    string OrgPath = dtFiles;
                    if (env != "development")
                    {
                        OrgPath = dtFiles.Replace(desFile, "") + "\\drawing\\" + desFile;
                    }
                    int ItemView = searchForm.ItemView;
                    
                    string Fname = fi.Name;
                    temp = System.IO.Path.Combine(System.IO.Path.GetTempPath(), Guid.NewGuid().ToString() + $"{Fname}");
                    System.IO.File.Copy(OrgPath, temp, true);
                    // RemoveOpaqueColormapFrom1BPP(OrgPath);
                    if (bgImgRotation != 0)
                    {
                      //  RotateImagefile(temp, bgImgRotation);
                    }
                    string ImageFile = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "ImageFile_" + Guid.NewGuid().ToString() + desFile);
                    string SelImageFile = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "SelImageFile_" + Guid.NewGuid().ToString() + desFile);
                    string processImageFile = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "processImageFile_" + Guid.NewGuid().ToString() + desFile);
                    System.IO.File.Copy(temp, ImageFile, true);
                    System.IO.File.Copy(temp, SelImageFile, true);
                    System.IO.File.Copy(temp, processImageFile, true);
                    decimal s_x = 0;
                    decimal s_y = 0;
                    decimal s_w = 0;
                    decimal s_h = 0;
                    string croppedname = string.Empty;
                    int imagewidth = 0;
                    int imageheight = 0;
                    if (searchForm.selectedRegion == "Selected Region")
                    {
                        System.IO.File.Delete(temp);
                        List<OCRResults> request = searchForm.originalRegions.Where(x1 => x1.isballooned == false).ToList();
                        foreach (var obj in request)
                        {
                            s_x = obj.x;
                            s_y = obj.y;
                            s_w = obj.width;
                            s_h = obj.height;
                            System.Drawing.RectangleF rectElipse = new System.Drawing.RectangleF((float)obj.x, (float)obj.y, (float)obj.width, (float)obj.height);
                            lstCircle.Add(new Circle_AutoBalloon { Bounds = rectElipse });
                            string OriginalImage = OrgPath;
                            System.Drawing.Rectangle rectElipse1 = new System.Drawing.Rectangle((int)obj.x, (int)obj.y, (int)obj.width, (int)obj.height);
                            Bitmap originalImage = new Bitmap(SelImageFile);
                            Bitmap croppedImage = CropImage(originalImage, rectElipse1);
                            Bitmap newImage = ChangeResolution(croppedImage, 200.0f);
                            string cropname = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "cropname_" + Guid.NewGuid().ToString() + $"text_region.png");
                            // Save or use the new image
                            newImage.Save(cropname);
                            temp = cropname;
                        }
                    }
                    else if (searchForm.selectedRegion == "Unselected Region")
                    {
                        List<OCRResults> request = searchForm.originalRegions.Where(x1 => x1.isballooned == false).ToList();
                        foreach (var obj in request)
                        {
                            System.Drawing.RectangleF rectElipse = new System.Drawing.RectangleF((float)obj.x, (float)obj.y, (float)obj.width, (float)obj.height);
                            lstCircle.Add(new Circle_AutoBalloon { Bounds = rectElipse });
                        }
                    }
                    var image = new OpenCvSharp.Mat(temp, OpenCvSharp.ImreadModes.Color);
                    if (searchForm.selectedRegion == "Full Image" || searchForm.selectedRegion == "Selected Region" || searchForm.selectedRegion == "Unselected Region")
                    {
                        image = new OpenCvSharp.Mat(ImageFile, OpenCvSharp.ImreadModes.Color);
                        imagewidth = image.Width;
                        imageheight = image.Height;
                    }
                    if (searchForm.selectedRegion == "Selected Region")
                    {
                        image = new OpenCvSharp.Mat(temp, OpenCvSharp.ImreadModes.Color);
                    }
                    if (searchForm.selectedRegion != "Unselected Region")
                    {
                        List<OCRResults> request1 = searchForm.originalRegions.Where(x1 => x1.isballooned == true).ToList();
                        lstoCRResults = request1;
                    }
                    else
                    {
                        List<OCRResults> request1 = searchForm.originalRegions.Where(x1 => (x1.isballooned == true && x1.Page_No != pageNo)).ToList();
                        lstoCRResults = request1;
                    }
                    #endregion

                    StringBuilder FWords = new StringBuilder();
                    List<AutoBalloon_OCR> auto_ocrresults = new List<AutoBalloon_OCR>();
                    List<AutoBalloon_OCR> auto_ocrresults_largeimage = new List<AutoBalloon_OCR>();
                    var originposition = searchForm.origin;
                    var origin = originposition.First();
                    List<AG_OCR> ag_ocrresults = new List<AG_OCR>();
                    int agocr = 0;
                    string customLanguagePath = (new System.IO.DirectoryInfo(Environment.CurrentDirectory).FullName) + @"\tessdata";

                    if ((origin.scale < 1 || (origin.scale == 1 && imagewidth > 12800)) && (imagewidth > 12800 || imageheight > 12800) && searchForm.selectedRegion == "Full Image")
                    {
                        float padding = 200;
                        float originalHeight = origin.fullHeight;
                        float originalWidth = origin.fullWidth;
                        float scale = 0;
                        float widthScale = 0;
                        float heightScale = 0;
                        if (imageheight < imagewidth)
                        {
                            float sacledratio = imagewidth / imageheight;
                            widthScale = (float)imagewidth / originalWidth;
                            heightScale = (float)imageheight / originalHeight;
                            scale = Math.Min(widthScale, heightScale);
                            padding = padding * scale;

                        }

                        OpenCvSharp.Mat largeImage = Cv2.ImRead(temp, OpenCvSharp.ImreadModes.Color);
                        int gg = 0;

                        if (largeImage != null && !largeImage.Empty())
                        {

                            double divisor = 10000;
                            double dividend = originalWidth;
                            double quotient = dividend / divisor;
                            if (dividend % divisor != 0)
                            {
                                quotient = Math.Ceiling(quotient);
                            }
                            int roundedQuotient = (int)Math.Round(quotient);
                            int tileWidth = largeImage.Width / roundedQuotient; // Define the width of each tile
                            int tileHeight = largeImage.Height; // Define the height of each tile
                            StringBuilder Words_large = new StringBuilder();

                            #region Large image Slice and Iterate
                            using (var engine = new TesseractEngine(customLanguagePath, "IMSsym1", EngineMode.Default))
                            {
                                //int y = 0;
                                //for (int y = 0; y < largeImage.Rows; y += tileHeight)
                                //{
                                for (int x = 0; x < largeImage.Cols; x += tileWidth)
                                {
                                    int width = Math.Min(tileWidth, largeImage.Cols - x);
                                    //int height = Math.Min(tileHeight, largeImage.Rows - y);
                                    int height = tileHeight;
                                    gg++;
                                    agocr++;
                                    OpenCvSharp.Rect rect = new OpenCvSharp.Rect(x, 0, width, height);
                                    // y += tileHeight;
                                    OpenCvSharp.Mat tile = new OpenCvSharp.Mat(largeImage, rect);
                                    // Convert Mat to Bitmap
                                    Bitmap bitmap = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(tile);

                                    // Save the Bitmap as a temporary file
                                    string tempFile = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "largeimage_" + Guid.NewGuid().ToString() + $"text_region.png");

                                    //string tempFile = $"temp_{x}_{y}.png";
                                    bitmap.Save(tempFile, System.Drawing.Imaging.ImageFormat.Png);

                                    using (var imgnew = Tesseract.Pix.LoadFromFile(tempFile)) // Preprocess the tile for OCR
                                    {
                                        using (var page = engine.Process(imgnew, Tesseract.PageSegMode.Auto))
                                        {
                                            using (var iter = page.GetIterator())
                                            {
                                                iter.Begin();


                                                int kk = 1;
                                                do
                                                {
                                                    if (iter.TryGetBoundingBox(Tesseract.PageIteratorLevel.Word, out var textLineBox))
                                                    {
                                                        OpenCvSharp.Rect textRegionRect = new OpenCvSharp.Rect(textLineBox.X1, textLineBox.Y1, textLineBox.X2 - textLineBox.X1, textLineBox.Y2 - textLineBox.Y1);
                                                        string word = iter.GetText(Tesseract.PageIteratorLevel.Word);
                                                        string regionText = word;
                                                        if (string.IsNullOrWhiteSpace(word) || word == "  " || word == " " || word == null)
                                                        {
                                                            continue;
                                                        }
                                                        if (word == "-³-" || word == "++³" || word == "--")
                                                        {
                                                            continue;
                                                        }
                                                        if (regionText != "" || regionText != null)
                                                        {
                                                            //if (padding < textLineBox.X1 && textLineBox.X1 < (imagewidth - padding) && padding < textLineBox.Y1 && textLineBox.Y1 < (imageheight - padding))
                                                            //{
                                                            if (gg == 1)
                                                            {
                                                                Words_large.AppendLine(regionText + "-->rect-->" + Convert.ToString(textRegionRect.X) + "," + Convert.ToString(textRegionRect.Y) + "," + Convert.ToString(textRegionRect.Width) + "," + Convert.ToString(textRegionRect.Height));
                                                                //  auto_ocrresults_largeimage.Add(new AutoBalloon_OCR { Ocr_Text = regionText, X_Axis = textLineBox.X1, Y_Axis = textLineBox.Y1, Width = textLineBox.X2 - textLineBox.X1, Height = textLineBox.Y2 - textLineBox.Y1, Qty = 1, No = kk });
                                                                kk++;
                                                            }
                                                            else
                                                            {
                                                                Words_large.AppendLine(regionText + "-->rect-->" + Convert.ToString(tileWidth + textRegionRect.X) + "," + Convert.ToString(textRegionRect.Y) + "," + Convert.ToString(textRegionRect.Width) + "," + Convert.ToString(textRegionRect.Height));
                                                                // auto_ocrresults_largeimage.Add(new AutoBalloon_OCR { Ocr_Text = regionText, X_Axis = tileWidth + textLineBox.X1, Y_Axis = textLineBox.Y1, Width = textLineBox.X2 - textLineBox.X1, Height = textLineBox.Y2 - textLineBox.Y1, Qty = 1, No = kk });
                                                                kk++;
                                                            }

                                                            int cont = 0, cx = 0, cy = 0, xx = 0, yy = 0, ww = 0, hh = 0, nx = 0;
                                                            if (gg == 1)
                                                            {

                                                                cx = (int)(textLineBox.X1 * widthScale);
                                                                xx = (int)(textLineBox.X1 * widthScale);
                                                                nx = (int)(textLineBox.X1 * widthScale);
                                                            }
                                                            else
                                                            {
                                                                cx = (int)((x + textLineBox.X1) * widthScale);
                                                                xx = (int)((x + textLineBox.X1) * widthScale);
                                                                nx = (int)((x + textLineBox.X1) * widthScale);
                                                            }
                                                            cont = ag_ocrresults.Count();
                                                            cy = (int)(textLineBox.Y1 * heightScale);
                                                            yy = (int)(textLineBox.Y1 * heightScale);
                                                            ww = (int)((textLineBox.X2 - textLineBox.X1) * widthScale);
                                                            hh = (int)((textLineBox.Y2 - textLineBox.Y1) * heightScale);
                                                            if (searchForm.selectedRegion == "Selected Region")
                                                            {
                                                                var croppedRegion = lstCircle.Last();
                                                                var simage = new OpenCvSharp.Mat(ImageFile, OpenCvSharp.ImreadModes.Color);
                                                                float cry = croppedRegion.Bounds.Y;
                                                                float crx = croppedRegion.Bounds.X;
                                                                float crw = croppedRegion.Bounds.Width;
                                                                float crh = croppedRegion.Bounds.Height;
                                                                decimal wsf = (decimal)crw / (decimal)simage.Width;
                                                                decimal hsf = (decimal)crh / (decimal)simage.Height;

                                                                var cx1 = (int)(textLineBox.X1 * wsf);
                                                                var cy1 = (int)(textLineBox.Y1 * hsf);

                                                                cx = textLineBox.X1 + (int)s_x + cx1;
                                                                xx = textLineBox.X1 + (int)s_x + cx1;
                                                                nx = textLineBox.X1 + (int)s_x + cx1;
                                                                cy = textLineBox.Y1 + (int)s_y + cy1;
                                                                yy = textLineBox.Y1 + (int)s_y + cy1;
                                                            }
                                                            //if (cy > 200 && cx > 200 && cx < (imagewidth - 200) && cy < (imageheight - 200)) {
                                                            if (cont == 0)
                                                            {
                                                                ag_ocrresults.Add(new AG_OCR { GroupID = agocr, cx = cx, nx = nx, cy = cy, x = xx, y = yy, w = ww, h = hh, text = regionText });
                                                            }
                                                            if (cont > 0)
                                                            {
                                                                var last = ag_ocrresults.Last();
                                                                var checky = xx - (last.x + last.w);
                                                                if (Math.Sign(checky) != -1 && (last.x + last.w) < xx && last.x < xx && (checky < 70))
                                                                {

                                                                    ag_ocrresults.Add(new AG_OCR { GroupID = agocr, cx = cx, nx = nx, cy = cy, x = xx, y = yy, w = ww, h = hh, text = regionText });

                                                                }
                                                                else
                                                                {
                                                                    agocr++;
                                                                    ag_ocrresults.Add(new AG_OCR { GroupID = agocr, cx = cx, nx = nx, cy = cy, x = xx, y = yy, w = ww, h = hh, text = regionText });

                                                                }
                                                            }
                                                            //}

                                                            //}
                                                        }
                                                    }
                                                }
                                                while (iter.Next(Tesseract.PageIteratorLevel.Word));

                                            }
                                        }
                                    }
                                }
                                //}
                                objerr.WriteErrorLog("large image Words: " + Words_large);
                            }
                            #endregion
                        }
                        else
                        {
                            objerr.WriteErrorLog("Image not found or unable to read.");
                        }
                        #region Large image X axis based group the nearest text 
                        List<AG_OCR> groupedByX = new List<AG_OCR>();
                        List<AutoBalloon_OCR> auto_group_ocrresults_largeimage = new List<AutoBalloon_OCR>();
                        var grouped_ocrresults = ag_ocrresults.GroupBy(i => i.GroupID).ToList();
                        foreach (var group in grouped_ocrresults)
                        {
                            int cx, cy, nx = 0; int xx, yy, ww, hh = 0;
                            String text = string.Empty;
                            int i = 1;
                            foreach (var g in group)
                            {
                                text += " " + g.text;
                                if (group.Count() == i)
                                {
                                    var first = group.First();
                                    cx = first.x;
                                    cy = first.y;

                                    var last = group.Last();
                                    xx = first.x;
                                    yy = first.y;
                                    ww = (last.x + last.w) - first.x;
                                    hh = last.h;
                                    nx = first.nx;
                                    text = text.Trim();
                                    text = Regex.Replace(text, @"\r\n|\n", "");
                                    text = text.Trim();
                                    text = text.Replace("═1-°°", "-1.00").Replace("\r\n", "");
                                    //if (text.Trim().Length < 2)
                                    //continue;
                                    text = text
                                        .Replace("î", "0")
                                        .Replace("ï", "1")
                                        .Replace("ð", "2")
                                        .Replace("ñ", "3")
                                        .Replace("ò", "4")
                                        .Replace("ó", "5")
                                        .Replace("ô", "6")
                                        .Replace("õ", "7")
                                        .Replace("ö", "8")
                                        .Replace("÷", "9")
                                        .Replace("(2)", "¡")
                                        .Replace("(Z)", "¡")
                                        .Replace("(/)", "¡")
                                        ;
                                    if (text.StartsWith("I"))
                                    {
                                        text = text.Substring(1);
                                    }
                                    if (Regex.IsMatch(text, @"[A-Z][0-9]{1}$"))
                                    {
                                        continue;
                                    }
                                    if (text.Contains("X"))
                                    {
                                        string[] hastext = text.Split("X");
                                        if (hastext[0].Trim().Length > 0 && hastext[1].Trim().Length > 0 && Regex.IsMatch(text, @"^((\d+(\.\d+))|(\d+)|(\.\d+)(?:\s|))X((?:\s|)(\d+))$"))
                                        {
                                            var deg = hastext[1].Trim();
                                            if (hastext[1].Trim().Length == 3 && hastext[1].Trim().EndsWith("0"))
                                            {
                                                hastext[1] = hastext[1].Substring(0, hastext[1].Length - 1) + "°";
                                            }
                                            text = hastext[0] + "X" + hastext[1];
                                        }
                                    }
                                    if (text.Contains("RëGHT") || text.Contains("RIGHT") || text.Contains("LEFT"))
                                    {
                                        string[] hastext = text.Split("RëGHT");
                                        if (hastext[0].Trim().Length > 0)
                                        {
                                            text = hastext[0];
                                        }
                                        hastext = text.Split("RIGHT");
                                        if (hastext[0].Trim().Length > 0)
                                        {
                                            text = hastext[0];
                                        }
                                        hastext = text.Split("LEFT");
                                        if (hastext[0].Trim().Length > 0)
                                        {
                                            text = hastext[0];
                                        }

                                    }
                                    if (text.StartsWith("(") && !text.EndsWith(")"))
                                    {
                                        text = text + ")";
                                    }
                                    if (!text.StartsWith("(") && text.EndsWith(")"))
                                    {
                                        text = "(" + text;
                                    }
                                    if (text.Contains("SEAT") && Regex.IsMatch(text.Trim(), @"^(((\d+(?:°))(?:±)(?:\d+(?:°))(?:\s))|(?:\d+(?:°))(?:\s))(?:SEAT)$"))
                                    {
                                        string[] hastext = text.Split("SEAT");
                                        if (hastext[0].Trim().Length > 0)
                                        {
                                            text = hastext[0].Trim();
                                        }

                                    }

                                    Dictionary<string, string> replacements = new Dictionary<string, string>{

                                        {".³","" },
                                        {"³-","" },
                                        {".ë","" },
                                        {"-³ ",""},
                                        { "-═ ",""},
                                        { "-----",""},
                                        { "ð","2"},
                                        { " ±","±"},
                                        { "à","¡"},
                                        { "±-","±."},
                                        { "ó","6"},

                                        { "-X",""},
                                        { "APART",""}

                                      };
                                    foreach (var replacement in replacements)
                                    {
                                        text = text.Replace(replacement.Key, replacement.Value);
                                    }
                                    if (string.IsNullOrWhiteSpace(text) || text == "" || text == "  " || text == " " || text == null)
                                    {
                                        continue;
                                    }
                                    if (text.Contains(".."))
                                    {
                                        string[] spltxt = text.Split("..");
                                        text = spltxt[0] + " -" + spltxt[1];

                                    }
                                    if (text.StartsWith(","))
                                    {
                                        text = text.Substring(1);
                                    }
                                    if (text.StartsWith("³"))
                                    {
                                        text = text.Substring(1);
                                    }
                                    if (text.EndsWith("³"))
                                    {
                                        text = text.Substring(0, text.Length - 1);
                                    }
                                    if (text.EndsWith("."))
                                    {
                                        text = text.Substring(0, text.Length - 1);
                                    }
                                    if (text.Contains("+"))
                                    {
                                        if (Regex.IsMatch(text, @"^\d+\s\d+(\.\d+)?(\+\.\d+)?$"))
                                        {
                                            text = text.Replace(" ", ".");
                                        }
                                    }
                                    if (text.Contains("-"))
                                    {
                                        if (Regex.IsMatch(text, @"^\d+\s\d+(\.\d+)?(\-\.\d+)?$"))
                                        {
                                            text = text.Replace(" ", ".");
                                        }
                                    }
                                    if (text.StartsWith("R"))
                                    {
                                        text = text.Replace(",", ".").Replace(" ", ".")
                                                   .Replace("î", "0")
                                                   .Replace("ï", "1")
                                                   .Replace("ð", "2")
                                                   .Replace("ñ", "3")
                                                   .Replace("ò", "4")
                                                   .Replace("ó", "5")
                                                   .Replace("ô", "6")
                                                   .Replace("õ", "7")
                                                   .Replace("ö", "8")
                                                   .Replace("÷", "9");
                                    }

                                    bool containsBracket = text.Contains("(") && text.Contains(")");
                                    if (containsBracket)
                                        continue;
                                    text = text.Trim();
                                    text = Regex.Replace(text, @"\r\n?|\n", "");
                                    text = text.Trim();
                                    if (text.Trim().Length < 2)
                                        continue;
                                    groupedByX.Add(new AG_OCR { GroupID = g.GroupID, cx = cx, nx = nx, cy = cy, x = xx, y = yy, w = ww, h = hh, text = text });
                                }
                                i++;
                            }
                        }
                        #endregion

                        #region Large Image join the PluseMinus 
                        List<AG_OCR> sortByPluseMinuse = new List<AG_OCR>();
                        List<AG_OCR> sortByY = new List<AG_OCR>();
                        List<int> snewList = new List<int>();

                        int scount = 1;
                        foreach (var i in groupedByX)
                        {
                            if (i.text.Contains("+"))
                            {
                                sortByPluseMinuse.Add(new AG_OCR { GroupID = scount, cx = i.cx, cy = i.cy, x = i.x, y = i.y, w = i.w, h = i.h, text = i.text });
                                snewList.Add(i.GroupID);


                                var last = sortByPluseMinuse.Last();
                                var neartext = groupedByX
                                           .Where(item => !snewList.Contains(item.GroupID))
                                           .Where(item =>
                                                (item.text.Length > 2 && (item.y - last.y) < 70 && Math.Sign(item.y - last.y) != -1 && Math.Sign((item.x + item.w) - (last.x + last.w)) != -1 && ((item.x + item.w) - (last.x + last.w)) < (last.h + 30)))

                                           .ToList();


                                if (neartext.Count() == 0)
                                {

                                    if (sortByPluseMinuse.Any())
                                    {
                                        sortByPluseMinuse.RemoveAt(sortByPluseMinuse.Count - 1);
                                    }
                                    if (snewList.Any())
                                    {
                                        snewList.RemoveAt(snewList.Count - 1);
                                    }


                                }
                                foreach (var ni in neartext)
                                {
                                    sortByPluseMinuse.Add(new AG_OCR { GroupID = scount, cx = ni.cx, cy = ni.cy, x = ni.x, y = ni.y, w = ni.w, h = ni.h, text = ni.text });
                                    snewList.Add(ni.GroupID);
                                }
                                if (neartext.Count() == 0)
                                {
                                    scount++;
                                }

                            }
                        }

                        StringBuilder FiWords = new StringBuilder();
                        groupedByX.RemoveAll(item => snewList.Contains(item.GroupID));
                        if (groupedByX.Count() > 0)
                        {
                            int newId = 1;
                            foreach (var i in groupedByX)
                            {
                                i.GroupID = newId++;
                                OpenCvSharp.Rect textRegionRect = new OpenCvSharp.Rect(i.x, i.y, i.w, i.h);
                                FiWords.AppendLine(i.text.ToString() + " <=> " + i.GroupID.ToString() + " <=> " + textRegionRect.ToString());
                            }
                        }
                        if (sortByPluseMinuse.Count() > 0)
                        {
                            var grouped_Y_ocrresults = sortByPluseMinuse.GroupBy(i => i.GroupID).ToList();

                            foreach (var group in grouped_Y_ocrresults)
                            {
                                var last_Gx = groupedByX.Last();
                                var last_GroupID = last_Gx.GroupID;
                                int cx, cy, nx = 0; int xx, yy, ww, hh = 0;
                                String text = string.Empty;
                                int i = 1;
                                foreach (var g in group)
                                {
                                    if (g.text.Contains("+"))
                                    {
                                        if (Regex.IsMatch(g.text, @"^\d+\s\d+(\.\d+)?(\+\.\d+)?$"))
                                        {
                                            g.text = g.text.Replace(" ", ".");
                                        }
                                    }
                                    if (g.text.Contains("-"))
                                    {
                                        if (Regex.IsMatch(g.text, @"^\d+\s\d+(\.\d+)?(\-\.\d+)?$"))
                                        {
                                            g.text = g.text.Replace(" ", ".");
                                        }
                                    }

                                    text += " " + g.text;
                                    if (group.Count() == i)
                                    {
                                        var first = group.First();
                                        cx = first.x;
                                        cy = first.y;

                                        var last = group.Last();
                                        xx = first.x;
                                        yy = first.y;
                                        ww = (last.x + last.w) - first.x;
                                        hh = last.h;
                                        nx = first.nx;
                                        text = text.Trim();
                                        last_GroupID++;
                                        OpenCvSharp.Rect textRegionRect = new OpenCvSharp.Rect(xx, yy, ww, hh);
                                        FiWords.AppendLine(text.ToString() + " <=> " + last_GroupID.ToString() + " <=> " + textRegionRect.ToString());
                                        groupedByX.Add(new AG_OCR { GroupID = last_GroupID, cx = cx, nx = nx, cy = cy, x = xx, y = yy, w = ww, h = hh, text = text });
                                    }
                                    i++;

                                }

                            }

                        }
                        #endregion

                        #region Large Image List the Initial items

                        List<Item> items = new List<Item>();
                        foreach (var i in groupedByX)
                        {
                            auto_group_ocrresults_largeimage.Add(new AutoBalloon_OCR
                            {
                                Ocr_Text = i.text,
                                X_Axis = (int)(i.x),
                                Y_Axis = (int)(i.y),
                                Width = (int)(i.w),
                                Height = (int)(i.h),
                                Qty = 1,
                                No = i.GroupID
                            });
                            items.Add(new Item { X = i.x, Y = i.y, W = i.w, H = i.h, Text = i.text, isBallooned = false });

                        }
                        #endregion

                        #region Large Image get Exclude boundary
                        StringBuilder FWordsSelected = new StringBuilder();
                        int aftheight = 320;
                        int finalyaxis = 0;
                        int finalxaxis = 0;
                        int paddingy = 220;
                        int paddingx = 320;
                        var infoBox = items.Where((s) => s.Text.Contains("PRODUCTS AND TECHNOLOGY")).Select((c, i) => { return c; }).ToList();

                        if (infoBox.Count > 0)
                        {
                            var iBox = infoBox.First();
                            finalyaxis = iBox.Y + 100;
                            finalxaxis = iBox.X + 100;
                        }
                        #endregion

                        // Specify the threshold for grouping based on X coordinate
                        int thresholdX = 50;
                        // Group items based on their X coordinates
                        List<List<Item>> groupedItems = GroupItemsByX(items, thresholdX);

                        #region Large Image Filter posible balloon 
                        List<List<Item>> currentGroup = new List<List<Item>>();
                        foreach (var g in groupedItems)
                        {

                            foreach (var i in g)
                            {
                                if (searchForm.selectedRegion != "Selected Region" && paddingx < i.X && i.X < (imagewidth - paddingx) && paddingy < i.Y && i.Y < (imageheight - paddingy))
                                {
                                    string txtval = OcrTextOptimization(i.Text, i.X, i.Y, i.W, i.H, imagewidth, imageheight, searchForm);
                                    if (txtval != "")
                                    {
                                        if (finalyaxis > 0 && finalxaxis > 0 && i.Y > finalyaxis && i.X > finalxaxis)
                                        {
                                            continue;
                                        }
                                        if (i.Y > (imageheight - 350))
                                        {
                                            continue;
                                        }
                                        if(searchForm.selectedRegion == "Unselected Region")
                                        {
                                            var eBox = lstCircle.Last();
                                            float eY1axis = eBox.Bounds.Y;
                                            float eX1axis = eBox.Bounds.X;
                                            float eX2axis = eBox.Bounds.Width;
                                            float eY2axis = eBox.Bounds.Height;
                                        }
                                        i.isBallooned = true;
                                        string[] rtext = txtval.Split(",");
                                        i.Text = rtext[0];
                                    }
                                }
                                if (searchForm.selectedRegion == "Selected Region")
                                {
                                    string txtval = OcrTextOptimization(i.Text, i.X, i.Y, i.W, i.H, imagewidth, imageheight, searchForm);
                                    if (txtval != "")
                                    {
                                        i.isBallooned = true;
                                        string[] rtext = txtval.Split(",");
                                        i.Text = rtext[0];
                                    }
                                }
                                // Console.WriteLine("X:  "+ i.X+" , Y:  "+i.Y+" , Text:   "+i.Text + " , isBallooned "+ i.isBallooned);
                            }
                            if (g.Any(i => i.isBallooned == true && i.Text != ""))
                            {
                                currentGroup.Add(g);
                            }
                        }
                        #endregion

                        #region Large Get active items  
                        List<ActiveItems> activeItems = new List<ActiveItems>();
                        int ai = 1;
                        foreach (var g in currentGroup)
                        {
                            int nh = 0;
                            int count = 1;
                            foreach (var i in g)
                            {
                                if (i.isBallooned == true)
                                {
                                    var citem = activeItems.Where(a => a.GroupID == ai).ToList();
                                    if (activeItems.Count > 0 && citem.Count() > 0)
                                    {
                                        var last = citem.Last();
                                        last.NH = nh;
                                    }
                                    nh = i.H;
                                    activeItems.Add(new ActiveItems { X = i.X, Y = i.Y, W = i.W, H = i.H, NH = nh, Text = i.Text, isBallooned = true, GroupID = ai });
                                }
                                else
                                {
                                    nh = nh + i.H;
                                    if (g.Count() == count)
                                    {
                                        var citem = activeItems.Where(a => a.GroupID == ai).ToList();
                                        if (activeItems.Count > 0 && citem.Count() > 0)
                                        {
                                            var last = citem.Last();
                                            last.NH = nh;
                                        }
                                    }

                                }
                                count++;
                            }
                            ai++;
                        }
                        #endregion

                        #region Large Image Sort all item Y axis based
                        var sortedItems = activeItems.OrderBy(item => item.Y).ThenBy(item => item.X).ToList();
                        #endregion

                        #region Large Image N-X items Get Filtered
                        int aId = 1;
                        List<int> NxList = new List<int>();
                        List<ActiveItems> NxactiveItems = new List<ActiveItems>();
                        foreach (var i in sortedItems)
                        {

                            if (i.Text.Contains("X") && Regex.IsMatch(i.Text, @"^((\d+)(?:\s|))X$"))
                            {
                                NxList.Add(i.GroupID);
                                var neartext = sortedItems
                                           .Where(item => !NxList.Contains(item.GroupID))
                                           .Where(a =>
                                                    (Math.Abs(a.Y - i.Y) < 50 && Math.Abs(a.X - i.X) < 100)
                                                )
                                           .ToList();
                                foreach (var ni in neartext)
                                {
                                    var maxwidth = Math.Max(ni.W, i.W);
                                    NxList.Add(ni.GroupID);
                                    NxactiveItems.Add(new ActiveItems { X = ni.X, Y = ni.Y - i.H, W = maxwidth, H = ni.H + i.H, NH = ni.H + i.H, Text = i.Text + " " + ni.Text, isBallooned = true, GroupID = ni.GroupID });
                                }

                            }
                        }
                        if (NxactiveItems.Count() > 0)
                        {
                            sortedItems.RemoveAll(item => NxList.Contains(item.GroupID));
                            foreach (var i in NxactiveItems)
                            {
                                sortedItems.Add(new ActiveItems { X = i.X, Y = i.Y, W = i.W, H = i.H, NH = i.NH, Text = i.Text, isBallooned = true, GroupID = i.GroupID });
                            }

                        }
                        sortedItems = sortedItems.OrderBy(item => item.Y).ThenBy(item => item.X).ToList();
                        foreach (var i in sortedItems)
                        {
                            i.GroupID = aId++;
                        }
                        #endregion

                        #region Large Image Find the parent to create sub balloon

                        List<AGF_OCR> sortByParent = new List<AGF_OCR>();
                        List<int> pnewList = new List<int>();
                        int gcount = 1;
                        foreach (var i in sortedItems)
                        {
                            if (!pnewList.Contains(i.GroupID))
                            {
                                sortByParent.Add(new AGF_OCR { GroupID = gcount, parentID = 0, x = i.X, y = i.Y, w = i.W, h = i.NH, text = i.Text });
                                pnewList.Add(i.GroupID);
                            }
                            foreach (var ii in sortedItems)
                            {
                                var last = sortByParent.Last();
                                List<ActiveItems> sortsParent = new List<ActiveItems>();
                                sortsParent.Add(new ActiveItems { X = ii.X, Y = ii.Y, W = ii.W, H = ii.H, NH = ii.NH, Text = ii.Text, isBallooned = true, GroupID = ii.GroupID });
                                var neartext = sortsParent
                                       .Where(item => !pnewList.Contains(item.GroupID))
                                       .Where(a =>
                                                (
                                                    (Math.Abs(a.Y - last.y) < 50 && Math.Abs(a.X - last.x) < 100 && Regex.IsMatch(last.text, @"^([0-9]+(:?X))+.*$"))
                                                    ||
                                                    ((Math.Abs(a.Y - (last.y + last.h)) < 50) && ((last.x + last.w) >= a.X && Math.Abs((last.x + last.w) - (a.X + a.W)) < 100))
                                                )
                                            )
                                       .ToList();
                                foreach (var ni in neartext)
                                {
                                    pnewList.Add(ni.GroupID);
                                    sortByParent.Add(new AGF_OCR { GroupID = gcount, parentID = gcount, x = ni.X, y = ni.Y, w = ni.W, h = ni.H, text = ni.Text });
                                }
                            }
                            gcount++;

                        }
                        #endregion

                        #region Large Image Final Filter

                        foreach (var i in sortByParent)
                        {
                            OpenCvSharp.Rect textRegionRect = new OpenCvSharp.Rect(i.x, i.y, i.w, i.h);
                            FWords.AppendLine(i.text + " <=> " + i.GroupID + " <=> " + textRegionRect.ToString());
                            bool subballoon = false;
                            if (i.parentID == i.GroupID)
                            {
                                subballoon = true;
                            }
                            //string txtval = OcrTextOptimization(i.text, 0, 0, 0, 0, imagewidth, imageheight, searchForm);

                           // if (txtval != "")
                            //{
                                //string[] rtext = txtval.Split(",");
                                auto_ocrresults.Add(new AutoBalloon_OCR { parent = i.parentID, subballoon = subballoon, Ocr_Text = i.text, X_Axis = Convert.ToInt32(i.x), Y_Axis = Convert.ToInt32(i.y), Width = Convert.ToInt32(i.w), Height = Convert.ToInt32(i.h), Qty = 1, No = i.GroupID });

                           // }
                        }
                        #endregion

                        #region Large Image oldcode filter valid balloon process
                        /*
                        foreach (var dd in auto_group_ocrresults_largeimage)
                        {

                        

                                if ( padding < dd.X_Axis && dd.X_Axis < (imagewidth - padding) && padding < dd.Y_Axis && dd.Y_Axis < (imageheight - padding))
                                {
                                    OpenCvSharp.Rect textRegionRect = new OpenCvSharp.Rect(dd.X_Axis, dd.Y_Axis, dd.Width, dd.Height);

                                    string txtval = OcrTextOptimization(dd.Ocr_Text, dd.X_Axis, dd.Y_Axis, dd.Width, dd.Height, imagewidth, imageheight, searchForm);
                                    if (txtval != "")
                                    {
                                        if (dd.Y_Axis > finalyaxis && dd.X_Axis  >  finalxaxis)
                                        {
                                        continue;
                                        }

                                            int cnt = auto_ocrresults.Count();
                                        string[] resultss = txtval.Split(",");
                                        FWords.AppendLine(resultss[0] + " <=> " + (cnt + 1) + " <=> " + textRegionRect.ToString());
                                        try
                                        {
                                            auto_ocrresults.Add(new AutoBalloon_OCR { Ocr_Text = resultss[0], X_Axis = Convert.ToInt32(resultss[1]), Y_Axis = Convert.ToInt32(resultss[2]), Width = Convert.ToInt32(resultss[3]), Height = Convert.ToInt32(resultss[4]), Qty = 1, No = cnt + 1 });

                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                    }
                                }
                                 
                        }
                        */
                        #endregion

                        objerr.WriteErrorLog(" large image filter words =>  " + FWords);


                    }
                    else
                    {
                        using (var gray = new OpenCvSharp.Mat())
                        using (var engine = new TesseractEngine(customLanguagePath, "IMSsym1", EngineMode.Default))
                        {
                            Cv2.CvtColor(image, gray, ColorConversionCodes.BGR2GRAY);
                            Cv2.Threshold(gray, gray, 0, 255, ThresholdTypes.Binary | ThresholdTypes.Otsu);
                            // Perform OCR on the text block
                            objerr.WriteErrorLog(" Selection " + temp);
                            using (var blockPage = engine.Process(Tesseract.Pix.LoadFromFile(temp), Tesseract.PageSegMode.Auto))
                            {
                                using (var iter = blockPage.GetIterator())
                                {
                                    iter.Begin();
                                    StringBuilder Words = new StringBuilder();
                                    int regionIndex = 0;
                                    int kk = 1;
                                    List<AutoBalloon_OCR> auto_ocrresults_selected = new List<AutoBalloon_OCR>();

                                    #region iterate small/ selected region 
                                    do
                                    {
                                        if (iter.TryGetBoundingBox(Tesseract.PageIteratorLevel.Word, out var textLineBox))
                                        {
                                            // Create a Rect object using OpenCvSharp 's bounding box coordinates
                                            OpenCvSharp.Rect textRegionRect = new OpenCvSharp.Rect(textLineBox.X1, textLineBox.Y1, textLineBox.X2 - textLineBox.X1, textLineBox.Y2 - textLineBox.Y1);
                                            string word = iter.GetText(Tesseract.PageIteratorLevel.Word);
                                            string regionText = word;
                                            if (string.IsNullOrWhiteSpace(regionText) || regionText == "" || regionText == null || regionText == " ")
                                            {
                                                continue;
                                            }

                                            if (word == "³-³-" || word == "++³" || word == "-³-" || word == "--" || word == "³")
                                            {
                                                continue;
                                            }
                                           
                                            Words.AppendLine(regionText +" " + textLineBox.ToString());
                                            auto_ocrresults_selected.Add(new AutoBalloon_OCR { Ocr_Text = regionText, X_Axis = textLineBox.X1, Y_Axis = textLineBox.Y1, Width = textLineBox.X2 - textLineBox.X1, Height = textLineBox.Y2 - textLineBox.Y1, Qty = 1, No = kk });
                                            kk++;
                                            int cont = 0, cx = 0, cy = 0, xx = 0, yy = 0, ww = 0, hh = 0, nx = 0;
                                            cx = textLineBox.X1;
                                            xx = textLineBox.X1;
                                            nx = textLineBox.X1;

                                            cont = ag_ocrresults.Count();
                                            cy = textLineBox.Y1;
                                            yy = textLineBox.Y1;
                                            ww = (textLineBox.X2 - textLineBox.X1);
                                            hh = (textLineBox.Y2 - textLineBox.Y1);
                                            if (searchForm.selectedRegion == "Selected Region")
                                            {
                                                var croppedRegion = lstCircle.Last();
                                                var simage = new OpenCvSharp.Mat(ImageFile, OpenCvSharp.ImreadModes.Color);
                                                float cry = croppedRegion.Bounds.Y;
                                                float crx = croppedRegion.Bounds.X;
                                                float crw = croppedRegion.Bounds.Width;
                                                float crh = croppedRegion.Bounds.Height;
                                                decimal wsf = (decimal)crw / (decimal)simage.Width;
                                                decimal hsf = (decimal)crh / (decimal)simage.Height;

                                                var cx1 = (int)(  textLineBox.X1 * wsf);
                                                var cy1 = (int)(  textLineBox.Y1 * hsf);

                                                cx = textLineBox.X1 + (int) s_x + cx1;
                                                xx = textLineBox.X1 + (int) s_x + cx1;
                                                nx = textLineBox.X1 + (int) s_x + cx1;
                                                cy = textLineBox.Y1 + (int) s_y + cy1;
                                                yy = textLineBox.Y1 + (int) s_y + cy1;
                                            }
                                            //if (cy > 200 && cx > 200 && cx < (imagewidth - 200) && cy < (imageheight - 200)) {
                                            if (cont == 0)
                                            {
                                                ag_ocrresults.Add(new AG_OCR { GroupID = agocr, cx = cx, nx = nx, cy = cy, x = xx, y = yy, w = ww, h = hh, text = regionText });
                                            }
                                            if (cont > 0)
                                            {
                                                var last = ag_ocrresults.Last();
                                                var checky = xx - (last.x + last.w);
                                                if (/*Math.Sign(checky) != -1 && */ (last.x + last.w) < xx && last.x < xx && (checky < 50))
                                                {

                                                    ag_ocrresults.Add(new AG_OCR { GroupID = agocr, cx = cx, nx = nx, cy = cy, x = xx, y = yy, w = ww, h = hh, text = regionText });

                                                }
                                                else
                                                {
                                                    agocr++;
                                                    ag_ocrresults.Add(new AG_OCR { GroupID = agocr, cx = cx, nx = nx, cy = cy, x = xx, y = yy, w = ww, h = hh, text = regionText });

                                                }
                                            }

                                            regionIndex++;
                                        }
                                    } while (iter.Next(Tesseract.PageIteratorLevel.Word));
                                    objerr.WriteErrorLog(" Words " + Words);
                                    if (searchForm.selectedRegion == "Selected Region")
                                    {
                                        objerr.WriteErrorLog(" lstCircle " + lstCircle.Last().Bounds.ToString());
                                    }
                                    #endregion

                                    #region Selected Group nearby X coordinates words
                                    List<AG_OCR> groupedByX = new List<AG_OCR>();
                                    StringBuilder GxWords = new StringBuilder();
                                    List<AutoBalloon_OCR> auto_group_ocrresults_selected = new List<AutoBalloon_OCR>();
                                    var grouped_ocrresults = ag_ocrresults.GroupBy(i => i.GroupID).ToList();
                                    foreach (var group in grouped_ocrresults)
                                    {
                                        int cx, cy, nx = 0; int xx, yy, ww, hh = 0;
                                        String text = string.Empty;
                                        int i = 1;
                                        foreach (var g in group)
                                        {
                                            var gtext = g.text.Replace("\r\n", "").Replace("\n", "").Replace("═P", "");
                                            if (gtext == "P" || gtext == "═P")
                                            {
                                                continue;
                                            }
                                            if (Regex.IsMatch(gtext, @"^(((?:[A-Z.]+)((?:î)|(?:ë))+(?:[A-Z.,-]+)((?:î)|(?:ë))(?:[A-Z.,-]+))|(((?:î)|(?:ë))+(?:[A-Z.,-]+)((?:î)|(?:ë))(?:[A-Z.,-]+))|(((?:î)|(?:ë))+(?:[A-Z.,-]+))|((?:[A-Z.,-]+)((?:î)|(?:ë))+[.,-])|((?:[A-Z.]+)((?:î)|(?:ë))+(?:[A-Z.,-]+)((?:î)|(?:ë))+)|((?:[A-Z.]+)((?:î)|(?:ë))+(?:[A-Z.,-]+)))$"))
                                            {
                                                gtext = gtext.Replace("ë.", "L");
                                                gtext = gtext.Replace("ë", "I");
                                                gtext = gtext.Replace("î", "O");
                                            }
                                            int gtcDigit = CountDigits(gtext);
                                            int gtcAlpha = CountAlphabetChars(gtext);
                                            int totc = gtcDigit + gtcAlpha;
                                            int totcWidth = totc * 50;
                                            int gttextLength = gtext.Length;
                                            if (gttextLength == totc && totcWidth < g.w)
                                            {
                                                //gtext = "";
                                            }
                                            if (gtext == "7F" || gtext  == "Nù." || gtext == "═ç" || gtext == "══" || gtext == "-" || gtext == "═P" || gtext == "═" || gtext == "P")
                                            {
                                                gtext = "";
                                            }
                                            if (gttextLength == 1 && ( gtext =="4" || gtext =="F" ) )
                                            {
                                                continue;
                                            }
                                            // concat all grouped text
                                            text += " " + gtext;

                                            if (group.Count() == i)
                                            {
                                                if (text.EndsWith("-"))
                                                {
                                                    if (text.Length > 1)
                                                    {
                                                        text = text.Substring(0, text.Length - 1).Trim();
                                                    }
                                                    else
                                                    {
                                                        text = string.Empty;
                                                    }
                                                }
                                                if (Regex.IsMatch(text, @"^((?:ç)(?:\s)?(?:[\-.])+(\d+)(?:\s)?(?:[A-Z]{1}))$"))
                                                {
                                                    text = text.Replace("-", ".");
                                                }
                                                if (Regex.IsMatch(text, @"^(((?:ç)|(?:─)|)(?:\s)?(?:¡)?(?:\s)?(?:\.\d+)(?:\/)?(?:\d+?\.\d+)?(?:\s)?(?:[A-Z]{1})?(?:Þ)?(?:\s)?(?:[A-Z]{1})?(?:Þ)?)$"))
                                                {
                                                    string mainValuePattern = @"^-?(([(?:¡)|(?:ç)|(?:─)|(?:\s+)]+)?([(?:\.\d+)|(?:/)|(?:\d+\.\d+)]+))?";
                                                }

                                                var first = group.First();
                                                cx = first.x;
                                                cy = first.y;

                                                var last = group.Last();
                                                xx = first.x;
                                                yy = first.y;
                                                ww = (last.x + last.w) - first.x;
                                                hh = group.Max(i => i.h);
                                                nx = first.nx;
                                                text = text.Trim();
                                                text = Regex.Replace(text, @"\r\n?|\n", "");
                                                text = text.Trim();
                                                text = text.Replace("═1-°°", "-1.00").Replace("\r\n", "").Replace("--", "");
                                                text = text
                                                    .Replace("î", "0")
                                                    .Replace("ï", "1")
                                                    .Replace("ð", "2")
                                                    .Replace("ñ", "3")
                                                    .Replace("ò", "4")
                                                    .Replace("ó", "5")
                                                    .Replace("ô", "6")
                                                    .Replace("õ", "7")
                                                    .Replace("ö", "8")
                                                    .Replace("÷", "9")
                                                    .Replace("(2)", "¡")
                                                    .Replace("(Z)", "¡")
                                                    .Replace("(/)", "¡")
                                                    ;
                                                if (text.StartsWith("I"))
                                                {
                                                    text = text.Substring(1);
                                                }
                                                if (Regex.IsMatch(text, @"[A-Z][0-9]{1}$"))
                                                {
                                                    continue;
                                                }

                                                if (text.Trim().Length < 1)
                                                    continue;
                                                if (Regex.IsMatch(text, @"[0-9]{1,}O$"))
                                                {
                                                    text = text.Replace("O", "°");
                                                }
                                                if (Regex.IsMatch(text, @"^([A-Z]{1})+(\s+)+(\w+)"))
                                                {
                                                    string[] spltxt = text.Split(" ");
                                                    //string[] newArray = spltxt.Skip(1).ToArray();
                                                    //text = string.Join(" ",newArray);
                                                }
                                                if (text.StartsWith("(") && !text.EndsWith(")"))
                                                {
                                                    text = text + ")";
                                                }
                                                if (!text.StartsWith("(") && text.EndsWith(")"))
                                                {
                                                    text = "(" + text;
                                                }
                                                if (text.Contains("X"))
                                                {
                                                    string[] hastext = text.Split("X");
                                                    if (hastext[0].Trim().Length > 0 && hastext[1].Trim().Length > 0 && Regex.IsMatch(text, @"^((\d+(\.\d+))|(\d+)|(\.\d+)(?:\s|))X((?:\s|)(\d+))$"))
                                                    {
                                                        var deg = hastext[1].Trim();
                                                        if (hastext[1].Trim().Length == 3 && hastext[1].Trim().EndsWith("0"))
                                                        {
                                                            hastext[1] = hastext[1].Substring(0, hastext[1].Length - 1) + "°";
                                                        }
                                                        text = hastext[0] + "X" + hastext[1];
                                                    }
                                                }
                                                if (text.Contains("RëGIHT") || text.Contains("RëGHT") || text.Contains("RIGHT") || text.Contains("LEFT"))
                                                {
                                                    string[] hastext = text.Split("RëGHT");
                                                    if (hastext[0].Trim().Length > 0)
                                                    {
                                                        text = hastext[0];
                                                    }
                                                    hastext = text.Split("RëGIHT");
                                                    if (hastext[0].Trim().Length > 0)
                                                    {
                                                        text = hastext[0];
                                                    }
                                                    hastext = text.Split("RIGHT");
                                                    if (hastext[0].Trim().Length > 0)
                                                    {
                                                        text = hastext[0];
                                                    }
                                                    hastext = text.Split("LEFT");
                                                    if (hastext[0].Trim().Length > 0)
                                                    {
                                                        text = hastext[0];
                                                    }

                                                }

                                                if (text.Contains("SEAT") && Regex.IsMatch(text.Trim(), @"^(((\d+(?:°))(?:±)(?:\d+(?:°))(?:\s))|(?:\d+(?:°))(?:\s))(?:SEAT)$"))
                                                {
                                                    string[] hastext = text.Split("SEAT");
                                                    if (hastext[0].Trim().Length > 0)
                                                    {
                                                        text = hastext[0].Trim();
                                                    }

                                                }

                                                Dictionary<string, string> replacements = new Dictionary<string, string>{


                                        {"³-","" },
                                        {".ë","" },
                                        {"-³ ",""},
                                        {"-³",""},
                                        {"³+",""},
                                        {"+³",""},
                                        {".³",""},
                                        {"³.",""},


                                        { "-═ ",""},
                                        { "-----",""},
                                        { "ð","2"},
                                        { " ±","±"},
                                        { "à","¡"},
                                        { "±-","±."},
                                        { "ó","6"},
                                        { "ç A",""},
                                        { "-X",""},
                                        { "APART",""}



                                      };
                                                if (string.IsNullOrWhiteSpace(text) || text == "" || text == "  " || text == " " || text == null)
                                                {
                                                    continue;
                                                }
                                                if (text.Contains(".."))
                                                {
                                                    string[] spltxt = text.Split("..");
                                                    text = spltxt[0] + " -" + spltxt[1];

                                                }
                                                if (text.Contains("+"))
                                                {
                                                    if (Regex.IsMatch(text, @"^\d+\s\d+(\.\d+)?(\+\.\d+)?$"))
                                                    {
                                                        text = text.Replace(" ", ".");
                                                    }
                                                }
                                                if (text.Contains("-"))
                                                {
                                                    if (Regex.IsMatch(text, @"^\d+\s\d+(\.\d+)?(\-\.\d+)?$"))
                                                    {
                                                        text = text.Replace(" ", ".");
                                                    }
                                                }
                                                if (text.StartsWith(","))
                                                {
                                                    text = text.Substring(1);
                                                }
                                                if (text.StartsWith("³"))
                                                {
                                                    text = text.Substring(1);
                                                }
                                                if (text.EndsWith("³"))
                                                {
                                                    text = text.Substring(0, text.Length - 1);
                                                }
                                                if (text.EndsWith("."))
                                                {
                                                    text = text.Substring(0, text.Length - 1);
                                                }

                                                if (text.StartsWith("R"))
                                                {
                                                    text = text.Replace(",", ".")
                                                               .Replace("î", "0")
                                                               .Replace("ï", "1")
                                                               .Replace("ð", "2")
                                                               .Replace("ñ", "3")
                                                               .Replace("ò", "4")
                                                               .Replace("ó", "5")
                                                               .Replace("ô", "6")
                                                               .Replace("õ", "7")
                                                               .Replace("ö", "8")
                                                               .Replace("÷", "9");
                                                    if (text == "R.00")
                                                        text = "R.005";
                                                }
                                                if (text.Contains("UNF") || text.Contains("UN"))
                                                {
                                                    text = text.Replace("═", "-")
                                                        .Replace("2BJ", "2B")
                                                        .Replace("»", "¨")
                                                        ;
                                                }
                                                if (text.Contains("ë═ëë"))
                                                {
                                                    text = text.Replace("ë═ëë", "HI");
                                                }

                                                foreach (var replacement in replacements)
                                                {
                                                    text = text.Replace(replacement.Key, replacement.Value);
                                                }
                                                bool containsBracket = text.Contains("(") || text.Contains(")");
                                                if (containsBracket)
                                                    //continue;
                                                    text = text.Trim();
                                                text = Regex.Replace(text, @"\r\n?|\n", "");
                                                text = text.Trim();
                                               // if (text.Trim().Length < 2)
                                                 //   continue;
                                                GxWords.AppendLine(text);
                                                groupedByX.Add(new AG_OCR { GroupID = g.GroupID, cx = cx, nx = nx, cy = cy, x = xx, y = yy, w = ww, h = hh, text = text });
                                            }
                                            i++;
                                        }
                                    }
                                    objerr.WriteErrorLog(" Groupped x axis Words " + GxWords);
                                    #endregion

                                    #region Selected join the PluseMinus 
                                    List<AG_OCR> sortByPluseMinuse = new List<AG_OCR>();
                                    List<int> snewList = new List<int>();

                                    int scount = 1;
                                    foreach (var i in groupedByX)
                                    {
                                        if (i.text.Contains("+"))
                                        {
                                            sortByPluseMinuse.Add(new AG_OCR { GroupID = scount, cx = i.cx, cy = i.cy, x = i.x, y = i.y, w = i.w, h = i.h, text = i.text });
                                            snewList.Add(i.GroupID);


                                            var last = sortByPluseMinuse.Last();
                                            var neartext = groupedByX
                                                       .Where(item => !snewList.Contains(item.GroupID))
                                                       .Where(item =>
                                                            (item.text.Length > 2 && (item.y - last.y) < 70 && Math.Sign(item.y - last.y) != -1 && Math.Sign((item.x + item.w) - (last.x + last.w)) != -1 && ((item.x + item.w) - (last.x + last.w)) < (last.h + 30)))

                                                       .ToList();


                                            if (neartext.Count() == 0)
                                            {

                                                if (sortByPluseMinuse.Any())
                                                {
                                                    sortByPluseMinuse.RemoveAt(sortByPluseMinuse.Count - 1);
                                                }
                                                if (snewList.Any())
                                                {
                                                    snewList.RemoveAt(snewList.Count - 1);
                                                }
                                                scount++;

                                            }
                                            foreach (var ni in neartext)
                                            {
                                                sortByPluseMinuse.Add(new AG_OCR { GroupID = scount, cx = ni.cx, cy = ni.cy, x = ni.x, y = ni.y, w = ni.w, h = ni.h, text = ni.text });
                                                snewList.Add(ni.GroupID);
                                            }
                                            if (neartext.Count() > 0)
                                            {
                                                scount++;
                                            }

                                        }
                                    }

                                    StringBuilder FiWords = new StringBuilder();
                                    groupedByX.RemoveAll(item => snewList.Contains(item.GroupID));
                                    if (groupedByX.Count() > 0)
                                    {
                                        int newId = 1;
                                        foreach (var i in groupedByX)
                                        {
                                            i.GroupID = newId++;
                                            OpenCvSharp.Rect textRegionRect = new OpenCvSharp.Rect(i.nx, i.cy, i.w, i.h);
                                            FiWords.AppendLine(i.text.ToString() + " <=> " + i.GroupID.ToString() + " <=> " + textRegionRect.ToString());
                                        }
                                    }

                                    if (sortByPluseMinuse.Count() > 0)
                                    {
                                        var grouped_PluseMinuse = sortByPluseMinuse.GroupBy(i => i.GroupID).Take(2).ToList();

                                        foreach (var group in grouped_PluseMinuse)
                                        {
                                            var last_Gx = groupedByX.Last();
                                            var last_GroupID = last_Gx.GroupID;
                                            int cx, cy, nx = 0; int xx, yy, ww, hh = 0;
                                            String text = string.Empty;
                                            int i = 1;
                                            string prefix_PluseMinuse = string.Empty;
                                            string sufix_PluseMinuse = string.Empty;
                                            foreach (var g in group)
                                            {
                                                if (g.text.Contains("+"))
                                                {
                                                    if (Regex.IsMatch(g.text, @"^\d+\s\d+(\.\d+)?(\+\.\d+)?$"))
                                                    {
                                                        g.text = g.text.Replace(" ", ".");
                                                    }
                                                }
                                                if (g.text.Contains("-"))
                                                {
                                                    if (Regex.IsMatch(g.text, @"^\d+\s\d+(\.\d+)?(\-\.\d+)?$"))
                                                    {
                                                        g.text = g.text.Replace(" ", ".");
                                                    }
                                                }

                                                if (g.text.StartsWith("+"))
                                                {
                                                    sufix_PluseMinuse = g.text;
                                                }
                                                else
                                                {
                                                    prefix_PluseMinuse = g.text;
                                                }
                                                if (g.text.StartsWith("-"))
                                                {
                                                    sufix_PluseMinuse = g.text;
                                                }
                                                else
                                                {
                                                    prefix_PluseMinuse = g.text;
                                                }

                                                if (group.Count() == i)
                                                {
                                                    text = prefix_PluseMinuse + " " + sufix_PluseMinuse;
                                                    var first = group.First();
                                                    cx = first.x;
                                                    cy = first.y;

                                                    var last = group.Last();
                                                    xx = first.x;
                                                    yy = first.y;
                                                    ww = (last.x + last.w) - first.x;
                                                    hh = group.Max(ii => ii.h);
                                                    nx = first.nx;
                                                    text = text.Trim();
                                                    last_GroupID++;
                                                    OpenCvSharp.Rect textRegionRect = new OpenCvSharp.Rect(xx, yy, ww, hh);
                                                    FiWords.AppendLine(text.ToString() + " <=> " + last_GroupID.ToString() + " <=> " + textRegionRect.ToString());
                                                    groupedByX.Add(new AG_OCR { GroupID = last_GroupID, cx = cx, nx = nx, cy = cy, x = xx, y = yy, w = ww, h = hh, text = text });
                                                }
                                                i++;

                                            }

                                        }

                                    }
                                    #endregion

                                    #region Selected Image List the Initial items
                                    List<Item> items = new List<Item>();
                                    foreach (var i in groupedByX)
                                    {
                                        auto_group_ocrresults_selected.Add(new AutoBalloon_OCR
                                        {
                                            Ocr_Text = i.text,
                                            X_Axis = (int)(i.x),
                                            Y_Axis = (int)(i.y),
                                            Width = (int)(i.w),
                                            Height = (int)(i.h),
                                            Qty = 1,
                                            No = i.GroupID
                                        });
                                        items.Add(new Item { X = i.x, Y = i.y, W = i.w, H = i.h, Text = i.text, isBallooned = false });

                                    }
                                    #endregion

                                    #region Selected get Exclude boundary
                                    StringBuilder FWordsSelected = new StringBuilder();
                                    int aftheight = 320;
                                    int finalyaxis = 0;
                                    int finalxaxis = 0;
                                    int paddingy = 220;
                                    int paddingx = 320;
                                    var infoBox = items.Where((s) => s.Text.Contains("PRODUCTS AND TECHNOLOGY")).Select((c, i) => { return c; }).ToList();

                                    if (infoBox.Count > 0)
                                    {
                                        var iBox = infoBox.First();
                                        finalyaxis = iBox.Y + 100;
                                        finalxaxis = iBox.X + 100;
                                    }
                                    #endregion

                                    // Specify the threshold for grouping based on X coordinate
                                    int thresholdX = 50;

                                    // Group items based on their X coordinates
                                    List<List<Item>> groupedItems = GroupItemsByX(items, thresholdX);

                                    #region Selected  Filter posible balloon 
                                    List<List<Item>> currentGroup = new List<List<Item>>();
                                    foreach (var g in groupedItems)
                                    {

                                        foreach (var i in g)
                                        {
                                            if (searchForm.selectedRegion != "Selected Region" && paddingx < i.X && i.X < (imagewidth - paddingx) && paddingy < i.Y && i.Y < (imageheight - paddingy))
                                            {
                                                string txtval = OcrTextOptimization(i.Text, i.X, i.Y, i.W, i.H, imagewidth, imageheight, searchForm);
                                                if (txtval != "")
                                                {
                                                    if (finalyaxis > 0 && finalxaxis > 0 && i.Y > finalyaxis && i.X > finalxaxis)
                                                    {
                                                        continue;
                                                    }
                                                    if (i.Y > (imageheight - 350))
                                                    {
                                                        continue;
                                                    }
                                                    if (searchForm.selectedRegion == "Unselected Region")
                                                    {
                                                        var eBox = lstCircle.Last();
                                                        float eY1axis = eBox.Bounds.Y;
                                                        float eX1axis = eBox.Bounds.X;
                                                        float eX2axis = eBox.Bounds.Width + eX1axis;
                                                        float eY2axis = eBox.Bounds.Height + eY1axis;
                                                        int ix = i.X;
                                                        int iy = i.Y;
                                                        int ix2 = i.X + i.W;
                                                        int iy2 = i.Y + i.H;
                                                        if ( ( ( ix < eX2axis && ix > eX1axis ) || (ix2 < eX2axis && ix2 > eX1axis) ) && ( ( iy >= eY1axis && iy <= eY2axis) || (iy2 <= eY1axis && iy2 >= eY2axis)) )
                                                        {
                                                            continue;
                                                        }
                                                       
                                                    }
                                                    i.isBallooned = true;
                                                    string[] rtext = txtval.Split(",");
                                                    i.Text = rtext[0];
                                                }
                                            }
                                            if (searchForm.selectedRegion == "Selected Region")
                                            {
                                                string txtval = OcrTextOptimization(i.Text, i.X, i.Y, i.W, i.H, imagewidth, imageheight, searchForm);
                                                if (txtval != "")
                                                {
                                                    i.isBallooned = true;
                                                    string[] rtext = txtval.Split(",");
                                                    i.Text = rtext[0];
                                                    i.X = i.X ;
                                                    i.W = i.W ;
                                                    i.Y = i.Y ;
                                                    i.H = i.H ;
                                                }
                                            }
                                            // Console.WriteLine("X:  "+ i.X+" , Y:  "+i.Y+" , Text:   "+i.Text + " , isBallooned "+ i.isBallooned);
                                        }
                                        if (g.Any(i => i.isBallooned == true && i.Text != ""))
                                        {
                                            currentGroup.Add(g);
                                        }
                                    }
                                    #endregion

                                    #region Selected Get active items  
                                    List<ActiveItems> activeItems = new List<ActiveItems>();
                                    int ai = 1;
                                    foreach (var g in currentGroup)
                                    {
                                        int nh = 0;
                                        int count = 1;
                                        foreach (var i in g)
                                        {
                                            if (i.isBallooned == true)
                                            {
                                                var citem = activeItems.Where(a => a.GroupID == ai).ToList();
                                                if (activeItems.Count > 0 && citem.Count() > 0)
                                                {
                                                    var last = citem.Last();
                                                    last.NH = nh;
                                                }
                                                nh = i.H;
                                                activeItems.Add(new ActiveItems { X = i.X, Y = i.Y, W = i.W, H = i.H, NH = nh, Text = i.Text, isBallooned = true, GroupID = ai });
                                            }
                                            else
                                            {
                                                nh = nh + i.H;
                                                if (g.Count() == count)
                                                {
                                                    var citem = activeItems.Where(a => a.GroupID == ai).ToList();
                                                    if (activeItems.Count > 0 && citem.Count() > 0)
                                                    {
                                                        var last = citem.Last();
                                                        last.NH = last.NH + nh;
                                                        last.Y = last.Y + nh;
                                                    }
                                                }

                                            }
                                            count++;
                                        }
                                        ai++;
                                    }
                                    #endregion

                                    #region Selected Sort all the item by Y
                                    var sortedItems = activeItems.OrderBy(item => item.Y).ThenBy(item => item.X).ToList();
                                    #endregion

                                    #region Selected N-X to sort items
                                    int aId = 1;
                                    int abId = 1;
                                    foreach (var i in sortedItems)
                                    {
                                        i.GroupID = abId++;
                                    }
                                    List<int> NxList = new List<int>();
                                    List<ActiveItems> NxactiveItems = new List<ActiveItems>();
                                    foreach (var i in sortedItems)
                                    {
                                        string text = i.Text;
                                        int X = i.X;
                                        int Y = i.Y;
                                        int W = i.W;
                                        int H = i.H;
                                        int NH = i.NH;
                                        int GroupID = i.GroupID;

                                        if (text.Contains("X") && Regex.IsMatch(text, @"^((\d+)(?:\s|))X$"))
                                        {
                                            NxList.Add(GroupID);
                                            var neartext = sortedItems
                                                       .Where(item => !NxList.Contains(item.GroupID))
                                                       .Where(a =>
                                                                (Math.Abs(a.Y - (Y + H)) < 50 && Math.Abs((a.X + a.W) - (X + W)) < 100)
                                                            )
                                                       .ToList();
                                            foreach (var ni in neartext)
                                            {
                                                var maxwidth = Math.Max(ni.W, W);
                                                NxList.Add(ni.GroupID);
                                                NxactiveItems.Add(new ActiveItems { X = ni.X, Y = ni.Y - H, W = maxwidth, H = ni.H + H, NH = ni.H + H, Text = text + " " + ni.Text, isBallooned = true, GroupID = ni.GroupID });
                                            }

                                        }
                                    }
                                    if (NxactiveItems.Count() > 0)
                                    {
                                        sortedItems.RemoveAll(item => NxList.Contains(item.GroupID));
                                        foreach (var i in NxactiveItems)
                                        {
                                            sortedItems.Add(new ActiveItems { X = i.X, Y = i.Y, W = i.W, H = i.H, NH = i.NH, Text = i.Text, isBallooned = true, GroupID = i.GroupID });
                                        }

                                    }
                                    sortedItems = sortedItems.OrderBy(item => item.Y).ThenBy(item => item.X).ToList();
                                    foreach (var i in sortedItems)
                                    {
                                        i.GroupID = aId++;
                                    }
                                    #endregion

                                    #region Selected Find the parent to create sub balloon
                                    List<AGF_OCR> sortByParent = new List<AGF_OCR>();
                                    List<int> pnewList = new List<int>();
                                    int gcount = 1;
                                    foreach (var i in sortedItems)
                                    {
                                        if (!pnewList.Contains(i.GroupID))
                                        {

                                            sortByParent.Add(new AGF_OCR { GroupID = gcount, parentID = 0, x = i.X, y = i.Y, w = i.W, h = i.H, text = i.Text });
                                            pnewList.Add(i.GroupID);
                                        }
                                        foreach (var ii in sortedItems)
                                        { 
                                            var last = sortByParent.Last();
                                            List<ActiveItems> sortsParent = new List<ActiveItems>();
                                            sortsParent.Add(new ActiveItems { X = ii.X, Y = ii.Y, W = ii.W, H = ii.H, NH = ii.NH, Text = ii.Text, isBallooned = true, GroupID = ii.GroupID });
                                            var neartext = sortsParent
                                                   .Where(item => !pnewList.Contains(item.GroupID))
                                                   .Where(a =>
                                                        (
                                                        (Math.Abs(a.Y - (last.y + last.h)) < 50 && Math.Abs((a.X + a.W) - (last.x + last.w)) < 100 && Math.Abs(a.X - last.x) < 150)
                                                        ||
                                                        (Math.Abs(a.Y - (last.y + last.h)) < 50 && Math.Abs((a.X + a.W) - (last.x + last.w)) < 100 && Regex.IsMatch(last.text, @"^([0-9]+(:?X))+.*$"))
                                                        ||
                                                        Regex.IsMatch(a.Text, @"^(((?:ç)|(?:─)|)(?:\s)?(?:¡)?(?:\s)?(?:\.\d+)(?:\/)?(?:\d+?\.\d+)?(?:\s)?(?:[A-Z]{1})?(?:Þ)?(?:\s)?(?:[A-Z]{1})?(?:Þ)?(?:\s)?(?:[A-Z]{1})?(?:Þ)?)$") && Math.Abs(a.Y - (last.y + last.h)) < 50 && (Math.Abs((last.x + last.w) - (a.X + a.W)) < 100))
                                                        )
                                                   .ToList();

                                            foreach (var ni in neartext)
                                            {
                                                pnewList.Add(ni.GroupID);
                                                sortByParent.Add(new AGF_OCR { GroupID = gcount, parentID = gcount, x = ni.X, y = ni.Y, w = ni.W, h = ni.H, text = ni.Text });
                                            }
                                        }
                                        gcount++;

                                    }
                                    #endregion

                                    #region Selected Image Final filter
                                    foreach (var i in sortByParent)
                                    {
                                        OpenCvSharp.Rect textRegionRect = new OpenCvSharp.Rect(i.x, i.y, i.w, i.h);
                                        FWordsSelected.AppendLine(i.text + " <=> " + i.GroupID + " <=> " + textRegionRect.ToString());
                                        bool subballoon = false;
                                        if (i.parentID == i.GroupID)
                                        {
                                            subballoon = true;
                                        }
                                        if (i.text.Contains("¨"))
                                        {
                                            string[] resultss = i.text.Split("¨");
                                            for(var s=0; s < resultss.Length; s++)
                                            {
                                                if (s > 0)
                                                {
                                                    i.parentID = i.GroupID;
                                                    subballoon = true;
                                                }
                                                auto_ocrresults.Add(new AutoBalloon_OCR { parent = i.parentID, subballoon = subballoon, Ocr_Text = resultss[s], X_Axis = Convert.ToInt32(i.x), Y_Axis = Convert.ToInt32(i.y), Width = Convert.ToInt32(i.w), Height = Convert.ToInt32(i.h), Qty = 1, No = i.GroupID });
                                            }
                                        }
                                        else
                                        {
                                            auto_ocrresults.Add(new AutoBalloon_OCR { parent = i.parentID, subballoon = subballoon, Ocr_Text = i.text, X_Axis = Convert.ToInt32(i.x), Y_Axis = Convert.ToInt32(i.y), Width = Convert.ToInt32(i.w), Height = Convert.ToInt32(i.h), Qty = 1, No = i.GroupID });
                                        }
                                        

                                    }
                                    #endregion

                                    #region Selected Oldcode Filter valid balloon process
                                    /***

                                    foreach (var dd in auto_group_ocrresults_selected)
                                    {
                                        if (paddingx < dd.X_Axis && dd.X_Axis < (imagewidth - paddingx) && paddingy < dd.Y_Axis && dd.Y_Axis < (imageheight - paddingy))
                                        {
                                            OpenCvSharp.Rect textRegionRect = new OpenCvSharp.Rect(dd.X_Axis, dd.Y_Axis, dd.Width, dd.Height);

                                            string txtval = OcrTextOptimization(dd.Ocr_Text, dd.X_Axis, dd.Y_Axis, dd.Width, dd.Height, imagewidth, imageheight, searchForm);
                                            if (txtval != "")
                                            {
                                                if (finalyaxis > 0 && finalxaxis > 0 && dd.Y_Axis > finalyaxis && dd.X_Axis > finalxaxis)
                                                {
                                                    continue;
                                                }
                                                if(dd.Y_Axis > (imageheight - 350))
                                                {
                                                    continue;
                                                }

                                                int cnt = auto_ocrresults.Count();
                                                string[] resultss = txtval.Split(",");
                                                FWordsSelected.AppendLine(resultss[0] + " <=> " + (cnt + 1) + " <=> " + textRegionRect.ToString());
                                                try
                                                {
                                                    auto_ocrresults.Add(new AutoBalloon_OCR { Ocr_Text = resultss[0], X_Axis = Convert.ToInt32(resultss[1]), Y_Axis = Convert.ToInt32(resultss[2]), Width = Convert.ToInt32(resultss[3]), Height = Convert.ToInt32(resultss[4]), Qty = 1, No = cnt + 1 });

                                                }
                                                catch (Exception ex)
                                                {

                                                }
                                            }
                                        }
                                            
                                        
                                    }
                                    */
                                    #endregion

                                    objerr.WriteErrorLog(" selected image filter words =>  " + FWordsSelected);


                                }
                            }
                        }
                    }

                    Int64 ballooncid = 1;
                    if (lstoCRResults.Count > 0)
                    {
                        ballooncid = lstoCRResults.Where(r => r.Balloon != null).Max(r => Convert.ToInt64(r.Balloon.Substring(0, r.Balloon.IndexOf('.') > 0 ? r.Balloon.IndexOf('.') : r.Balloon.Length))) + 1;
                    }
                    bool surface_finish = false;
                    bool isplmin = false;
                    string isplmin_spec = "";
                    string isplmin_pltol = "";
                    string isplmin_mintol = "";
                    foreach (var i in auto_ocrresults)
                    {
                        string ocrtext = i.Ocr_Text;
                        Int64 ocr_X = (int)i.X_Axis;
                        Int64 ocr_Y = (int)i.Y_Axis;
                        Int64 ocr_W = (int)i.Width;
                        Int64 ocr_H = (int)i.Height;

                        #region Main Balloon Filter
                        ocrtext = ocrTextTransform(ocrtext.Trim());
                        int digitCount = CountDigits(ocrtext);

                        surface_finish = false;
                        if (Regex.IsMatch(ocrtext, @"^(((?:ç)|(?:─)|)(?:\s)?(?:¡)?(?:\s)?(?:\.\d+)(?:\/)?(?:\d+?\.\d+)?(?:\s)?(?:[A-Z]{1})?(?:Þ)?(?:\s)?(?:[A-Z]{1})?(?:Þ)?)$"))
                        {
                            surface_finish = true;
                        }
                        if (!ocrtext.Contains("BOX") && !ocrtext.Contains("X") && digitCount < 1 && !surface_finish)
                        {
                            continue;
                        }
                        string Min, Max, Nominal, Type1, SubType, Unit, ToleranceType, PlusTolerance, MinusTolerance;
                        BubbleDrawing.CommonMethods cmt = new BubbleDrawing.CommonMethods();
                        cmt.GetMinMaxValues(ocrtext.Trim(), out Min, out Max, out Nominal, out Type1, out SubType, out Unit, out ToleranceType, out PlusTolerance, out MinusTolerance);

                        object isplmincheck1 = new { isplmin = false, isplmin_spec = "", isplmin_pltol = "", isplmin_mintol = "" };
                        checkedPluseMinuse(ocrtext.Trim(), out isplmincheck1);
                        Type type = isplmincheck1.GetType();
                        isplmin = (bool)type.GetProperty("isplmin").GetValue(isplmincheck1);
                        isplmin_spec = (string)type.GetProperty("isplmin_spec").GetValue(isplmincheck1);
                        isplmin_pltol = (string)type.GetProperty("isplmin_pltol").GetValue(isplmincheck1);
                        isplmin_mintol = (string)type.GetProperty("isplmin_mintol").GetValue(isplmincheck1);

                        bool isDigitPresent = ocrtext.Any(c => char.IsDigit(c));
                        string oldtext = string.Empty;
                        if (!ocrtext.Contains("BOX") && ocrtext.Contains("X") && isDigitPresent)
                        {
                            if (Regex.IsMatch(ocrtext, @"^((\.\d+)(?:\s|))X((?:\s|)(\d+))?°$"))
                            {
                                oldtext = ocrtext;
                            }
                        }
                        int Num_Qty;
                        getQty(ocrtext.Trim(), out Num_Qty);

                        bool isletonly = Regex.IsMatch(ocrtext, @"^[a-zA-Z]+$");
                        List<string> stringList = new List<string> { "«", "´", "Ú", "Û", "»" };
                        // Check if the text exactly matches any item in the list
                        bool isMatch = stringList.Any(item => item.Equals(ocrtext, StringComparison.OrdinalIgnoreCase));
                        if ((isletonly || ocrtext.Length <= 1 || ocrtext == "" || ocrtext == "X") && !isMatch)
                        {
                            continue;
                        }
                        if (Regex.IsMatch(ocrtext, @"^[a-zA-Z.]+$"))
                        {
                            continue;
                        }
                        if (!ocrtext.Contains("SPCL") && (ocrtext.Contains("7V") || ocrtext.Contains("çV") || (ocrtext.Contains("ç") && ocrtext.Contains("°")) || ocrtext.Contains("çç") || ocrtext.Contains("43X") || ocrtext.Contains("Xç") || ocrtext.Contains("///") || ocrtext.Contains("7Z") || ocrtext.Contains("ZZ") || ocrtext.Contains("J.") || ocrtext.Contains("±V") || ocrtext.Contains("Zç") || ocrtext.Contains("WV") || ocrtext.Contains("Jç") || ocrtext.Contains("1ç") || ocrtext.Contains("çE")))
                        {
                            continue;
                        }
                        if (Num_Qty > 1)
                        {
                            if (ocrtext.Contains("X"))
                            {
                                var startIndex = ocrtext.IndexOf("X");
                                var length = ocrtext.Length;
                                string substring = string.Empty;
                                if (startIndex >= 0 && startIndex + length <= ocrtext.Length)
                                {
                                     substring = ocrtext.Substring(startIndex, length);
                                }
                                else
                                {
                                    string prefix = ocrtext.Substring(0, startIndex+1);
                                     substring = ocrtext.Replace(prefix, "").Trim();
                                }
                                ocrtext = substring;
                            }
                        }
                        #endregion

                        OCRResults oCRResults = new OCRResults();
                        var Balloon = string.Empty;
                        var subBalloon = auto_ocrresults.Where(a => a.parent == i.No && a.No == i.No).ToList();
                        var parent = auto_ocrresults.Where(a => a.subballoon == false && a.parent == i.parent && a.No == i.No).ToList();
                        if (parent == null || parent.Count() == 0)
                        {
                            continue;
                        }

                        #region Based on Qty and sub-balloon based generate Balloons

                        if (Num_Qty == 1 && subBalloon.Count() == 0)
                        {
                            Balloon = Convert.ToString(ballooncid);
                            oCRResults = balloonProcess(ImageFile,drawingNo, revNo, pageNo, desFile, Balloon, ocrtext, Nominal, Min, Max, searchForm, ocr_X, ocr_Y, ocr_W, ocr_H, s_x, s_y, s_w, s_h, Type1, SubType, Unit, Num_Qty, ToleranceType, PlusTolerance, MinusTolerance, isplmin, isplmin_mintol, isplmin_pltol, isplmin_spec);
                            lstoCRResults.Add(oCRResults);
                        }
                        if (Num_Qty == 1 && subBalloon.Count() > 0)
                        {
                            Balloon = Convert.ToString(string.Join(".", ballooncid, 1));
                            oCRResults = balloonProcess(ImageFile,drawingNo, revNo, pageNo, desFile, Balloon, ocrtext, Nominal, Min, Max, searchForm, ocr_X, ocr_Y, ocr_W, ocr_H, s_x, s_y, s_w, s_h, Type1, SubType, Unit, Num_Qty, ToleranceType, PlusTolerance, MinusTolerance, isplmin, isplmin_mintol, isplmin_pltol, isplmin_spec);
                            lstoCRResults.Add(oCRResults);
                            // sub Balloon process
                            Int64 sb = 2;
                            foreach (var ii in subBalloon)
                            {
                                string ocrtext1 = ii.Ocr_Text;
                                Int64 ocr_X1 = (int)ii.X_Axis;
                                Int64 ocr_Y1 = (int)ii.Y_Axis;
                                Int64 ocr_W1 = (int)ii.Width;
                                Int64 ocr_H1 = (int)ii.Height;
                                var sBalloon = string.Empty;
                                surface_finish = false;

                                ocrtext1 = ocrTextTransform(ocrtext1.Trim());

                                if (Regex.IsMatch(ocrtext1, @"^(((?:ç)|(?:─)|)(?:\s)?(?:¡)?(?:\s)?(?:\.\d+)(?:\/)?(?:\d+?\.\d+)?(?:\s)?(?:[A-Z]{1})?(?:Þ)?(?:\s)?(?:[A-Z]{1})?(?:Þ)?)$"))
                                {
                                    surface_finish = true;
                                }

                                string Min1, Max1, Nominal1, Type11, SubType1, Unit1, ToleranceType1, PlusTolerance1, MinusTolerance1;
                                cmt.GetMinMaxValues(ocrtext1.Trim(), out Min1, out Max1, out Nominal1, out Type11, out SubType1, out Unit1, out ToleranceType1, out PlusTolerance1, out MinusTolerance1);
                                bool isplmin11 = false;
                                string isplmin_spec11 = "";
                                string isplmin_pltol11 = "";
                                string isplmin_mintol11 = "";
                                object isplmincheck11 = new { isplmin = false, isplmin_spec = "", isplmin_pltol = "", isplmin_mintol = "" };
                                checkedPluseMinuse(ocrtext1.Trim(), out isplmincheck11);
                                Type type1 = isplmincheck11.GetType();
                                isplmin11 = (bool)type1.GetProperty("isplmin").GetValue(isplmincheck11);
                                isplmin_spec11 = (string)type1.GetProperty("isplmin_spec").GetValue(isplmincheck11);
                                isplmin_pltol11 = (string)type1.GetProperty("isplmin_pltol").GetValue(isplmincheck11);
                                isplmin_mintol11 = (string)type1.GetProperty("isplmin_mintol").GetValue(isplmincheck11);

                                bool isDigitPresent1 = ocrtext1.Any(c => char.IsDigit(c));
                                string oldtext1 = string.Empty;
                                if (!ocrtext1.Contains("BOX") && ocrtext1.Contains("X") && isDigitPresent1)
                                {
                                    if (Regex.IsMatch(ocrtext1, @"^((\.\d+)(?:\s|))X((?:\s|)(\d+))?°$"))
                                    {
                                        oldtext1 = ocrtext1;
                                    }
                                }
                                int Num_Qty1;
                                getQty(ocrtext1.Trim(), out Num_Qty1);

                                bool isletonly1 = Regex.IsMatch(ocrtext, @"^[a-zA-Z]+$");
                                List<string> stringList1 = new List<string> { "«", "´", "Ú", "Û", "»" };
                                // Check if the text exactly matches any item in the list
                                bool isMatch1 = stringList.Any(item => item.Equals(ocrtext1, StringComparison.OrdinalIgnoreCase));
                                if ((isletonly1 || ocrtext1.Length <= 1 || ocrtext1 == "" || ocrtext1 == "X") && !isMatch1)
                                {
                                    continue;
                                }
                                if (Regex.IsMatch(ocrtext1, @"^[a-zA-Z.]+$"))
                                {
                                    continue;
                                }
                                if (!ocrtext1.Contains("SPCL") && (ocrtext1.Contains("7V") || ocrtext1.Contains("çV") || (ocrtext1.Contains("ç") && ocrtext1.Contains("°")) || ocrtext1.Contains("çç") || ocrtext1.Contains("43X") || ocrtext1.Contains("Xç") || ocrtext1.Contains("///") || ocrtext1.Contains("7Z") || ocrtext1.Contains("ZZ") || ocrtext1.Contains("J.") || ocrtext1.Contains("±V") || ocrtext1.Contains("Zç") || ocrtext1.Contains("WV") || ocrtext1.Contains("Jç") || ocrtext1.Contains("1ç") || ocrtext1.Contains("çE")))
                                {
                                    continue;
                                }
                                sBalloon = Convert.ToString(string.Join(".", ballooncid, sb));
                                oCRResults = balloonProcess(ImageFile,drawingNo, revNo, pageNo, desFile, sBalloon, ocrtext1, Nominal1, Min1, Max1, searchForm, ocr_X1, ocr_Y1, ocr_W1, ocr_H1, s_x, s_y, s_w, s_h, Type11, SubType1, Unit1, Num_Qty1, ToleranceType1, PlusTolerance1, MinusTolerance1, isplmin11, isplmin_mintol11, isplmin_pltol11, isplmin_spec11);
                                lstoCRResults.Add(oCRResults);
                                sb++;
                            }

                        }
                        if (Num_Qty > 1 && subBalloon.Count() == 0)
                        {
                            for (var qi = 1; qi <= Num_Qty; qi++)
                            {
                                Balloon = Convert.ToString(string.Join(".", ballooncid, qi));
                                oCRResults = balloonProcess(ImageFile,drawingNo, revNo, pageNo, desFile, Balloon, ocrtext, Nominal, Min, Max, searchForm, ocr_X, ocr_Y, ocr_W, ocr_H, s_x, s_y, s_w, s_h, Type1, SubType, Unit, Num_Qty, ToleranceType, PlusTolerance, MinusTolerance, isplmin, isplmin_mintol, isplmin_pltol, isplmin_spec);
                                lstoCRResults.Add(oCRResults);
                            }
                        }
                        if (Num_Qty > 1 && subBalloon.Count() > 0)
                        {
                            for (var qi = 1; qi <= Num_Qty; qi++)
                            {
                                Balloon = Convert.ToString(string.Join(".", ballooncid, qi));
                                oCRResults = balloonProcess(ImageFile,drawingNo, revNo, pageNo, desFile, Balloon, ocrtext, Nominal, Min, Max, searchForm, ocr_X, ocr_Y, ocr_W, ocr_H, s_x, s_y, s_w, s_h, Type1, SubType, Unit, Num_Qty, ToleranceType, PlusTolerance, MinusTolerance, isplmin, isplmin_mintol, isplmin_pltol, isplmin_spec);
                                lstoCRResults.Add(oCRResults);
                                Int64 sb = 1;
                                foreach (var ii in subBalloon)
                                {
                                    string ocrtext1 = ii.Ocr_Text;
                                    Int64 ocr_X1 = (int)ii.X_Axis;
                                    Int64 ocr_Y1 = (int)ii.Y_Axis;
                                    Int64 ocr_W1 = (int)ii.Width;
                                    Int64 ocr_H1 = (int)ii.Height;
                                    var sBalloon = string.Empty;
                                    surface_finish = false;

                                    ocrtext1 = ocrTextTransform(ocrtext1.Trim());

                                    if (Regex.IsMatch(ocrtext1, @"^(((?:ç)|(?:─)|)(?:\s)?(?:¡)?(?:\s)?(?:\.\d+)(?:\/)?(?:\d+?\.\d+)?(?:\s)?(?:[A-Z]{1})?(?:Þ)?(?:\s)?(?:[A-Z]{1})?(?:Þ)?)$"))
                                    {
                                        surface_finish = true;
                                    }

                                    string Min1, Max1, Nominal1, Type11, SubType1, Unit1, ToleranceType1, PlusTolerance1, MinusTolerance1;
                                    cmt.GetMinMaxValues(ocrtext1.Trim(), out Min1, out Max1, out Nominal1, out Type11, out SubType1, out Unit1, out ToleranceType1, out PlusTolerance1, out MinusTolerance1);
                                    bool isplmin11 = false;
                                    string isplmin_spec11 = "";
                                    string isplmin_pltol11 = "";
                                    string isplmin_mintol11 = "";
                                    object isplmincheck11 = new { isplmin = false, isplmin_spec = "", isplmin_pltol = "", isplmin_mintol = "" };
                                    checkedPluseMinuse(ocrtext1.Trim(), out isplmincheck11);
                                    Type type1 = isplmincheck11.GetType();
                                    isplmin11 = (bool)type1.GetProperty("isplmin").GetValue(isplmincheck11);
                                    isplmin_spec11 = (string)type1.GetProperty("isplmin_spec").GetValue(isplmincheck11);
                                    isplmin_pltol11 = (string)type1.GetProperty("isplmin_pltol").GetValue(isplmincheck11);
                                    isplmin_mintol11 = (string)type1.GetProperty("isplmin_mintol").GetValue(isplmincheck11);

                                    bool isDigitPresent1 = ocrtext1.Any(c => char.IsDigit(c));
                                    string oldtext1 = string.Empty;
                                    if (!ocrtext1.Contains("BOX") && ocrtext1.Contains("X") && isDigitPresent1)
                                    {
                                        if (Regex.IsMatch(ocrtext1, @"^((\.\d+)(?:\s|))X((?:\s|)(\d+))?°$"))
                                        {
                                            oldtext1 = ocrtext1;
                                        }
                                    }
                                    int Num_Qty1;
                                    getQty(ocrtext1.Trim(), out Num_Qty1);

                                    bool isletonly1 = Regex.IsMatch(ocrtext, @"^[a-zA-Z]+$");
                                    List<string> stringList1 = new List<string> { "«", "´", "Ú", "Û", "»" };
                                    // Check if the text exactly matches any item in the list
                                    bool isMatch1 = stringList.Any(item => item.Equals(ocrtext1, StringComparison.OrdinalIgnoreCase));
                                    if ((isletonly1 || ocrtext1.Length <= 1 || ocrtext1 == "" || ocrtext1 == "X") && !isMatch1)
                                    {
                                        continue;
                                    }
                                    if (Regex.IsMatch(ocrtext1, @"^[a-zA-Z.]+$"))
                                    {
                                        continue;
                                    }
                                    if (!ocrtext1.Contains("SPCL") && (ocrtext1.Contains("7V") || ocrtext1.Contains("çV") || (ocrtext1.Contains("ç") && ocrtext1.Contains("°")) || ocrtext1.Contains("çç") || ocrtext1.Contains("43X") || ocrtext1.Contains("Xç") || ocrtext1.Contains("///") || ocrtext1.Contains("7Z") || ocrtext1.Contains("ZZ") || ocrtext1.Contains("J.") || ocrtext1.Contains("±V") || ocrtext1.Contains("Zç") || ocrtext1.Contains("WV") || ocrtext1.Contains("Jç") || ocrtext1.Contains("1ç") || ocrtext1.Contains("çE")))
                                    {
                                        continue;
                                    }
                                    sBalloon = Convert.ToString(string.Join(".", Balloon, sb));
                                    oCRResults = balloonProcess(ImageFile,drawingNo, revNo, pageNo, desFile, sBalloon, ocrtext1, Nominal1, Min1, Max1, searchForm, ocr_X1, ocr_Y1, ocr_W1, ocr_H1, s_x, s_y, s_w, s_h, Type11, SubType1, Unit1, Num_Qty1, ToleranceType1, PlusTolerance1, MinusTolerance1, isplmin11, isplmin_mintol11, isplmin_pltol11, isplmin_spec11);
                                    lstoCRResults.Add(oCRResults);
                                    sb++;
                                }
                            }
                        }

                        #endregion

                        ballooncid++;
                    }
                  #region old Balloon process logic
                    /****

                    int ij = 1;
                    int beforeqty = 1;
                    int dotbaldis = 0;
                    int ballondrawedno = 0;
              
                    string afterocrtext = "";
                    string secafterocrtext = "";
                    Int64 balincid = 0;
                    int ploccur_x = 0;
                    int ploccur_y = 0;
                    int ploccur_no = 0;
                    string ploccur_ocrtxt = "";
                    var nextocr211 = "";
                    foreach (var ii in auto_ocrresults)
                    {
                        if (lstoCRResults.Count > 0)
                        {
                            balincid = lstoCRResults.Where(r => r.Balloon != null).Max(r => Convert.ToInt64(r.Balloon.Substring(0, r.Balloon.IndexOf('.') > 0 ? r.Balloon.IndexOf('.') : r.Balloon.Length))) + 1;
                        }
                        else
                        {
                            balincid = 1;
                        }
                        string ocrtext = ii.Ocr_Text;
                        if (ocrtext == "63")
                        {
                            ocrtext = "»";
                        }
                        if (ocrtext == "X5°" || ocrtext == "45Z" || ocrtext == "45" || ocrtext == "45O" || ocrtext == "42")
                        {
                            ocrtext = "45°";
                        }
                        if (ocrtext == "32")
                        {
                            ocrtext = "´";
                        }
                        if (ocrtext == "38" || ocrtext == "30" || ocrtext == "396" || ocrtext == "390" || ocrtext == "300" || ocrtext == "3905" || ocrtext == "398" || ocrtext == "30O")
                        {
                            ocrtext = "30°";
                        }
                        if (ocrtext == "150" || ocrtext == "15")
                        {
                            ocrtext = "15°";
                        }
                        if (ocrtext == "100" || ocrtext == "10")
                        {
                            ocrtext = "10°";
                        }
                        if (ocrtext == "250" || ocrtext == "25")
                        {
                            ocrtext = "25°";
                        }
                        if (ocrtext == "200")
                        {
                            ocrtext = "20°";
                        }
                        if (ocrtext == "900")
                        {
                            ocrtext = "90°";
                        }
                        if (ocrtext == "û")
                        {
                            ocrtext = "";
                        }
                        if (ocrtext == "Rù6")
                        {
                            ocrtext = "R.6";
                        }
                        if (ocrtext == "R.î3" || ocrtext == "R.îñ")
                        {
                            ocrtext = "R.03";
                        }
                        if (ocrtext == ".îîóë")
                        {
                            ocrtext = ".005";
                        }
                        if ( Regex.IsMatch(ocrtext, @"^(((?:ç)|(?:─)|)(?:\s)?(?:¡)?(?:\s)?(?:\.\d+)(?:\/)?(?:\d+?\.\d+)?(?:\s)?(?:[A-Z]{1})?(?:Þ)?(?:\s)?(?:[A-Z]{1})?(?:Þ)?)$") )
                        {

                            string mainValuePattern = @"^-?(([(?:¡)|(?:ç)|(?:─)|(?:\s+)]+)?([(?:\.\d+)|(?:/)|(?:\d+\.\d+)]+))?";
                            if (ocrtext.Contains("─"))
                            {
                                string[] spltxt = ocrtext.Split("─");
                                if (spltxt[1].Length > 0 && !spltxt[1].StartsWith(" "))
                                {
                                    if (spltxt[0] == "") { ocrtext = "─" + " " + spltxt[1]; }
                                    else { ocrtext = spltxt[0] + "─ " + spltxt[1];  }
                                }
                            }
 
                            if (ocrtext.Contains("ç"))
                            {
                                string[] spltxt = ocrtext.Split("ç");
                                if (spltxt[1].Length > 0 && !spltxt[1].StartsWith(" "))
                                {
                                    if (spltxt[0] == "") { ocrtext = "ç" + " " + spltxt[1]; }
                                    else { ocrtext = spltxt[0] + "ç " + spltxt[1]; }
                                    
                                }
                            }
    

                            Match mainValueMatch = Regex.Match(ocrtext, mainValuePattern);
                            string mainValue = mainValueMatch.Value;
                            string[] mspltxt = ocrtext.Split(mainValue);
                            if (mspltxt[1].Length > 0)
                            {
                                if (Regex.IsMatch(mspltxt[1], @"^[A-Z]")) { ocrtext = mainValue + " " + mspltxt[1]; }
                                 
                                
                            }
                            Dictionary<string, string> surface_replace = new Dictionary<string, string>{
                                {".","ù"},
                                {"0","î"},
                                {"1","ï"},
                                {"2","ð"},
                                {"3","ñ"},
                                {"4", "ò"},
                                {"5","ó"},
                                {"6","ô"},
                                {"7", "õ"},
                                {"8","ö" },
                                {"9","÷" },

                                {"─","û" },
                                {"¡","à" },
                                {" ","ì" },

                                {"A","À" },
                                {"B","Á" },
                                {"C","Â" },
                                {"D","Ã" },
                                {"E","Ä" },
                                {"F","Å" },
                                {"G","Æ" },
                                {"H","Ç" },
                                {"I","È" },
                                {"J","É" },
                                {"K","Ê" },
                                {"L","Ë" },
                                {"M","Ì" },
                                {"N","Í" },
                                {"O","Î" }



                               };

                            foreach (var replacement in surface_replace)
                            {
                                ocrtext = ocrtext.Replace(replacement.Key, replacement.Value);
                            }
                            ocrtext = "ë" + ocrtext + "í";
                            surface_finish = true;
                        }

                        string Min, Max, Nominal, Type, SubType, Unit, ToleranceType, PlusTolerance, MinusTolerance;
                        BubbleDrawing.CommonMethods cmt = new BubbleDrawing.CommonMethods();
                        cmt.GetMinMaxValues(ocrtext.Trim(), out Min, out Max, out Nominal, out Type, out SubType, out Unit, out ToleranceType, out PlusTolerance, out MinusTolerance);
                        int Num_Qty = 1;
                        int maxpreqty = 1;
                        int beforeid = 1;
                        bool isDigitPresent = ocrtext.Any(c => char.IsDigit(c));
                        if (nextocr211 != "" && nextocr211 == ocrtext)
                        {
                            nextocr211 = "";
                            continue;
                        }
                        if (ocrtext.Contains("/") || ocrtext.Contains("#"))
                        {
                            var nextocr11 = auto_ocrresults.Where(x => x.No == ii.No - 1)
                                                      .Select(x => x.Ocr_Text)
                                                      .FirstOrDefault();
                            var nextocr111 = auto_ocrresults.Where(x => x.No == ii.No + 1)
                                            .Select(x => x.Ocr_Text)
                                            .FirstOrDefault();
                            if (nextocr111 == "VAM")
                            {
                                nextocr211 = auto_ocrresults.Where(x => x.No == ii.No + 2)
                                                .Select(x => x.Ocr_Text)
                                                .FirstOrDefault();
                                var nextocr311 = auto_ocrresults.Where(x => x.No == ii.No + 3)
                                                .Select(x => x.Ocr_Text)
                                                .FirstOrDefault();
                                if (nextocr311 == "BOX" || nextocr311 == "PIN")
                                {
                                    ocrtext = nextocr11 + " " + ocrtext + " " + nextocr111 + " " + nextocr211 + " " + nextocr311;
                                }
                            }
                        }

                        if (searchForm.selectedRegion == "Full Image" || searchForm.selectedRegion == "Unselected Region")
                        {
                            if (ocrtext.Contains("+") && ocrtext.Contains("-"))
                            {
                                string mainValuePattern = @"^-?((?:¡|)(\d+|(?:\.d+)|)(?:\.\d+))?";
                                string positiveValuePattern = @"\+((\d+|(?:\.d+)|)(?:\.\d+))?";
                                string negativeValuePattern = @"-((\d+|(?:\.d+)|)(?:\.\d+))?";

                                Match mainValueMatch = Regex.Match(ocrtext, mainValuePattern);
                                MatchCollection positiveValueMatches = Regex.Matches(ocrtext, positiveValuePattern);
                                MatchCollection negativeValueMatches = Regex.Matches(ocrtext, negativeValuePattern);


                                string mainValue = mainValueMatch.Value;
                                string positive = string.Empty;
                                string negative = string.Empty;
                                foreach (Match match in positiveValueMatches)
                                {
                                    positive = match.Value;
                                }
                                foreach (Match match in negativeValueMatches)
                                {
                                    negative = match.Value;
                                }
                                isplmin = true;
                                isplmin_spec = mainValue;
                                isplmin_pltol = positive.Replace("+", "");
                                isplmin_mintol = negative.Replace("-", "");
                            }
                            else
                            {
                                if (ocrtext.Contains(" + ") && ocrtext.Length > 1)
                                {
                                    ploccur_x = ii.X_Axis;
                                    ploccur_y = ii.Y_Axis;
                                    ploccur_no = ii.No;
                                    ploccur_ocrtxt = ii.Ocr_Text;
                                    continue;
                                }
                                if (ploccur_no > 0)
                                {
                                    if (ii.X_Axis == ploccur_x - 216 && ii.Y_Axis == ploccur_y + 50)
                                    {
                                        isplmin_pltol = ploccur_ocrtxt.Replace("+", "");
                                        isplmin = true;
                                    }
                                    try
                                    {
                                        if (ocrtext.Contains("."))
                                        {
                                            int cnn = ocrtext.Count(x => x == '.');
                                            if (cnn > 2)
                                            {
                                                try
                                                {
                                                    isplmin_spec = ocrtext.Substring(0, ocrtext.IndexOf(".", ocrtext.IndexOf(".") + 1));
                                                    int dotFirstIndex = ocrtext.IndexOf('.');
                                                    int dotLastIndex = ocrtext.LastIndexOf('.');
                                                    string result = ocrtext.Substring((dotFirstIndex + 1), (dotLastIndex - dotFirstIndex) + 2);
                                                    var ind1 = result.IndexOf('.');
                                                    var ind2 = result.IndexOf('.', ind1);
                                                    isplmin_mintol = result.Substring(ind1 + ind2 - 1);
                                                }
                                                catch (Exception)
                                                {

                                                }
                                            }
                                        }
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                            }
                            
                        }
                        else
                        {
                            if (ii.No == 1 && isDigitPresent && !ocrtext.Contains("+") && !ocrtext.Contains("±") && !ocrtext.Contains("X") && isplmin)
                            {
                                isplmin_spec = ocrtext;
                                continue;
                            }
                            if (ocrtext.Contains("+") && ocrtext.Contains("-"))
                            {
                                string mainValuePattern = @"^-?((?:¡|)(\d+|(?:\.d+)|)(?:\.\d+))?";
                                string positiveValuePattern = @"\+((\d+|(?:\.d+)|)(?:\.\d+))?";
                                string negativeValuePattern = @"-((\d+|(?:\.d+)|)(?:\.\d+))?";

                                Match mainValueMatch = Regex.Match(ocrtext, mainValuePattern);
                                MatchCollection positiveValueMatches = Regex.Matches(ocrtext, positiveValuePattern);
                                MatchCollection negativeValueMatches = Regex.Matches(ocrtext, negativeValuePattern);


                                string mainValue = mainValueMatch.Value;
                                string positive = string.Empty;
                                string negative = string.Empty;
                                foreach (Match match in positiveValueMatches)
                                {
                                    positive = match.Value;
                                }
                                foreach (Match match in negativeValueMatches)
                                {
                                    negative = match.Value;
                                }
                                isplmin = true;
                                isplmin_spec = mainValue;
                                isplmin_pltol = positive;
                                isplmin_mintol = negative;
                            }
                            else
                            {
                                if (ocrtext.Contains("+"))
                                {
                                    isplmin = true;
                                    if (ocrtext.Length == 1)
                                    {
                                        var nextocr = auto_ocrresults.Where(x => x.No == ii.No + 1)
                                                      .Select(x => x.Ocr_Text)
                                                      .FirstOrDefault();
                                        if (!nextocr.Contains("."))
                                        {
                                            isplmin_pltol = "." + nextocr;
                                        }
                                        var nextocr1 = auto_ocrresults.Where(x => x.No == ii.No + 2)
                                                        .Select(x => x.Ocr_Text)
                                                        .FirstOrDefault();
                                        var nextocr2 = auto_ocrresults.Where(x => x.No == ii.No + 3)
                                                        .Select(x => x.Ocr_Text)
                                                        .FirstOrDefault();
                                        var nextocr3 = auto_ocrresults.Where(x => x.No == ii.No + 4)
                                                        .Select(x => x.Ocr_Text)
                                                        .FirstOrDefault();
                                        if (nextocr1.Contains(".") && nextocr1.Contains("¡") && nextocr1 != "" && nextocr2 != "")
                                        {
                                            isplmin_spec = nextocr1 + nextocr2.Replace(".", "");
                                        }
                                        if (nextocr3 != "")
                                        {
                                            isplmin_mintol = "." + nextocr3;
                                        }
                                    }
                                    else
                                    {
                                        if (ocrtext.LastIndexOf('+') == ocrtext.Length - 1)
                                        {
                                            isplmin_spec = ocrtext.Replace("+", "");
                                        }
                                        else
                                        {
                                            isplmin_pltol = ocrtext.Replace("+", "");
                                        }
                                        continue;
                                    }
                                }
                                else if (isplmin && (ocrtext.Contains("-") || ocrtext.Contains("..") || ocrtext.Contains(".")))
                                {
                                    isplmin = true;
                                    if (ocrtext.Contains(".."))
                                    {
                                        string[] spltxt = ocrtext.Split("..");
                                        isplmin_spec = spltxt[0];
                                        isplmin_mintol = spltxt[1].Replace("-", "");
                                        string pattern11 = @"^\.+$";
                                        Regex regex11 = new Regex(pattern11);
                                        MatchCollection matches11 = regex11.Matches(isplmin_mintol);
                                        if (matches11.Count == 0)
                                        {
                                            isplmin_mintol = "." + isplmin_mintol;
                                        }
                                    }
                                    else if (ocrtext.Contains("."))
                                    {
                                        isplmin_spec = ocrtext.Substring(0, ocrtext.IndexOf(".", ocrtext.IndexOf(".") + 1));

                                        int count1 = ocrtext.Count(x => x == '.');
                                        if (count1 == 2)
                                        {
                                            var splvals = ocrtext.Split(".")[2];
                                            int aStr = isplmin_pltol.Length - isplmin_pltol.IndexOf(".") - 1;
                                            isplmin_mintol = splvals.Insert(splvals.Length - aStr, ".");
                                        }
                                    }
                                    else
                                    {
                                        isplmin_mintol = ocrtext.Replace("-", "");
                                    }
                                    if (ii.No != auto_ocrresults.Count)
                                    {
                                        continue;
                                    }
                                }
                            }
                        }
                        string oldtext = string.Empty;
                        if (!ocrtext.Contains("BOX") && ocrtext.Contains("X") && isDigitPresent)
                        {
                            if (Regex.IsMatch(ocrtext, @"^((\.\d+)(?:\s|))X((?:\s|)(\d+))?°$"))
                            {
                                oldtext = ocrtext;
                            }
                        }
                         if (!ocrtext.Contains("BOX") && ocrtext.Contains("X") && isDigitPresent)
                        {
                            string qty = "";
                            if (ocrtext.Length > 2)
                            {
                                int count = Regex.Matches(ocrtext, "X").Count;
                                if (count > 1)
                                {
                                    string[] result4 = ocrtext.Split('X');
                                    if (Char.IsNumber(result4[0], 0))
                                    {
                                        qty = result4[0];
                                    }
                                    else if (Char.IsNumber(result4[1], 0))
                                    {
                                        qty = result4[1];
                                    }
                                }
                                else
                                {
                                    qty = ocrtext.Substring(0, ocrtext.IndexOf("X")).Replace(" ", "");
                                }
                            }
                            else
                            {
                                qty = ocrtext.Substring(0, ocrtext.IndexOf("X")).Replace(" ", "");
                            }
                            int value;
                            if (int.TryParse(qty, out value))
                                Num_Qty = Convert.ToInt16(qty);
                            beforeqty = value;
                            string[] s = ocrtext.Split('X');
                            if (s.Length > 1)
                            {
                                ocrtext = s[1].Replace(",", "").Replace("O", "°");
                                try
                                {
                                    if (Convert.ToInt16(ocrtext) == beforeqty)
                                    {
                                        foreach (var jj in auto_ocrresults.Where(r => r.No == ii.No))
                                        {
                                            jj.Qty = beforeqty;
                                        }
                                        continue;
                                    }
                                }
                                catch (Exception ex)
                                {

                                }
                                string tmp = Regex.Replace(ocrtext, "[^$0-9.]", "");
                                if (tmp != "")
                                {
                                    cmt.GetMinMaxValues(tmp.Trim(), out Min, out Max, out Nominal, out Type, out SubType, out Unit, out ToleranceType, out PlusTolerance, out MinusTolerance);
                                }
                                if (ocrtext != "")
                                {
                                    foreach (var jj in auto_ocrresults.Where(r => r.No == ii.No))
                                    {
                                        jj.Qty = beforeqty;
                                    }
                                    maxpreqty = beforeqty;
                                }
                                else
                                {
                                    if (beforeqty > 1)
                                    {
                                        foreach (var jj in auto_ocrresults.Where(r => r.No == ii.No))
                                        {
                                            jj.Qty = beforeqty;
                                        }
                                        maxpreqty = beforeqty;
                                    }
                                    continue;
                                }
                            }
                            else
                            {
                                foreach (var jj in auto_ocrresults.Where(r => r.No == ii.No))
                                {
                                    jj.Qty = beforeqty;
                                }
                                continue;
                            }
                            foreach (var jj in auto_ocrresults.Where(r => r.No == ii.No))
                            {
                                jj.Qty = beforeqty;
                            }
                        }
                        OCRResults oCRResults = new OCRResults();
                        System.Drawing.RectangleF rectElipse = new System.Drawing.RectangleF(ii.X_Axis, ii.Y_Axis, ii.Width, ii.Height);
                        Circle_AutoBalloon hitCircle2 = lstCircle.FirstOrDefault(x1 => x1.Bounds.Contains(rectElipse.Location));
                        if ((searchForm.selectedRegion == "Full Image" && hitCircle2 == null) || (searchForm.selectedRegion == "Unselected Region" && hitCircle2 == null) || (searchForm.selectedRegion == "Selected Region"))
                        {
                            beforeid = ii.No - 1;
                            if (maxpreqty > 1)
                            {
                                beforeid = ii.No;
                            }
                            beforeqty = auto_ocrresults.Where(x => x.No == beforeid && ((ii.X_Axis - x.X_Axis < 150) || (ii.Y_Axis- x.Y_Axis < 150) ) ) 
                          .Select(x => x.Qty)
                          .FirstOrDefault();
                            if (afterocrtext == ii.Ocr_Text || secafterocrtext == ii.Ocr_Text)
                            {
                                if (secafterocrtext == ii.Ocr_Text)
                                {
                                    afterocrtext = "";
                                    secafterocrtext = "";
                                }
                                continue;
                            }
                            if (ii.Ocr_Text == "ç")
                            {
                                afterocrtext = auto_ocrresults.Where(x => x.No == ii.No + 1).Select(x => x.Ocr_Text).FirstOrDefault();
                                secafterocrtext = auto_ocrresults.Where(x => x.No == ii.No + 2).Select(x => x.Ocr_Text).FirstOrDefault();
                                if (!afterocrtext.Contains("X") && !Regex.IsMatch(secafterocrtext, @"\d*\.\d+|\d+"))
                                {
                                    ocrtext = ii.Ocr_Text + afterocrtext + secafterocrtext;
                                }
                                else
                                {
                                    afterocrtext = "";
                                    secafterocrtext = "";
                                }
                            }
                            if (ocrtext.Contains("0G"))
                            {
                                ocrtext = ocrtext.Replace("0G", "00");
                            }
                            bool isletonly = Regex.IsMatch(ocrtext, @"^[a-zA-Z]+$");
                            List<string> stringList = new List<string>
                                            {
                                                "«",
                                                "´",
                                                "Ú",
                                                "Û",
                                                "»"
                                            };

                            // Check if the text exactly matches any item in the list
                            bool isMatch = stringList.Any(item => item.Equals(ocrtext, StringComparison.OrdinalIgnoreCase));
                            if ((isletonly || ocrtext.Length <= 1 || ocrtext == "" || ocrtext == "X") && !isMatch)
                            {
                                continue;
                            }
                            if (Regex.IsMatch(ocrtext, @"^[a-zA-Z.]+$"))
                            {
                                continue;
                            }
                            if (!ocrtext.Contains("SPCL") && ( ocrtext.Contains("7V") || ocrtext.Contains("çV") || (ocrtext.Contains("ç") && ocrtext.Contains("°")) || ocrtext.Contains("çç") || ocrtext.Contains("43X") || ocrtext.Contains("Xç") || ocrtext.Contains("///") || ocrtext.Contains("7Z") || ocrtext.Contains("ZZ") || ocrtext.Contains("J.") || ocrtext.Contains("±V") || ocrtext.Contains("Zç") || ocrtext.Contains("WV") || ocrtext.Contains("Jç") || ocrtext.Contains("1ç") || ocrtext.Contains("çE")))
                            {
                                continue;
                            }
                            if (beforeqty > 1 || maxpreqty > 1)
                            {

                                cmt.GetMinMaxValues(ocrtext.Trim(), out Min, out Max, out Nominal, out Type, out SubType, out Unit, out ToleranceType, out PlusTolerance, out MinusTolerance);
                                for (int k = 1; k <= beforeqty; k++)
                                {
                                    if (k == 1)
                                    {
                                        dotbaldis++;
                                    }
                                    OCRResults oCRResults1 = new OCRResults();
                                    string cnt = k.ToString();
                                    oCRResults1.BaloonDrwID = 0;
                                    oCRResults1.DrawingNumber = drawingNo;
                                    oCRResults1.Revision = revNo.ToUpper();
                                    oCRResults1.Page_No = pageNo;
                                    oCRResults1.BaloonDrwFileID = desFile;
                                    oCRResults1.ProductionOrderNumber = "N/A";
                                    oCRResults1.Part_Revision = "N/A";
                                    oCRResults1.Balloon = Convert.ToString(string.Join(".", Convert.ToInt16(balincid), cnt));
                                    oCRResults1.Spec = ocrtext.ToString();
                                    oCRResults1.Nominal = Nominal;
                                    oCRResults1.Minimum = Min;
                                    oCRResults1.Maximum = Max;
                                    oCRResults1.MeasuredBy = username;
                                    oCRResults1.MeasuredOn = DateTime.Now;
                                    if (searchForm.selectedRegion == "Selected Region")
                                    {
                                        oCRResults1.Crop_X_Axis = (int)ii.X_Axis + (int)s_x - 100;
                                        oCRResults1.Crop_Y_Axis = (int)ii.Y_Axis + (int)s_y - 100;
                                        oCRResults1.Crop_Width = (int)ii.Width + (int)s_w;
                                        oCRResults1.Crop_Height = (int)ii.Height + (int)s_h;
                                        oCRResults1.Circle_X_Axis = (int)ii.X_Axis + (int)s_x - 100;
                                        oCRResults1.Circle_Y_Axis = (int)ii.Y_Axis + (int)s_y - 100;
                                        oCRResults1.Circle_Width = 28;
                                        oCRResults1.Circle_Height = 28;
                                    }
                                    else
                                    {
                                        if (ii.X_Axis < 28)
                                        {
                                            oCRResults1.Crop_X_Axis = (int)ii.X_Axis + 29;
                                            oCRResults1.Circle_X_Axis = (int)ii.X_Axis + 29;
                                        }
                                        else
                                        {
                                            oCRResults1.Crop_X_Axis = (int)ii.X_Axis;
                                            oCRResults1.Circle_X_Axis = (int)ii.X_Axis;
                                        }
                                        oCRResults1.Crop_Y_Axis = ii.Y_Axis;
                                        oCRResults1.Crop_Width = ii.Width;
                                        oCRResults1.Crop_Height = ii.Height;
                                        oCRResults1.Circle_Y_Axis = (int)ii.Y_Axis;
                                        oCRResults1.Circle_Width = 28;
                                        oCRResults1.Circle_Height = 28;
                                    }
                                    oCRResults1.Type = Type;
                                    oCRResults1.SubType = SubType;
                                    oCRResults1.Unit = Unit;
                                    oCRResults1.Quantity = (int)beforeqty;
                                    if (Min != "" && Max != "")
                                    {
                                        oCRResults1.ToleranceType = ToleranceType;
                                    }
                                    else
                                    {
                                        oCRResults1.ToleranceType = "Default";
                                    }
                                    if (ocrtext.Contains("R."))
                                    {
                                        oCRResults1.ToleranceType = "Linear";
                                    }
                                    if (PlusTolerance != "")
                                    {
                                        oCRResults1.PlusTolerance = "+" + PlusTolerance;
                                    }
                                    else
                                    {
                                        oCRResults1.PlusTolerance = "0";
                                    }
                                    if (MinusTolerance != "")
                                    {
                                        oCRResults1.MinusTolerance = "-" + MinusTolerance;
                                    }
                                    else
                                    {
                                        oCRResults1.MinusTolerance = "0";
                                    }
                                    oCRResults1.MaxTolerance = "";
                                    oCRResults1.MinTolerance = "";
                                    oCRResults1.CreatedBy = username;
                                    oCRResults1.CreatedDate = DateTime.Now;
                                    oCRResults1.ModifiedBy = "";
                                    oCRResults1.ModifiedDate = DateTime.Now;
                                    oCRResults1.x = ii.X_Axis;
                                    oCRResults1.y = ii.Y_Axis;
                                    oCRResults1.width = ii.Width;
                                    oCRResults1.height = ii.Height;
                                    oCRResults1.id = "";
                                    oCRResults1.selectedRegion = "Full Image";
                                    if (isplmin && isplmin_mintol != "" && isplmin_pltol != "" && isplmin_spec != "")
                                    {
                                        oCRResults1.Spec = isplmin_spec;
                                        oCRResults1.Nominal = isplmin_spec;
                                        try
                                        {
                                            oCRResults1.Minimum = Convert.ToString(Convert.ToDecimal(isplmin_spec.Replace("¡", "")) - Convert.ToDecimal(isplmin_mintol));
                                            oCRResults1.Maximum = Convert.ToString(Convert.ToDecimal(isplmin_spec.Replace("¡", "")) + Convert.ToDecimal(isplmin_pltol));
                                            oCRResults1.MinusTolerance = "-" + Convert.ToString(isplmin_mintol);
                                            oCRResults1.PlusTolerance = "+" + Convert.ToString(isplmin_pltol);
                                        }
                                        catch (Exception)
                                        {

                                        }
                                        oCRResults1.ToleranceType = "Linear";
                                        oCRResults1.Type = "Dimension";
                                        oCRResults1.Unit = "Inches";
                                        oCRResults1.SubType = "Circularity";
                                    }
                                    if (ocrtext.Contains("BOX") || ocrtext.Contains("PIN"))
                                    {
                                        oCRResults1.ToleranceType = "Linear";
                                        oCRResults1.Unit = "";
                                        oCRResults1.Type = "";
                                        oCRResults1.SubType = "";
                                        oCRResults1.Minimum = "0";
                                        oCRResults1.Maximum = "0";
                                        oCRResults1.PlusTolerance = "0";
                                        oCRResults1.MinusTolerance = "0";
                                    }
                                    lstoCRResults.Add(oCRResults1);
                                }
                                isplmin = false;
                                isplmin_mintol = "";
                                isplmin_pltol = "";
                                isplmin_spec = "";
                                ij++;
                                beforeqty = 1;
                                foreach (var jj in auto_ocrresults.Where(r => r.No == ii.No))
                                {
                                    jj.Qty = 1;
                                }
                            }
                            else
                            {
                                if (surface_finish)
                                {
                                    var surf_parent = auto_ocrresults.Where(w => (w.Y_Axis < ii.Y_Axis )).FirstOrDefault();
                                }
                                if (oldtext !="")
                                {
                                    ocrtext = oldtext;
                                }
                                var hdrnew1 = auto_ocrresults.Where(w => (w.Y_Axis == ii.Y_Axis + 21)).FirstOrDefault();
                                if (hdrnew1 != null)
                                {
                                    ballondrawedno = hdrnew1.No;
                                }
                                if (ballondrawedno > 0 && ii.No == ballondrawedno && ii.Ocr_Text.Contains("+"))
                                {
                                    ballondrawedno = 0;
                                    continue;
                                }
                                oCRResults.BaloonDrwID = 0;
                                oCRResults.DrawingNumber = drawingNo;
                                oCRResults.Revision = revNo.ToUpper();
                                oCRResults.Page_No = pageNo;
                                oCRResults.BaloonDrwFileID = desFile;
                                oCRResults.ProductionOrderNumber = "N/A";
                                oCRResults.Part_Revision = "N/A";
                                oCRResults.Balloon = Convert.ToString(Convert.ToInt16(balincid));
                                oCRResults.Spec = ocrtext.ToString();
                                oCRResults.Nominal = Nominal;
                                oCRResults.Minimum = Min;
                                oCRResults.Maximum = Max;
                                oCRResults.MeasuredBy = username;
                                oCRResults.MeasuredOn = DateTime.Now;
                                if (searchForm.selectedRegion == "Selected Region")
                                {
                                    oCRResults.Crop_X_Axis = (int)ii.X_Axis + (int)s_x - 100;
                                    oCRResults.Crop_Y_Axis = (int)ii.Y_Axis + (int)s_y - 100;
                                    oCRResults.Crop_Width = (int)ii.Width + (int)s_w;
                                    oCRResults.Crop_Height = (int)ii.Height + (int)s_h;
                                    oCRResults.Circle_X_Axis = (int)ii.X_Axis + (int)s_x - 100;
                                    oCRResults.Circle_Y_Axis = (int)ii.Y_Axis + (int)s_y - 100;
                                    oCRResults.Circle_Width = (int)ii.Width + (int)s_w;
                                    oCRResults.Circle_Height = (int)ii.Height + (int)s_h;
                                }
                                else
                                {
                                    if (ii.X_Axis < 28)
                                    {
                                        oCRResults.Crop_X_Axis = (int)ii.X_Axis + 29;
                                        oCRResults.Circle_X_Axis = (int)ii.X_Axis + +29;
                                    }
                                    else
                                    {
                                        oCRResults.Crop_X_Axis = (int)ii.X_Axis;
                                        oCRResults.Circle_X_Axis = (int)ii.X_Axis;
                                    }
                                    oCRResults.Crop_Y_Axis = ii.Y_Axis;
                                    oCRResults.Crop_Width = ii.Width;
                                    oCRResults.Crop_Height = ii.Height;
                                    oCRResults.Circle_Y_Axis = (int)ii.Y_Axis;
                                    oCRResults.Circle_Width = (int)ii.Width;
                                    oCRResults.Circle_Height = (int)ii.Height;
                                }
                                oCRResults.Type = Type;
                                oCRResults.SubType = SubType;
                                oCRResults.Unit = Unit;
                                oCRResults.Quantity = (int)Num_Qty;
                                if (Min != "" && Max != "")
                                {
                                    oCRResults.ToleranceType = ToleranceType;
                                }
                                else
                                {
                                    oCRResults.ToleranceType = "Default";
                                }
                                if (ocrtext.Contains("R.") || oldtext != "")
                                {
                                    oCRResults.ToleranceType = "Linear";
                                }
                                if (PlusTolerance != "")
                                {
                                    oCRResults.PlusTolerance = "+" + PlusTolerance;
                                }
                                else
                                {
                                    oCRResults.PlusTolerance = "0";
                                }
                                if (MinusTolerance != "")
                                {
                                    oCRResults.MinusTolerance = "-" + MinusTolerance;
                                }
                                else
                                {
                                    oCRResults.MinusTolerance = "0";
                                }
                                oCRResults.MaxTolerance = "";
                                oCRResults.MinTolerance = "";
                                oCRResults.CreatedBy = username;
                                oCRResults.CreatedDate = DateTime.Now;
                                oCRResults.ModifiedBy = "";
                                oCRResults.ModifiedDate = DateTime.Now;
                                oCRResults.x = ii.X_Axis;
                                oCRResults.y = ii.Y_Axis;
                                oCRResults.width = ii.Width;
                                oCRResults.height = ii.Height;
                                oCRResults.id = "";
                                oCRResults.selectedRegion = "Full Image";
                                if (isplmin && isplmin_mintol != "" && isplmin_pltol != "" && isplmin_spec != "")
                                {
                                    oCRResults.Spec = isplmin_spec;
                                    oCRResults.Nominal = isplmin_spec;
                                    try
                                    {
                                        oCRResults.Minimum = Convert.ToString(Convert.ToDecimal(isplmin_spec.Replace("¡", "")) - Convert.ToDecimal(isplmin_mintol));
                                        oCRResults.Maximum = Convert.ToString(Convert.ToDecimal(isplmin_spec.Replace("¡", "")) + Convert.ToDecimal(isplmin_pltol));
                                        oCRResults.MinusTolerance = "-" + Convert.ToString(isplmin_mintol);
                                        oCRResults.PlusTolerance = "+" + Convert.ToString(isplmin_pltol);
                                    }
                                    catch (Exception)
                                    {

                                    }
                                    oCRResults.ToleranceType = "Linear";
                                    oCRResults.Type = "Dimension";
                                    oCRResults.Unit = "Inches";
                                    oCRResults.SubType = "Circularity";
                                    isplmin = false;
                                    isplmin_mintol = "";
                                    isplmin_pltol = "";
                                    isplmin_spec = "";
                                }
                                if (ocrtext.Contains("BOX") || ocrtext.Contains("PIN"))
                                {
                                    oCRResults.ToleranceType = "Linear";
                                    oCRResults.Unit = "";
                                    oCRResults.Type = "";
                                    oCRResults.SubType = "";
                                    oCRResults.Minimum = "0";
                                    oCRResults.Maximum = "0";
                                    oCRResults.PlusTolerance = "0";
                                    oCRResults.MinusTolerance = "0";
                                }
                                if (Type == "Surface Finish")
                                {
                                    oCRResults.ToleranceType = "Linear";
                                }
                                lstoCRResults.Add(oCRResults);

                                ij++;
                            }
                        }
                    }
                    */
                  #endregion

                    if (searchForm.selectedRegion == "Selected Region")
                    {
                        System.IO.File.Delete(temp);
                    }
                    System.IO.File.Delete(processImageFile);
                    System.IO.File.Delete(ImageFile);

                    /*
                    objbaldet.drawingNo = searchForm.CdrawingNo;
                    objbaldet.revNo = searchForm.CrevNo;
                    objbaldet.totalPage = searchForm.totalPage;
                    objbaldet.pageNo = searchForm.pageNo;
                    objbaldet.rotate = searchForm.rotate;
                    objbaldet.ballonDetails = lstoCRResults;
                    returnObject = balcon.create(objbaldet);
                    */
                    returnObject = lstoCRResults;
                }
                catch (Exception ex)
                {
                    objerr.WriteErrorToText(ex);
                    returnObject = lstoCRResults;// new List<object>();
                    //returnObject = balcon.get(searchForm.CdrawingNo, searchForm.CrevNo);

                }
                return StatusCode(StatusCodes.Status200OK, returnObject);
            }
        }

        #endregion

        #region Saving All the Balloons
        [HttpPost]
        [Route("saveAllBalloons")]
        public async Task<ActionResult<ResetBalloon>> saveAllBalloons(BubbleDrawingAutomationWeb.Controllers.CreateBalloon searchForm)
        {
            if (_dbcontext.TblConfigurations == null)
            {
                return NotFound();
            }
            else
            {
                BubbleDrawingAutomationWeb.Controllers.BalloonController balcon = new BubbleDrawingAutomationWeb.Controllers.BalloonController(_dbcontext);
                BubbleDrawingAutomationWeb.Controllers.CreateBalloon createBalloon = new BubbleDrawingAutomationWeb.Controllers.CreateBalloon();
                IEnumerable<object> returnObject = new List<object>();
                createBalloon.drawingNo = searchForm.drawingNo;
                createBalloon.revNo = searchForm.revNo;
                createBalloon.totalPage = searchForm.totalPage;
                createBalloon.pageNo = searchForm.pageNo;
                createBalloon.rotate = searchForm.rotate;
                createBalloon.ballonDetails = searchForm.ballonDetails;
                returnObject = balcon.create(createBalloon);
                return StatusCode(StatusCodes.Status200OK, returnObject);
            }
        }

        #endregion

        #region Pop-up Balloon Update
        public string getNomianal(string OCR_Text, string Nominal)
        {
            int count = OCR_Text.Count(f => f == '.');
            BubbleDrawing.CommonMethods cmt = new BubbleDrawing.CommonMethods();
            string[] minmax;
            string nominalv = string.Empty;
            if ((OCR_Text.Contains("±") || OCR_Text.Contains("+") || OCR_Text.Contains("-") || count == 1 || count == 2 || count == 3 || OCR_Text.Contains("Û") || OCR_Text.Contains("Ú") || OCR_Text.Contains("´") || OCR_Text.Contains("»") || OCR_Text.Contains("«") || OCR_Text.Contains("ëûí") || OCR_Text.Contains("ëÐí")) && !OCR_Text.Contains("°"))
            {

                minmax = cmt.AssignMinMaxValue(OCR_Text).Split(',');
                if (minmax.Length > 0)
                {
                    nominalv = minmax[0];

                }

            }
            else if (Nominal.Contains("°"))
            {
                if (OCR_Text.Contains("°") && OCR_Text.Contains("±"))
                {
                    nominalv = Convert.ToString(OCR_Text.Substring(0, OCR_Text.IndexOf("±")).Replace("°", ""));
                }
                else
                {
                    nominalv = Nominal.Replace("°", "");
                }
            }
            return nominalv;
        }
        [HttpPost("specificationUpdate")]
        public IActionResult specificationUpdate(Specification i)
        {
            string Min, Max, Nominal, Type, SubType, Unit, ToleranceType, PlusTolerance, MinusTolerance;
            BubbleDrawing.CommonMethods cmt = new BubbleDrawing.CommonMethods();
            var OCR_Text = i.spec.Trim();
            if (OCR_Text.Contains("X"))
            {
                OCR_Text = OCR_Text.Substring(OCR_Text.IndexOf("X"), OCR_Text.Length - OCR_Text.IndexOf("X")).Replace("X", "").Trim(); ;
            }
            cmt.GetMinMaxValues(OCR_Text, out Min, out Max, out Nominal, out Type, out SubType, out Unit, out ToleranceType, out PlusTolerance, out MinusTolerance);

            bool isplmin = false;
            string isplmin_spec = "";
            string isplmin_pltol = "";
            string isplmin_mintol = "";
            List<OCRResults> originalRegions = i.originalRegions;

            string plusTolerance = i.plusTolerance;
            string minusTolerance = i.minusTolerance;
            isplmin_pltol = plusTolerance.Replace("+", "");
            isplmin_mintol = minusTolerance.Replace("-", "");
            var oldocr = originalRegions.Select((s) => new { Spec = s.Spec, Min = s.Minimum, Max = s.Maximum, MinusTolerance = s.MinusTolerance, PlusTolerance = s.PlusTolerance, ToleranceType = s.ToleranceType }).FirstOrDefault();
            string oldplusTolerance = oldocr.PlusTolerance;
            string oldminusTolerance = oldocr.MinusTolerance;
            string oldToleranceType = oldocr.ToleranceType;
            string oldMin = oldocr.Min;
            string oldMax = oldocr.Max;
            string oldSpec = oldocr.Spec;
            string old_pltol = oldplusTolerance.Replace("+", "");
            string old_mintol = oldminusTolerance.Replace("-", "");
            string nominalv = string.Empty;
            if ((isplmin_pltol != old_pltol && old_pltol != "") || (isplmin_mintol != old_mintol && old_mintol != ""))
            {
                isplmin = true;
                if (isplmin && isplmin_mintol != "" && isplmin_pltol != "")
                {
                    string minv = string.Empty;
                    string maxv = string.Empty;

                    string plustolerancev = string.Empty;
                    string minustolerancev = string.Empty;
                    nominalv = getNomianal(OCR_Text, Nominal);
                    if (nominalv != "")
                    {
                        try
                        {
                            minv = Convert.ToString(Convert.ToDecimal(nominalv) - Convert.ToDecimal(isplmin_mintol));
                            maxv = Convert.ToString(Convert.ToDecimal(nominalv) + Convert.ToDecimal(isplmin_pltol));
                            minustolerancev = Convert.ToString(isplmin_mintol);
                            plustolerancev = Convert.ToString(isplmin_pltol);
                            string[] doubleArrayPlusTolerance = plustolerancev.Split('.');
                            string[] doubleArrayMinusTolerance = minustolerancev.Split('.');
                            var mlen = 0; var min1slen = 0;
                            var plen = 0; var max1slen = 0;
                            if (Convert.ToDecimal(isplmin_mintol) > 0)
                            {
                                Decimal min1 = Convert.ToDecimal(minv);
                                if (doubleArrayMinusTolerance.Length > 1)
                                {
                                    mlen = doubleArrayMinusTolerance[1].Length;
                                }
                                if (mlen < 2) { mlen = 2; }
                                string[] min1s = minv.Split('.');
                                if (min1s.Length > 1)
                                {
                                    min1slen = min1s[1].Length;
                                }
                                int miValue = Math.Max(mlen, min1slen);
                                minv = min1.ToString($"F{miValue}");
                            }
                            else { minv = ""; }
                            if (Convert.ToDecimal(isplmin_pltol) > 0)
                            {
                                Decimal max1 = Convert.ToDecimal(maxv);
                                if (doubleArrayPlusTolerance.Length > 1)
                                {
                                    plen = doubleArrayPlusTolerance[1].Length;
                                }
                                if (plen < 2) { plen = 2; }
                                string[] max1s = maxv.Split('.');
                                if (max1s.Length > 1)
                                {
                                    max1slen = max1s[1].Length;
                                }
                                int miiValue = Math.Max(plen, max1slen);
                                maxv = max1.ToString($"F{miiValue}");
                            }
                            else { maxv = ""; }
                            Min = minv;
                            Max = maxv;
                            Nominal = nominalv;
                            PlusTolerance = plustolerancev;
                            MinusTolerance = minustolerancev;
                        }
                        catch (Exception ex)
                        {
                            Min = i.minimum;
                            Max = i.maximum;
                            Nominal = OCR_Text;
                            PlusTolerance = isplmin_pltol;
                            MinusTolerance = isplmin_mintol;
                        }
                    }
                    else
                    {
                        Min = i.minimum;
                        Max = i.maximum;
                        Nominal = OCR_Text;
                        PlusTolerance = isplmin_pltol;
                        MinusTolerance = isplmin_mintol;
                    }
                }

            }
            string rspec = string.Empty;
            rspec = i.spec;
            string Num_Qty = "1";
            if (!rspec.Contains("BOX") && rspec.Contains("X"))
            {
                string qty = rspec.Substring(0, rspec.IndexOf("X")).Replace(" ", "");
                rspec = rspec.Replace(qty + "X ", "");
                int value;
                if (int.TryParse(qty, out value))
                    Num_Qty = Convert.ToString(qty);
            }
            if (i.toleranceType != ToleranceType)
            {
                ToleranceType = i.toleranceType;
            }

            if ((i.maximum != Max && i.maximum != oldMax) || (i.minimum != Min && i.minimum != oldMin))
            {
                Max = i.maximum;
                Min = i.minimum;
                PlusTolerance = isplmin_pltol;
                MinusTolerance = isplmin_mintol;
                string minv = string.Empty;
                string maxv = string.Empty;
                string plustolerancev = string.Empty;
                string minustolerancev = string.Empty;
                /*
                nominalv = getNomianal(OCR_Text, Nominal);
                if (nominalv != "")
                {
                    try
                    {
                        minustolerancev = Convert.ToString(Math.Abs(Convert.ToDecimal(Min) + Convert.ToDecimal(nominalv)));
                        plustolerancev = Convert.ToString(Math.Abs(Convert.ToDecimal(Max) - Convert.ToDecimal(nominalv)));
                        PlusTolerance = plustolerancev;
                        MinusTolerance = minustolerancev;
                    }
                    catch (Exception ex)
                    {
                        PlusTolerance = isplmin_pltol;
                        MinusTolerance = isplmin_mintol;
                    }
                }
                else
                {
                    PlusTolerance = isplmin_pltol;
                    MinusTolerance = isplmin_mintol;
                }
                */
            }
            /*
            else {
                nominalv = getNomianal(OCR_Text, Nominal);
                if (isplmin_pltol != PlusTolerance)
                {
                    string maxv = string.Empty;
                    string plustolerancev = string.Empty;
                    var plen = 0; var max1slen = 0;
                    if (nominalv != "")
                    {
                        try
                        {
                            maxv = Convert.ToString(Math.Abs(Convert.ToDecimal(nominalv) - Convert.ToDecimal(isplmin_pltol)));
                            plustolerancev = Convert.ToString(isplmin_pltol);
                            string[] doubleArrayPlusTolerance = plustolerancev.Split('.');
                            Decimal max1 = Convert.ToDecimal(maxv);
                            if (doubleArrayPlusTolerance.Length > 1)
                            {
                                plen = doubleArrayPlusTolerance[1].Length;
                            }
                            if (plen < 2) { plen = 2; }
                            string[] max1s = maxv.Split('.');
                            if (max1s.Length > 1)
                            {
                                max1slen = max1s[1].Length;
                            }
                            int miiValue = Math.Max(plen, max1slen);
                            maxv = max1.ToString($"F{miiValue}");
                            Max = maxv;

                        }
                        catch (Exception ex)
                        {
                            Max = i.maximum;
                        }
                    }
                    else
                    {
                        Max = i.maximum;
                    }
                    PlusTolerance = isplmin_pltol;

                }
                if (isplmin_mintol != MinusTolerance)
                {
                    string minv = string.Empty;
                    string minustolerancev = string.Empty;
                    var mlen = 0; var min1slen = 0;
                    if (nominalv != "")
                    {
                        try
                        {
                            minv = Convert.ToString(Math.Abs(Convert.ToDecimal(nominalv) - Convert.ToDecimal(isplmin_mintol)));
                            minustolerancev = Convert.ToString(isplmin_mintol);
                            string[] doubleArrayMinusTolerance = minustolerancev.Split('.');
                            Decimal min1 = Convert.ToDecimal(minv);
                            if (doubleArrayMinusTolerance.Length > 1)
                            {
                                mlen = doubleArrayMinusTolerance[1].Length;
                            }
                            if (mlen < 2) { mlen = 2; }
                            string[] min1s = minv.Split('.');
                            if (min1s.Length > 1)
                            {
                                min1slen = min1s[1].Length;
                            }
                            int miValue = Math.Max(mlen, min1slen);
                            minv = min1.ToString($"F{miValue}");
                            Min = minv;
                        }
                        catch (Exception ex)
                        {
                            Min = i.minimum;
                        }
                    }
                    else
                    {
                        Min = i.minimum;
                    }
                    MinusTolerance = isplmin_mintol;
                }
            }
            */
            string rmax = string.Empty;
            string rmin = string.Empty;
            rmax = i.maximum;
            rmin = i.minimum;
            if ((oldSpec == rspec) && (rmax == oldMax) && (rmin == oldMin) && (isplmin_pltol == old_pltol) && (isplmin_mintol == old_mintol))
            {
                Max = i.maximum;
                Min = i.minimum;
                PlusTolerance = isplmin_pltol;
                MinusTolerance = isplmin_mintol;
            }

            var dataList = new List<KeyValuePair<string, string>>();
            dataList.Add(new KeyValuePair<String, String>("Min", Min));
            dataList.Add(new KeyValuePair<String, String>("Max", Max));
            dataList.Add(new KeyValuePair<String, String>("Nominal", Nominal));
            dataList.Add(new KeyValuePair<String, String>("Type", Type));
            dataList.Add(new KeyValuePair<String, String>("SubType", SubType));
            dataList.Add(new KeyValuePair<String, String>("Unit", Unit));
            dataList.Add(new KeyValuePair<String, String>("ToleranceType", ToleranceType));
            if (PlusTolerance != "")
            {
                dataList.Add(new KeyValuePair<String, String>("PlusTolerance", "+" + PlusTolerance));
            }
            else
            {
                dataList.Add(new KeyValuePair<String, String>("PlusTolerance", "0"));
            }
            if (MinusTolerance != "")
            {
                dataList.Add(new KeyValuePair<String, String>("MinusTolerance", "-" + MinusTolerance));
            }
            else
            {
                dataList.Add(new KeyValuePair<String, String>("MinusTolerance", "0"));
            }
            dataList.Add(new KeyValuePair<String, String>("Num_Qty", Num_Qty));
            dataList.Add(new KeyValuePair<String, String>("Date", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffffffzzz")));
            return StatusCode(StatusCodes.Status200OK, dataList);
        }

        
        [HttpPost("specAutoPopulate")]
        public IActionResult specAutoPopulate(Specification i)
        {
            string Min, Max, Nominal, Type, SubType, Unit, ToleranceType, PlusTolerance, MinusTolerance;
            BubbleDrawing.CommonMethods cmt = new BubbleDrawing.CommonMethods();
            var OCR_Text = i.spec.Trim();
            if (OCR_Text.Contains("X"))
            {
                OCR_Text = OCR_Text.Substring(OCR_Text.IndexOf("X"), OCR_Text.Length - OCR_Text.IndexOf("X")).Replace("X", "").Trim(); ;
            }
            cmt.GetMinMaxValues(OCR_Text, out Min, out Max, out Nominal, out Type, out SubType, out Unit, out ToleranceType, out PlusTolerance, out MinusTolerance);
            string rspec = string.Empty;
            rspec = i.spec;
            string Num_Qty = "1";
            if (!rspec.Contains("BOX") && rspec.Contains("X"))
            {
                string qty = rspec.Substring(0, rspec.IndexOf("X")).Replace(" ", "");
                rspec = rspec.Replace(qty + "X ", "");
                int value;
                if (int.TryParse(qty, out value))
                    Num_Qty = Convert.ToString(qty);
            }
            string isplmin_pltol = string.Empty;
            string isplmin_mintol = string.Empty;
            List<OCRResults> originalRegions = i.originalRegions;

            string plusTolerance = i.plusTolerance;
            string minusTolerance = i.minusTolerance;
            isplmin_pltol = plusTolerance.Replace("+", "");
            isplmin_mintol = minusTolerance.Replace("-", "");
            var oldocr = originalRegions.Select((s) => new { Spec = s.Spec, Min = s.Minimum, Max = s.Maximum, MinusTolerance = s.MinusTolerance, PlusTolerance = s.PlusTolerance, ToleranceType = s.ToleranceType }).FirstOrDefault();
            string oldplusTolerance = oldocr.PlusTolerance;
            string oldminusTolerance = oldocr.MinusTolerance;
            string oldToleranceType = oldocr.ToleranceType;
            string oldMin = oldocr.Min;
            string oldMax = oldocr.Max;
            string oldSpec = oldocr.Spec;
            string old_pltol = oldplusTolerance.Replace("+", "").Trim();
            string old_mintol = oldminusTolerance.Replace("-", "").Trim();
            string nominalv = string.Empty;
            if (i.toleranceType != ToleranceType)
            {
                ToleranceType = i.toleranceType;
            }
            if (isplmin_pltol != PlusTolerance && ((isplmin_pltol != "0" || isplmin_pltol != "0.0") && isplmin_pltol != "") && (old_pltol != "" && old_pltol != "0"))
            {
                string maxv = string.Empty;
                string plustolerancev = string.Empty;
                nominalv = getNomianal(OCR_Text, Nominal);
                if (nominalv != "")
                {
                    try
                    {
                        maxv = Convert.ToString(Convert.ToDecimal(nominalv) + Convert.ToDecimal(isplmin_pltol));
                        plustolerancev = Convert.ToString(isplmin_pltol);
                        string[] doubleArrayPlusTolerance = plustolerancev.Split('.');
                        var plen = 0; var max1slen = 0;
                        if (Convert.ToDecimal(isplmin_pltol) > 0)
                        {
                            Decimal max1 = Convert.ToDecimal(maxv);
                            if (doubleArrayPlusTolerance.Length > 1)
                            {
                                plen = doubleArrayPlusTolerance[1].Length;
                            }
                            if (plen < 2) { plen = 2; }
                            string[] max1s = maxv.Split('.');
                            if (max1s.Length > 1)
                            {
                                max1slen = max1s[1].Length;
                            }
                            int miiValue = Math.Max(plen, max1slen);
                            maxv = max1.ToString($"F{miiValue}");
                        }
                        else { maxv = ""; }
                        if (isplmin_pltol == "0" || isplmin_pltol == "0.0")
                        {
                            maxv = nominalv;
                        }
                            Max = maxv;
                        Nominal = nominalv;
                        PlusTolerance = plustolerancev;

                    }
                    catch (Exception ex)
                    {
                        Max = i.maximum;
                        Nominal = OCR_Text;
                        PlusTolerance = isplmin_pltol;
                    }
                }
                else
                {
                    Max = i.maximum;
                    Nominal = OCR_Text;
                    PlusTolerance = isplmin_pltol;
                }
            }

            if (isplmin_mintol != MinusTolerance && (isplmin_mintol != "" && ( isplmin_mintol != "0" || isplmin_mintol != "0.0" )) && (old_mintol != "" && old_mintol != "0"))
            {
                string minv = string.Empty;
                string minustolerancev = string.Empty;
                nominalv = getNomianal(OCR_Text, Nominal);
                if (nominalv != "")
                {
                    try {
                        minv = Convert.ToString(Convert.ToDecimal(nominalv) - Convert.ToDecimal(isplmin_mintol));
                        minustolerancev = Convert.ToString(isplmin_mintol);
                        string[] doubleArrayMinusTolerance = minustolerancev.Split('.');
                        var mlen = 0; var min1slen = 0;
                        if (Convert.ToDecimal(isplmin_mintol) > 0)
                        {
                            Decimal min1 = Convert.ToDecimal(minv);
                            if (doubleArrayMinusTolerance.Length > 1)
                            {
                                mlen = doubleArrayMinusTolerance[1].Length;
                            }
                            if (mlen < 2) { mlen = 2; }
                            string[] min1s = minv.Split('.');
                            if (min1s.Length > 1)
                            {
                                min1slen = min1s[1].Length;
                            }
                            int miValue = Math.Max(mlen, min1slen);
                            minv = min1.ToString($"F{miValue}");
                        }
                        else { minv = ""; }
                        if(isplmin_mintol == "0" || isplmin_mintol == "0.0")
                        {
                            minv = nominalv;
                        }
                        Min = minv;
                        Nominal = nominalv;
                        MinusTolerance = minustolerancev;
                    } catch (Exception ex) {
                        Min = i.minimum;
                        Nominal = OCR_Text;
                        MinusTolerance = isplmin_mintol;
                    }
                }
                else
                {
                    Min = i.minimum;
                    Nominal = OCR_Text;
                    MinusTolerance = isplmin_mintol;
                }
            }

            var dataList = new List<KeyValuePair<string, string>>();
            dataList.Add(new KeyValuePair<String, String>("Min", Min));
            dataList.Add(new KeyValuePair<String, String>("Max", Max));
            dataList.Add(new KeyValuePair<String, String>("Nominal", Nominal));
            dataList.Add(new KeyValuePair<String, String>("Type", Type));
            dataList.Add(new KeyValuePair<String, String>("SubType", SubType));
            dataList.Add(new KeyValuePair<String, String>("Unit", Unit));
            dataList.Add(new KeyValuePair<String, String>("ToleranceType", ToleranceType));
            if (PlusTolerance != "")
            {
                dataList.Add(new KeyValuePair<String, String>("PlusTolerance", "+" + PlusTolerance));
            }
            else
            {
                dataList.Add(new KeyValuePair<String, String>("PlusTolerance", "0"));
            }
            if (MinusTolerance != "")
            {
                dataList.Add(new KeyValuePair<String, String>("MinusTolerance", "-" + MinusTolerance));
            }
            else
            {
                dataList.Add(new KeyValuePair<String, String>("MinusTolerance", "0"));
            }
            dataList.Add(new KeyValuePair<String, String>("Quantity", Num_Qty));
            dataList.Add(new KeyValuePair<String, String>("Date", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffffffzzz")));
            return StatusCode(StatusCodes.Status200OK, dataList);
        }
        #endregion

        #region Drawing Image rotate
        [HttpPost("rotate")]
        public IActionResult RotateImage(RotateBalloon searchForm)
        {
            string dtFiles = searchForm.drawingDetails;
            int rotation = searchForm.rotation;
            string env = this._appSettings.ENVIRONMENT;
            string clientPath = envcpath + "\\" + "ClientApp\\src\\drawing" + "\\";
            if (env != "development")
            {
                clientPath = clientPath + searchForm.sessionUserId + "\\";
                if (!Directory.Exists(clientPath))
                {
                    Directory.CreateDirectory(clientPath);
                }
            }
            FileInfo fi = new FileInfo(dtFiles);
            string desFile = fi.Name;
            string OrgPath = dtFiles;
            if (env != "development")
            {
                OrgPath = dtFiles.Replace(desFile, "") + "\\drawing\\" + desFile;
            }
            string clientImg = clientPath + desFile;
            string ImageFile = System.IO.Path.Combine(System.IO.Path.GetTempPath(), Guid.NewGuid().ToString() + desFile);
            System.IO.File.Copy(OrgPath, ImageFile, true);
            if (dtFiles == null || dtFiles == "")
            {
                return BadRequest("No file found.");
            }
            RotateImagefile(ImageFile, rotation);
            scaleImage(ImageFile, 0, true);
            System.Drawing.Bitmap.FromFile(ImageFile).Save(clientImg, System.Drawing.Imaging.ImageFormat.Png);
            List<object> returnObject = new List<object>();
            returnObject.Add(desFile);
            return StatusCode(StatusCodes.Status200OK, returnObject);
        }
        #endregion

        #region unspecified 
        [HttpPost]
        [Route("saveBalloons")]
        public async Task<ActionResult<AutoBalloon>> saveBalloons(AutoBalloon searchForm)
        {
            if (_dbcontext.TblConfigurations == null)
            {
                return NotFound();
            }
            else
            {
                List<OCRResults> lstoCRResults = new List<OCRResults>();
                lstoCRResults = searchForm.originalRegions;
                IEnumerable<object> returnObject = new List<object>();
                BubbleDrawingAutomationWeb.Controllers.BalloonController balcon = new BubbleDrawingAutomationWeb.Controllers.BalloonController(_dbcontext);
                BubbleDrawingAutomationWeb.Controllers.CreateBalloon objbaldet = new BubbleDrawingAutomationWeb.Controllers.CreateBalloon();
                objbaldet.drawingNo = searchForm.CdrawingNo;
                objbaldet.revNo = searchForm.CrevNo;
                objbaldet.totalPage = searchForm.totalPage;
                objbaldet.pageNo = searchForm.pageNo;
                objbaldet.ballonDetails = lstoCRResults;
                returnObject = balcon.update(objbaldet);
                return StatusCode(StatusCodes.Status200OK, returnObject);
            }
        }

        [HttpPost]
        [Route("resetBalloons")]
        public async Task<ActionResult<ResetBalloon>> resetBalloons(ResetBalloon searchForm)
        {
            if (_dbcontext.TblConfigurations == null)
            {
                return NotFound();
            }
            else
            {
                IEnumerable<object> returnObject = new List<object>();
                var query = _dbcontext.TblBaloonDrawingLiners.Where(w => w.DrawingNumber == searchForm.CdrawingNo.ToString() && w.Page_No == searchForm.pageNo && w.Revision == searchForm.CrevNo.ToString()).Count();
                if (query > 0)
                {
                    List<TblBaloonDrawingLiner> rList = _dbcontext.TblBaloonDrawingLiners.Where(w => w.DrawingNumber == searchForm.CdrawingNo.ToString() && w.Page_No == searchForm.pageNo && w.Revision == searchForm.CrevNo.ToString()).ToList();
                    _dbcontext.TblBaloonDrawingLiners.RemoveRange(rList);
                    _dbcontext.SaveChanges();
                    var oquery = _dbcontext.TblBaloonDrawingLiners
                   .Where(p => p.DrawingNumber == searchForm.CdrawingNo.ToString() && p.Revision == searchForm.CrevNo.ToString()).ToList();
                    var test = oquery.OrderBy(f => f.DrawLineID).Select(e => new { sl = e.Balloon.Contains(".") ? Convert.ToInt64(e.Balloon.Substring(0, e.Balloon.IndexOf("."))) : Convert.ToInt64(e.Balloon), DrawLineID = e.DrawLineID }).DistinctBy(i => i.sl).ToList();
                    var oquery1 = _dbcontext.TblBaloonDrawingLiners.Where(w => w.DrawingNumber == searchForm.CdrawingNo.ToString() && w.Page_No != searchForm.pageNo && w.Revision == searchForm.CrevNo.ToString()).OrderBy(f => f.Page_No).ToList();
                    if (oquery1.Count() > 0)
                    {
                        Int64 j = 1;
                        foreach (var i in test.OrderBy(f => f.DrawLineID).ToList())
                        {
                            var ck = _dbcontext.TblBaloonDrawingLiners
                                .Where(p => p.DrawingNumber == searchForm.CdrawingNo.ToString())
                                .Where(p => p.Revision == searchForm.CrevNo.ToString())
                                .Where(p => p.DrawLineID == i.DrawLineID)
                                .Where(p => p.Balloon.Contains(i.sl + "."))
                                .OrderBy(f => f.DrawLineID)
                                .ToList();
                            if (ck.Count() > 0)
                            {
                                var inner = _dbcontext.TblBaloonDrawingLiners
                                .Where(p => p.DrawingNumber == searchForm.CdrawingNo.ToString())
                                .Where(p => p.Revision == searchForm.CrevNo.ToString())
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
                }
                else
                {
                    var oquery = _dbcontext.TblBaloonDrawingLiners
                  .Where(p => p.DrawingNumber == searchForm.CdrawingNo.ToString() && p.Revision == searchForm.CrevNo.ToString()).ToList();
                    var test = oquery.OrderBy(f => f.DrawLineID).Select(e => new { sl = e.Balloon.Contains(".") ? Convert.ToInt64(e.Balloon.Substring(0, e.Balloon.IndexOf("."))) : Convert.ToInt64(e.Balloon), DrawLineID = e.DrawLineID }).DistinctBy(i => i.sl).ToList();
                    var oquery1 = _dbcontext.TblBaloonDrawingLiners.Where(w => w.DrawingNumber == searchForm.CdrawingNo.ToString() && w.Page_No != searchForm.pageNo && w.Revision == searchForm.CrevNo.ToString()).OrderBy(f => f.Page_No).ToList();
                    if (oquery1.Count() > 0)
                    {
                        Int64 j = 1;
                        foreach (var i in test.OrderBy(f => f.DrawLineID).ToList())
                        {
                            var ck = _dbcontext.TblBaloonDrawingLiners
                                .Where(p => p.DrawingNumber == searchForm.CdrawingNo.ToString())
                                .Where(p => p.Revision == searchForm.CrevNo.ToString())
                                .Where(p => p.DrawLineID == i.DrawLineID)
                                .Where(p => p.Balloon.Contains(i.sl + "."))
                                .OrderBy(f => f.DrawLineID)
                                .ToList();
                            if (ck.Count() > 0)
                            {
                                var inner = _dbcontext.TblBaloonDrawingLiners
                                .Where(p => p.DrawingNumber == searchForm.CdrawingNo.ToString())
                                .Where(p => p.Revision == searchForm.CrevNo.ToString())
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
                }
                BubbleDrawingAutomationWeb.Controllers.BalloonController balcon = new BubbleDrawingAutomationWeb.Controllers.BalloonController(_dbcontext);
                returnObject = balcon.get(searchForm.CdrawingNo, searchForm.CrevNo);
                return StatusCode(StatusCodes.Status200OK, returnObject);
            }
        }

        [HttpPost]
        [Route("deleteBalloons")]
        public async Task<ActionResult<DeleteBalloon>> deleteBalloons(DeleteBalloon searchForm)
        {
            if (_dbcontext.TblConfigurations == null)
            {
                return NotFound();
            }
            else
            {
                IEnumerable<object> returnObject = new List<object>();
                BubbleDrawingAutomationWeb.Controllers.BalloonController balcon = new BubbleDrawingAutomationWeb.Controllers.BalloonController(_dbcontext);
                BubbleDrawingAutomationWeb.Controllers.DeleteBalloon objbaldet = new BubbleDrawingAutomationWeb.Controllers.DeleteBalloon();
                objbaldet.drawingNo = searchForm.drawingNo;
                objbaldet.revNo = searchForm.revNo;
                objbaldet.totalPage = searchForm.totalPage;
                objbaldet.pageNo = searchForm.pageNo;
                objbaldet.deleteItem = searchForm.deleteItem;
                returnObject = balcon.delete(objbaldet);
                return StatusCode(StatusCodes.Status200OK, returnObject);
            }
        }


        [HttpPost("reOrderBalloons")]
        public async Task<ActionResult<DeleteBalloon>> reOrderBalloons(ResetBalloon searchForm)
        {
            if (_dbcontext.TblConfigurations == null)
            {
                return NotFound();
            }
            else
            {
                IEnumerable<object> returnObject = new List<object>();
                BubbleDrawingAutomationWeb.Controllers.BalloonController balcon = new BubbleDrawingAutomationWeb.Controllers.BalloonController(_dbcontext);
                BubbleDrawingAutomationWeb.Controllers.ResetBalloon objReCreate = new BubbleDrawingAutomationWeb.Controllers.ResetBalloon();
                objReCreate = searchForm;
                returnObject = balcon.reOrder(objReCreate);
                return StatusCode(StatusCodes.Status200OK, returnObject);
            }

        }

        #endregion

        #endregion

    }
}
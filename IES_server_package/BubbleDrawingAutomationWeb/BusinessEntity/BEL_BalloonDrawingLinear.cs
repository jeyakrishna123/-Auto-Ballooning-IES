using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleDrawing.BusinessEntity
{
   public class BEL_BalloonDrawingLinear
    {
        private long _drawLineID;

        public long DrawLineID
        {
            get { return _drawLineID; }
            set { _drawLineID = value; }
        }

        private long _baloonDrwID;

        public long BaloonDrwID
        {
            get { return _baloonDrwID; }
            set { _baloonDrwID = value; }
        }
        private string _baloonDrwFileID;

        public string BaloonDrwFileID
        {
            get { return _baloonDrwFileID; }
            set { _baloonDrwFileID = value; }
        }

        private int _page_No;

        public int Page_No
        {
            get { return _page_No; }
            set { _page_No = value; }
        }

        private string _drawingNumber;

        public string DrawingNumber
        {
            get { return _drawingNumber; }
            set { _drawingNumber = value; }
        }

        private string _revision;

        public string Revision
        {
            get { return _revision; }
            set { _revision = value; }
        }

        private string _balloon;

        public string Balloon
        {
            get { return _balloon; }
            set { _balloon = value; }
        }

        private string _spec;

        public string Spec
        {
            get { return _spec; }
            set { _spec = value; }
        }

        private string _nominal;

        public string Nominal
        {
            get { return _nominal; }
            set { _nominal = value; }
        }

        private string _minimum;

        public string Minimum
        {
            get { return _minimum; }
            set { _minimum = value; }
        }
        private string _maximum;

        public string Maximum
        {
            get { return _maximum; }
            set { _maximum = value; }
        }

        private string _measuredBy;

        public string MeasuredBy
        {
            get { return _measuredBy; }
            set { _measuredBy = value; }
        }

        private DateTime _measuredOn;

        public DateTime MeasuredOn
        {
            get { return _measuredOn; }
            set { _measuredOn = value; }
        }

        private int _circle_X_Axis;

        public int Circle_X_Axis
        {
            get { return _circle_X_Axis; }
            set { _circle_X_Axis = value; }
        }

        private int _circle_Y_Axis;

        public int Circle_Y_Axis
        {
            get { return _circle_Y_Axis; }
            set { _circle_Y_Axis = value; }
        }

        private int _circle_Width;

        public int Circle_Width
        {
            get { return _circle_Width; }
            set { _circle_Width = value; }
        }

        private int _circle_Height;

        public int Circle_Height
        {
            get { return _circle_Height; }
            set { _circle_Height = value; }
        }

        private int _balloon_Thickness;

        public int Balloon_Thickness
        {
            get { return _balloon_Thickness; }
            set { _balloon_Thickness = value; }
        }

        private int _balloon_Text_FontSize;

        public int Balloon_Text_FontSize
        {
            get { return _balloon_Text_FontSize; }
            set { _balloon_Text_FontSize = value; }
        }


        private decimal _zoomFactor;

        public decimal ZoomFactor
        {
            get { return _zoomFactor; }
            set { _zoomFactor = value; }
        }

        private int _crop_X_Axis;

        public int Crop_X_Axis
        {
            get { return _crop_X_Axis; }
            set { _crop_X_Axis = value; }
        }

        private int _crop_Y_Axis;

        public int Crop_Y_Axis
        {
            get { return _crop_Y_Axis; }
            set { _crop_Y_Axis = value; }
        }

        private int _crop_Width;

        public int Crop_Width
        {
            get { return _crop_Width; }
            set { _crop_Width = value; }
        }

        private int _crop_Height;

        public int Crop_Height
        {
            get { return _crop_Height; }
            set { _crop_Height = value; }
        }

        private string _createdBy;

        public string CreatedBy
        {
            get { return _createdBy; }
            set { _createdBy = value; }
        }

        private string _createdDate;

        public string CreatedDate
        {
            get { return _createdDate; }
            set { _createdDate = value; }
        }

        private string _modifiedBy;

        public string ModifiedBy
        {
            get { return _modifiedBy; }
            set { _modifiedBy = value; }
        }

        private DateTime _modifiedDate;

        public DateTime ModifiedDate
        {
            get { return _modifiedDate; }
            set { _modifiedDate = value; }
        }

    }
}

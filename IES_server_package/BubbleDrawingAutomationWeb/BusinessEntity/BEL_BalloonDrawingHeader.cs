using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleDrawing.BusinessEntity
{
  public  class BEL_BalloonDrawingHeader
    {
       
        private long  _baloonDrwFileID;

        public long BaloonDrwFileID
        {
            get { return _baloonDrwFileID; }
            set { _baloonDrwFileID = value; }
        }

        private string _drawingNumber;
  
        public string DrawingNumber
        {
            get { return _drawingNumber; }
            set { _drawingNumber = value; }
        }

        private int _total_Page_No;

        public int Total_Page_No
        {
            get { return _total_Page_No; }
            set { _total_Page_No = value; }
        }
        private string _revision;
        public string Revision
        {
            get { return _revision; }
            set { _revision = value; }
        }

        private string _Part_Revision;

        public string Part_Revision
        {
            get { return _Part_Revision; }
            set { _Part_Revision = value; }
        }

        private string _PartNo;

        public string PartNo
        {
            get { return _PartNo; }
            set { _PartNo = value; }
        }

        private string _ProductionOrderNo;

        public string ProductionOrderNo
        {
            get { return _ProductionOrderNo; }
            set { _ProductionOrderNo = value; }
        }

        private string _createdBy;

        public string CreatedBy
        {
            get { return _createdBy; }
            set { _createdBy = value; }
        }

        private DateTime _createdDate;

        public DateTime CreatedDate
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

        private int _PageNo;

        public int PageNo
        {
            get { return _PageNo; }
            set { _PageNo = value; }
        }
 
        private string _Rotate_properties;
        [DataType(DataType.Text)]
        public string Rotate_properties
        {
            get { return _Rotate_properties; }
            set { _Rotate_properties = value; }
        }
    }
}

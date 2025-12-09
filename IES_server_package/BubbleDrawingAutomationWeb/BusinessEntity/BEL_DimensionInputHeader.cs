using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleDrawing.BusinessEntity
{
  public  class BEL_DimensionInputHeader
    {
        private long _baloonDrwID;

        public long BaloonDrwID
        {
            get { return _baloonDrwID; }
            set { _baloonDrwID = value; }
        }

        private int _total_Page_No;

        public int Total_Page_No
        {
            get { return _total_Page_No; }
            set { _total_Page_No = value; }
        }

        private string _drawingNumber;

        public string DrawingNumber 
        {
            get { return _drawingNumber; }
            set { _drawingNumber = value; }
        }

        private string _productionOrderNumber;

        public string ProductionOrderNumber
        {
            get { return _productionOrderNumber; }
            set { _productionOrderNumber = value; }
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

        private string _OperationNo;

        public string OperationNo
        {
            get { return _OperationNo; }
            set { _OperationNo = value; }
        }

        private string _ConfirmationNo;

        public string ConfirmationNo
        {
            get { return _ConfirmationNo; }
            set { _ConfirmationNo = value; }
        }

        private string _part;

        public string Part
        {
            get { return _part; }
            set { _part = value; }
        }

        private string _batch;

        public string Batch
        {
            get { return _batch; }
            set { _batch = value; }
        }

        private int _quantity;

        public int Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
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

       

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleDrawing.BusinessEntity
{
    public class Dimensions
    {
        private string _ProductionOrderNumber;

        public string ProductionOrderNumber
        {
            get { return _ProductionOrderNumber; }
            set { _ProductionOrderNumber = value; }
        }

        private string _DrawingNumber;

        public string DrawingNumber
        {
            get { return _DrawingNumber; }
            set { _DrawingNumber = value; }
        }

        private string _Comments;

        public string Comments
        {
            get { return _Comments; }
            set { _Comments = value; }
        }

        private string _RemarksonlyforQcInput;

        public string RemarksonlyforQcInput
        {
            get { return _RemarksonlyforQcInput; }
            set { _RemarksonlyforQcInput = value; }
        }
        
        private string _baloon;

        public string Baloon
        {
            get { return _baloon; }
            set { _baloon = value; }
        }

        private string _Specification;

        public string Specification
        {
            get { return _Specification; }
            set { _Specification = value; }
        }
        
        private string _serial;

        public string Serial
        {
            get { return _serial; }
            set { _serial = value; }
        }

        private string _nominal;

        public string Nominal
        {
            get { return _nominal; }
            set { _nominal = value; }
        }

        private string  _minimum;

        public string  Minimum
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


        private string _actual;

        public string Actual
        {
            get { return _actual; }
            set { _actual = value; }
        }

        private string _decision;

        public  string Decision
        {
            get { return _decision; }
            set { _decision = value; }
        }

        private string _decisionby;

        public string DecisionBy
        {
            get { return _decisionby; }
            set { _decisionby = value; }
        }

        private string _guageid;

        public string GuageID
        {
            get { return _guageid; }
            set { _guageid = value; }
        }

        private decimal min_max_1;

        public decimal Min_Max_1
        {
            get { return min_max_1; }
            set { min_max_1 = value; }
        }

        private decimal min_max_2;

        public decimal Min_Max_2
        {
            get { return min_max_2; }
            set { min_max_2 = value; }
        }

        private decimal min_max_3;

        public decimal Min_Max_3
        {
            get { return min_max_3; }
            set { min_max_3 = value; }
        }

        private decimal min_max_4;

        public decimal Min_Max_4
        {
            get { return min_max_4; }
            set { min_max_4 = value; }
        }
        private decimal min_max_angles;

        public decimal Min_Max_Angles
        {
            get { return min_max_angles; }
            set { min_max_angles = value; }
        }

        private bool _DimensionChecked;

        public bool DimensionChecked
        {
            get { return _DimensionChecked; }
            set { _DimensionChecked = value; }
        }

        private string _Operation;

        public string Operation
        {
            get { return _Operation; }
            set { _Operation = value; }
        }
        
        private bool _OK;

        public bool OK
        {
            get { return _OK; }
            set { _OK = value; }
        }

        private int _ApproveStatus;

        public int ApproveStatus
        {
            get { return _ApproveStatus; }
            set { _ApproveStatus = value; }
        }


    }
}

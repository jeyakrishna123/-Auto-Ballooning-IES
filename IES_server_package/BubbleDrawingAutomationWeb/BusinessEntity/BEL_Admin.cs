using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleDrawing.BusinessEntity
{
  public  class BEL_Admin
    {
        private string _userID;
        public string UserID
        {
            get { return _userID; }
            set { _userID = value; }
        }
        private string _userName;
   
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        private string _workCenter;

        public string WorkCenter
        {
            get { return _workCenter; }
            set { _workCenter = value; }
        }

        private bool _planner;

        public bool Planner
        {
            get { return _planner; }
            set { _planner = value; }
        }

        private bool _operator;

        public bool Operator
        {
            get { return _operator; }
            set { _operator = value; }
        }

        private bool _quality;

        public bool Quality
        {
            get { return _quality; }
            set { _quality = value; }
        }

        private bool _admin;

        public bool Admin
        {
            get { return _admin; }
            set { _admin = value; }
        }

       

    }
}

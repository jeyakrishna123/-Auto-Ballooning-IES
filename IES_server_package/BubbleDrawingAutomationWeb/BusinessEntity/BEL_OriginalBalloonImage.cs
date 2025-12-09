using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleDrawing.BusinessEntity
{
   public class BEL_OriginalBalloonImage
    {

        private long _file_Header_ID;

        public long File_Header_ID
        {
            get { return _file_Header_ID; }
            set { _file_Header_ID = value; }
        }


        private long _balloon_Header_ID;

        public long Balloon_Header_ID
        {
            get { return _balloon_Header_ID; }
            set { _balloon_Header_ID = value; }
        }

        private string _baloonDrwFileID;

        public string BaloonDrwFileID
        {
            get { return _baloonDrwFileID; }
            set { _baloonDrwFileID = value; }
        }

        private int _total_Page_No;

        public int Total_Page_No
        {
            get { return _total_Page_No; }
            set { _total_Page_No = value; }
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

        private byte[] _baloonFile;

        public byte[] BaloonFile
        {
            get { return _baloonFile; }
            set { _baloonFile = value; }
        }

        private string _baloonFilePath;

        public string BaloonFilePath
        {
            get { return _baloonFilePath; }
            set { _baloonFilePath = value; }
        }

        private string _baloonFileName;

        public string BaloonFileName
        {
            get { return _baloonFileName; }
            set { _baloonFileName = value; }
        }

        private string _baloonFileType;

        public string BaloonFileType
        {
            get { return _baloonFileType; }
            set { _baloonFileType = value; }
        }

        private byte[] _Original_File;

        public byte[] Original_File
        {
            get { return _Original_File; }
            set { _Original_File = value; }
        }
        private string _original_Path;

        public string Original_Path
        {
            get { return _original_Path; }
            set { _original_Path = value; }
        }

        private int _image_Width;

        public int Image_Width
        {
            get { return _image_Width; }
            set { _image_Width = value; }
        }

        private int _image_Height;

        public int Image_Height
        {
            get { return _image_Height; }
            set { _image_Height = value; }
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

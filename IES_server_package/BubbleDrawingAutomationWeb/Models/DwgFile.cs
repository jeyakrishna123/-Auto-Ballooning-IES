using System;
using System.Collections.Generic;

namespace BubbleDrawingAutomationWeb.Models;

public partial class DwgFile
{
    public int Oid { get; set; }

    public int SrvrShrOid { get; set; }

    public int DrawingOid { get; set; }

    public string DwgFileNm { get; set; } = null!;

    public bool? DltFlg { get; set; }

    public DateTime Crtms { get; set; }

    public DateTime Updtms { get; set; }

    public string? LstUpdBy { get; set; }

    public virtual Drawing DrawingO { get; set; } = null!;

    public virtual SrvrShr SrvrShrO { get; set; } = null!;
}

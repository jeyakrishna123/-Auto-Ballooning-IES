using System;
using System.Collections.Generic;

namespace BubbleDrawingAutomationWeb.Models;

public partial class SrvrShr
{
    public int Oid { get; set; }

    public string ShrNm { get; set; } = null!;

    public bool? ActvFlg { get; set; } = false;

    public DateTime Crtms { get; set; }

    public DateTime Updtms { get; set; }

    public string? LstUpdBy { get; set; }

    public virtual ICollection<DwgFile> DwgFiles { get; set; } = new List<DwgFile>();
}

using System;
using System.Collections.Generic;

namespace BubbleDrawingAutomationWeb.Models;

public partial class XSrvrType
{
    public int Oid { get; set; }

    public string SrvrTypeCd { get; set; } = null!;

    public string TypeNm { get; set; } = null!;

    public bool ActvFlg { get; set; }

    public DateTime Crtms { get; set; }

    public DateTime Updtms { get; set; }

    public string? LstUpdBy { get; set; }

    public virtual ICollection<AppSrvrDbSrvr> AppSrvrDbSrvrs { get; set; } = new List<AppSrvrDbSrvr>();
}

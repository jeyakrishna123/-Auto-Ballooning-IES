using System;
using System.Collections.Generic;

namespace BubbleDrawingAutomationWeb.Models;

public partial class AppSrvrDbSrvr
{
    public int Oid { get; set; }

    public string SrvrTypeCd { get; set; } = null!;

    public string AppSrvrNm { get; set; } = null!;

    public string DbSrvr { get; set; } = null!;

    public string DbNm { get; set; } = null!;

    public int SrvrTypeOid { get; set; }

    public bool ActvFlg { get; set; }

    public DateTime Crtms { get; set; }

    public DateTime Updtms { get; set; }

    public string? LstUpdBy { get; set; }

    public virtual XSrvrType SrvrTypeO { get; set; } = null!;
}

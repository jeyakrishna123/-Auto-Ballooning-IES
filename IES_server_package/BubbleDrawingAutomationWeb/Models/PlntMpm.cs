using System;
using System.Collections.Generic;

namespace BubbleDrawingAutomationWeb.Models;

public partial class PlntMpm
{
    public int Oid { get; set; }

    public int PlntOid { get; set; }

    public int MpmSrvrOid { get; set; }

    public bool? ActvFlg { get; set; } = false;

    public DateTime Crtms { get; set; }

    public DateTime Updtms { get; set; }

    public string? LstUpdtBy { get; set; }

    public virtual MpmSrvr MpmSrvrO { get; set; } = null!;

    public virtual Plnt PlntO { get; set; } = null!;
}

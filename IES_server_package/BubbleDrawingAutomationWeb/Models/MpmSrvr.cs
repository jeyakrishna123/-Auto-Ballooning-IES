using System;
using System.Collections.Generic;

namespace BubbleDrawingAutomationWeb.Models;

public partial class MpmSrvr
{
    public int Oid { get; set; }

    public string MpmSrvrNm { get; set; } = null!;

    public string MpmDbNm { get; set; } = null!;

    public bool? ActvFlg { get; set; } = false;

    public DateTime Crtms { get; set; }

    public DateTime Uptms { get; set; }

    public string? LstUpdtBy { get; set; }

    public virtual ICollection<PlntMpm> PlntMpms { get; set; } = new List<PlntMpm>();
}

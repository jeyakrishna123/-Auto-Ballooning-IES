using System;
using System.Collections.Generic;

namespace BubbleDrawingAutomationWeb.Models;

public partial class Empl
{
    public int Oid { get; set; }

    public string EmplNbr { get; set; } = null!;

    public string UsrId { get; set; } = null!;

    public int PlntOid { get; set; }

    public string FrstNm { get; set; } = null!;

    public string LstNm { get; set; } = null!;

    public bool? ActvFlg { get; set; } = false;

    public DateTime Crtms { get; set; }

    public DateTime Updtms { get; set; }

    public string? LstUpdBy { get; set; }

    public virtual ICollection<AppSecurity> AppSecurities { get; set; } = new List<AppSecurity>();

    public virtual Plnt PlntO { get; set; } = null!;
}

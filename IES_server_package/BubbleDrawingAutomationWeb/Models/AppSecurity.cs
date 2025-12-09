using System;
using System.Collections.Generic;

namespace BubbleDrawingAutomationWeb.Models;

public partial class AppSecurity
{
    public int Oid { get; set; }

    public int EmplOid { get; set; }

    public bool ChkOutDwgs { get; set; }

    public bool BalloonPrnts { get; set; }

    public bool RecordDims { get; set; }

    public bool ChangeDims { get; set; }

    public DateTime Crtms { get; set; }

    public DateTime Updtms { get; set; }

    public string? LstUpdBy { get; set; }

    public virtual Empl EmplO { get; set; } = null!;
}

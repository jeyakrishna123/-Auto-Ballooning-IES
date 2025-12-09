using System;
using System.Collections.Generic;

namespace BubbleDrawingAutomationWeb.Models;

public partial class Gage
{
    public int Oid { get; set; }

    public int DimsActualOid { get; set; }

    public string Gage1 { get; set; } = null!;

    public string? GageSn { get; set; }

    public DateTime Crtms { get; set; }

    public DateTime Updtms { get; set; }

    public string? LstUpdBy { get; set; }

    public virtual DimsActual DimsActualO { get; set; } = null!;
}

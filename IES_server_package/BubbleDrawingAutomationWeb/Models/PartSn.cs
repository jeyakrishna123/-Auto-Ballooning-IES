using System;
using System.Collections.Generic;

namespace BubbleDrawingAutomationWeb.Models;

public partial class PartSn
{
    public int Oid { get; set; }

    public int DwgDimsOid { get; set; }

    public int PartSn1 { get; set; }

    public DateTime Crtms { get; set; }

    public DateTime Updtms { get; set; }

    public string? LstUpdBy { get; set; }

    public virtual ICollection<DimsActual> DimsActuals { get; set; } = new List<DimsActual>();

    public virtual DwgDim DwgDimsO { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace BubbleDrawingAutomationWeb.Models;

public partial class DimsActualAudit
{
    public int Oid { get; set; }

    public int DimsActualOid { get; set; }

    public string ChangedBy { get; set; } = null!;

    public DateTime? ChangedDt { get; set; }

    public int DrawingOid { get; set; }

    public int BalloonNbr { get; set; }

    public int PartSerialNbrOid { get; set; }

    public decimal DimActual { get; set; }

    public int EmplOid { get; set; }

    public int UomOid { get; set; }

    public bool? DimPass { get; set; }

    public DateTime Crtms { get; set; }

    public DateTime Updtms { get; set; }

    public string? LstUpdtBy { get; set; }

    public virtual DimsActual DimsActualO { get; set; } = null!;
}

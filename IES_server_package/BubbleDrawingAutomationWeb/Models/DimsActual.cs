using System;
using System.Collections.Generic;

namespace BubbleDrawingAutomationWeb.Models;

public partial class DimsActual
{
    public int Oid { get; set; }

    public int BalloonNbr { get; set; }

    public int PartSnOid { get; set; }

    public decimal DimActual { get; set; }

    public int EmplOid { get; set; }

    public int UomOid { get; set; }

    public bool? DimPass { get; set; }

    public DateTime Crtms { get; set; }

    public DateTime Updtms { get; set; }

    public string? LstUpdtBy { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<DimsActualAudit> DimsActualAudits { get; set; } = new List<DimsActualAudit>();

    public virtual ICollection<Gage> Gages { get; set; } = new List<Gage>();

    public virtual PartSn PartSnO { get; set; } = null!;
}

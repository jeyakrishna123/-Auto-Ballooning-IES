using System;
using System.Collections.Generic;

namespace BubbleDrawingAutomationWeb.Models;

public partial class PrchOrdr
{
    public int Oid { get; set; }

    public string PrchOrdrNbr { get; set; } = null!;

    public int PlntOid { get; set; }

    public DateTime Crtms { get; set; }

    public DateTime Updtms { get; set; }

    public string? LstUpdBy { get; set; }

    public virtual Plnt PlntO { get; set; } = null!;

    public virtual ICollection<PrchOrdrItm> PrchOrdrItms { get; set; } = new List<PrchOrdrItm>();
}

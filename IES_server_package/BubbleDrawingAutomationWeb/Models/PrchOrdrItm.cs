using System;
using System.Collections.Generic;

namespace BubbleDrawingAutomationWeb.Models;

public partial class PrchOrdrItm
{
    public int Oid { get; set; }

    public int PrchOrdrOid { get; set; }

    public string PrchOrdrItmNbr { get; set; } = null!;

    public decimal Qty { get; set; }

    public int MtrlOid { get; set; }

    public DateTime Crtms { get; set; }

    public DateTime Updtms { get; set; }

    public string? LstUpdBy { get; set; }

    public virtual Mtrl MtrlO { get; set; } = null!;

    public virtual PrchOrdr PrchOrdrO { get; set; } = null!;
}

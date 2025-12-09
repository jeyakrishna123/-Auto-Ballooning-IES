using System;
using System.Collections.Generic;

namespace BubbleDrawingAutomationWeb.Models;

public partial class ProdOrdr
{
    public int Oid { get; set; }

    public int? PlntOid { get; set; }

    public string ProdOrdrNbr { get; set; } = null!;

    public decimal Qty { get; set; }

    public int? MtrlOid { get; set; }

    public string? BtchNbr { get; set; }

    public DateTime? Crtms { get; set; }

    public DateTime? Updtms { get; set; }

    public string? LstUpdBy { get; set; }

    public virtual Mtrl? MtrlO { get; set; }

    public virtual Plnt? PlntO { get; set; }
}

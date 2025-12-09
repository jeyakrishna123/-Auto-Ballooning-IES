using System;
using System.Collections.Generic;

namespace BubbleDrawingAutomationWeb.Models;

public partial class Mtrl
{
    public int Oid { get; set; }

    public string MtrlNbr { get; set; } = null!;

    public DateTime Crtms { get; set; }

    public DateTime Updtms { get; set; }

    public string? LstUpdBy { get; set; }

    public virtual ICollection<Drawing> Drawings { get; set; } = new List<Drawing>();

    public virtual ICollection<PrchOrdrItm> PrchOrdrItms { get; set; } = new List<PrchOrdrItm>();

    public virtual ICollection<ProdOrdr> ProdOrdrs { get; set; } = new List<ProdOrdr>();
}

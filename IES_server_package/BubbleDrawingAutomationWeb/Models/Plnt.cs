using System;
using System.Collections.Generic;

namespace BubbleDrawingAutomationWeb.Models;

public partial class Plnt
{
    public int Oid { get; set; }

    public string PlntCd { get; set; } = null!;

    public string? PlntDsc { get; set; }

    public bool ActvFlg { get; set; }

    public DateTime Crtms { get; set; }

    public DateTime Updtms { get; set; }

    public string? LstUpdBy { get; set; }

    public virtual ICollection<Empl> Empls { get; set; } = new List<Empl>();

    public virtual ICollection<PlntMpm> PlntMpms { get; set; } = new List<PlntMpm>();

    public virtual ICollection<PrchOrdr> PrchOrdrs { get; set; } = new List<PrchOrdr>();

    public virtual ICollection<ProdOrdr> ProdOrdrs { get; set; } = new List<ProdOrdr>();
}

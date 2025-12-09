using System;
using System.Collections.Generic;

namespace BubbleDrawingAutomationWeb.Models;

public partial class TblUom
{
    public int Oid { get; set; }

    public string UomCd { get; set; } = null!;

    public string? Dsc { get; set; }

    public bool? ActvFlg { get; set; } = false;

    public DateTime Crtms { get; set; }

    public DateTime Updtms { get; set; }

    public string? LstUpdBy { get; set; }

    public virtual ICollection<DwgDim> DwgDims { get; set; } = new List<DwgDim>();
}

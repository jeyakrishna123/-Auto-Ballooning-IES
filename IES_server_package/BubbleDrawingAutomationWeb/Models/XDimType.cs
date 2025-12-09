using System;
using System.Collections.Generic;

namespace BubbleDrawingAutomationWeb.Models;

public partial class XDimType
{
    public int Oid { get; set; }

    public string DimType { get; set; } = null!;

    public bool? ActvFlg { get; set; } = false;

    public DateTime Crtms { get; set; }

    public DateTime Updtms { get; set; }

    public string? LstUpdtBy { get; set; }

    public virtual ICollection<DwgDim> DwgDims { get; set; } = new List<DwgDim>();
}

using System;
using System.Collections.Generic;

namespace BubbleDrawingAutomationWeb.Models;

public partial class Drawing
{
    public int Oid { get; set; }

    public int MtrlOid { get; set; }

    public string DwgNbr { get; set; } = null!;

    public string? DwgRev { get; set; }

    public DateTime Crtms { get; set; }

    public DateTime Updtms { get; set; }

    public string? LstUpdBy { get; set; }

    public virtual ICollection<DwgDim> DwgDims { get; set; } = new List<DwgDim>();

    public virtual ICollection<DwgFile> DwgFiles { get; set; } = new List<DwgFile>();

    public virtual Mtrl MtrlO { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace BubbleDrawingAutomationWeb.Models;

public partial class DwgDim
{
    public int Oid { get; set; }

    public int DrawingOid { get; set; }

    public int BalloonNbr { get; set; }

    public int? DimTypeOid { get; set; }

    public decimal? DimNominal { get; set; }

    public decimal? DimMax { get; set; }

    public decimal? DimMin { get; set; }

    public int UomOid { get; set; }

    public virtual XDimType? DimTypeO { get; set; }

    public virtual Drawing DrawingO { get; set; } = null!;

    public virtual ICollection<PartSn> PartSns { get; set; } = new List<PartSn>();

    public virtual TblUom UomO { get; set; } = null!;
}

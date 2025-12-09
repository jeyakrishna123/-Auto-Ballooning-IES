using System;
using System.Collections.Generic;

namespace BubbleDrawingAutomationWeb.Models;

public partial class TblMeasureType
{
    public int TypeId { get; set; }

    public string? TypeName { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
}

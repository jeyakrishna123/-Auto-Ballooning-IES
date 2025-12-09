using System;
using System.Collections.Generic;

namespace BubbleDrawingAutomationWeb.Models;

public partial class TblDimensionInputHistory
{
    public long Oid { get; set; }

    public string? BaloonDrwFileId { get; set; }

    public int? PageNo { get; set; }

    public string? DrawingNumber { get; set; }

    public string? Revision { get; set; }

    public string? ProductionOrderNumber { get; set; }

    public string? SerialNo { get; set; }

    public string? Balloon { get; set; }

    public string? Actual { get; set; }

    public string? Decision { get; set; }

    public string? DecisionBy { get; set; }

    public string? GaugeId { get; set; }

    public string? Operation { get; set; }

    public string? Comments { get; set; }

    public string? WorkCenter { get; set; }

    public string? MeasuredBy { get; set; }

    public DateTime? MeasuredOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
}

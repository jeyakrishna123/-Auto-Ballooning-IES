using System;
using System.Collections.Generic;

namespace BubbleDrawingAutomationWeb.Models;

public partial class TblDimensionInputHeader
{
    public long BaloonDrwId { get; set; }

    public int? TotalPageNo { get; set; }

    public string? ConfirmationNo { get; set; }

    public string? DrawingNumber { get; set; }

    public string? ProductionOrderNumber { get; set; }

    public string? Revision { get; set; }

    public string? PartRevision { get; set; }

    public string? Part { get; set; }

    public string? OperationNo { get; set; }

    public string? Batch { get; set; }

    public int? Quantity { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public string? Status { get; set; }

    public string? ApprovedBy { get; set; }

    public DateTime? ApprovedDate { get; set; }
}

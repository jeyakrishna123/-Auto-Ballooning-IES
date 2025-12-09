using System;
using System.Collections.Generic;

namespace BubbleDrawingAutomationWeb.Models;

public partial class TblAuditHistory
{
    public long AuditLogId { get; set; }

    public string? ProductionOrderNumber { get; set; }

    public string? DrawingNumber { get; set; }

    public string? SerialNo { get; set; }

    public string? Balloon { get; set; }

    public string? CurrentValue { get; set; }

    public string? ChangedValue { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }
}

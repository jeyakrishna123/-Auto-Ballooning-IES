using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BubbleDrawingAutomationWeb.Models;

public partial class TblBaloonDrawingHeader
{
    [Key]
    public long BaloonDrwID { get; set; }

    public string? ProductionOrderNumber { get; set; }

    public string? DrawingNumber { get; set; }

    public int? Total_Page_No { get; set; }

    public string? Revision { get; set; }

    public string? Part_Revision { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public string? RotateProperties { get; set; }
}

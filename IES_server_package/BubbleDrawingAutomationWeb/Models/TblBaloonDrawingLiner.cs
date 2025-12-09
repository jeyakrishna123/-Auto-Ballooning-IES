using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BubbleDrawingAutomationWeb.Models;

public partial class TblBaloonDrawingLiner
{
    [Key]
    public long DrawLineID { get; set; }

    public long? BaloonDrwID { get; set; }

    public string? BaloonDrwFileID { get; set; }

    public string? ProductionOrderNumber { get; set; }

    public string? Part_Revision { get; set; }

    public int? Page_No { get; set; }

    public string? DrawingNumber { get; set; }

    public string? Revision { get; set; }

    public string? Balloon { get; set; }

    public string? Spec { get; set; }

    public string? Nominal { get; set; }

    public string? Minimum { get; set; }

    public string? Maximum { get; set; }

    public string? MeasuredBy { get; set; }

    public DateTime? MeasuredOn { get; set; }

    public int? Circle_X_Axis { get; set; }

    public int? Circle_Y_Axis { get; set; }

    public int? Circle_Width { get; set; }

    public int? Circle_Height { get; set; }

    public int? Balloon_Thickness { get; set; }

    public int? Balloon_Text_FontSize { get; set; }

    public decimal? ZoomFactor { get; set; }

    public int? Crop_X_Axis { get; set; }

    public int? Crop_Y_Axis { get; set; }

    public int? Crop_Width { get; set; }

    public int? Crop_Height { get; set; }

    public string? Type { get; set; }

    public string? SubType { get; set; }

    public string? Unit { get; set; }

    public int? Quantity { get; set; }

    public string? ToleranceType { get; set; }

    public string? PlusTolerance { get; set; }

    public string? MinusTolerance { get; set; }

    public string? MaxTolerance { get; set; }

    public string? MinTolerance { get; set; }

    public byte[]? CropImage { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool? IsCritical { get; set; }
}

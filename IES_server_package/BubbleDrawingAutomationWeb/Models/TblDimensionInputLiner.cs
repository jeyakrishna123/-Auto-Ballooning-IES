using System;
using System.Collections.Generic;

namespace BubbleDrawingAutomationWeb.Models;

public partial class TblDimensionInputLiner
{
    public long DrawLineId { get; set; }

    public long? BaloonDrwId { get; set; }

    public string? BaloonDrwFileId { get; set; }

    public int? PageNo { get; set; }

    public string? DrawingNumber { get; set; }

    public string? Revision { get; set; }

    public string? ProductionOrderNumber { get; set; }

    public string? SerialNo { get; set; }

    public string? Balloon { get; set; }

    public string? Spec { get; set; }

    public string? Nominal { get; set; }

    public string? Minimum { get; set; }

    public string? Maximum { get; set; }

    public string? Actual { get; set; }

    public string? Decision { get; set; }

    public string? DecisionBy { get; set; }

    public string? GaugeId { get; set; }

    public string? Operation { get; set; }

    public string? Comments { get; set; }

    public string? WorkCenter { get; set; }

    public string? RemarksonlyforQcInput { get; set; }

    public string? MeasuredBy { get; set; }

    public DateTime? MeasuredOn { get; set; }

    public int? CircleXAxis { get; set; }

    public int? CircleYAxis { get; set; }

    public int? CircleWidth { get; set; }

    public int? CircleHeight { get; set; }

    public int? BalloonThickness { get; set; }

    public int? BalloonTextFontSize { get; set; }

    public decimal? ZoomFactor { get; set; }

    public int? CropXAxis { get; set; }

    public int? CropYAxis { get; set; }

    public int? CropWidth { get; set; }

    public int? CropHeight { get; set; }

    public bool? DimensionChecked { get; set; }

    public int? InspectionSet { get; set; }

    public string? CompletePercentage { get; set; }

    public string? Status { get; set; }

    public int? ApproveStatus { get; set; }

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

    public string? FolderPdfreport { get; set; }

    public string? FolderDwngreport { get; set; }

    public string? FolderExcelreport { get; set; }

    public bool IsVerified { get; set; }
}

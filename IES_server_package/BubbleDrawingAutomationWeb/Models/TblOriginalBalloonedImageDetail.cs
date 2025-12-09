using System;
using System.Collections.Generic;

namespace BubbleDrawingAutomationWeb.Models;

public partial class TblOriginalBalloonedImageDetail
{
    public long FileHeaderId { get; set; }

    public long? BalloonHeaderId { get; set; }

    public string BaloonDrwFileId { get; set; } = null!;

    public int? TotalPageNo { get; set; }

    public int? PageNo { get; set; }

    public string? DrawingNumber { get; set; }

    public string? Revision { get; set; }

    public byte[]? BaloonFile { get; set; }

    public string? BaloonFilePath { get; set; }

    public string? BaloonFileName { get; set; }

    public string? BaloonFileType { get; set; }

    public byte[]? OriginalFile { get; set; }

    public string? OriginalPath { get; set; }

    public int? ImageWidth { get; set; }

    public int? ImageHeight { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
}

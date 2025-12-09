using System;
using System.Collections.Generic;

namespace BubbleDrawingAutomationWeb.Models;

public partial class TblComment
{
    public long Id { get; set; }

    public string? Comment { get; set; }

    public string? Description { get; set; }

    public bool? IsMandatory { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
}

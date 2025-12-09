using System;
using System.Collections.Generic;

namespace BubbleDrawingAutomationWeb.Models;

public partial class TblUserAccess
{
    public string UserId { get; set; } = null!;

    public string? UserName { get; set; }

    public string? WorkCenter { get; set; }

    public bool? Planner { get; set; }

    public bool? Operator { get; set; }

    public bool? Quality { get; set; }

    public bool? Admin { get; set; }
}

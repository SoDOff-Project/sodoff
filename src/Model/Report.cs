using System;
using System.ComponentModel.DataAnnotations;
using sodoff.Model;

namespace sodoff.Model;

public class Report
{
    [Key]
    public int Id { get; set; }

    public int VikingId { get; set; }

    public int ReportedVikingId { get; set; }

    public int ReportType { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Viking? Viking { get; set; }

    public virtual Viking? ReportedViking { get; set; }
}

﻿using System.ComponentModel.DataAnnotations;

namespace JobReady;

public class Education
{
    [Key]
    public long Id { get; set; }

    [Required]
    public long UserId { get; set; }
    public UserAccount User { get; set; }

    public long? SchoolId { get; set; }
    public University School { get; set; }

    [Required]
    public string SchoolName { get; set; }

    public DegreeType? Degree { get; set; }

    public string OtherDegree { get;set; }

    [Display(Name = "Field Of Study")]
    public long? MajorId { get; set; }
    public Major Major { get; set; }

    public long? Grade { get; set; }

    public string Description { get; set; }

    [Required]
    [Display(Name = "Start Date")]
    public DateTime? StartDate { get; set; }

    [Required]
    [Display(Name = "End Date (or Expected)")]
    public DateTime? EndDate { get; set; }
}
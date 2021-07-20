using System;
using System.ComponentModel.DataAnnotations;

namespace allinfo.Models
{
    public enum Division
    {
        Atlantic, Southeast, Central, Northwest, Southwest, Pacific
    }
    public class Team
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Display(Name = "Image")]
        public string AvatarURL { get; set; }
        [Required]
        [Display(Name = "Division")]
        public Division division { get; set; }
        [Required]
        [Display(Name = "Head coach")]
        public string HeadCoach { get; set;}
        [Required]
        public string Arena { get; set; }
        public double payroll {get; set; }
        [Required]
        [Display(Name = "Short name")]
        public string ShortName { get; set; }
        [Required]
        public string teamColor1 { get; set; }
        [Required]
        public string teamColor2 { get; set; }
        [Required]
        public int championships {get; set;}
    }
}
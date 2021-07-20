using System;
using System.ComponentModel.DataAnnotations;

namespace allinfo.Models
{
    public enum agegroup
    {
        Freshman, Sophomore, Junior, Senior, International
    }
    public class Prospect
    {
        public int ID { get; set; }
        
        [Required]
        public string Name { get; set;}
        [Required]
        public string playingPosition {get; set; }
        [Required]
        public int age { get; set; }
        [Required]
        public int height { get; set; }
        [Required]
        public string report { get; set;}
        [Required]
        public string college {get; set; }
        [Required]
        public agegroup group { get; set; }
        [Required]
        public int rank {get; set; }
        [Required]
        public int TeamId { get; set; }
        public string teamAvi { get{ return Team.AvatarURL;} }
        private Team _team;
        public Team Team { get { return _team ?? (_team = new Team());} set { _team = value;}}
        [Required]
        public int shirtNumber { get; set; }
        [Display(Name = "Image")]
        public string aviUrl {get; set; }
        [Required]
        public string stat1 { get; set; }
        [Required]
        public string stat2 { get; set; }
        [Required]
        public string stat3 { get; set; }
        [Required]
        public string stat4 { get; set; }
        [Required]
        public string stat5 { get; set; }
        [Required]
        public string stat6 { get; set; }
        [Required]
        public string stat7 { get; set; }
        [Required]
        public string stat8 { get; set; }
    }
}
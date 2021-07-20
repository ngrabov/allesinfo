using System;
using System.ComponentModel.DataAnnotations;

namespace allinfo.Models
{
    public enum Option
    {
        Player, Team, Team2, No
    }
    public enum Position
    {
        C, PF, SF, SG, PG
    }
    public enum FreeAgency
    {
        UFA, RFA
    }
    public class Player
    {
        public int ID { get; set; }
        
        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; }
        
        [Required]
        [Display(Name = "First Name")]
        [StringLength(50, MinimumLength = 2)]
        public string FirstName { get; set; }

        [Display(Name = "Full Name")]
        public string FullName {
            get 
            { 
                return FirstName + " " + LastName;
            }
            set{;}
        }

        [Required]
        [Display(Name = "Date of birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy.}")] 
        public DateTime DOB { get; set; }

        [Required]
        [Display(Name = "Team")]
        public int TeamId { get; set;}

        [Required]
        [Display(Name = "Playing position")]
        public Position playingPosition { get; set; }

        [Required]
        [Display(Name = "Free agent type")]
        public FreeAgency faType{ get; set; }

        [Display(Name = "Points per game")]
        public double ppg { get; set; }

        public string nationality { get{ return Nationality.nationalityName;} }
        
        [Required]
        [Display(Name = "Nation")]
        public int NationalityID { get; set;}

        [Required]
        [Display(Name = "Height")]
        public int height { get; set; }

        [Required]
        [Display(Name = "2K rating")]
        public int NBA2KRating { get; set; }

        [Required]
        [Display(Name = "Shirt number")]
        public int shirtNumber { get; set; }

        [Required]
        [Display(Name = "2020-21 salary")]
        public double salary1 { get; set; }
        
        [Required]
        [Display(Name = "2021-22 salary")]
        public double salary2 { get; set; }

        [Required]
        [Display(Name = "2022-23 salary")]
        public double salary3 { get; set; }

        [Required]
        [Display(Name = "2023-24 salary")]
        public double salary4 { get; set; }

        [Required]
        [Display(Name = "2024-25 salary")]
        public double salary5 { get; set; }

        [Required]
        [Display(Name = "Contract option")]
        public Option contractOption { get; set; }
        public int contractLength {get; set;}

        [Display(Name = "Team")]
        public string TeamName { get { return Team.Name;}}

        public string contractDetails { get; set; }

        private Team _team;
        private Nationality _nationality;
        public Nationality Nationality { get { return _nationality ?? (_nationality = new Nationality());} set { _nationality = value;}}
        public Team Team { get { return _team ?? (_team = new Team());} set { _team = value;}}

        [Display(Name = "Image")]
        public string AvatarURL { get; set; }

        public string Accolades { get; set; }

        public int age { 
            get
            { 
                if(DateTime.Now.Month > DOB.Month)
                {
                    return DateTime.Now.Year - DOB.Year;
                }
                else
                {
                    if(DateTime.Now.Month < DOB.Month)
                    {
                        return DateTime.Now.Year - DOB.Year - 1;
                    }
                    else
                    {
                        if(DateTime.Now.Day < DOB.Day)
                        {
                            return DateTime.Now.Year - DOB.Year - 1;
                        }
                        else
                        {
                            return DateTime.Now.Year - DOB.Year;
                        }
                    }
                }   
            } 
        }
    }
}
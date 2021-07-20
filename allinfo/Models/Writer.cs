using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace allinfo.Models
{
    public class Writer : IdentityUser<int>
    {
        [Display(Name = "Last Name")]
        [Required]
        [PersonalData]
        [StringLength(20, MinimumLength = 2)]
        [RegularExpression(@"^[A-ZČĆŽĐŠ]+[a-zA-ZšđčćžŠĐŽĆČ\-\. ]*$", ErrorMessage = "Last name must start with an uppercase letter. Only letters and hyphen sign (-) are supported.")]
        public string LastName { get; set; }

        [Required]
        [PersonalData]
        [Display(Name = "First Name")]
        [StringLength(20, MinimumLength = 2)]
        [RegularExpression(@"[A-ZŠĐČĆŽ]+[A-Za-zšđčćžŠĐČĆŽ\-\. ]*$",ErrorMessage="First name must start with an uppercase letter. Only letters, dot and hyphen sign (-) are supported.")] 
        public string FirstName { get; set; }

        [Required]
        [PersonalData]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy.}")] 
        public DateTime DOB { get; set; }

        [Display(Name = "Full name")]
        public string FullName { get { return FirstName + " " + LastName; } }
        [Display(Name = "Twitter handle (not mandatory)")]
        public string TwitterHandle { get; set; }
        public bool isAdmin { get; set; }
        public bool isManager { get; set; }
        public bool isModerator { get; set; }

        private ICollection<Article> _articles;
        public ICollection<Article> Articles { get{ return _articles ?? (_articles = new List<Article>()); } set{ _articles = value;} }
        public int ArticleCount { get { return Articles.Count; }  }
    }
}
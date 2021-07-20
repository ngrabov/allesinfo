using System;
using System.ComponentModel.DataAnnotations;

namespace allinfo.Models
{
    public class Comment
    {
        public int CommentID {get; set;}
        public string UserID {get; set;}
        public string UserFullName {get{ return _userName;} set{_userName = value;}}
        public int ArticleID {get; set;}
        [Required]
        [StringLength(1000, MinimumLength = 3)]
        public string Text {get; set;}

        [DisplayFormat(DataFormatString = "{0:dd.MM.yy. HH:mm}")] 
        [Display(Name = "Written")]
        public DateTime TimeWritten { get { return _timewritten; } set { _timewritten = value; }}
        private DateTime _timewritten;
        private string _userName;
    }
}
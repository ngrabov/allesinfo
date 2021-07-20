using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Text.RegularExpressions;

namespace allinfo.Models
{
    public enum Field
    {
        Rumors, Transactions, Draft
    }
    public class Article
    {
        public int ArticleID { get; set; }
        [Display(Name = "Writer")]
        public int WriterId  { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3)]
        [RegularExpression(@"[A-Z0-9ŠĐŽĆČ\']+[A-Za-z0-9\-\.\:\;\,\(\)\’\'\+\?\!\%\&\/\=\<\>\#\*čćđžš ]*$", ErrorMessage = "Input error. Headline must start with an uppercase "
        + "letter, or a number. Some of the characters you entered might not be supported as headline characters.")]
        public string Headline { get; set; }
        [Required]
        [StringLength(30000, MinimumLength = 3)]
        public string Text { get; set; }
        [Required(ErrorMessage="Please select one.")]
        public Field Field  { get; set; } 
        [Display(Name = "Content")]
        public string SubText 
        { 
            get 
            { 
                if(HttpUtility.HtmlDecode(Regex.Replace(Text, "<[^>]*(>|$)", string.Empty)).Length <= 140)
                {
                    return HttpUtility.HtmlDecode(Regex.Replace(Text, "<[^>]*(>|$)", string.Empty));
                }
                else
                {
                    return HttpUtility.HtmlDecode(Regex.Replace(Text, "<[^>]*(>|$)", string.Empty)).Substring(0, 140) + "...";
                }
            }
        }
        [Display(Name = "Writer")]
        public string WriterName { get { return Writer.FullName ; } }
        [Display(Name = "Number of words")]
        public int WordCount { get {return Text.Split(" ").Length;}}
        [Display(Name = "Time to read")]
        public int TimeToRead { get {return (int)WordCount/200;}}
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy. HH:mm}")] 
        [Display(Name = "Written")]
        public DateTime TimeWritten { get { return _timewritten; } set { _timewritten = value; }}
        private DateTime _timewritten;

        [Required(ErrorMessage="Please select an image to upload.")]
        [Display(Name = "Image")]
        public string HeadImageURL { get; set; }
        [Required]
        [StringLength(150, MinimumLength = 10)]
        public string SubHeadline { get; set; }
        public string Tags { get; set; }
        public bool isModerated { get; set; }
        public bool isImportant { get; set; }

        private Writer _writer;
        public Writer Writer { get { return _writer ?? (_writer = new Writer()); }  set { _writer = value; }  }
    }
}
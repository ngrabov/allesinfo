using System;
using System.ComponentModel.DataAnnotations;

namespace allinfo.Models
{
    public class Nationality
    {
        public int ID { get; set; }
        public string nationalityName { get; set; }
        public string flagURL { get; set; }
    }
}
using System.Collections.Generic;
using allinfo.Models;

namespace allinfo.ViewModels
{
    #nullable enable
    public class TeamsViewModel
    {
        public int? TeamId { get; set;}
        public List<Player>? Players {get; set;}
        public List<Prospect>? Prospects {get; set;}
        public Position? position {get; set;}
        public int? minage { get; set;}
        public int? maxage { get; set;}
    }
}
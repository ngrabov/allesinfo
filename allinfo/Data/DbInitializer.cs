using allinfo.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace allinfo.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(NewsContext context, IServiceProvider services, string Pw) 
        {
            await context.Database.EnsureCreatedAsync();

            if(context.Writers.Any())
            {
                return;
            }

            var writers = new Writer[]
            {
                new Writer{FirstName="Chris",LastName="Haynes",DOB = new DateTime(1961, 1, 1), TwitterHandle="https://twitter.com/ChrisBHaynes", UserName = "chay@allinfo.com", Email = "chay@allinfo.com"},
                new Writer{FirstName="Bill",LastName="Oram",DOB = new DateTime(1966, 4, 5), TwitterHandle="https://twitter.com/billoram", UserName = "boramzz@allinfo.com", Email = "boramzz@allinfo.com"},
                new Writer{FirstName="Shams",LastName="Charania",DOB = new DateTime(1960, 7, 13), TwitterHandle="https://twitter.com/ShamsCharania", UserName = "shamsC@allinfo.com", Email = "shamsC@allinfo.com"},
                new Writer{FirstName="Adrian",LastName="Wojnarowski",DOB = new DateTime(1962, 5, 21), TwitterHandle="https://twitter.com/wojespn", UserName = "wojoespn@allinfo.com", Email = "wojoespn@allinfo.com"},
                new Writer{FirstName="Eric",LastName="Pincus",DOB = new DateTime(1946, 11, 25), TwitterHandle="https://twitter.com/EricPincus", UserName = "ericpi@allinfo.com", Email = "ericpi@allinfo.com"},
                new Writer{FirstName="Zach",LastName="Lowe",DOB = new DateTime(1956, 1, 28), TwitterHandle="https://twitter.com/ZachLowe_NBA", UserName = "zlowe@allinfo.com", Email = "zlowe@allinfo.com"},
                new Writer{FirstName="Marc",LastName="Stein",DOB = new DateTime(1963, 2, 16), TwitterHandle="https://twitter.com/TheSteinLine", UserName = "thestein@allinfo.com", Email = "thestein@allinfo.com"},
                new Writer{FirstName="Alex",LastName="Regla",DOB = new DateTime(1962, 10, 2), TwitterHandle="https://twitter.com/AlexmRegla", UserName = "alexreg@allinfo.com", Email = "alexreg@allinfo.com"},
                new Writer{FirstName="Mike",LastName="Schmitz",DOB = new DateTime(1966, 4, 9), TwitterHandle="https://twitter.com/Mike_Schmitz", UserName = "mschmizz@allinfo.com", Email = "mschmizz@allinfo.com"},
                new Writer{FirstName="Alex",LastName="Kennedy",DOB = new DateTime(1966, 6, 10), TwitterHandle="https://twitter.com/AlexKennedyNBA", UserName = "alexkay@allinfo.com", Email = "alexkay@allinfo.com"},
                new Writer{FirstName="David",LastName="Aldridge",DOB = new DateTime(1969, 3, 15), TwitterHandle="https://twitter.com/davidaldridgedc", UserName = "davidaldy@allinfo.com", Email = "davidaldy@allinfo.com"},
                new Writer{FirstName="Tom",LastName="Haberstroh",DOB = new DateTime(1968, 9, 14), TwitterHandle="https://twitter.com/tomhaberstroh", UserName = "tomhstroh@allinfo.com", Email = "tomhstroh@allinfo.com"},
                new Writer{FirstName="Jeremy",LastName="Woo",DOB = new DateTime(1946, 12, 18), TwitterHandle="https://twitter.com/JeremyWoo", UserName = "jaywoo@allinfo.com", Email = "jaywoo@allinfo.com"},
                new Writer{FirstName="Matt",LastName="Dollinger",DOB = new DateTime(1966, 4, 3), TwitterHandle="https://twitter.com/matt_dollinger", UserName = "mdollz@allinfo.com", Email = "mdollz@allinfo.com"},
                new Writer{FirstName="Paolo",LastName="Uggetti", DOB = new DateTime(1949, 8, 8), TwitterHandle="https://twitter.com/PaoloUggetti", UserName = "puggett@allinfo.com", Email = "puggett@allinfo.com"},
                new Writer{FirstName="Rachel",LastName="Nichols", DOB = new DateTime(1964, 3, 4), TwitterHandle="https://twitter.com/Rachel__Nichols", UserName = "rachyrach@allinfo.com", Email = "rachyrach@allinfo.com"},
                new Writer{FirstName="Stephen A.",LastName="Smith", DOB = new DateTime(1955, 4, 3), TwitterHandle="https://twitter.com/stephenasmith", UserName = "stevesmith@allinfo.com", Email = "stevesmith@allinfo.com"},
                new Writer{FirstName="Max",LastName="Kellerman", DOB = new DateTime(1957, 4, 9), TwitterHandle="https://twitter.com/maxkellerman", UserName = "maxkell@allinfo.com", Email = "maxkell@allinfo.com"}
            };

            var roleManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();
            await EnsureRoleAsync(roleManager, "Manager");

            foreach (Writer w in writers)
            {
                var userManager = services.GetRequiredService<UserManager<Writer>>();
                string pass = w.FirstName + "123456;";

                w.isAdmin = false;
                w.isManager = true;
                w.EmailConfirmed = true;
                w.PhoneNumberConfirmed = true;
                context.Writers.Add(w);

                await userManager.CreateAsync(w, Pw);
                await userManager.AddToRoleAsync(w, "Manager");
            }

            context.SaveChanges();

            var articles = new Article[]
            {
                new Article{WriterId=1,Field=Field.Rumors,Headline="Four potential destinations for Fred VanVleet in free agency",Text="Fred VanVleet has " +
                "accomplished everything the Toronto Raptors have asked from him and more. He closed Finals games for them against the Warriors, " +
                "including a Game 6 fourth quarter where he made three big threes to clinch the 2019 title. With Kawhi Leonard departing and no immediate " +
                "replacement, the 6-foot-1 guard took a big chunk of those minutes, was a huge part in leading the Raptors to a second-round run and " +
                "elevated his status to a quasi-All Star.\n\n The 26-year-old has accomplished all this while earning $19.5 million in his four " +
                "seasons in Toronto. Not bad for an undrafted rookie. After earning the minimum in his first two seasons, the Raptors " +
                "re-signed him to the maximum amount his early Bird rights allowed him to receive. Now the Raptors hold his full Bird rights and he is " +
                "eligible to earn up to the maximum salary.\n\n There is a very strong chance his average starting salary exceeds his $19.5 million career " +
                "earnings total. So which teams had a chance of landing his services?",TimeWritten= new DateTime(2019, 10, 20, 13, 33, 21),HeadImageURL="fvv.jpg"},
                new Article{WriterId=3,Field=Field.Rumors,Headline="Mario Hezonja decides on player option",Text="Portland Trail Blazers guard Mario Hezonja " +
                "will exercise his $1.98 million player option for the upcoming 2020-21 season, league sources told HoopsHype.\n\n The former fifth " +
                "overall pick in the 2015 draft spent three seasons with the Orlando Magic and one season with the New York Knicks before joining Portland last season.\n\n " +
                "Hezonja has career averages of 6.9 points and 3.1 rebounds in 18.5 minutes per game during his five seasons in the league. The Croatian " +
                "guard will enter unrestricted free agency in the summer of 2021. ",TimeWritten= new DateTime(2019, 10, 20, 15, 13, 6),HeadImageURL="hez.jpg"},
                new Article{WriterId=4,Field=Field.Draft,Headline="2020 NBA aggregate mock draft 9.0: Tracking trends for top prospects",Text="The 2020 NBA pre-draft " +
                "process was by far the longest in league history, which gave players plenty of time to both help or hurt themselves.\n\n This is the " +
                "ninth edition of our aggregate mock draft for this class and by reviewing where each player fell in previous rankings, " +
                "we can track who has raised their star the highest and who has fallen from grace.\n\n We looked at mock drafts from NBADraft.net, " +
                "ESPN, The Athletic, Bleacher Report, CBS Sports, The Ringer, NBABigBoard.com, SI.com, USA TODAY Sports Media Group’s Rookie Wire, USA Today " +
                "and Yahoo to give us a more clear understanding of consensus projections.\n\n Please note that the range included for each player " +
                "is not based on our own reporting or intel and it only reflects the data pulled from the various mock drafts. The full list of our latest " +
                "aggregate mock draft rankings can be found here.\n\n HoopsHype’s Alberto de Roa contributed research " +
                "to this report.",TimeWritten= new DateTime(2019, 10, 20, 14, 11, 9),HeadImageURL="draft.jpg"},
                new Article{WriterId=8,Field=Field.Draft,Headline="2017 NBA re-draft: The way it should have been",Text="It’s far too early to draw definitive takes " +
                "on the 2017 NBA draft class, but that doesn’t mean we can’t see trends about which players are rising to the top of the group.\n\n " +
                "The actual No. 1 pick that year, Markelle Fultz, has been, to this point, a disappointment. Obviously, that means his spot in a " +
                "potential re-draft hypothetical is going to someone else.\n\n The question is: who? \n\n Should that spot go to Donovan Mitchell, who in " +
                "just three seasons has blossomed into one of the top 2-guards in the game? Or what about Bam Adebayo, the Swiss Army knife of a big man " +
                "who can do just about everything on a basketball court besides shoot threes? Or how about Jayson Tatum, the high-scoring, " +
                "efficient wing with an array of bucket-getting tricks to choose from in his bag? Below, check out the 2017 NBA re-draft, " +
                "the way it should have been.",TimeWritten= new DateTime(2019, 10, 20, 13, 5, 0),HeadImageURL="2017.jpg"},
                new Article{WriterId=18,Field=Field.Transactions,Headline="D'Antoni resigns",Text="LAKE BUENA VISTA, Fla. (AP) — Mike D’Antoni has told the " +
                "Houston Rockets that he will not be back as coach with them next season, essentially choosing free agency over a return to the club " +
                "with whom he has spent the last four seasons.\n\n " + 
                "D’Antoni told the Rockets on Sunday — not even a full day after the team’s season ended with a playoff loss to the Los Angeles Lakers — " +
                "that he would seek coaching options elsewhere for next season and not return to Houston. Team owner Tilman Fertitta confirmed the move later Sunday.\n\n " +
                "“I would like to thank Mike D’Antoni and his wife, Laurel, for their incredible contributions to the Houston Rockets organization " +
                "and the Houston community,” Fertitta said. “Mike is a true professional and an amazing basketball mind. He is a winner, " +
                "and we have been blessed to have such an outstanding coach and leader to work with the past four seasons.”\n\n " + 
                "ESPN first reported the 69-year-old coach’s decision.",TimeWritten= new DateTime(2020, 10, 10, 18, 5, 0),HeadImageURL="dantoni.jpg"},
                new Article{WriterId=10,Field=Field.Transactions,Headline="Zizic back in Europe",Text="CLEVELAND, Ohio -- Ante Zizic is headed back overseas.\n\n " +
                "Zizic, one of the few remaining pieces of the Kyrie Irving blockbuster, has signed a two-year contract with Maccabi Tel Aviv, " +
                "the Israeli powerhouse that started recruiting Zizic months ago and eventually beat out Real Madrid.\n\n " +
                "“I am proud and happy to join the Maccabi family,” Zizic said, according to the team’s official site. “It is a family I know " +
                "since I was 16 years old, and to wear the yellow jersey that my big brother wore, I want to thank the owners, (team manager) " +
                "Nikola (Vujcic) and coach Giannis (Sfairopoulos) for believing in me. I can’t wait to meet the fans. See you soon in Tel Aviv.”\n\n " +
                "Selected by the Boston Celtics with the 23rd pick of the 2016 NBA Draft, Zizic joined Cleveland in 2017 as part of a four-piece haul " +
                "from the Celtics for All-Star Irving. Zizic, 23, spent three seasons with the Cavs before opting to return to Europe for a more prominent " +
                "role and a chance to compete for a championship.\n\n “Ante is a player with qualities that will give us a lot of power " +
                "in the paint and I am very happy that we signed him”, Sfairopoulos said of Zizic. “He will help Maccabi with his skills and professionalism " +
                "to succeed and meet our goals. I am very happy to coach him because I also coached his big brother Andrea at Olympiacos.\n\n " +
                "“I’m glad he had a past at Maccabi, that he was already in Tel Aviv, trained here a bit and felt what this great club is. " +
                "It is important that now Maccabi has managed to sign him and attach him to the family. We will fight together for Maccabi’s goals.”\n\n " +
                "In 113 NBA games, Zizic averaged 6.0 points and 4.9 rebounds. He spent the offseason leading into the 2019-20 campaign shedding weight " +
                "and getting into better shape, hoping to prove that he was still a fit in the modern NBA. But Zizic never got the chance. " +
                "Buried on the depth chart, he played just 22 games, averaging 4.4 points and 3.0 rebounds.\n\n " +
                "The Cavs declined his fourth-year option ahead of the 2019-20 season, making him an unrestricted free agent this summer.",
                TimeWritten= new DateTime(2020, 10, 13, 16, 15, 0),HeadImageURL="zizic.jpg"},
                new Article{WriterId=12,Field=Field.Transactions,Headline="Divac replaced by Dumars",Text="The Sacramento Kings announced today that Vlade Divac " +
                "has stepped down as General Manager. In the interim, Joe Dumars has been named Executive Vice President of Basketball Operations and will " +
                "immediately assume General Manager duties.\n\n “This was a difficult decision, but we believe it is the best path ahead as we work to build " +
                "a winning team that our loyal fans deserve,” said Kings Owner and Chairman Vivek Ranadivé. “We are thankful for Vlade’s leadership, " +
                "commitment and hard work both on and off the court. He will always be a part of our Kings family.”\n\n “It has been an honor and a privilege " +
                "to serve as the General Manager for the Kings,” said Vlade Divac. “I want to thank Vivek for the opportunity and recognize all of the " +
                "incredible colleagues who I had the great pleasure of working with during my tenure. Sacramento and the Kings will always hold a special " +
                "place in my heart and I wish them all the best moving forward.”\n\n The team will work with Dumars to develop a long-term strategy for " +
                "the organization’s basketball operations structure, to include a search for a permanent General Manager. No further personnel decisions " +
                "will be made until a new General Manager is in place.\n\n “Joe has become a trusted and valued advisor since joining the team last year, " +
                "and I am grateful to have him take on this role at an important time for the franchise,” said Ranadivé.\n\n In June 2019, Dumars " +
                "was named Special Advisor to the General Manager and has over 30 years of NBA experience both as a player and front office executive " +
                "in addition to an extensive executive business background.",TimeWritten= new DateTime(2020, 10, 12, 15, 43, 0),HeadImageURL="divac.jpg"}
            };
            
            var readers = new Reader[]
            {
                new Reader{FirstName="Milan Lane",LastName="Jovanovic",DOB= new DateTime(1981,10,22)},
                new Reader{FirstName="Branislav Bane",LastName="Ivanovic",DOB = new DateTime(1984, 1, 19)}
            };

            foreach (Reader r in readers)
            {
                context.Readers.Add(r);
            }

            context.SaveChanges();

            foreach (Article a in articles)
            {
                context.Articles.Add(a);
            }

            context.SaveChanges();

            var roleAdmin = services.GetRequiredService<RoleManager<IdentityRole<int>>>();
            await EnsureRoleAsync(roleAdmin, "Administrator");

            var userAdmin = services.GetRequiredService<UserManager<Writer>>();
            await EnsureAdminAsync(userAdmin, Pw);
        } 

        private static async Task EnsureRoleAsync(RoleManager<IdentityRole<int>> roleManager, string role)
        {
            var alreadyExists = await roleManager.RoleExistsAsync(role);
            if(alreadyExists) return;

            await roleManager.CreateAsync(new IdentityRole<int>(role));
        }

        private static async Task EnsureAdminAsync(UserManager<Writer> userManager, string userPw)
        {
            var testAdmin = await userManager.Users
                .Where(x => x.UserName == "admin@allinfo.hr")
                .SingleOrDefaultAsync();

            if(testAdmin!=null) return;

            testAdmin = new Writer
            {
                UserName = "admin@allinfo.hr",
                Email = "admin@allinfo.hr",  
                LastName = "Admin",
                FirstName = "Adminsky",
                isAdmin = true,
                DOB = new DateTime(1990, 1, 1) 
            };
            
            await userManager.CreateAsync(testAdmin, userPw);
            await userManager.AddToRoleAsync(testAdmin, "Administrator");
        }
    }
}
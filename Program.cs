using System.Collections.Generic;
using Lab1.Repository;
using Lab1.UI;

namespace Lab1
{
    internal class Program
    {
        private static List<string> _teamNames = new List<string>();
        private static List<string> _schoolNames = new List<string>();

        public static void Main(string[] args)
        {
            string filePathDirectory = "./";

            TeamFileRepository teamRepository =
                new TeamFileRepository(new TeamValidator(), filePathDirectory + "teams.txt");
            Service<int, Team> teamService = new Service<int, Team>(teamRepository);

            PlayerFileRepository playerRepository =
                new PlayerFileRepository(new PlayerValidator(), filePathDirectory + "players.txt", teamRepository);
            Service<int, Player> playerService = new Service<int, Player>(playerRepository);

            GameFileRepository gameRepository =
                new GameFileRepository(new GameValidator(), filePathDirectory + "games.txt", teamRepository);
            GameService gameService = new GameService(gameRepository);

            ActivePlayerFileRepository activePlayersRepository =
                new ActivePlayerFileRepository(new ActivePlayerValidator(), filePathDirectory + "activePlayers.txt");
            ActivePlayerService activePlayerService = new ActivePlayerService(activePlayersRepository);

            AddTeamNames();
            AddSchoolNames();

            //TODO add junk to repository
            AddJunk(teamRepository,playerRepository,gameRepository,activePlayersRepository);
            
            ConsoleUi consoleUi = new ConsoleUi(playerService, teamService, gameService, activePlayerService,
                _teamNames, _schoolNames);
            consoleUi.Run();
        }

        private static void AddJunk(TeamFileRepository teamRepository, PlayerFileRepository playerRepository, GameFileRepository gameRepository, ActivePlayerFileRepository activePlayersRepository)
        {
            
        }

        private static void AddTeamNames()
        {
            _teamNames.Add("Houston Rockets");
            _teamNames.Add("Los Angeles Lakers");
            _teamNames.Add("LA Clippers");
            _teamNames.Add("Chicago Bulls");
            _teamNames.Add("Cleveland Cavaliers");
            _teamNames.Add("Utah Jazz");
            _teamNames.Add("Brooklyn Nets");
            _teamNames.Add("New Orleans Pelicans");
            _teamNames.Add("Indiana Pacers");
            _teamNames.Add("Toronto Raptors");
            _teamNames.Add("Charlotte Hornets");
            _teamNames.Add("Phoenix Suns");
            _teamNames.Add("Portland TrailBlazers");
            _teamNames.Add("Golden State Warriors");
            _teamNames.Add("Washington Wizards");
            _teamNames.Add("San Antonio Spurs");
            _teamNames.Add("Orlando Magic");
            _teamNames.Add("Denver Nuggets");
            _teamNames.Add("Detroit Pistons");
            _teamNames.Add("Atlanta Hawks");
            _teamNames.Add("Dallas Mavericks");
            _teamNames.Add("Sacramento Kings");
            _teamNames.Add("Oklahoma City Thunder");
            _teamNames.Add("Boston Celtics");
            _teamNames.Add("New York Knicks");
            _teamNames.Add("Minnesota Timberwolves");
            _teamNames.Add("Miami Heat");
            _teamNames.Add("Milwaukee Bucks");
        }

        private static void AddSchoolNames()
        {
            _schoolNames.Add("Scoala Gimnaziala \"Horea\"");
            _schoolNames.Add("Scoala Gimnaziala \"Octavian Goga\"");
            _schoolNames.Add("Liceul Teoretic \"Lucian Blaga\"");
            _schoolNames.Add("Scoala Gimnaziala \"Ioan Bob\" ");
            _schoolNames.Add("Scoala Gimnaziala \"Ion Creanga\"");
            _schoolNames.Add("Colegiul National Pedagogic \"Gheorghe Lazar\" ");
            _schoolNames.Add("Scoala Gimnaziala Internationala SPECTRUM");
            _schoolNames.Add("Colegiul National \"Emil Racovita\"");
            _schoolNames.Add("Colegiul National \"George Cosbuc\"");
            _schoolNames.Add("Scoala Gimnaziala \"Ion Agarbiceanu\"");
            _schoolNames.Add("Liceul Teoretic \"Avram Iancu\"");
            _schoolNames.Add("Scoala Gimnaziala \"Constantin Brancusi\"");
            _schoolNames.Add("Liceul Teoretic \"Onisifor Ghibu\"");
            _schoolNames.Add("Liceul cu Program Sportiv Cluj-Napoca");
            _schoolNames.Add("Liceul Teoretic \"Nicolae Balcescu\"");
            _schoolNames.Add("Liceul Teoretic \"Gheorghe Sincai\"");
            _schoolNames.Add("Scoala \"Nicolae Titulescu\"");
            _schoolNames.Add("Scoala Gimnaziala \"Liviu Rebreanu\"");
            _schoolNames.Add("Scoala Gimnaziala \"Iuliu Hatieganu\"");
            _schoolNames.Add("Liceul Teoretic \"Bathory Istvan\"");
            _schoolNames.Add("Colegiul National \"George Baritiu\"");
            _schoolNames.Add("Liceul Teoretic \"Apaczai Csere Janos\"");
            _schoolNames.Add("Seminarul Teologic Ortodox");
            _schoolNames.Add("Liceul de Informatica \"Tiberiu Popoviciu\"");
            _schoolNames.Add("Scoala Gimnaziala \"Alexandru Vaida – Voevod\"");
            _schoolNames.Add("Liceul Teoretic ELF");
            _schoolNames.Add("Scoala Gimnaziala \"Gheorghe Sincai\" Floresti");
        }
    }
}
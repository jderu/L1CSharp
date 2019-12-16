using System;
using System.Collections.Generic;
using System.Linq;
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

            //adds data to repositories
            //AddJunk(teamRepository, playerRepository, gameRepository, activePlayersRepository);

            ConsoleUi consoleUi = new ConsoleUi(playerService, teamService, gameService, activePlayerService,
                _teamNames, _schoolNames);
            consoleUi.Run();
        }

        private static void AddJunk(TeamFileRepository teamRepository, PlayerFileRepository playerRepository,
            GameFileRepository gameRepository, ActivePlayerFileRepository activePlayersRepository)
        {
            Random rand = new Random(DateTime.Now.Millisecond);
            string[] firstNames =
            {
                "Jonathan", "Will A.", "Robert E. O.", "Dio", "George", "Joseph", "Caesar A.", "Lisa", "Rudol", "Suzi",
                "Kars", "Esidisi", "Wamuu", "Santana", "Jotaro", "Muhammad", "Noriaki", "Jean Pierre", "Iggy", "Holy",
                "Vanilla", "Hol", "Daniel J.", "Oingo", "Boingo", "Anubis", "Telence", "Josuke", "Okuyasu", "Koichi",
                "Rohan", "Hayato", "Yukako", "Tonio", "Tomoko", "Yoshikage", "Giorno", "Bruno", "Leone", "Guido",
                "Guido", "Narancia", "Trish", "Coco", "Pericolo", "Diavolo", "Cioccolata", "Polpo", "Pesci", ""
            };
            string[] lastNames =
            {
                "Joestar", "Zeppeli", "Speedwagon", "Brando", "Lisa", "von Stroheim", "Q", "Kujo", "Avdol", "Kakyoin",
                "Polnareff", "Ice", "Horse", "D'Arby", "Higashikata", "Nijimura", "Hirose", "Kishibe", "Kawajiri",
                "Yamagishi", "Trussardi", "Kira", "Giovanna", "Bucciarati", "Abbacchio", "Mista", "Ghirga", "Una",
                "Jumbo", "Doppio", ""
            };

            //adding random teams
            int id = 0;
            foreach (string teamName in _teamNames)
                teamRepository.Save(new Team(id++, teamName));

            //adding arandom players
            id = 0;
            var schoolTeams = Enumerable.Range(0, _teamNames.Count).OrderBy(x => rand.Next()).Take(_schoolNames.Count);
            foreach (var i in schoolTeams.Select((teamId, index) => (index, teamId)))
            {
                Team team = teamRepository.FindOne(i.teamId);
                for (int j = 0; j < rand.Next(15, 25); j++)
                    playerRepository.Save(new Player(id++, GenerateRandomName(rand, firstNames, lastNames),
                        _schoolNames[i.index], team));
            }

            //adding random games
            DateTime start = new DateTime(1995, 1, 1);
            DateTime end = DateTime.Today;
            int range = (end - start).Days;
            for (int i = 0; i < _schoolNames.Count - 1; i++)
            for (int j = i + 1; j < _schoolNames.Count; j++)
                if (rand.Next(3) == 0)
                {
                    Team team1 = teamRepository.FindOne(i);
                    Team team2 = teamRepository.FindOne(j);
                    gameRepository.Save(new Game(team1, team2, start.AddDays(rand.Next(range))));
                }

            //adding random active players
            var players = playerRepository.FindAll().ToList();
            foreach (Game game in gameRepository.FindAll())
            {
                int team1Id = game.Id.Team1Id;
                int team2Id = game.Id.Team2Id;

                AddActivePlayerToGame(team1Id, team2Id, team1Id, players, rand, activePlayersRepository);
                AddActivePlayerToGame(team1Id, team2Id, team2Id, players, rand, activePlayersRepository);
            }
        }

        private static void AddActivePlayerToGame(int team1Id, int team2Id, int teamId, List<Player> players,
            Random rand, ActivePlayerFileRepository activePlayersRepository)
        {
            var teamPlayers = players.Where(p => p.Team.Id.Equals(team1Id)).ToList();
            var teamPlaying = Enumerable.Range(0, teamPlayers.Count()).OrderBy(x => rand.Next()).ToArray();
            int i = 0;
            foreach (Player player in teamPlayers)
            {
                if (teamPlaying[i] < 15)
                {
                    int score = 0;
                    if (teamPlaying[i] < 11)
                    {
                        int scoreProbability = rand.Next(1, 101);
                        if (scoreProbability > 95)
                            score = 2;
                        else if (scoreProbability > 80)
                            score = 1;
                        activePlayersRepository.Save(new ActivePlayer(player.Id, team1Id, team2Id, score,
                            ActivityState.Playing));
                    }
                    else
                        activePlayersRepository.Save(new ActivePlayer(player.Id, team1Id, team2Id, score,
                            ActivityState.Reserve));
                }

                i++;
            }
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
            //_schoolNames.Add("Scoala Gimnaziala \"Gheorghe Sincai\" Floresti");
        }

        private static string GenerateRandomName(Random rand, string[] firstName, string[] lastName)
        {
            return (firstName[rand.Next(firstName.Length)] + " " + lastName[rand.Next(lastName.Length)]).Trim();
        }
    }
}
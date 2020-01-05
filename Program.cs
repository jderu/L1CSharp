using System;
using System.Collections.Generic;
using System.Linq;
using Lab1.Repository;
using Lab1.UI;

namespace Lab1 {
    internal class Program {
        private static List<string> _teamNames = new List<string>(new[] {
            "Houston Rockets", "Los Angeles Lakers", "LA Clippers", "Chicago Bulls", "Cleveland Cavaliers", "Utah Jazz",
            "Brooklyn Nets", "New Orleans Pelicans", "Indiana Pacers", "Toronto Raptors", "Charlotte Hornets", "Phoenix Suns",
            "Portland TrailBlazers", "Golden State Warriors", "Washington Wizards", "San Antonio Spurs", "Orlando Magic",
            "Denver Nuggets", "Detroit Pistons", "Atlanta Hawks", "Dallas Mavericks", "Sacramento Kings", "Oklahoma City Thunder",
            "Boston Celtics", "New York Knicks", "Minnesota Timberwolves", "Miami Heat", "Milwaukee Bucks"
        });

        private static List<string> _schoolNames = new List<string>(new[] {
            "Scoala Gimnaziala \"Octavian Goga\"", "Liceul Teoretic \"Lucian Blaga\"", "Scoala Gimnaziala \"Ioan Bob\"",
            "Scoala Gimnaziala \"Ion Creanga\"", "Colegiul National Pedagogic \"Gheorghe Lazar\"",
            "Scoala Gimnaziala Internationala SPECTRUM", "Colegiul National \"Emil Racovita\"", "Colegiul National \"George Cosbuc\"",
            "Scoala Gimnaziala \"Ion Agarbiceanu\"", "Liceul Teoretic \"Avram Iancu\"", "Scoala Gimnaziala \"Constantin Brancusi\"",
            "Liceul Teoretic \"Onisifor Ghibu\"", "Liceul cu Program Sportiv Cluj-Napoca", "Liceul Teoretic \"Nicolae Balcescu\"",
            "Liceul Teoretic \"Gheorghe Sincai\"", "Scoala \"Nicolae Titulescu\"", "Scoala Gimnaziala \"Liviu Rebreanu\"",
            "Scoala Gimnaziala \"Iuliu Hatieganu\"", "Liceul Teoretic \"Bathory Istvan\"", "Colegiul National \"George Baritiu\"",
            "Liceul Teoretic \"Apaczai Csere Janos\"", "Seminarul Teologic Ortodox", "Liceul de Informatica \"Tiberiu Popoviciu\"",
            "Scoala Gimnaziala \"Alexandru Vaida – Voevod\"", "Liceul Teoretic ELF", "Scoala Gimnaziala \"Gheorghe Sincai\" Floresti"
        });

        private static string filePathDirectory = "./";

        public static void Main() {
            TeamFileRepository teamRepository = new TeamFileRepository(new TeamValidator(), filePathDirectory + "teams.txt");
            Service<int, Team> teamService = new Service<int, Team>(teamRepository);

            PlayerFileRepository playerRepository = new PlayerFileRepository(new PlayerValidator(),
                filePathDirectory + "players.txt", teamRepository);
            Service<int, Player> playerService = new Service<int, Player>(playerRepository);

            GameFileRepository gameRepository =
                new GameFileRepository(new GameValidator(), filePathDirectory + "games.txt", teamRepository);
            GameService gameService = new GameService(gameRepository);

            ActivePlayerFileRepository activePlayersRepository =
                new ActivePlayerFileRepository(new ActivePlayerValidator(), filePathDirectory + "activePlayers.txt");
            ActivePlayerService activePlayerService = new ActivePlayerService(activePlayersRepository);

            //adds data to repositories
            AddJunk(teamRepository, playerRepository, gameRepository, activePlayersRepository);

            ConsoleUi consoleUi = new ConsoleUi(playerService, teamService, gameService, activePlayerService,
                _teamNames, _schoolNames);
            consoleUi.Run();
        }

        private static void AddJunk(TeamFileRepository teamRepository, PlayerFileRepository playerRepository,
            GameFileRepository gameRepository, ActivePlayerFileRepository activePlayersRepository) {
            Random rand = new Random(DateTime.Now.Millisecond);
            string[] firstNames = {
                "Jonathan", "Will A.", "Robert E. O.", "Dio", "George", "Joseph", "Caesar A.", "Lisa", "Rudol", "Suzi", "Kars",
                "Esidisi", "Wamuu", "Santana", "Jotaro", "Muhammad", "Noriaki", "Jean Pierre", "Iggy", "Holy", "Vanilla", "Hol",
                "Daniel J.", "Oingo", "Boingo", "Anubis", "Telence", "Josuke", "Okuyasu", "Koichi", "Rohan", "Hayato", "Yukako",
                "Tonio", "Tomoko", "Yoshikage", "Giorno", "Bruno", "Leone", "Guido", "Guido", "Narancia", "Trish", "Coco", "Pericolo",
                "Diavolo", "Cioccolata", "Polpo", "Pesci", ""
            };
            string[] lastNames = {
                "Joestar", "Zeppeli", "Speedwagon", "Brando", "Lisa", "von Stroheim", "Q", "Kujo", "Avdol", "Kakyoin", "Polnareff",
                "Ice", "Horse", "D'Arby", "Higashikata", "Nijimura", "Hirose", "Kishibe", "Kawajiri", "Yamagishi", "Trussardi", "Kira",
                "Giovanna", "Bucciarati", "Abbacchio", "Mista", "Ghirga", "Una", "Jumbo", "Doppio", ""
            };

            //adding random teams
            int id = 0;
            foreach (string teamName in _teamNames) teamRepository.Save(new Team(id++, teamName));

            //adding arandom players
            id = 0;
            var schoolTeams = Enumerable.Range(0, _teamNames.Count).OrderBy(x => rand.Next()).Take(_schoolNames.Count).ToList();
            foreach (var i in schoolTeams.Select((teamId, index) => (index, teamId))) {
                Team team = teamRepository.FindOne(i.teamId);
                for (int j = 0; j < rand.Next(15, 25); j++)
                    playerRepository.Save(new Player(id++, GenerateRandomName(rand, firstNames, lastNames), _schoolNames[i.index],
                        team));
            }

            //adding random games
            DateTime start = new DateTime(1995, 1, 1);
            DateTime end = DateTime.Today;
            int range = (end - start).Days;
            for (int i = 0; i < _schoolNames.Count - 1; i++)
                for (int j = i + 1; j < _schoolNames.Count; j++)
                    if (rand.Next(3) == 0) {
                        Team team1 = teamRepository.FindOne(i);
                        Team team2 = teamRepository.FindOne(j);
                        if (schoolTeams.Contains(team1.Id) && schoolTeams.Contains(team2.Id))
                            gameRepository.Save(new Game(team1, team2, start.AddDays(rand.Next(range))));
                    }

            //adding random active players
            var players = playerRepository.FindAll().ToList();
            foreach (Game game in gameRepository.FindAll()) {
                AddActivePlayerToGame(game.Id.Team1Id, game.Id.Team2Id, game.Id.Team1Id, players, rand, activePlayersRepository);
                AddActivePlayerToGame(game.Id.Team1Id, game.Id.Team2Id, game.Id.Team2Id, players, rand, activePlayersRepository);
            }
        }

        private static void AddActivePlayerToGame(int team1Id, int team2Id, int teamId, List<Player> players, Random rand,
            ActivePlayerFileRepository activePlayersRepository) {
            var teamPlayers = players.Where(p => p.Team.Id.Equals(teamId)).ToList();
            var teamPlaying = Enumerable.Range(0, teamPlayers.Count()).OrderBy(x => rand.Next()).ToArray();
            int i = 0;
            foreach (Player player in teamPlayers) {
                if (teamPlaying[i] < 15) {
                    int score = 0;
                    if (teamPlaying[i] < 11) {
                        int scoreProbability = rand.Next(1, 101);
                        if (scoreProbability > 95)
                            score = 2;
                        else if (scoreProbability > 80) score = 1;
                        activePlayersRepository.Save(new ActivePlayer(player.Id, team1Id, team2Id, score, ActivityState.Playing));
                    } else
                        activePlayersRepository.Save(new ActivePlayer(player.Id, team1Id, team2Id, score, ActivityState.Reserve));
                }

                i++;
            }
        }

        private static string GenerateRandomName(Random rand, string[] firstName, string[] lastName) {
            return (firstName[rand.Next(firstName.Length)] + " " + lastName[rand.Next(lastName.Length)]).Trim();
        }
    }
}
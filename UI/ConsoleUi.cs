using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab1.UI
{
    public class ConsoleUi
    {
        private Service<int, Player> _playerService;
        private Service<int, Team> _teamService;
        private GameService _gameService;
        private ActivePlayerService _activePlayerService;
        private List<string> _teamNames;
        private List<string> _schoolNames;

        public ConsoleUi(Service<int, Player> playerService, Service<int, Team> teamService,
            GameService gameService, ActivePlayerService activePlayerService, List<string> teamNames,
            List<string> schoolNames)
        {
            _playerService = playerService;
            _teamService = teamService;
            _gameService = gameService;
            _activePlayerService = activePlayerService;
            _teamNames = teamNames;
            _schoolNames = schoolNames;
        }

        private void PrintMenu()
        {
            Console.WriteLine("0 - Exit application\n" +
                              "1 - Show all players from a given team\n" +
                              "2 - Show active players from a given team and a given game\n" +
                              "3 - Show all games from a given time period\n" +
                              "4 - Show the score from a given game\n" +
                              "5 - Add Team\n" +
                              "6 - Add Player\n" +
                              "7 - Add Game\n" +
                              "8 - Add ActivePlayer");
        }

        public void Run()
        {
            bool running = true;
            PrintMenu();

            while (running)
            {
                try
                {
                    switch (ReadLine("Enter command:"))
                    {
                        case "0":
                            running = false;
                            _playerService.WriteAllToFile();
                            _teamService.WriteAllToFile();
                            _gameService.WriteAllToFile();
                            _activePlayerService.WriteAllToFile();
                            break;
                        case "1":
                            Command1();
                            break;
                        case "2":
                            Command2();
                            break;
                        case "3":
                            Command3();
                            break;
                        case "4":
                            Command4();
                            break;
                        case "5":
                            Command5();
                            break;
                        case "6":
                            Command6();
                            break;
                        case "7":
                            Command7();
                            break;
                        case "8":
                            Command8();
                            break;
                        default:
                            PrintMenu();
                            break;
                    }
                }
                catch (Exception e)
                {
                    if (e is UiException || e is ValidationException || e is FormatException)
                        Console.WriteLine(e.Message);
                    else
                        throw;
                }
            }
        }

        private string ReadLine(string text)
        {
            Console.WriteLine(text);
            return Console.ReadLine();
        }

        private void Command5()
        {
            int id = int.Parse(ReadLine("Write team id"));
            string name = ReadLine("Write team name");
            if (!_teamNames.Contains(name))
                throw new UiException("Not an allowed team name");
            _teamService.Save(new Team(id, name));
        }

        private void Command6()
        {
            int id = int.Parse(ReadLine("Write player id"));
            string name = ReadLine("Write player name");
            string school = ReadLine("Write player school");
            if (!_schoolNames.Contains(school))
                throw new UiException("Not an allowed school name");
            int teamId = int.Parse(ReadLine("Write player team id"));
            Team team = _teamService.FindOne(teamId);
            if (team == null)
                throw new UiException("Team not found");
            _playerService.Save(new Player(id, name, school, team));
        }

        private void Command7()
        {
            int team1Id = int.Parse(ReadLine("Write first team Id"));
            Team team1 = _teamService.FindOne(team1Id);
            if (team1 == null)
                throw new UiException("First team not found");
            int team2Id = int.Parse(ReadLine("Write second team Id"));
            Team team2 = _teamService.FindOne(team2Id);
            if (team2 == null)
                throw new UiException("Second team not found");
            DateTime date = DateTime.Parse(ReadLine("Write the date, format is dd/mm/yyyy"));
            _gameService.Save(new Game(team1, team2, date));
        }

        private void Command8()
        {
            int playerId = int.Parse(ReadLine("Write player id"));
            int team1Id = int.Parse(ReadLine("Write first team Id"));
            if (_teamService.FindOne(team1Id) == null)
                throw new UiException("First team not found");
            int team2Id = int.Parse(ReadLine("Write second team Id"));
            if (_teamService.FindOne(team2Id) == null)
                throw new UiException("Second team not found");

            int score = int.Parse(ReadLine("Write the scored points amount"));
            ActivityState state =
                (ActivityState) Enum.Parse(typeof(ActivityState), ReadLine("Write Playing or Reserve"));
            _activePlayerService.Save(new ActivePlayer(playerId, team1Id, team2Id, score, state));
        }

        private void Command1()
        {
            int teamId = int.Parse(ReadLine("Write team Id"));
            Team team = _teamService.FindOne(teamId);
            if (team == null)
                throw new UiException("Team not found");
            else
                foreach (Player player in _playerService.FindAll())
                    if (player.Team.Id.Equals(team.Id))
                        Console.WriteLine(player);
        }

        private void Command2()
        {
            int team1Id = int.Parse(ReadLine("Write first team Id"));
            int team2Id = int.Parse(ReadLine("Write second team Id"));
            int teamId = int.Parse(ReadLine("Write the Id of the team you want to see the active players"));
            Game game = _gameService.FindOne(new GameId<int>(team1Id, team2Id));
            if (game == null)
                throw new UiException("Teams: " + team1Id + " and " + team2Id + " haven't played a game yet.");
            if (team1Id != teamId && team2Id != teamId)
                throw new UiException("Team with the id " + teamId + " hasn't played in that game.");
            foreach (var player in _activePlayerService.ShowAllActivePlayersFromTeamFromGame(game.Id, teamId,
                _playerService))
                Console.WriteLine(player);
        }

        private void Command3()
        {
            DateTime startDate = DateTime.Parse(ReadLine("Write the start of the time period, format is dd/mm/yyyy"));
            DateTime endDate = DateTime.Parse(ReadLine("Write the end of the time period, format is dd/mm/yyyy"));
            foreach (var game in _gameService.ShowAllGamesBetween(startDate, endDate))
                Console.WriteLine(game);
        }

        private void Command4()
        {
            int team1Id = int.Parse(ReadLine("Write first team Id"));
            int team2Id = int.Parse(ReadLine("Write second team Id"));
            Game game = _gameService.FindOne(new GameId<int>(team1Id, team2Id));
            if (game == null)
                throw new UiException("Teams: " + team1Id + " and " + team2Id + " haven't played a game yet.");
            Console.WriteLine(_activePlayerService.CalculateGameScore(game.Id));
        }
    }
}
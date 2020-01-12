using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab1.UI {
    public class ConsoleUi {
        private PlayerService _playerService;
        private Service<int, Team> _teamService;
        private GameService _gameService;
        private ActivePlayerService _activePlayerService;
        private List<string> _teamNames;
        private List<string> _schoolNames;

        public ConsoleUi(PlayerService playerService, Service<int, Team> teamService, GameService gameService,
            ActivePlayerService activePlayerService, List<string> teamNames, List<string> schoolNames) {
            _playerService = playerService;
            _teamService = teamService;
            _gameService = gameService;
            _activePlayerService = activePlayerService;
            _teamNames = teamNames;
            _schoolNames = schoolNames;
        }

        private void PrintMenu() {
            Console.WriteLine("0 - Exit application\n" + "1 - Show all players from a given team\n" +
                              "2 - Show active players from a given team and a given game\n" +
                              "3 - Show all games from a given time period\n" + "4 - Show the score from a given game\n");
        }

        public void Run() {
            bool running = true;
            PrintMenu();

            while (running) {
                try {
                    switch (ReadLine("Enter command:")) {
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
                        default:
                            PrintMenu();
                            break;
                    }
                }
                catch (Exception e) {
                    if (e is UiException || e is ValidationException || e is FormatException)
                        Console.WriteLine(e.Message);
                    else
                        throw;
                }
            }
        }

        private string ReadLine(string text) {
            Console.WriteLine(text);
            return Console.ReadLine();
        }

        private void Command1() {
            int teamId = int.Parse(ReadLine("Write team Id"));
            Team team = _teamService.FindOne(teamId);
            if (team == null) throw new UiException("Team not found");
            foreach (Player player in _playerService.ShowAllPlayersFromATeam(team.Id)) Console.WriteLine(player);
        }

        private void Command2() {
            int team1Id = int.Parse(ReadLine("Write first team Id"));
            int team2Id = int.Parse(ReadLine("Write second team Id"));
            int teamId = int.Parse(ReadLine("Write the Id of the team you want to see the active players"));
            Game game = _gameService.FindOne(new GameId<int>(team1Id, team2Id));
            if (game == null) throw new UiException("Teams: " + team1Id + " and " + team2Id + " haven't played a game yet.");
            if (team1Id != teamId && team2Id != teamId)
                throw new UiException("Team with the id " + teamId + " hasn't played in that game.");
            foreach (var player in _activePlayerService.ShowAllActivePlayersFromTeamFromGame(game.Id, teamId, _playerService))
                Console.WriteLine(player);
        }

        private void Command3() {
            DateTime startDate = DateTime.Parse(ReadLine("Write the start of the time period, format is dd/mm/yyyy"));
            DateTime endDate = DateTime.Parse(ReadLine("Write the end of the time period, format is dd/mm/yyyy"));
            foreach (var game in _gameService.ShowAllGamesBetween(startDate, endDate)) Console.WriteLine(game);
        }

        private void Command4() {
            int team1Id = int.Parse(ReadLine("Write first team Id"));
            int team2Id = int.Parse(ReadLine("Write second team Id"));
            GameId<int> gameId = new GameId<int>(team1Id, team2Id);
            Game game = _gameService.FindOne(gameId);
            if (game == null) throw new UiException("Teams: " + team1Id + " and " + team2Id + " haven't played a game yet.");
            Console.WriteLine(_activePlayerService.CalculateGameScore(gameId, _playerService));
        }
    }
}
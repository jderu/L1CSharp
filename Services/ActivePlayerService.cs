using System.Collections.Generic;
using System.Linq;
using Lab1.Repository;

namespace Lab1 {
    public class ActivePlayerService : Service<ActivePlayerId<int, GameId<int>>, ActivePlayer> {
        public ActivePlayerService(FileRepository<ActivePlayerId<int, GameId<int>>, ActivePlayer> repository) : base(repository) { }

        public IEnumerable<Player> ShowAllActivePlayersFromTeamFromGame(GameId<int> gameId, int teamId,
            Service<int, Player> playerService) {
            return _repository.FindAll().Where(ap => ap.GameId.Equals(gameId)).Select(ap => playerService.FindOne(ap.PlayerId))
                .Where(p => p.Team.Id.Equals(teamId));
        }

        public string CalculateGameScore(GameId<int> gameId, PlayerService playerService) {
            int team1Score = _repository.FindAll()
                .Where(ap => ap.GameId.Equals(gameId) && playerService.FindOne(ap.PlayerId).Team.Id.Equals(gameId.Team1Id))
                .Sum(ap => ap.Score);
            int team2Score = _repository.FindAll()
                .Where(ap => ap.GameId.Equals(gameId) && playerService.FindOne(ap.PlayerId).Team.Id.Equals(gameId.Team2Id))
                .Sum(ap => ap.Score);
            return "The score is: " + team1Score + "-" + team2Score + ".";
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using Lab1.Repository;

namespace Lab1 {
    public class PlayerService : Service<int, Player> {
        public PlayerService(FileRepository<int, Player> repository) : base(repository) { }

        public IEnumerable<Player> ShowAllPlayersFromATeam(int teamId) {
            return _repository.FindAll().Where(p => p.Team.Id.Equals(teamId));
        }
    }
}
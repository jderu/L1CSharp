using System;
using System.Collections.Generic;
using System.Linq;
using Lab1.Repository;

namespace Lab1
{
    public class GameService : Service<GameId<int>, Game>
    {
        public GameService(FileRepository<GameId<int>, Game> repository) : base(repository)
        {
        }

        public IEnumerable<Game> ShowAllGamesBetween(DateTime startDate, DateTime endDate)
        {
            return _repository.FindAll().Where(g => g.Date >= startDate && g.Date <= endDate);
        }
    }
}
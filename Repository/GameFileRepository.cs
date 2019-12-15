using System;
using System.Collections.Generic;

namespace Lab1.Repository
{
    public class GameFileRepository : FileRepository<GameId<int>, Game>
    {
        private FileRepository<int, Team> _teamRepository;

        public GameFileRepository(IValidator<Game> validator, string filePath, FileRepository<int, Team> teamRepository)
            : base(validator, filePath)
        {
            _teamRepository = teamRepository;
            ReadAllFromFile();
        }

        protected override Game ReadEntity(string line)
        {
            string[] fields = line.Split(new[] {"||"}, StringSplitOptions.RemoveEmptyEntries);
            Team team1 = _teamRepository.FindOne(int.Parse(fields[0]));
            Team team2 = _teamRepository.FindOne(int.Parse(fields[1]));
            return new Game(team1, team2, DateTime.Parse(fields[2]));
        }

        protected override string WriteEntity(Game entity)
        {
            return entity.Id.Team1Id + "||" + entity.Id.Team2Id + "||" + entity.Date;
        }
    }
}
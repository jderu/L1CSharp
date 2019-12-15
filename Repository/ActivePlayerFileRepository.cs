using System;
using System.Collections.Generic;

namespace Lab1.Repository
{
    public class ActivePlayerFileRepository : FileRepository<ActivePlayerId<int, GameId<int>>, ActivePlayer>
    {
        public ActivePlayerFileRepository(IValidator<ActivePlayer> validator, string filePath) : base(validator,
            filePath)
        {
            ReadAllFromFile();
        }

        protected override ActivePlayer ReadEntity(string line)
        {
            string[] fields = line.Split(new[] {"||"}, StringSplitOptions.RemoveEmptyEntries);
            return new ActivePlayer(int.Parse(fields[0]), int.Parse(fields[1]), int.Parse(fields[2]),
                int.Parse(fields[3]),
                (ActivityState) Enum.Parse(typeof(ActivityState), fields[4]));
        }

        protected override string WriteEntity(ActivePlayer entity)
        {
            return entity.PlayerId + "||" + entity.GameId.Team1Id + "||" + entity.GameId.Team2Id + "||" + entity.Score +
                   "||" + entity.State;
        }
    }
}
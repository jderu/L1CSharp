using System;

namespace Lab1.Repository
{
    public class TeamFileRepository : FileRepository<int, Team>
    {
        public TeamFileRepository(IValidator<Team> validator, string filePath) : base(validator, filePath)
        {
            ReadAllFromFile();
        }

        protected override Team ReadEntity(string line)
        {
            string[] fields = line.Split(new[] {"||"}, StringSplitOptions.RemoveEmptyEntries);
            return new Team(int.Parse(fields[0]), fields[1]);
        }

        protected override string WriteEntity(Team entity)
        {
            return entity.Id + "||" + entity.Name;
        }
    }
}
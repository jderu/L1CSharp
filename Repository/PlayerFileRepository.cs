using System;

namespace Lab1.Repository
{
    public class PlayerFileRepository : FileRepository<int, Player>
    {
        private FileRepository<int, Team> _teamRepository;

        public PlayerFileRepository(IValidator<Player> validator, string filePath,
            FileRepository<int, Team> teamRepository) : base(validator, filePath)
        {
            _teamRepository = teamRepository;
            ReadAllFromFile();
        }

        protected override Player ReadEntity(string line)
        {
            string[] fields = line.Split(new[] {"||"}, StringSplitOptions.RemoveEmptyEntries);
            Team team = _teamRepository.FindOne(int.Parse(fields[3]));
            return new Player(int.Parse(fields[0]), fields[1], fields[2], team);
        }

        protected override string WriteEntity(Player entity)
        {
            return entity.Id + "||" + entity.Name + "||" + entity.School + "||" + entity.Team.Id;
        }
    }
}
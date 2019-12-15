namespace Lab1
{
    public class Player : Student
    {
        public Team Team { get; set; }

        public Player(int id, string name, string school, Team team) : base(id, name, school)
        {
            Team = team;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, {nameof(Team)}: {Team}";
        }
    }
}
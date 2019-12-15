namespace Lab1
{
    public class Team : Entity<int>
    {
        public Team(int id, string name) : base(id)
        {
            Name = name;
        }

        public string Name { get; set; }

        public override string ToString()
        {
            return $"{base.ToString()}, {nameof(Name)}: {Name}";
        }
    }
}
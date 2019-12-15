namespace Lab1
{
    public class Entity<ID>
    {
        public Entity(ID id)
        {
            Id = id;
        }

        public ID Id { get; set; }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}";
        }
    }
}
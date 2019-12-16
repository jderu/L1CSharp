namespace test
{
    public class GameId
    {
        private int id1;
        private int id2;

        public GameId(int id1, int id2)
        {
            this.id1 = id1;
            this.id2 = id2;
        }

        public int Id1
        {
            get => id1;
            set => id1 = value;
        }

        public int Id2
        {
            get => id2;
            set => id2 = value;
        }

        protected bool Equals(GameId other)
        {
            return id1 == other.id1 && id2 == other.id2 || id1 == other.id2 && id2 == other.id1;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((GameId) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (id1 * 397) ^ id2;
            }
        }
    }
}
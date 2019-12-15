namespace Lab1
{
    public class Student : Entity<int>
    {
        public Student(int id, string name, string school) : base(id)
        {
            this.Name = name;
            this.School = school;
        }

        public string Name { get; set; }
        public string School { get; set; }

        public override string ToString()
        {
            return $"{base.ToString()}, {nameof(Name)}: {Name}, {nameof(School)}: {School}";
        }
    }
}
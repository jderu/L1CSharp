using System;
using System.Collections.Generic;

namespace Lab1
{
    public class Game : Entity<GameId<int>>
    {
        public Game(Team team1, Team team2, DateTime date) : base(new GameId<int>(team1.Id, team2.Id))
        {
            this.Team1 = team1;
            this.Team2 = team2;
            this.Date = date;
        }

        private Team _team1;

        public Team Team1
        {
            get => _team1;
            set
            {
                _team1 = value;
                Id.Team1Id = value.Id;
            }
        }

        private Team _team2;

        public Team Team2
        {
            get => _team2;
            set
            {
                _team2 = value;
                Id.Team2Id = value.Id;
            }
        }

        public DateTime Date { get; set; }
    }

    public class GameId<T>
    {
        public T Team1Id { get; set; }
        public T Team2Id { get; set; }

        public GameId(T team1Id, T team2Id)
        {
            Team1Id = team1Id;
            Team2Id = team2Id;
        }

        public override string ToString()
        {
            return $"{nameof(Team1Id)}: {Team1Id}, {nameof(Team2Id)}: {Team2Id}";
        }

        protected bool Equals(GameId<T> other)
        {
            return EqualityComparer<T>.Default.Equals(Team1Id, other.Team1Id) && EqualityComparer<T>.Default.Equals(Team2Id, other.Team2Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((GameId<T>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (EqualityComparer<T>.Default.GetHashCode(Team1Id) * 397) ^ EqualityComparer<T>.Default.GetHashCode(Team2Id);
            }
        }
    }
}
using System;
using System.Collections.Generic;

namespace Lab1
{
    public class ActivePlayer : Entity<ActivePlayerId<int, GameId<int>>>
    {
        public ActivePlayer(int playerId, int team1, int team2, int score, ActivityState state) : base(
            new ActivePlayerId<int, GameId<int>>(playerId, new GameId<int>(team1, team2)))
        {
            Score = score;
            State = state;
        }

        public int PlayerId
        {
            get => Id.PlayerId;
            set => Id.PlayerId = value;
        }

        public GameId<int> GameId
        {
            get => Id.GameId;
            set => Id.GameId = value;
        }

        public int Score { get; set; }
        public ActivityState State { get; set; }

        public override string ToString()
        {
            return
                $"{nameof(PlayerId)}: {PlayerId}, {nameof(GameId)}: {GameId}, {nameof(Score)}: {Score}, {nameof(State)}: {State}";
        }
    }

    public class ActivePlayerId<T1, T2>
    {
        public T1 PlayerId { get; set; }
        public T2 GameId { get; set; }

        public ActivePlayerId(T1 playerId, T2 gameId)
        {
            PlayerId = playerId;
            GameId = gameId;
        }

        public override string ToString()
        {
            return $"{nameof(PlayerId)}: {PlayerId}, {nameof(GameId)}: {GameId}";
        }

        protected bool Equals(ActivePlayerId<T1, T2> other)
        {
            return EqualityComparer<T1>.Default.Equals(PlayerId, other.PlayerId) &&
                   EqualityComparer<T2>.Default.Equals(GameId, other.GameId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ActivePlayerId<T1, T2>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (EqualityComparer<T1>.Default.GetHashCode(PlayerId) * 397) ^
                       EqualityComparer<T2>.Default.GetHashCode(GameId);
            }
        }
    }
}
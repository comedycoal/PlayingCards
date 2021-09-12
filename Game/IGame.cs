namespace PlayingCards.Game
{
    /// <summary>
    /// Interface for classes handling game's logistics.
    /// </summary>
    public interface IGame
    {
        public bool Started { get; }

        public bool? Paused { get; }

        public bool? Ended { get;}

        public ISaveObject Save();

        public void Load(ISaveObject saveObject);

        public void Start();

        public void Pause();

        public void Resume();

        public void End();

        public void Reset();
    }
}

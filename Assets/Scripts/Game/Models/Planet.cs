namespace Game.Models
{
    public class Planet
    {
        public Orbit Orbit { get; }

        public Planet(Orbit orbit)
        {
            Orbit = orbit;
        }
    }
}
namespace Game.Models
{
    public class Orbit
    {
        public float Speed { get; }
        public float Radius { get; }

        public Orbit(float speed, float radius)
        {
            Speed = speed;
            Radius = radius;
        }
    }
}
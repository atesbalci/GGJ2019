namespace Game.Models.Data
{
    public class OrbitData
    {
        public float Speed { get; private set; }
        public float Radius { get; private set; }

        public OrbitData(float speed, float radius)
        {
            Speed = speed;
            Radius = radius;
        }
    }
}
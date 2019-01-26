namespace Game.Models
{
    public enum ShipState
    {
        None,
        Idle,
        Moving
    }

    public class Ship
    {
        public float MaxMoveSpeed { get; private set; }
        public float RotationSpeed { get; private set; }
        public float Acceleration { get; private set; }

        public float CurrentSpeed;
        public ShipState State;

        public void SetValue(float maxMoveSpeed, float rotationSpeed, float acceleration)
        {
            MaxMoveSpeed = maxMoveSpeed;
            RotationSpeed = rotationSpeed;
            Acceleration = acceleration;
        }
    }

}

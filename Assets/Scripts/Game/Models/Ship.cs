using System;

namespace Game.Models
{
    public enum ShipState
    {
        Idle,
        Moving
    }

    public class Ship
    {
        public float MaxMoveSpeed { get; private set; }
        public float RotationSpeed { get; private set; }
        public float Acceleration { get; private set; }

        public float CurrentSpeed;

        public event Action<ShipState> OnStateChanged;

        private ShipState _state;

        public ShipState State
        {
            get { return _state; }
            set
            {
                if (value != _state)
                {
                    _state = value;
                    OnStateChanged?.Invoke(_state);
                }
            }
        }
        
        public void SetValue(float maxMoveSpeed, float rotationSpeed, float acceleration)
        {
            MaxMoveSpeed = maxMoveSpeed;
            RotationSpeed = rotationSpeed;
            Acceleration = acceleration;
        }
    }

}

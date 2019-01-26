using System;

namespace Game.Models
{
    public enum ShipState
    {
        Idle,
        Moving,
        Landing,
    }

    public class Ship
    {
        public float MaxMoveSpeed { get; private set; }
        public float RotationSpeed { get; private set; }
        public float Acceleration { get; private set; }

        public float MaxFuel { get; private set; }
        public float MaxLifeSupport { get; private set; }
        public float FuelFlow { get; private set; }

        public float Fuel { get; set; }
        public float LifeSupport { get; set; }

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
        
        public void SetShipParameters(float maxMoveSpeed, float rotationSpeed, float acceleration, float maxFuel, float maxLifeSupport, float fuelflow)
        {
            MaxMoveSpeed = maxMoveSpeed;
            RotationSpeed = rotationSpeed;
            Acceleration = acceleration;
            MaxFuel = maxFuel;
            MaxLifeSupport = maxLifeSupport;
            FuelFlow = fuelflow;
        }

        public void FillConsumables()
        {
            Fuel = MaxFuel;
            LifeSupport = MaxLifeSupport;
        }
    }

}

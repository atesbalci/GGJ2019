using System;
using UniRx;

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
        public FloatReactiveProperty MaxMoveSpeed { get; private set; }
        public FloatReactiveProperty RotationSpeed { get; private set; }
        public FloatReactiveProperty Acceleration { get; private set; }

        public FloatReactiveProperty MaxFuel { get; private set; }
        public FloatReactiveProperty MaxLifeSupport { get; private set; }

        public FloatReactiveProperty FuelFlow { get; private set; }
        public FloatReactiveProperty LifeSupportFlow { get; private set; }

        public FloatReactiveProperty Fuel { get; set; }
        public FloatReactiveProperty LifeSupport { get; set; }

        public float CurrentSpeed;

        public event Action<ShipState> OnStateChanged;
        private ShipState _state;
        public ShipState State
        {
            get => _state;
            set
            {
                if (value != _state)
                {
                    _state = value;
                    OnStateChanged?.Invoke(_state);
                }
            }
        }
        
        public void SetShipParameters(float maxMoveSpeed,
            float rotationSpeed,
            float acceleration,
            float maxFuel,
            float maxLifeSupport,
            float fuelFlow,
            float lifeSupportFlow)
        {
            MaxMoveSpeed = new FloatReactiveProperty(maxMoveSpeed);
            RotationSpeed = new FloatReactiveProperty(rotationSpeed);
            Acceleration = new FloatReactiveProperty(acceleration);
            MaxFuel = new FloatReactiveProperty(maxFuel);
            MaxLifeSupport = new FloatReactiveProperty(maxLifeSupport);
            FuelFlow = new FloatReactiveProperty(fuelFlow);
            LifeSupportFlow = new FloatReactiveProperty(lifeSupportFlow);
        }

        public void FillConsumables()
        {
            Fuel = new FloatReactiveProperty(MaxFuel.Value);
            LifeSupport = new FloatReactiveProperty(0f);
        }
    }

}

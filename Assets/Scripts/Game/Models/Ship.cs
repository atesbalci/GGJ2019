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

        public Ship()
        {
            MaxMoveSpeed = new FloatReactiveProperty();
            RotationSpeed = new FloatReactiveProperty();
            Acceleration = new FloatReactiveProperty();
            MaxFuel = new FloatReactiveProperty();
            MaxLifeSupport = new FloatReactiveProperty();
            FuelFlow = new FloatReactiveProperty();
            LifeSupportFlow = new FloatReactiveProperty();
            Fuel = new FloatReactiveProperty();
            LifeSupport = new FloatReactiveProperty();
        }

        public void SetShipParameters(float maxMoveSpeed,
            float rotationSpeed,
            float acceleration,
            float maxFuel,
            float maxLifeSupport,
            float fuelFlow,
            float lifeSupportFlow)
        {
            MaxMoveSpeed.Value = maxMoveSpeed;
            RotationSpeed.Value = rotationSpeed;
            Acceleration.Value = acceleration;
            MaxFuel.Value = maxFuel;
            MaxLifeSupport.Value = maxLifeSupport;
            FuelFlow.Value = fuelFlow;
            LifeSupportFlow.Value = lifeSupportFlow;
        }

        public void FillConsumables()
        {
            Fuel.Value = MaxFuel.Value;
            LifeSupport.Value = 0f;
        }
    }

}

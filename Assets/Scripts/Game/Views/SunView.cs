using DG.Tweening;
using UnityEngine;

namespace Game.Views
{
    [RequireComponent(typeof(LensFlare))]
    public class SunView : MonoBehaviour
    {
        private const float MinBrightness = 0.2f;
        private const float MaxBrightness = 0.25f;
        private const float CycleFrequency = 1f;

        private LensFlare _lensFlare;
        
        private void Awake()
        {
            _lensFlare = GetComponent<LensFlare>();
        }

        private void Update()
        {
            var state = Time.time * CycleFrequency;
            var stateFloor = Mathf.FloorToInt(state);
            state -= stateFloor;
            state = (stateFloor % 2 == 0) ? (1f - state) : state;
            _lensFlare.brightness = Mathf.Lerp(MinBrightness, MaxBrightness, state);
        }
    }
}
using DG.Tweening;
using UnityEngine;

using Random = UnityEngine.Random;

namespace Lighting
{
    [RequireComponent(typeof(Light))]
    public class LightFlickering: MonoBehaviour
    {
        [SerializeField] private Vector2 _flickersMultiplierRange = new Vector2(0.7f, 0.9f);
        [SerializeField] private Vector2 _flickersDurationRange = new Vector2(0.5f, 1f);

        private Light _light;
        private AutoSwitchLight _lightSwitch;

        private void OnValidate()
        {
            float durationX = _flickersDurationRange.x;
            float durationY = _flickersDurationRange.y;
            float multiplierX = _flickersMultiplierRange.x;
            float multiplierY = _flickersMultiplierRange.y;

            if (_flickersDurationRange.x > _flickersDurationRange.y) {
                _flickersDurationRange.x = durationY;
                _flickersDurationRange.y = durationX;
            }

            switch (_flickersMultiplierRange.x)
            {
                case > 1:
                    _flickersMultiplierRange.x = 1;
                    break;
                case < 0:
                    _flickersMultiplierRange.x = 0;
                    break;
            }

            switch (_flickersMultiplierRange.y)
            {
                case > 1:
                    _flickersMultiplierRange.y = 1;
                    break;
                case < 0:
                    _flickersMultiplierRange.y = 0;
                    break;
            }

            if (_flickersMultiplierRange.x > _flickersMultiplierRange.y) {
                _flickersMultiplierRange.x = multiplierY;
                _flickersMultiplierRange.y = multiplierX;
            }
        }

        private void Awake()
        {
            _light = GetComponent<Light>();
            _lightSwitch = GetComponent<AutoSwitchLight>();
        }

        private void OnEnable()
        {
            _lightSwitch.OnSwitchLightOn += Flicker;
        }

        private void OnDisable()
        {
            _light.DOKill(true);
            _lightSwitch.OnSwitchLightOn -= Flicker;
        }

        [ContextMenu("Test flickers")]
        private void Flicker()
        {
            float duration = Random.Range(_flickersDurationRange.x, _flickersDurationRange.y);
            float startingIntensity = _light.intensity * Random.Range(_flickersMultiplierRange.x, _flickersMultiplierRange.y);

            _light.DOIntensity(startingIntensity, duration).From().SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutCubic);
        }
    }
}
using System.Collections;
using TMPro;
using UnityEngine;

namespace UI.Effects
{
    public class SmoothBlink : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private float _duration = 1f;
        [SerializeField] private float _frequency = 1f;

        private float Frequency => 1 / _frequency;
        private WaitForSeconds _interval;

        private void Awake()
        {
            _interval = new WaitForSeconds(Frequency);
            StartCoroutine(Blink());
        }

        private IEnumerator Blink()
        {
            float t = 0;
            Color initialColor = _text.color;

            while (gameObject.activeInHierarchy)
            {
                while (t < 1)
                {
                    t += Time.deltaTime / (_duration / 2);
                    _text.color = Color.Lerp(initialColor, Color.white, t);
                    yield return null;
                }

                yield return _interval;
                t = 0;

                while (t < 1)
                {
                    t += Time.deltaTime / (_duration / 2);
                    _text.color = Color.Lerp(Color.white, initialColor, t);
                    yield return null;
                }

                t = 0;
                yield return _interval;
            }
        }
    }
}
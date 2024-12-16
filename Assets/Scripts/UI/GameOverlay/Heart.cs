using UnityEngine;

namespace UI.GameOverlay
{
    public class Heart: MonoBehaviour
    {
        [SerializeField] private GameObject _halfHeartLeft;
        [SerializeField] private GameObject _halfHeartRight;

        public bool IsFull => _halfHeartLeft.activeSelf && _halfHeartRight.activeSelf;

        public void Fill()
        {
            _halfHeartLeft.SetActive(true);
            _halfHeartRight.SetActive(true);
        }

        public void Damage()
        {
            if (IsFull)
            {
                _halfHeartLeft.SetActive(false);
            }
            else
            {
                _halfHeartRight.SetActive(false);
            }
        }

        public void Empty()
        {
            _halfHeartLeft.SetActive(false);
            _halfHeartRight.SetActive(false);
        }
    }
}
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GameOverlay
{
    public class ExperienceDisplay: MonoBehaviour
    {
        [Header("References")]
        /* [SerializeField] */ private Slider _exp;
        /* [SerializeField] */ private TMP_Text _level;

        private void Awake()
        {
            _exp = GameObject.Find("UI/GameOverlay/HUD/Experience/Gauge").GetComponent<Slider>();
            _level = GameObject.Find("UI/GameOverlay/HUD/Experience/Level").GetComponent<TMP_Text>();
        }

        private void OnEnable()
        {
            Character.OnExpChanged += UpdateXP;
            Character.OnLevelUp += UpdateLevelAndStats;
        }

        private void OnDisable()
        {
            Character.OnExpChanged -= UpdateXP;
            Character.OnLevelUp -= UpdateLevelAndStats;
        }

        private void UpdateXP(int value)
        {
            _exp.value = value;
        }

        private void UpdateLevelAndStats()
        {
            _level.text = $"Lv. {Character.Instance.Level}";
            _exp.maxValue = Character.Instance.RequireEXP;
        }
    }
}
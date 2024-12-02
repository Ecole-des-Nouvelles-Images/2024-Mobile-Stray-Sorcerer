using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PlayerUI: MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Slider _exp;
        [SerializeField] private TMP_Text _level;

        private void OnEnable()
        {
            Character.OnHpChanged += UpdateHP;
            Character.OnExpChanged += UpdateXP;
            Character.OnLevelUp += UpdateLevelAndStats;
        }

        private void OnDisable()
        {
            Character.OnHpChanged -= UpdateHP;
            Character.OnExpChanged -= UpdateXP;
            Character.OnLevelUp -= UpdateLevelAndStats;
        }

        private void UpdateHP(int value)
        {
            // TODO: Manage heart-quarters
        }

        private void UpdateXP(int value)
        {
            _exp.value = value;
        }

        private void UpdateLevelAndStats()
        {
            _level.text = $"Lv. {Character.Instance.Level}";
            _exp.maxValue = Character.Instance.RequireEXP;

            // TODO: Update MaxHealth panel
            // TODO: Update PauseOverlay's statistics panel
        }
    }
}
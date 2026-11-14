using TMPro;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class VersionBinder : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<TMP_Text>().text = $"v {Application.version}";
        }
    }
}

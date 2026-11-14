using System;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

namespace UI
{
    public static class LocalizationText
    {
        private const string TableName = "UIStrings";

        public static void Set(TMP_Text text, string key, params object[] arguments)
        {
            if (!text)
                throw new ArgumentNullException(nameof(text));

            var localizeEvent = text.GetComponent<LocalizeStringEvent>();
            if (!localizeEvent)
                throw new InvalidOperationException(
                    $"The text object '{text.gameObject.name}' has no LocalizeStringEvent.");

            var reference = new LocalizedString(TableName, key)
            {
                Arguments = arguments
            };
            localizeEvent.StringReference = reference;
            localizeEvent.RefreshString();
        }
    }
}

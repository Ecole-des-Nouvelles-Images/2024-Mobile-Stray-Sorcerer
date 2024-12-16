using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Utils
{
    public class WaitForUIButtons : CustomYieldInstruction, System.IDisposable
    {
        private struct ButtonCallback
        {
            public Button button;
            public UnityAction listener;
        }

        private List<ButtonCallback> m_Buttons = new();
        private System.Action<Button> m_Callback = null;

        public override bool keepWaiting => PressedButton == null;
        public Button PressedButton { get; private set; } = null;

        public WaitForUIButtons(System.Action<Button> aCallback, params Button[] aButtons)
        {
            m_Callback = aCallback;
            m_Buttons.Capacity = aButtons.Length;
            foreach (Button b in aButtons)
            {
                if (b == null)
                    continue;
                ButtonCallback bc = new() { button = b };
                bc.listener = () => OnButtonPressed(bc.button);
                m_Buttons.Add(bc);
            }

            Reset();
        }

        public WaitForUIButtons(params Button[] aButtons) : this(null, aButtons)
        {
        }

        private void OnButtonPressed(Button button)
        {
            PressedButton = button;
            RemoveListeners();
            if (m_Callback != null)
                m_Callback(button);
        }

        private void InstallListeners()
        {
            foreach (ButtonCallback bc in m_Buttons)
                if (bc.button != null)
                    bc.button.onClick.AddListener(bc.listener);
        }

        private void RemoveListeners()
        {
            foreach (ButtonCallback bc in m_Buttons)
                if (bc.button != null)
                    bc.button.onClick.RemoveListener(bc.listener);
        }

        public new WaitForUIButtons Reset()
        {
            RemoveListeners();
            PressedButton = null;
            InstallListeners();
            base.Reset();
            return this;
        }

        public WaitForUIButtons ReplaceCallback(System.Action<Button> aCallback)
        {
            m_Callback = aCallback;
            return this;
        }

        public void Dispose()
        {
            RemoveListeners();
            m_Callback = null;
            m_Buttons.Clear();
        }
    }
}
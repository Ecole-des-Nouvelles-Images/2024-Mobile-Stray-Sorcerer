using Manager;
using UnityEngine;

namespace UI
{
    public abstract class EndGameUI : MonoBehaviour
    {
        public void ReturnTitle()
        {
            Time.timeScale = 1;
            SceneLoader.Instance.FullReload();
        }

        /// <summary>
        /// The InputSystemUIInputModule shares the player's PlayerInput action asset.
        /// When the player is disabled/destroyed (death, maze exit), and since the "UI" map may already
        /// have been disabled by a previous SwitchCurrentActionMap("Player"), the UI module is left with
        /// disabled actions: buttons can be selected by code but never clicked/submitted.
        /// </summary>
        protected static void EnableUIInputActions()
        {
            if (UnityEngine.EventSystems.EventSystem.current == null)
                return;

            UnityEngine.InputSystem.UI.InputSystemUIInputModule module =
                UnityEngine.EventSystems.EventSystem.current.GetComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();

            if (module == null)
                return;

            EnableAction(module.point);
            EnableAction(module.move);
            EnableAction(module.submit);
            EnableAction(module.cancel);
            EnableAction(module.leftClick);
            EnableAction(module.scrollWheel);
        }

        private static void EnableAction(UnityEngine.InputSystem.InputActionReference reference)
        {
            if (reference != null && reference.action != null)
                reference.action.Enable();
        }
    }
}

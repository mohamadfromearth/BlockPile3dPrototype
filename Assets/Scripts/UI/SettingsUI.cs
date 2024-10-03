using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    [System.Serializable]
    public class SettingsUI
    {
        [SerializeField] private Button cancelButton;
        [SerializeField] private Button musicButton;
        [SerializeField] private Button soundButton;
        [SerializeField] private Button vibrationButton;
        [SerializeField] private Image musicImage;
        [SerializeField] private Image soundImage;
        [SerializeField] private Image vibrationImage;
        [SerializeField] private Button quitToMenu;
        [SerializeField] private Button retryButton;


        public void AddCancelClickListener(UnityAction action)
        {
        }

        public void RemoveCancelClickListener(UnityAction action)
        {
        }

        public void AddMusicClickListener(UnityAction action)
        {
        }

        public void RemoveMusicClickListener(UnityAction action)
        {
        }

        public void AddSoundClickListener(UnityAction action)
        {
        }

        public void RemoveSoundClickListener(UnityAction action)
        {
        }

        public void VibrationClickListener(UnityAction action)
        {
        }

        public void RemoveVibrationClickListener(UnityAction action)
        {
        }
    }
}
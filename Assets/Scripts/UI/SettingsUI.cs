using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    [System.Serializable]
    public class SettingsUI
    {
        [SerializeField] private Button musicButton;
        [SerializeField] private Button soundButton;
        [SerializeField] private Button exitButton;


        [SerializeField] private Image musicImage;
        [SerializeField] private Image soundImage;
        [SerializeField] private TwoButtonsDialog exitDialog;


        public void AddMusicClickListener(UnityAction action) => musicButton.onClick.AddListener(action);

        public void RemoveMusicClickListener(UnityAction action) => musicButton.onClick.RemoveListener(action);

        public void AddSoundClickListener(UnityAction action) => soundButton.onClick.AddListener(action);

        public void RemoveSoundClickListener(UnityAction action) => soundButton.onClick.RemoveListener(action);


        public void AddBackToMenuClickListener(UnityAction action) => exitDialog.AddLeftButtonClickListener(action);


        public void RemoveBackToMenuClickListener(UnityAction action) =>
            exitDialog.RemoveLeftButtonClickListener(action);


        public void AddCancelDialogClickListener(UnityAction action) => exitDialog.AddCancelClickListener(action);
        public void RemoveCancelDialogClickListener(UnityAction action) => exitDialog.RemoveCancelClickListener(action);


        public void AddExitClickListener(UnityAction action) => exitButton.onClick.AddListener(action);
        public void RemoveExitClickListener(UnityAction action) => exitButton.onClick.RemoveListener(action);


        public void ShowExitDialog() => exitDialog.Show();


        public void HideExitDialog() => exitDialog.Hide();


        public void AddRetryClickListener(UnityAction action) => exitDialog.AddRightButtonClickListener(action);


        public void RemoveRetryClickListener(UnityAction action) => exitDialog.RemoveRightButtonClickListener(action);


        public void SetSoundSprite(Sprite soundSprite)
        {
            soundImage.sprite = soundSprite;
        }

        public void SetMusicSprite(Sprite musicSprite)
        {
            musicImage.sprite = musicSprite;
        }
    }
}
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utils;

namespace UI
{
    [System.Serializable]
    public class SettingsUI
    {
        [SerializeField] private Button cancelButton;
        [SerializeField] private Button musicButton;
        [SerializeField] private Button soundButton;
        [SerializeField] private Image musicImage;
        [SerializeField] private Image soundImage;
        [SerializeField] private GameObject panel;
        [SerializeField] private Transform background;
        [SerializeField] private Button settingsShowUpButton;


        public void AddCancelClickListener(UnityAction action) => cancelButton.onClick.AddListener(action);

        public void RemoveCancelClickListener(UnityAction action) => cancelButton.onClick.RemoveListener(action);

        public void AddMusicClickListener(UnityAction action) => musicButton.onClick.AddListener(action);

        public void RemoveMusicClickListener(UnityAction action) => musicButton.onClick.RemoveListener(action);

        public void AddSoundClickListener(UnityAction action) => soundButton.onClick.AddListener(action);

        public void RemoveSoundClickListener(UnityAction action) => soundButton.onClick.RemoveListener(action);


        public void AddSettingsClickListener(UnityAction action) => settingsShowUpButton.onClick.AddListener(action);

        public void RemoveSettingsClickListener(UnityAction action) =>
            settingsShowUpButton.onClick.RemoveListener(action);

        public void SetSoundSprite(Sprite soundSprite)
        {
            soundImage.sprite = soundSprite;
        }

        public void SetMusicSprite(Sprite musicSprite)
        {
            musicImage.sprite = musicSprite;
        }


        public void Show(Sprite musicSprite, Sprite soundSprite)
        {
            panel.SetActive(true);
            musicImage.sprite = musicSprite;
            soundImage.sprite = soundSprite;
            background.ShowPopUp();
        }


        public void Hide()
        {
            panel.SetActive(false);
            background.localScale = Vector3.zero;
        }
    }
}
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class MenuUI : MonoBehaviour
    {
        [SerializeField] private Image soundImage;
        [SerializeField] private Image musicImage;
        [SerializeField] private Image backgroundImage;

        [SerializeField] private GameObject loadingPanel;
        [SerializeField] private Transform loadingBlocksImage;

        [SerializeField] private TextMeshProUGUI levelText;

        [SerializeField] private Image fortuneWheelProgress;
        [SerializeField] private TextMeshProUGUI fortuneWheelProgressText;

        [SerializeField] private TextMeshProUGUI playText;


        public void SetPlayText(string text) => playText.text = text;


        public void SetBackground(Sprite backgroundSprite) => backgroundImage.sprite = backgroundSprite;


        public void SetSoundImage(Sprite soundSprite) => soundImage.sprite = soundSprite;


        public void SetMusicImage(Sprite musicSprite) => musicImage.sprite = musicSprite;


        public void ShowLoading() => loadingPanel.SetActive(true);


        public void HideLoading() => loadingPanel.SetActive(false);


        public void SetLevelText(string level) => levelText.text = level;


        public void StartLoadingAnimation() =>
            loadingBlocksImage.DOScale(new Vector3(1.3f, 1.3f, 1.3f), 0.5f).SetLoops(-1, LoopType.Yoyo);

        public void SetFortuneWheelProgress(float progress) => fortuneWheelProgress.fillAmount = progress;

        public void SetFortuneWheelProgressText(string text) => fortuneWheelProgressText.text = text;
    }
}
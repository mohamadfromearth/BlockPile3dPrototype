using System.Collections;
using Data;
using Managers;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;
using Zenject;

namespace Menu
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField] private MenuUI menuUI;


        [SerializeField] private SettingsRepository settingRepository;
        [SerializeField] private FortuneWheelRepository fortuneWheelRepository;

        [SerializeField] private Sprite landscapeBack;
        [SerializeField] private Sprite portraitBack;

        [Inject] private ILevelRepository _levelRepository;
        [SerializeField] private ColorRepository colorRepository;


        [SerializeField] private float loadingDuration = 0.7f;


        private void Start()
        {
            var musicSource = MusicAudioSource.GetInstance();
            if (settingRepository.IsMusicOn) musicSource.Play();
            menuUI.SetMusicImage(settingRepository.GetMusicSprite());
            menuUI.SetSoundImage(settingRepository.GetSoundSprite());
            var background = Screen.width > Screen.height ? landscapeBack : portraitBack;
            menuUI.SetBackground(background);
            menuUI.StartLoadingAnimation();
            menuUI.SetLevelText("Level " + (_levelRepository.LevelIndex + 1));
            StartCoroutine(HideLoadingRoutine());
            ColorMapper.SetColorStringToIndexDic(colorRepository.colorDataList);
        }


        public void OnPlayClick()
        {
            menuUI.ShowLoading();
            StartCoroutine(PlayClickRoutine());
        }


        private IEnumerator PlayClickRoutine()
        {
            yield return new WaitForSeconds(loadingDuration - 0.2f);
            SceneManager.LoadSceneAsync("Game");
        }


        private IEnumerator HideLoadingRoutine()
        {
            yield return new WaitForSeconds(loadingDuration);
            menuUI.HideLoading();
        }


        public void OnSoundClick()
        {
            settingRepository.ToggleSound();
            menuUI.SetSoundImage(settingRepository.GetSoundSprite());
        }


        public void OnMusicClick()
        {
            settingRepository.ToggleMusic();
            menuUI.SetMusicImage(settingRepository.GetMusicSprite());
        }


        public void FortuneWheelClick()
        {
            menuUI.SetFortuneWheelProgressText(
                $"{fortuneWheelRepository.GetProgressIndex()}/{fortuneWheelRepository.GetProgressTarget()}");
            menuUI.SetFortuneWheelProgress(fortuneWheelRepository.GetProgress());
        }
    }
}
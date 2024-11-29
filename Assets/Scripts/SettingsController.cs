using System;
using Data;
using Event;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

[Serializable]
public class SettingsController
{
    [SerializeField] private SettingsRepository settingsRepository;
    [SerializeField] private SettingsUI settingsUI;

    private EventChannel _channel;


    public void Init(EventChannel channel)
    {
        _channel = channel;
        settingsUI.AddMusicClickListener(OnMusicClick);
        settingsUI.AddSoundClickListener(OnSoundClick);
        settingsUI.AddRetryClickListener(OnRetry);
        settingsUI.AddBackToMenuClickListener(OnBackToMenu);
        settingsUI.AddExitClickListener(OnExit);
        settingsUI.AddCancelDialogClickListener(OnExitDialogueCancel);
    }


    private void OnMusicClick()
    {
        settingsRepository.ToggleMusic();
        settingsUI.SetMusicSprite(settingsRepository.GetMusicSprite());
    }

    private void OnSoundClick()
    {
        settingsRepository.ToggleSound();
        settingsUI.SetSoundSprite(settingsRepository.GetSoundSprite());
    }

    private void OnRetry()
    {
        settingsUI.HideExitDialog();
        _channel.Rise<Retry>(new Retry());
    }

    private void OnCancelExitDialogue() => settingsUI.HideExitDialog();


    private void OnExit() => settingsUI.ShowExitDialog();


    private void OnBackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }


    private void OnExitDialogueCancel() => settingsUI.HideExitDialog();
}
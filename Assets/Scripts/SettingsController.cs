using System;
using Data;
using UI;
using UnityEngine;

[Serializable]
public class SettingsController
{
    [SerializeField] private SettingsRepository settingsRepository;
    [SerializeField] private SettingsUI settingsUI;


    public void Init()
    {
        
        Debug.Log("Settings Ui initialized");
        settingsUI.AddCancelClickListener(OnSettingsCancelClick);
        settingsUI.AddMusicClickListener(OnMusicClick);
        settingsUI.AddSoundClickListener(OnSoundClick);
        settingsUI.AddSettingsClickListener(OnSettingsClick);
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

    private void OnSettingsCancelClick() => settingsUI.Hide();


    private void OnSettingsClick() =>
        settingsUI.Show(settingsRepository.GetMusicSprite(), settingsRepository.GetSoundSprite());
}
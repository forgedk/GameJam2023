using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;


public class SoundController : MonoBehaviour
{

    private static string soundEffectsKey = "SoundEfects";
    private static string soundsMusicKey = "SoundMusic";

    [SerializeField]
    private Slider soundSliderEffect;

    [SerializeField]
    private Slider soundSliderMusic;



    // Start is called before the first frame update
    void Start()
    {
        LoadSaveMenuStats();


    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void LoadSaveMenuStats()
    {
        float loadVolumeSoundEffect = PlayerPrefs.GetFloat(soundEffectsKey);
        float loadVolumeSoundMusic = PlayerPrefs.GetFloat(soundsMusicKey);
        SetVolumeBar(loadVolumeSoundEffect, loadVolumeSoundMusic);
    }

    public void setVolumePrefabSoundEffect(Slider soundSlider) { 
        float volume = soundSlider.value;

        if (soundSlider == soundSliderEffect) {
            PlayerPrefs.SetFloat(soundEffectsKey, volume);

        }
        else
        {
            PlayerPrefs.SetFloat(soundsMusicKey, volume);
        }   
    }

    public void SetVolumeBar(float soundEffectVolume, float soundMusicVolume) {
        soundSliderEffect.value = soundEffectVolume;
        soundSliderMusic.value = soundMusicVolume;
    }

}

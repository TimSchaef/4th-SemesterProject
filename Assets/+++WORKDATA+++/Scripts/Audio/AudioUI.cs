using UnityEngine;
using UnityEngine.UI;

public class AudioUI : MonoBehaviour
{
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [SerializeField] private AudioClip musicClip;

    void Start()
    {
        AudioManager.Instance.ChangeMusic(musicClip);
        
        float master = PlayerPrefs.GetFloat("Master", 1f);
        float music = PlayerPrefs.GetFloat("Music", 1f);
        float sfx = PlayerPrefs.GetFloat("SFX", 1f);

        masterSlider.value = master;
        musicSlider.value = music;
        sfxSlider.value = sfx;

        AudioManager.Instance.SetMasterVolume(master);
        AudioManager.Instance.SetMusicVolume(music);
        AudioManager.Instance.SetSFXVolume(sfx);
        
        masterSlider.onValueChanged.AddListener(AudioManager.Instance.SetMasterVolume);
        musicSlider.onValueChanged.AddListener(AudioManager.Instance.SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(AudioManager.Instance.SetSFXVolume);
    }
}

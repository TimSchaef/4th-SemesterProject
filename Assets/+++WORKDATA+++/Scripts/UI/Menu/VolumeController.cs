using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeController : MonoBehaviour
{
    [Header("Audio Configuration")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private string exposedParameterName = "MusicVol";
    
    private Slider volumeSlider;

    private void Awake()
    {
        volumeSlider = GetComponent<Slider>();
        
        // Setup slider limits to match logarithmic audio attenuation
        volumeSlider.minValue = 0.0001f; 
        volumeSlider.maxValue = 1f;
    }

    private void Start()
    {
        // Load saved volume or default to 0.75
        float savedVolume = PlayerPrefs.GetFloat(exposedParameterName, 0.75f);
        volumeSlider.value = savedVolume;
        
        SetVolume(savedVolume);

        // Add listener to react when the player drags the slider
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float sliderValue)
    {
        // Convert linear slider value (0 to 1) to decibels (-80dB to 20dB)
        float decibelValue = Mathf.Log10(sliderValue) * 20;
        
        audioMixer.SetFloat(exposedParameterName, decibelValue);
        
        // Save the setting instantly
        PlayerPrefs.SetFloat(exposedParameterName, sliderValue);
    }
}
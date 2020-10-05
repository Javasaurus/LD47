using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioMixer master;

    public Slider BGMSlider;
    public Slider SFXSlider;
    public Slider VoiceSlider;
    public Slider MasterSlider;

    void Start()
    {
        SetMasterVolume(PlayerPrefs.GetFloat("VOL_MASTER", .75f));
        SetSFXVolume(PlayerPrefs.GetFloat("VOL_SFX", .75f));
        SetVoiceVolume(PlayerPrefs.GetFloat("VOL_VOICE", .75f));
        SetBGMVolume(PlayerPrefs.GetFloat("VOL_BGM", .75f));
    }

    public void SetMasterVolume( float vol )
    {
        MasterSlider.value = vol;
        float val = Mathf.Log10(vol) * 20;
        master.SetFloat("VOL_MASTER", val);
        PlayerPrefs.SetFloat("VOL_MASTER", vol);
    }

    public void SetSFXVolume( float vol )
    {
        SFXSlider.value = vol;
        float val = Mathf.Log10(vol) * 20;
        master.SetFloat("VOL_SFX", val * 2);
        PlayerPrefs.SetFloat("VOL_SFX", vol);
    }

    public void SetVoiceVolume( float vol )
    {
        VoiceSlider.value = vol;
        float val = Mathf.Log10(vol) * 20;
        master.SetFloat("VOL_VOICE", val);
        PlayerPrefs.SetFloat("VOL_VOICE", vol);
    }

    public void SetBGMVolume( float vol )
    {
        BGMSlider.value = vol;
        float val = Mathf.Log10(vol) * 20;
        master.SetFloat("VOL_BGM", val);
        PlayerPrefs.SetFloat("VOL_BGM", vol);
    }



}

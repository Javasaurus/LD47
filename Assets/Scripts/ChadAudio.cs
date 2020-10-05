using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ChadAudio : MonoBehaviour
{
    public VoiceLine[] clips;

    public AudioClip BreakTime;
    public AudioClip FenceBreach;
    public AudioClip GameOver;

    public static ChadAudio instance;
    private AudioSource audioSource;
    // Start is called before the first frame update
    private float playIntervalMax = 30f;
    private float playIntervalMin = 20f;
    private float timer;
    private VoiceLine lastClip;
    void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }
    void LateUpdate()
    {
        if (Time.time > timer)
        {
            timer = Time.time + Random.Range(playIntervalMin, playIntervalMax);
            PlayVoiceClip();
        }
    }

    public void PlayVoiceClip()
    {
        VoiceLine line = clips[Random.Range(0, clips.Length)];
        while (line.audio == lastClip.audio || line.txt == lastClip.txt)
        {
            line = clips[Random.Range(0, clips.Length)];
        }
        audioSource.PlayOneShot(line.audio);
        ScoreManager.instance.ShowMessage(line.txt);
        lastClip = line;
    }

    public void PlayBreak()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(BreakTime);
        }
    }

    public void PlayFenceBreach()
    {
        audioSource.PlayOneShot(FenceBreach);
        ScoreManager.instance.ShowMessage("The Fence was Breached");
    }

    public void PlayGameOver()
    {
        audioSource.PlayOneShot(GameOver);
    }

    [System.Serializable]
    public struct VoiceLine
    {
        public AudioClip audio;
        public string txt;
    }

}

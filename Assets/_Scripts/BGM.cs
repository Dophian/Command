using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BGM : MonoBehaviour
{

    public float fromSeconds;
    public float toSeconds;
    private AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        
    }

    public void Play()
    {
        audioSource.time = fromSeconds;
        audioSource.Play();


        if (toSeconds > 0 && toSeconds > fromSeconds)
            audioSource.SetScheduledEndTime(AudioSettings.dspTime + (toSeconds - fromSeconds));
    }

    private void Update()
    {
        //if (audioSource.time >= 
    }
}

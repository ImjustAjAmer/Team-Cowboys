using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Sources")]
    public AudioSource backGroundMusic;
    public AudioSource SFX;

    [Header("Audio Clips")]
    public AudioClip BGMs;
    public AudioClip Death;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        backGroundMusic.clip = BGMs;
        backGroundMusic.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySFX(AudioClip clip)
    {
        SFX.PlayOneShot(clip);
    }

}

using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Sources")]

    public AudioSource deathSFX;
    //public AudioSource buttonSFX;

    [Header("Audio Clips")]

    public AudioClip Death;
    //public AudioClip buttonSumbitSFX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySFX(AudioClip clip)
    {
        deathSFX.PlayOneShot(clip);
        //buttonSFX.PlayOneShot(clip);
        
    }

}

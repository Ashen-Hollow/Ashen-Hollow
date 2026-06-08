using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Componentes")]
    public AudioSource musicSource;

    [Header("Trilhas Sonoras (Músicas)")]
    public AudioClip menuMusic;
    public AudioClip levelMusic;
    public AudioClip bossMusic;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
           
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        
        if (menuMusic != null)
        {
            PlayMusic(menuMusic);
        }
    }

    public void PlayMusic(AudioClip clip)
    {
       
        if (musicSource.clip == clip) return;

        musicSource.clip = clip;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
}
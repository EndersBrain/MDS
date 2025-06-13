using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour //Aici se afla codul pentru muzica de fundal a jocului
{
    public static MusicManager Instance;
    public AudioClip introClip;
    public AudioClip loopClip;

    private AudioSource audioSource;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = 0.1f;
        audioSource.playOnAwake = false;
    }
    void Start()
    {
        StartCoroutine(PlayMusicWithLoop());
    }
    private IEnumerator PlayMusicWithLoop()
    {
        if (introClip != null)
        {
            audioSource.clip = introClip;
            audioSource.loop = false;
            audioSource.Play();
            while (audioSource.isPlaying)
                yield return null;
        }

        if (loopClip != null)
        {
            audioSource.clip = loopClip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }
}

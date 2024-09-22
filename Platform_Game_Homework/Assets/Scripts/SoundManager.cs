using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip bgmClip;
    [SerializeField] private AudioClip coinClip;
    [SerializeField] private AudioClip monsterDieClip;
    [SerializeField] private AudioClip petGetClip;
    [SerializeField] private AudioClip petDieClip;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = bgmClip;
        audioSource.loop = true;
    }

    private void Start()
    {
        PlayBGM();
    }

    public void PlayBGM()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public void StopBGM()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    public void PlayCoinSound()
    {
        audioSource.PlayOneShot(coinClip);
    }

    public void PlayMonsterDieSound()
    {
        audioSource.PlayOneShot(monsterDieClip);
    }

    public void PlayPetGetSound()
    {
        audioSource.PlayOneShot(petGetClip);
    }

    public void PlayPetDieSound()
    {
        audioSource.PlayOneShot(petDieClip);
    }
}

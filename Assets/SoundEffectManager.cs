using UnityEngine;
using UnityEngine.UI;

public class SoundEffectManager : MonoBehaviour
{
    private static SoundEffectManager Instance;

    private static AudioSource audioSource;
    private static SoundEffectLibary soundEffectLibary;
    [SerializeField] private Slider sfxSlider;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
            soundEffectLibary = GetComponent<SoundEffectLibary>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void Play(string soundName)
    {
        AudioClip audioclip = soundEffectLibary.GetRandomClip(soundName);
        if(audioclip != null)
        {
            audioSource.PlayOneShot(audioclip);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sfxSlider.onValueChanged.AddListener(delegate { OnValueChange(); });
    }

    public static void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }

    public void OnValueChange()
    {
        SetVolume(sfxSlider.value);
    }
}

using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SoundManager>();
                
                if (instance == null)
                {
                    GameObject managerObject = new GameObject("SoundManager");
                    instance = managerObject.AddComponent<SoundManager>();
                    DontDestroyOnLoad(managerObject);
                }
            }
            return instance;
        }
    }
    
    [Header("사운드 클립")]
    [SerializeField] private AudioClip buttonClickSound; // 버튼 클릭 사운드
    
    [Header("오디오 소스")]
    [SerializeField] private AudioSource musicSource; // 배경음악용
    [SerializeField] private AudioSource sfxSource; // 효과음용
    
    [Header("볼륨 설정")]
    [Range(0f, 1f)]
    [SerializeField] private float masterVolume = 1f;
    [Range(0f, 1f)]
    [SerializeField] private float musicVolume = 1f;
    [Range(0f, 1f)]
    [SerializeField] private float sfxVolume = 1f;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // DontDestroyOnLoad는 루트 GameObject에서만 작동하므로 부모가 없도록 보장
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
            Debug.Log("SoundManager: 초기화 시작");
            InitializeAudioSources();
            Debug.Log($"SoundManager: 초기화 완료 - ButtonClickSound: {(buttonClickSound != null ? buttonClickSound.name : "None")}");
        }
        else if (instance != this)
        {
            Debug.Log("SoundManager: 중복 인스턴스 제거");
            Destroy(gameObject);
        }
    }
    
    /// <summary>
    /// 오디오 소스를 초기화합니다.
    /// </summary>
    private void InitializeAudioSources()
    {
        // 배경음악용 오디오 소스
        if (musicSource == null)
        {
            GameObject musicObject = new GameObject("MusicSource");
            musicObject.transform.SetParent(transform);
            musicSource = musicObject.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.playOnAwake = false;
        }
        
        // 효과음용 오디오 소스
        if (sfxSource == null)
        {
            GameObject sfxObject = new GameObject("SFXSource");
            sfxObject.transform.SetParent(transform);
            sfxSource = sfxObject.AddComponent<AudioSource>();
            sfxSource.loop = false;
            sfxSource.playOnAwake = false;
        }
        
        UpdateVolumes();
    }
    
    /// <summary>
    /// 버튼 클릭 사운드를 재생합니다.
    /// </summary>
    public void PlayButtonClickSound()
    {
        if (sfxSource == null)
        {
            Debug.LogWarning("SoundManager: SFXSource가 초기화되지 않았습니다!");
            InitializeAudioSources();
            return;
        }
        
        if (buttonClickSound == null)
        {
            Debug.LogWarning("SoundManager: ButtonClickSound가 할당되지 않았습니다! 인스펙터에서 AudioClip을 할당해주세요.");
            return;
        }
        
        sfxSource.PlayOneShot(buttonClickSound, sfxVolume * masterVolume);
        Debug.Log($"SoundManager: 버튼 클릭 사운드 재생 - 볼륨: {sfxVolume * masterVolume}");
    }
    
    /// <summary>
    /// 지정된 사운드 클립을 재생합니다.
    /// </summary>
    /// <param name="clip">재생할 사운드 클립</param>
    /// <param name="volume">볼륨 (0~1)</param>
    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip, volume * sfxVolume * masterVolume);
        }
    }
    
    /// <summary>
    /// 배경음악을 재생합니다.
    /// </summary>
    /// <param name="clip">재생할 배경음악 클립</param>
    /// <param name="loop">반복 재생 여부</param>
    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (musicSource != null && clip != null)
        {
            musicSource.clip = clip;
            musicSource.loop = loop;
            musicSource.volume = musicVolume * masterVolume;
            musicSource.Play();
        }
    }
    
    /// <summary>
    /// 배경음악을 중지합니다.
    /// </summary>
    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }
    
    /// <summary>
    /// 마스터 볼륨을 설정합니다.
    /// </summary>
    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        UpdateVolumes();
    }
    
    /// <summary>
    /// 배경음악 볼륨을 설정합니다.
    /// </summary>
    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        UpdateVolumes();
    }
    
    /// <summary>
    /// 효과음 볼륨을 설정합니다.
    /// </summary>
    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
    }
    
    /// <summary>
    /// 모든 볼륨을 업데이트합니다.
    /// </summary>
    private void UpdateVolumes()
    {
        if (musicSource != null)
        {
            musicSource.volume = musicVolume * masterVolume;
        }
    }
    
    /// <summary>
    /// 버튼 클릭 사운드를 설정합니다.
    /// </summary>
    public void SetButtonClickSound(AudioClip clip)
    {
        buttonClickSound = clip;
    }
    
    /// <summary>
    /// 마스터 볼륨을 반환합니다.
    /// </summary>
    public float GetMasterVolume()
    {
        return masterVolume;
    }
    
    /// <summary>
    /// 배경음악 볼륨을 반환합니다.
    /// </summary>
    public float GetMusicVolume()
    {
        return musicVolume;
    }
    
    /// <summary>
    /// 효과음 볼륨을 반환합니다.
    /// </summary>
    public float GetSFXVolume()
    {
        return sfxVolume;
    }
}


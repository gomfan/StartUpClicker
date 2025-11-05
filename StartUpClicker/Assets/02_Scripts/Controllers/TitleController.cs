using UnityEngine;
using UnityEngine.UI;

public class TitleController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;
    
    [Header("Start Settings")]
    [SerializeField] private string gameSceneName = "Game";
    [SerializeField] private bool pressAnyKeyToStart = true;
    
    private GameSceneManager sceneManager;
    
    private void Start()
    {
        // GameSceneManager 인스턴스 가져오기
        sceneManager = GameSceneManager.Instance;
        
        // 버튼 이벤트 등록
        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
        }
        
        if (quitButton != null)
        {
            quitButton.onClick.AddListener(OnQuitButtonClicked);
        }
    }
    
    private void OnDestroy()
    {
        // 메모리 누수 방지를 위해 이벤트 해제
        if (startButton != null)
        {
            startButton.onClick.RemoveListener(OnStartButtonClicked);
        }
        
        if (quitButton != null)
        {
            quitButton.onClick.RemoveListener(OnQuitButtonClicked);
        }
    }
    
    private void Update()
    {
        if (!pressAnyKeyToStart)
        {
            return;
        }
        
        if (Input.anyKeyDown)
        {
            OnStartButtonClicked();
        }
    }
    
    /// <summary>
    /// 시작 버튼 클릭 시 호출됩니다.
    /// </summary>
    private void OnStartButtonClicked()
    {
        Debug.Log("Start Button Clicked!");
        
        // 버튼 클릭 사운드 재생
        PlayButtonClickSound();
        
        sceneManager.LoadScene(gameSceneName);
    }
    
    /// <summary>
    /// 종료 버튼 클릭 시 호출됩니다.
    /// </summary>
    private void OnQuitButtonClicked()
    {
        Debug.Log("Quit Button Clicked!");
        
        // 버튼 클릭 사운드 재생
        PlayButtonClickSound();
        
        sceneManager.QuitGame();
    }
    
    /// <summary>
    /// 버튼 클릭 사운드를 재생합니다.
    /// </summary>
    private void PlayButtonClickSound()
    {
        SoundManager soundManager = SoundManager.Instance;
        if (soundManager != null)
        {
            soundManager.PlayButtonClickSound();
        }
    }
}


using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 버튼에 자동으로 클릭 사운드를 추가하는 헬퍼 컴포넌트입니다.
/// 이 컴포넌트를 버튼에 추가하면 클릭 시 자동으로 사운드가 재생됩니다.
/// </summary>
[RequireComponent(typeof(Button))]
public class ButtonSoundHelper : MonoBehaviour
{
    [Header("사운드 설정")]
    [SerializeField] private bool useDefaultClickSound = true; // 기본 클릭 사운드 사용 여부
    [SerializeField] private AudioClip customClickSound; // 커스텀 클릭 사운드 (없으면 기본 사운드 사용)
    
    private Button button;
    private SoundManager soundManager;
    
    private void Awake()
    {
        button = GetComponent<Button>();
        
        if (button == null)
        {
            Debug.LogError($"ButtonSoundHelper ({gameObject.name}): Button 컴포넌트를 찾을 수 없습니다!");
        }
        else
        {
            Debug.Log($"ButtonSoundHelper ({gameObject.name}): 초기화 완료 - Button 컴포넌트 발견");
        }
    }
    
    private void Start()
    {
        // Start에서 SoundManager 인스턴스 가져오기 (초기화 순서 보장)
        if (soundManager == null)
        {
            soundManager = SoundManager.Instance;
        }
        
        if (soundManager == null)
        {
            Debug.LogWarning($"ButtonSoundHelper ({gameObject.name}): SoundManager를 찾을 수 없습니다! 씬에 SoundManager가 있는지 확인해주세요.");
        }
        else
        {
            Debug.Log($"ButtonSoundHelper ({gameObject.name}): SoundManager 연결 완료");
        }
        
        // 버튼 리스너 추가 (Start에서 한 번 더 확실하게)
        // 코루틴을 사용하여 다른 스크립트가 먼저 등록한 후에 등록
        StartCoroutine(RegisterButtonListenerDelayed());
    }
    
    private System.Collections.IEnumerator RegisterButtonListenerDelayed()
    {
        // 한 프레임 대기하여 다른 스크립트의 Start가 모두 실행되도록 함
        yield return null;
        
        if (button != null)
        {
            // 기존 리스너가 있는지 확인
            int listenerCount = button.onClick.GetPersistentEventCount();
            Debug.Log($"ButtonSoundHelper ({gameObject.name}): 버튼에 등록된 리스너 수: {listenerCount}");
            
            button.onClick.RemoveListener(OnButtonClicked); // 중복 방지
            button.onClick.AddListener(OnButtonClicked);
            
            // 리스너가 제대로 등록되었는지 확인
            int newListenerCount = button.onClick.GetPersistentEventCount();
            Debug.Log($"ButtonSoundHelper ({gameObject.name}): 버튼 클릭 리스너 등록 완료 (리스너 수: {newListenerCount})");
            
            // 버튼이 활성화되어 있는지 확인
            if (!button.interactable)
            {
                Debug.LogWarning($"ButtonSoundHelper ({gameObject.name}): 버튼이 비활성화되어 있습니다!");
            }
        }
    }
    
    private void OnEnable()
    {
        if (button != null)
        {
            button.onClick.RemoveListener(OnButtonClicked); // 중복 방지
            button.onClick.AddListener(OnButtonClicked);
            Debug.Log($"ButtonSoundHelper ({gameObject.name}): OnEnable - 리스너 등록");
        }
    }
    
    private void OnDisable()
    {
        if (button != null)
        {
            button.onClick.RemoveListener(OnButtonClicked);
        }
    }
    
    /// <summary>
    /// 버튼이 클릭되었을 때 호출됩니다.
    /// </summary>
    private void OnButtonClicked()
    {
        Debug.Log($"ButtonSoundHelper ({gameObject.name}): 버튼 클릭 감지됨!");
        
        // SoundManager 인스턴스 다시 가져오기 (씬 전환 후에도 작동하도록)
        if (soundManager == null)
        {
            soundManager = SoundManager.Instance;
        }
        
        if (soundManager == null)
        {
            Debug.LogWarning($"ButtonSoundHelper ({gameObject.name}): SoundManager를 찾을 수 없습니다!");
            return;
        }
        
        if (useDefaultClickSound && customClickSound == null)
        {
            // 기본 클릭 사운드 재생
            Debug.Log($"ButtonSoundHelper ({gameObject.name}): 기본 클릭 사운드 재생 시도");
            soundManager.PlayButtonClickSound();
        }
        else if (customClickSound != null)
        {
            // 커스텀 클릭 사운드 재생
            Debug.Log($"ButtonSoundHelper ({gameObject.name}): 커스텀 클릭 사운드 재생");
            soundManager.PlaySFX(customClickSound);
        }
        else
        {
            Debug.LogWarning($"ButtonSoundHelper ({gameObject.name}): 재생할 사운드가 설정되지 않았습니다! Use Default Click Sound: {useDefaultClickSound}, Custom Click Sound: {customClickSound != null}");
        }
    }
}


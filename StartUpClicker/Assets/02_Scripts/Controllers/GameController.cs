using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    [Header("클릭 가능한 스프라이트")]
    [SerializeField] private SpriteRenderer clickableSprite;
    
    [Header("점수 표시 UI")]
    [SerializeField] private TextMeshProUGUI scoreText;
    
    [Header("설정")]
    [SerializeField] private int clickValue = 1; // 클릭당 증가 값
    
    private int currentScore = 0; // 현재 점수
    
    private Camera mainCamera;
    
    private void Start()
    {
        mainCamera = Camera.main;
        
        // 점수 텍스트 초기화
        if (scoreText != null)
        {
            UpdateScoreText();
        }
        else
        {
            Debug.LogWarning("ScoreText가 할당되지 않았습니다!");
        }
        
        // 스프라이트가 없으면 경고
        if (clickableSprite == null)
        {
            Debug.LogWarning("ClickableSprite가 할당되지 않았습니다!");
        }
    }
    
    private void Update()
    {
        // 마우스 클릭 감지
        if (Input.GetMouseButtonDown(0))
        {
            CheckSpriteClick();
        }
        
        // 모바일 터치 감지
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            CheckSpriteClick();
        }
    }
    
    /// <summary>
    /// 스프라이트 클릭 여부를 확인하고 처리합니다.
    /// </summary>
    private void CheckSpriteClick()
    {
        if (clickableSprite == null || mainCamera == null)
            return;
        
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        
        // 터치 입력 처리
        if (Input.touchCount > 0)
        {
            mousePosition = mainCamera.ScreenToWorldPoint(Input.GetTouch(0).position);
        }
        
        // 스프라이트의 Collider2D를 사용하여 클릭 감지
        Collider2D hitCollider = Physics2D.OverlapPoint(mousePosition);
        
        if (hitCollider != null && hitCollider.gameObject == clickableSprite.gameObject)
        {
            OnSpriteClicked();
        }
        // Collider2D가 없을 경우, Bounds를 사용하여 클릭 감지
        else if (hitCollider == null)
        {
            Bounds spriteBounds = clickableSprite.bounds;
            if (spriteBounds.Contains(new Vector3(mousePosition.x, mousePosition.y, 0)))
            {
                OnSpriteClicked();
            }
        }
    }
    
    /// <summary>
    /// 스프라이트가 클릭되었을 때 호출됩니다.
    /// </summary>
    private void OnSpriteClicked()
    {
        // 점수 증가
        currentScore += clickValue;
        
        // 점수 텍스트 업데이트
        UpdateScoreText();
        
        Debug.Log($"클릭! 현재 점수: {currentScore}");
    }
    
    /// <summary>
    /// 점수 텍스트를 업데이트합니다.
    /// </summary>
    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = currentScore.ToString();
        }
    }
    
    /// <summary>
    /// 현재 점수를 반환합니다.
    /// </summary>
    public int GetCurrentScore()
    {
        return currentScore;
    }
    
    /// <summary>
    /// 점수를 설정합니다.
    /// </summary>
    public void SetScore(int score)
    {
        currentScore = score;
        UpdateScoreText();
    }
}


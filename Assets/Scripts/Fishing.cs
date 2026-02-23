using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Vector3 = UnityEngine.Vector3;

public class Fishing : MonoBehaviour
{
    #region Misc Serialized Fields
    [Header("Misc Serialized Fields")]
    [SerializeField] private SpriteRenderer _gameBoard;
    [SerializeField] private GameplayHeart _gameplayHeart;
    /// <summary>How long the delay is before starting a new heart-capture round after winning the previous heart</summary>
    [SerializeField] private float _wonHeartDelay; 
    /// <summary>How long the delay is before starting a new heart-capture round after losing the previous heart</summary>
    [SerializeField] private float _lostHeartDelay;
    #endregion
    
    #region Hook and Affection Bar Serialized Fields
    [FormerlySerializedAs("_affectionBarInput")]
    [Header("Affection Bar Serialized Fields")]
    [SerializeField] private InputActionReference _hookInput;
    [SerializeField] private SpriteRenderer _hook;
    [SerializeField] private SpriteRenderer _affectionBar; // child to hook, so it moves with it
    [SerializeField] private float _hookMoveSpeed;
    [SerializeField] private float _hookFallSpeed;
    #endregion 
    
    #region Progress Bar Serialized Fields
    [Header("Progress Bar Serialized Fields")]
    [SerializeField] private SpriteMask _progressBar;
    [SerializeField] private float progressFillSpeed;
    [SerializeField] private float progressDepleteSpeed;
    /// <summary>How much initial progress you have upon starting a new heart-capture round after winning the previous heart</summary>
    [SerializeField] private float _wonHeartInitialProgress;
    /// <summary>How much initial progress you have upon starting a new heart-capture round after losing the previous heart</summary>
    [SerializeField] private float _lostHeartInitialProgress;
    #endregion 
    
    #region Script Variables
    private bool _hookIsMoving;
    private float _topHookBounds;
    private float _bottomHookBounds;
    private float _progress = 20;
    private bool _betweenHearts;
    private const float ProgressBarMaxScale = 15f; // How can I have this not hard-coded?
    #endregion
    
    private void OnEnable()
    {
        _hookInput.action.Enable();
        _hookInput.action.started += StartHookInput;
        _hookInput.action.canceled += CancelHookInput;
    }

    private void OnDisable()
    {
        _hookInput.action.Disable();
        _hookInput.action.started -= StartHookInput;
        _hookInput.action.canceled -= CancelHookInput;
    }

    private void Start()
    {
        Bounds boardBounds = _gameBoard.bounds;
        Bounds affectionBarBounds = _affectionBar.bounds;
        _topHookBounds = boardBounds.max.y - affectionBarBounds.extents.y;
        _bottomHookBounds = boardBounds.min.y + affectionBarBounds.extents.y;
        
        // Debug.DrawLine(new Vector3(5f, _bottomHookBounds, 10f), new Vector3(9f, _bottomHookBounds, 10f), Color.purple, 10f);
        // Debug.DrawLine(new Vector3(5f, _topHookBounds, 10f), new Vector3(9f, _topHookBounds, 10f), Color.purple, 10f);
    }
    
    private void Update()
    {
        if (_betweenHearts) return; // game effectively pauses during delay between hearts
        MoveAffectionBar();
        UpdateProgress();
        UpdateProgressBar();
    }

    private void UpdateProgressBar()
    {
        Vector3 currentScale = _progressBar.transform.localScale;
        currentScale.y = ProgressBarMaxScale * (_progress / 100);
        _progressBar.transform.localScale = currentScale;
    }

    private void UpdateProgress()
    {
        if (_affectionBar.bounds.Intersects(_gameplayHeart.Bounds))
        {
            _progress += progressFillSpeed * Time.deltaTime;
        }
        else
        {
            _progress -= progressDepleteSpeed * Time.deltaTime;
        }
        SetProgress(_progress);

        if (Mathf.Approximately(_progress, 100))
        {
            HeartRoundOver(true);
        } else if (Mathf.Approximately(_progress, 0))
        {
            HeartRoundOver(false);
        }
    }

    private void HeartRoundOver(bool wonHeart)
    {
        if (wonHeart) GameUIManager.Instance.IncrementHeartCounter();
        _gameplayHeart.HideHeart();
        _betweenHearts = true;
        Helpers.Instance.Delay(wonHeart ? _wonHeartDelay : _lostHeartDelay, () =>
        {
            _betweenHearts = false;
            _gameplayHeart.ShowHeart();
            SetProgress(wonHeart ?  _wonHeartInitialProgress : _lostHeartInitialProgress);
        });
    }

    private void SetProgress(float progress)
    {
        _progress = Mathf.Clamp(progress, 0, 100);
    }

    private void MoveAffectionBar()
    {
        if (_hookIsMoving && _affectionBar.transform.position.y < _topHookBounds)
        {
            _hook.transform.position 
                = new Vector3(_hook.transform.position.x, _hook.transform.position.y + Time.deltaTime * _hookMoveSpeed, _hook.transform.position.z);
        }
        else if (_affectionBar.transform.position.y > _bottomHookBounds)
        {
            _hook.transform.position 
                = new Vector3(_hook.transform.position.x, _hook.transform.position.y - Time.deltaTime * _hookFallSpeed, _hook.transform.position.z);
        }
    }
    
    private void StartHookInput(InputAction.CallbackContext ctx)
    {
        _hookIsMoving = true;
    }
    
    private void CancelHookInput(InputAction.CallbackContext ctx)
    {
        _hookIsMoving = false;
    }
}

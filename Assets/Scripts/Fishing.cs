using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class Fishing : MonoBehaviour
{
    #region Misc Serialized Fields
    [Header("Misc Serialized Fields")]
    [SerializeField] private SpriteRenderer _gameBoard;
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
    
    #region Heart Serialized Fields
    [Header("Heart Serialized Fields")]
    [SerializeField] private SpriteRenderer _heart;
    [SerializeField] private float _heartMoveSpeed;
    [SerializeField] private float _chooseHeartDestinationDelay;
    #endregion
    
    #region Progress Bar Serialized Fields
    [Header("Progress Bar Serialized Fields")]
    [SerializeField] private SpriteMask _progressBar;
    /// <summary>How fast the progress bar fills or depletes</summary>
    [SerializeField] private float progressSpeed;
    /// <summary>How much initial progress you have upon starting a new heart-capture round after winning the previous heart</summary>
    [SerializeField] private float _wonHeartInitialProgress;
    /// <summary>How much initial progress you have upon starting a new heart-capture round after losing the previous heart</summary>
    [SerializeField] private float _lostHeartInitialProgress;
    #endregion 
    
    #region Script Variables
    private bool _hookIsMoving;
    private float _topHookBounds;
    private float _bottomHookBounds;
    private Vector3 _topHeartPosition;
    private Vector3 _bottomHeartPosition;
    private Vector3 _heartDestination;
    private float _progress = 20;
    private bool _betweenHearts;
    private const float ProgressBarMaxScale = 15f;
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
        Debug.DrawLine(new Vector3(5f, _bottomHookBounds, 10f), new Vector3(9f, _bottomHookBounds, 10f), Color.purple, 10f);
        Debug.DrawLine(new Vector3(5f, _topHookBounds, 10f), new Vector3(9f, _topHookBounds, 10f), Color.purple, 10f);
        
        Vector3 boardTopCenter = boardBounds.max - Vector3.right * boardBounds.extents.x;
        Vector3 boardBottomCenter = boardBounds.min + Vector3.right * boardBounds.extents.x;
        Bounds heartBounds = _heart.bounds;
        _topHeartPosition = boardTopCenter - Vector3.up * heartBounds.extents.y;
        _bottomHeartPosition = boardBottomCenter + Vector3.up * heartBounds.extents.y;
        
        InvokeRepeating(nameof(ChooseHeartDestination), 0, _chooseHeartDestinationDelay);
    }

    private void Update()
    {
        if (_betweenHearts) return; // game effectively pauses during delay between hearts
        MoveAffectionBar();
        MoveHeart();
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
        if (_affectionBar.bounds.Intersects(_heart.bounds))
        {
            _progress += progressSpeed * Time.deltaTime;
        }
        else
        {
            _progress -= progressSpeed * Time.deltaTime;
        }
        _progress = Mathf.Clamp(_progress, 0, 100);

        if (Mathf.Approximately(_progress, 100))
        {
            HeartWon();
        } else if (Mathf.Approximately(_progress, 0))
        {
            HeartLost();
        }
    }

    private void HeartWon()
    {
        GameUIManager.Instance.IncrementHeartCounter();
        _heart.sortingOrder = -10;
        _betweenHearts = true;
        Helpers.Instance.Delay(_wonHeartDelay, () =>
        {
            _betweenHearts = false;
            _heart.sortingOrder = 3;
            SetProgress(_wonHeartInitialProgress);
        });
    }

    private void HeartLost()
    {
        _heart.sortingOrder = -10;
        _betweenHearts = true;
        Helpers.Instance.Delay(_lostHeartDelay, () =>
        {
            _betweenHearts = false;
            _heart.sortingOrder = 3;
            SetProgress(_lostHeartInitialProgress);
        });
    }

    private void SetProgress(float progress)
    {
        _progress = Mathf.Clamp(progress, 0, 100);
    }

    private void ChooseHeartDestination()
    {
        _heartDestination = Vector3.Lerp(_bottomHeartPosition, _topHeartPosition, Random.Range(0f, 1f));
    }

    private void MoveHeart()
    {
        _heart.transform.position = Vector3.MoveTowards(_heart.transform.position, _heartDestination, _heartMoveSpeed * Time.deltaTime);
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

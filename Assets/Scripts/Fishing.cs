using UnityEngine;
using UnityEngine.InputSystem;
using Vector3 = UnityEngine.Vector3;

public class Fishing : MonoBehaviour
{
    #region References
    [SerializeField] private InputActionReference _affectionBarInput;
    [SerializeField] private SpriteRenderer _affectionBar;
    [SerializeField] private SpriteRenderer _gameBoard;
    [SerializeField] private SpriteMask _progressBar;
    [SerializeField] private SpriteRenderer _heart;
    [SerializeField] private float _heartMoveSpeed;
    [SerializeField] private float _affectionBarMoveSpeed;
    [SerializeField] private float _affectionBarFallSpeed;
    [SerializeField] private float progressSpeed; // how fast the progress bar fills or depletes
    [SerializeField] private float _chooseHeartDestinationDelay;
    #endregion
    
    #region Script Variables
    private bool _affectionBarIsMoving;
    private float _topBoardBounds;
    private float _bottomBoardBounds;
    private Vector3 _topHeartPosition;
    private Vector3 _bottomHeartPosition;
    private Vector3 _heartDestination;
    private float _progress = 20;
    #endregion

    private void OnEnable()
    {
        _affectionBarInput.action.Enable();
        _affectionBarInput.action.started += StartAffectionBarInput;
        _affectionBarInput.action.canceled += CancelAffectionBarInput;
    }

    private void OnDisable()
    {
        _affectionBarInput.action.Disable();
    }

    private void Start()
    {
        Bounds boardBounds = _gameBoard.bounds;
        Bounds affectionBarBounds = _affectionBar.bounds;
        _topBoardBounds = boardBounds.max.y - affectionBarBounds.extents.y;
        _bottomBoardBounds = boardBounds.min.y + affectionBarBounds.extents.y;
        
        Vector3 boardTopCenter = boardBounds.max - Vector3.right * boardBounds.extents.x;
        Vector3 boardBottomCenter = boardBounds.min + Vector3.right * boardBounds.extents.x;
        Bounds heartBounds = _heart.bounds;
        _topHeartPosition = boardTopCenter - Vector3.up * heartBounds.extents.y;
        _bottomHeartPosition = boardBottomCenter + Vector3.up * heartBounds.extents.y;
        
        InvokeRepeating(nameof(ChooseHeartDestination), 0, _chooseHeartDestinationDelay);
    }

    private void Update()
    {
        MoveAffectionBar();
        MoveHeart();
        UpdateProgress();
        UpdateProgressBar();
    }

    private void UpdateProgressBar()
    {
        Vector3 currentScale = _progressBar.transform.localScale;
        currentScale.y = 9.5f * (_progress / 100); // 8 is hard-coded and should be made into a variable or constant
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
        if (_affectionBarIsMoving && _affectionBar.transform.position.y < _topBoardBounds)
        {
            _affectionBar.transform.position 
                = new Vector3(_affectionBar.transform.position.x, _affectionBar.transform.position.y + Time.deltaTime * _affectionBarMoveSpeed, _affectionBar.transform.position.z);
        }
        else if (_affectionBar.transform.position.y > _bottomBoardBounds)
        {
            _affectionBar.transform.position 
                = new Vector3(_affectionBar.transform.position.x, _affectionBar.transform.position.y - Time.deltaTime * _affectionBarFallSpeed, _affectionBar.transform.position.z);
        }
    }
    
    private void StartAffectionBarInput(InputAction.CallbackContext ctx)
    {
        _affectionBarIsMoving = true;
    }
    
    private void CancelAffectionBarInput(InputAction.CallbackContext ctx)
    {
        _affectionBarIsMoving = false;
    }
}

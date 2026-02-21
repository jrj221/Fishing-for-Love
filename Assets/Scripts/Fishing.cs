using UnityEngine;
using UnityEngine.InputSystem;

public class Fishing : MonoBehaviour
{
    [SerializeField] private InputActionReference _affectionBarInput;
    [SerializeField] private SpriteRenderer _affectionBar;
    [SerializeField] private SpriteRenderer _gameBoard;
    [SerializeField] private SpriteMask _progressBar;
    [SerializeField] private SpriteRenderer _heart;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float fallSpeed;
    [SerializeField] private float progressSpeed; // how fast the progress bar fills or depletes

    private bool _affectionBarIsMoving;
    private float _topBoardBounds;
    private float _bottomBoardBounds;
    private float _progress = 20;

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
    }

    private void Update()
    {
        MoveAffectionBar();
        UpdateProgress();
        UpdateProgressBar();
    }

    private void UpdateProgressBar()
    {
        Vector3 currentScale = _progressBar.transform.localScale;
        currentScale.y = 8 * (_progress / 100); // 8 is hard-coded and should be made into a variable or constant
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

    private void MoveAffectionBar()
    {
        if (_affectionBarIsMoving && _affectionBar.transform.position.y < _topBoardBounds)
        {
            _affectionBar.transform.position 
                = new Vector3(_affectionBar.transform.position.x, _affectionBar.transform.position.y + Time.deltaTime * moveSpeed, _affectionBar.transform.position.z);
        }
        else if (_affectionBar.transform.position.y > _bottomBoardBounds)
        {
            _affectionBar.transform.position 
                = new Vector3(_affectionBar.transform.position.x, _affectionBar.transform.position.y - Time.deltaTime * fallSpeed, _affectionBar.transform.position.z);
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

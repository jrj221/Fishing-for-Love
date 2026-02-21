using UnityEngine;
using UnityEngine.InputSystem;

public class Fishing : MonoBehaviour
{
    [SerializeField] private InputActionReference _affectionBarInput;
    [SerializeField] private SpriteRenderer _affectionBar;
    [SerializeField] private SpriteRenderer _gameBoard;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float fallSpeed;

    private bool _affectionBarIsMoving;
    private float _topBoardBounds;
    private float _bottomBoardBounds;

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

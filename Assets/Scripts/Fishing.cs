using UnityEngine;
using UnityEngine.InputSystem;

public class Fishing : MonoBehaviour
{
    [SerializeField] private InputActionReference _affectionBarInput;
    [SerializeField] private GameObject _affectionBar;
    private bool _affectionBarIsMoving;

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

    private void Update()
    {
        if (_affectionBarIsMoving)
        {
            Debug.Log("Affection bar is moving");
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

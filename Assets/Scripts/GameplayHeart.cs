using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class GameplayHeart : MonoBehaviour
{
    [SerializeField] private float _heartMoveSpeed;
    [SerializeField] private float _chooseHeartDestinationDelay;
    [SerializeField] private SpriteRenderer _gameBoard;
    private SpriteRenderer _heart;
    private Vector3 _topHeartPosition;
    private Vector3 _bottomHeartPosition;
    private Vector3 _heartDestination;
    
    private void Awake()
    {  
        _heart = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        Bounds gameBoardBounds = _gameBoard.bounds;
        Vector3 boardTopCenter = gameBoardBounds.max - Vector3.right * gameBoardBounds.extents.x;
        Vector3 boardBottomCenter = gameBoardBounds.min + Vector3.right * gameBoardBounds.extents.x;
        
        Bounds heartBounds = _heart.bounds;
        _topHeartPosition = boardTopCenter - Vector3.up * heartBounds.extents.y;
        _bottomHeartPosition = boardBottomCenter + Vector3.up * heartBounds.extents.y;
        
        InvokeRepeating(nameof(ChooseHeartDestination), 0, _chooseHeartDestinationDelay);
    }

    private void Update()
    {
        MoveHeart();
    }
    
    public Bounds Bounds => _heart.bounds;

    private void MakeInvisible()
    {
        Color color = _heart.color;
        color.a = 0;
        _heart.color = color;
    }

    private void MakeVisible()
    {
        Color color = _heart.color;
        color.a = 1;
        _heart.color = color;
    }
    
    private void ChooseHeartDestination()
    {
        _heartDestination = Vector3.Lerp(_bottomHeartPosition, _topHeartPosition, Random.Range(0f, 1f));
    }

    private void MoveHeart()
    {
        _heart.transform.position = Vector3.MoveTowards(_heart.transform.position, _heartDestination, _heartMoveSpeed * Time.deltaTime);
    }

    public void HideHeart()
    {
        MakeInvisible();
    }

    public void ShowHeart()
    {
        MakeVisible();
    }
}

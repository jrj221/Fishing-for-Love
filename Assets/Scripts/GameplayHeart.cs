using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class GameplayHeart : MonoBehaviour
{
    public float HeartMoveSpeed { private get; set; }
    public float ChooseHeartDestinationDelay { private get; set; }
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
        
        InvokeRepeating(nameof(ChooseHeartDestination), 0, ChooseHeartDestinationDelay);
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
        Debug.Log("Choosing Heart Destination");
        _heartDestination = Vector3.Lerp(_bottomHeartPosition, _topHeartPosition, Random.Range(0f, 1f));
    }

    private void MoveHeart()
    {
        Debug.Log("Moving Heart?");
        _heart.transform.position = Vector3.MoveTowards(_heart.transform.position, _heartDestination, HeartMoveSpeed * Time.deltaTime);
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

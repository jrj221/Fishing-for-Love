using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BackgroundHeart : MonoBehaviour
{
    [SerializeField] private float animationTime;
    /// <summary>Heart has a 1 in chanceToFade chance to fade in or out every frame it isn't currently fading</summary>
    [SerializeField] private int chanceToFade;
    private SpriteRenderer _heart;
    private bool _inAnimation;
    private bool _isVisible;

    private void Awake()
    {
        _heart = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        InitializeHeartVisibility();
    }

    private void InitializeHeartVisibility()
    {
        _isVisible = Random.Range(0, 3) == 0; // about a quarter of them should spawn visible
        Color color = _heart.color;
        color.a = _isVisible ? 1 : 0;
        _heart.color = color;
    }

    private void Update()
    {
        if (_inAnimation) return;
        if (Random.Range(0, chanceToFade) != 0) return; // small chance to continue and fade in or out
        _inAnimation = true;
        StartCoroutine(_isVisible ? FadeOut(animationTime) : FadeIn(animationTime));
    }
    
    private IEnumerator FadeOut(float time)
    {
        for (float i = 100; i > -1; i--)
        {
            Color color = _heart.color;
            color.a = i / 100f;
            _heart.color = color;
            yield return new WaitForSeconds(time / 101);
        }
        _isVisible = false;
        _inAnimation = false;
    }
    
    private IEnumerator FadeIn(float time)
    {
        for (float i = 0; i < 101; i++)
        {
            Color color = _heart.color;
            color.a = i / 100f;
            _heart.color = color;
            yield return new WaitForSeconds(time / 101);
        }
        _isVisible = true;
        _inAnimation = false;
    }
}

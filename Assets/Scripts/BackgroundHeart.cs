using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BackgroundHeart : MonoBehaviour
{
    [SerializeField] private float animationTime;
    private SpriteRenderer _heart;
    private bool _inAnimation;
    private bool _visible;

    private void Awake()
    {
        _heart = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (_inAnimation) return;
        if (Random.Range(0, 300) != 0) return; 
        Debug.Log("Lucky");
        // good chance it happens about every 5 seconds. FPS dependent
        _inAnimation = true;
        StartCoroutine(_visible ? FadeOut(animationTime) : FadeIn(animationTime));
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
        _visible = false;
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
        _visible = true;
        _inAnimation = false;
    }
}

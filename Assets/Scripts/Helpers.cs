using System;
using System.Collections;
using UnityEngine;
// I know this isn't a great way to hold general functions, but I'm on a time crunch

public class Helpers : MonoBehaviour
{
    public static Helpers Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    
    public void Delay(float delay, Action action)
    {
        StartCoroutine(DelayRoutine(delay, action));
    }

    private IEnumerator DelayRoutine(float time, Action callback)
    {
        yield return new WaitForSeconds(time);
        callback();
    }
}

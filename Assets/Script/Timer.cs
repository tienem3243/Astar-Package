using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    int _IDCounter = 0;
    Dictionary<int,Coroutine> lookups= new Dictionary<int, Coroutine>() ;
    public int StartTimer(float time, Action onCompleted)
    {
        return StartTimer(time, null, onCompleted);
    }

    public virtual int StartTimer(float time, Action<float> onUpdate, Action onCompleted)
    {
        _IDCounter++;
        lookups[_IDCounter] = StartCoroutine(StartTimer(_IDCounter, time, onUpdate, onCompleted));

        return _IDCounter;
    }

    protected virtual IEnumerator StartTimer(int timerID, float time, Action<float> onUpdate, Action onCompleted)
    {
        while (time > 0)
        {
            time -= Time.deltaTime;
            onUpdate?.Invoke(time);
            yield return null;
        }
        lookups.Remove(timerID);
        onCompleted?.Invoke();
    }
}

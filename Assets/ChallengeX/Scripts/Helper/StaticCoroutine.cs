
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticCoroutine
{
    private class CoroutineHolder : MonoBehaviour
    {
    }

    private static CoroutineHolder _runner;

    private static CoroutineHolder Runner
    {
        get
        {
            if (_runner == null)
            {
                _runner = new GameObject("Static Coroutine RestClient").AddComponent<CoroutineHolder>();
                UnityEngine.Object.DontDestroyOnLoad(_runner);
            }

            return _runner;
        }
    }

    public static Coroutine StartCoroutine(IEnumerator coroutine)
    {
        //return Runner.StartCoroutine(coroutine);
        return UnityMainThreadDispatcher.Instance().EnqueueCh(coroutine);

    }

}

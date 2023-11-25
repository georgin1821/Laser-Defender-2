using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSingleton<T> : MonoBehaviour where T : SimpleSingleton<T>
{
    private static T instance;
    public static T Instance
    {
        get { return instance; }
    }

    public static bool IsInitialized
    {
        get { return instance != null; }
    }

    protected virtual void Awake()
    {
        InitializeInstance();
    }

    protected virtual void InitializeInstance()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            Debug.Log("[Singleton] Trying to instantitate a second instance of singleton");
        }
        else
        {
            instance = (T)this;
        }
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
    public void DestroySingleton()
    {
        instance = (T)null;
    }
}

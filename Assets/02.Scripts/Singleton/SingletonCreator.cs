using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonCreator : MonoBehaviour
{
    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        InitSingletonObject<UIManager>();
        InitSingletonObject<AtlasManager>();
    }

    private void InitSingletonObject<T>()
    {
        GameObject prefab = new GameObject(typeof(T).Name);

        if (prefab != null)
            prefab.AddComponent(typeof(T));
    }
}

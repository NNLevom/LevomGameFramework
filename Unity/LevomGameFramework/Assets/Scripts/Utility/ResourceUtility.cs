using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ResourceUtility
{
    public static GameObject LoadGameObject(string path)
    {
        var go = Resources.Load<GameObject>(path);
        return go ? GameObject.Instantiate(go) : null;
    }

    public static void LoadGameObject(string path, Action<GameObject> callback)
    {
        var go = Resources.Load<GameObject>(path);
        callback?.Invoke(go ? GameObject.Instantiate(go) : null);
    }
}

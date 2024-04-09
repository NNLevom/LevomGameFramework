using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public static class UnityHelper
{

    //自定义在窗体之上的渲染层级
    public static void AddCanvas(Canvas formCanvas, GameObject target, int addOrder)
    {
        if (formCanvas == null || target == null) return;
        bool isActive = target.activeSelf;
        target.SetActive(true);

        Canvas canvas = target.GetComponent<Canvas>();
        if (!canvas) canvas = target.AddComponent<Canvas>();

        canvas.pixelPerfect = formCanvas.pixelPerfect;
        canvas.overrideSorting = formCanvas.overrideSorting;
        canvas.sortingLayerName = formCanvas.sortingLayerName;
        canvas.sortingOrder = formCanvas.sortingOrder + addOrder;
        canvas.enabled = true;
        target.SetActive(isActive);

        var gr = target.GetComponent<GraphicRaycaster>();
        if (!gr) gr = target.AddComponent<GraphicRaycaster>();
        gr.enabled = true;
    }






    public static void ClearCanvas(GameObject target)
    {
        var gr = target.GetComponent<GraphicRaycaster>();
        if (gr) GameObject.Destroy(gr);

        var canvas = target.GetComponent<Canvas>();
        if (canvas) GameObject.Destroy(canvas);
    }

}

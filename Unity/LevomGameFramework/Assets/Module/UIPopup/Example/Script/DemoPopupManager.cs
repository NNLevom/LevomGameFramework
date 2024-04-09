using System.Collections.Generic;
using UnityEngine;
using UIPopup;

public class DemoPopupManager : PopupManager
{

    private void Start()
    {
        var canvas = GetComponent<Canvas>();
        base.Init<PopupType>(canvas, 10, DemoEvt.OpenPopup, DemoEvt.ClosePopup, "Prefabs");

        var list = new List<int> { (int)PopupType.Tip1, };
        AddPersistenceSign(list);
    }


    public enum PopupType
    {
        Tip1,
        Tip2,
        Tip3,
    }

    public void OpenPopup(object userData, PopupType popup)
    {
        base.OpenPopup(userData, (int)popup);
    }

    public void ClosePopup(PopupType popup)
    {
        base.ClosePopup((int)popup);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            OpenPopup("Tip1", PopupType.Tip1);
        }

        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            ClosePopup(PopupType.Tip1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            OpenPopup("Tip2", PopupType.Tip2);
        }

        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            ClosePopup(PopupType.Tip2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            OpenPopup("Tip3", PopupType.Tip3);
        }

        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            ClosePopup(PopupType.Tip3);
        }
    }
}

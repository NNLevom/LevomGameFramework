using System;
using UIPopup;
using UnityEngine;
using UnityEngine.UI;

public class PopupItemTips : MonoBehaviour, IPopupOpen, IPopupClose
{
    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void Open(object userData)
    {
        transform.Find("TxMsg").GetComponent<Text>().text = (string)userData;
        gameObject.SetActive(true);
    }
}

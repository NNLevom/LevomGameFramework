using System;

namespace UIPopup
{

    public interface IPopupClose
    {
        void Close();
    }

    public interface IPopupCloseArg
    {
        void Close(object userData);
    }

    //关闭时会销毁Item
    public interface IPopupOpen
    {
        void Open(object userData);
    }
}
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

    //�ر�ʱ������Item
    public interface IPopupOpen
    {
        void Open(object userData);
    }
}
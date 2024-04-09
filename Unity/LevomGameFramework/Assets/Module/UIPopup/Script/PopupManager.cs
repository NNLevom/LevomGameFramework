using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

namespace UIPopup
{
    public abstract class PopupManager : MonoBehaviour
    {
        private int popupOrderStart;//弹窗起始值

        private string msgListener_Open;//显示自定义弹窗
        private string msgListener_Close;//关闭自定义弹窗

        private string pathRoot;
        private bool isInit = false;

        protected Dictionary<int, Transform> dicItem;//已经加载Item
        private Dictionary<int, string> dicItemName;
        private List<int> listLoaddingItem;//正在加载的窗体

        private PopupManagerHelper popupManagerHelper;
        private Canvas formCanvas;

        public virtual bool Init<T>(Canvas formCanvas,int popupOrderStart, string msgListener_Open, string msgListener_Close, string pathRoot) where T : System.Enum
        {
            if (isInit)
            {
                foreach (Transform child in transform)
                {
                    child.gameObject.SetActive(false);
                }
                return false;
            }
            isInit = true;

            this.popupOrderStart = popupOrderStart;
            this.formCanvas = formCanvas;
            this.msgListener_Open = msgListener_Open;
            this.msgListener_Close = msgListener_Close;
            listLoaddingItem = new List<int>();
            // listOpenning = new List<int>();
            gameObject.SetActive(true);

            popupManagerHelper = gameObject.GetOrAddComponent<PopupManagerHelper>();
            popupManagerHelper.Init();

            AddListener();

            InitPopupItem<T>(pathRoot);

            return true;
        }

        private void InitPopupItem<T>(string pathRoot) where T : System.Enum
        {
            this.pathRoot = pathRoot;
            dicItem = new Dictionary<int, Transform>();
            dicItemName = new Dictionary<int, string>();

            foreach (Transform child in transform)
            {
                GameObject.Destroy(child.gameObject);
            }

            var types = System.Enum.GetValues(typeof(T));
            foreach (T popupType in types)
            {
                string goName = popupType.ToString();

                object oj = popupType;
                int key = (int)oj;

                dicItemName.Add(key, goName);
            }
        }

        //标记持久化的弹窗
        public void AddPersistenceSign(List<int> listIndex)
        {
            popupManagerHelper.AddPersistenceSign(listIndex);
        }
        //移除标记持久化的弹窗
        public void RemovePersistenceSign(List<int> listIndex)
        {
            popupManagerHelper.RemovePersistenceSign(listIndex);
        }

        //初始化数据
        public virtual void Clear()
        {
            popupManagerHelper?.InitData();
            dicItem.Clear();
            listLoaddingItem.Clear();

            foreach (Transform tf in transform)
            {
                Destroy(tf.gameObject);
            }

        }

        public void Clear(List<int> listIgnore)
        {
            Dictionary<int, Transform> dic = new Dictionary<int, Transform>();
            List<Transform> listTf = new List<Transform>();

            if (listIgnore != null)
                foreach (var key in listIgnore)
                {
                    if (dic.ContainsKey(key)) continue;

                    if (dicItem.TryGetValue(key, out var tf))
                    {
                        dic.Add(key, tf);
                        listTf.Add(tf);
                    }
                }

            popupManagerHelper?.InitData();
            dicItem.Clear();
            listLoaddingItem.Clear();

            foreach (var keyValue in dic)
            {
                dicItem.Add(keyValue.Key, keyValue.Value);
            }

            foreach (Transform tf in transform)
            {
                if (!listTf.Contains(tf))
                    Destroy(tf.gameObject);
            }

        }

        protected virtual void AddListener()
        {
            if (!isInit) return;
            if (msgListener_Open != null) Messenger.AddListener<object, int>(msgListener_Open, OpenPopup);
            if (msgListener_Close != null) Messenger.AddListener<int>(msgListener_Close, ClosePopup);
        }
        protected virtual void RemoveListener()
        {
            if (!isInit) return;
            if (msgListener_Open != null) Messenger.RemoveListener<object, int>(msgListener_Open, OpenPopup);
            if (msgListener_Close != null) Messenger.RemoveListener<int>(msgListener_Close, ClosePopup);
        }

        public virtual void OpenPopup(object userData, int index)
        {
            TryGetItem(index, false, (go) =>
            {
                if (go == null)
                {
                    Log.Error("获取对象失败:" + index);
                    return;
                }

                var item = go.GetComponent<IPopupOpen>();
                if (item == null)
                {
                    Log.Error("获取对象的接口IPopup 失败  :" + index);
                    return;
                }

                popupManagerHelper.RemoveTimerAction(index);

                item?.Open(userData);

            });

        }

        public virtual void ClosePopup(int index)
        {
            TryGetItem(index, true, (go) =>
            {
                if (go == null)
                {
                    Log.Info("获取对象失败:" + index);
                    return;
                }

                var item = go.GetComponent<IPopupClose>();
                if (item == null)
                {
                    Log.Error("获取对象的接口IPopup 失败  :" + index);
                    return;
                }

                item?.Close();
                CloseCallback(index);
            });
        }

        protected virtual void CloseCallback(int index)
        {
            if (dicItem != null && dicItem.TryGetValue(index, out var tfItem))
            {
                if (tfItem != null)
                {
                    popupManagerHelper.AddTimerAction(index, () =>
                    {
                        TryDestoryItem(index);
                    });
                }
                return;
            }
        }

        public void TryDestoryItem(int index)
        {
            if (dicItem != null && dicItem.TryGetValue(index, out var tfItem))
            {
                if (tfItem == null) return;

                dicItem.Remove(index);
                GameObject.Destroy(tfItem.gameObject);
            }
        }

        private void TryGetItem(int index, bool isClose, Action<Transform> callback)
        {
            if (dicItem != null && dicItem.TryGetValue(index, out Transform item))
            {
                callback?.Invoke(item);
            }
            else if (!isClose)
            {
                LoadItem(index, callback);
            }
            else if (isClose && listLoaddingItem.Contains(index))
            {
                Log.Error("未加载完成，但却请求了关闭 index：{0}", index);
            }
            else
            {
                //  Log.Error("不存在 index：{0}", index);
            }
        }

        private void LoadItem(int index, Action<Transform> callback)
        {
            string goName = "";
            if (dicItemName == null || !dicItemName.TryGetValue(index, out goName))
            {
                Log.Error("dicItemName 字典不存在:{0}", index);
                callback?.Invoke(null);
                return;
            }

            if (listLoaddingItem.Contains(index))
            {
                Log.Info("<color=red>页面在加载中:{0}</color>", index);
                return;
            }

            listLoaddingItem.Add(index);

            ResourceUtility.LoadGameObject(pathRoot + "/" + goName, (go) =>
            {
                go.name = goName;
                var tf = go.transform;
                tf.SetParent(transform, false);
                tf.SetAsLastSibling();

                if (dicItem.ContainsKey(index)) dicItem[index] = tf;
                else dicItem.Add(index, tf);

                listLoaddingItem.Remove(index);

                tf.localPosition = Vector3.one * 5000;
                UnityHelper.AddCanvas(formCanvas, go, popupOrderStart + index + 1);

                tf.localPosition = Vector3.zero;
                go.SetActive(false);
                callback?.Invoke(tf);

            });
        }


    }

}
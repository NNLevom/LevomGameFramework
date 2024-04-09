using UnityEngine;
using System.Collections.Generic;
using System;

namespace UIPopup
{

    public class PopupManagerHelper : MonoBehaviour
    {
        private float stayTime = 10;//关闭弹窗后，停留时间后会销毁
        private float checkTime = 1;//检测时间间隔
        private Dictionary<int, TimerData> dicTimerAction;
        private List<int> listKey;
        public List<int> listIndexPersistenceSign { get; private set; }//持久化的弹窗


        private List<int> needDestoryList;
        private float t;

        public void Init()
        {
            dicTimerAction = new Dictionary<int, TimerData>();
            listKey = new List<int>();
            t = 0;
            needDestoryList = new List<int>();
        }

        public void InitData()
        {
            listKey.Clear();
            dicTimerAction.Clear();
        }

        //标记持久化的弹窗
        public void AddPersistenceSign(List<int> listKey)
        {
            listIndexPersistenceSign = listKey;
        }

        //标记持久化的弹窗
        public void RemovePersistenceSign(List<int> listKey)
        {
            if (listIndexPersistenceSign == null || listKey == null) return;

            foreach (var key in listKey)
            {
                listIndexPersistenceSign.Remove(key);
            }
        }

        private bool IsPersistenceSign(int key)
        {
            if (listIndexPersistenceSign == null) return false;
            return listIndexPersistenceSign.Contains(key);
        }

        public void AddTimerAction(int key, Action action)
        {
            if (IsPersistenceSign(key)) return;

            TimerData timerData = new TimerData
            {
                key = key,
                action = action,
                stayTime = stayTime
            };

            if (dicTimerAction.ContainsKey(key))
            {
                dicTimerAction[key] = timerData;
            }
            else
            {
                dicTimerAction.Add(key, timerData);
            }

            if (!listKey.Contains(key)) listKey.Add(key);

        }

        public void RemoveTimerAction(int key)
        {
            dicTimerAction.Remove(key);
            listKey.Remove(key);
        }


        private void Update()
        {
            t += Time.deltaTime;
            if (t < checkTime) return;

            t = 0;


            foreach (var key in listKey)
            {
                if (dicTimerAction.TryGetValue(key, out var value))
                {
                    value.stayTime -= checkTime;
                    dicTimerAction[key] = value;
                }
            }

            needDestoryList.Clear();
            foreach (var keyValue in dicTimerAction)
            {
                float time = keyValue.Value.stayTime - checkTime;

                //需要销毁
                if (time <= 0)
                {
                    needDestoryList.Add(keyValue.Key);
                }
            }

            foreach (var key in needDestoryList)
            {
                if (dicTimerAction.TryGetValue(key, out var value))
                {
                    value.action?.Invoke();
                }

                RemoveTimerAction(key);
            }

            needDestoryList.Clear();
        }


        private struct TimerData
        {
            public Action action;
            public float stayTime;
            public int key;
        }

    }
}
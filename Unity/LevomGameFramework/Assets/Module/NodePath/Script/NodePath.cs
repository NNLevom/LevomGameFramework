using UnityEngine;
using System.Collections.Generic;
using System;

namespace NodePath
{
    public static class NodePath<T> where T : class
    {
        static Queue<INode<T>> noodeQueue;
        static List<INode<T>> ignoranceList;


        public static IEnumerator<T> Path(INode<T> start, INode<T> expect, Action<int, List<INode<T>>> callback)
        {
            yield return null;
            var code = Path(start, expect, out var pathNodeList);
            callback?.Invoke(code, pathNodeList);
        }

        /// <summary>
        /// 最短路径的步数 返回值： -1查找失败 0就是原点 >0获取成功（返回移动步数）
        /// <para>pathNodeList: 返回导航路径，从expect->start  首位元素为expect 末位元素为start</para>
        /// </summary>
        public static int Path(INode<T> start, INode<T> expect, out List<INode<T>> pathNodeList)
        {
            pathNodeList = new List<INode<T>>();
            if (start == null || expect == null) return -1;
            if (start == expect) return 0;
            if (start.Equals(expect)) return 0;

            noodeQueue ??= new Queue<INode<T>>();
            ignoranceList ??= new List<INode<T>>();
            noodeQueue.Clear();
            ignoranceList.Clear();

            start.PathNumber = 0;
            start.PathParent = null;
            noodeQueue.Enqueue(start);

            while (noodeQueue.Count != 0)
            {
                var node = noodeQueue.Dequeue();
                if (node == null) continue;

                if (IsAchieve(node, expect))
                {
                    var pathNode = node;
                    while (pathNode != null)
                    {
                        pathNodeList.Add(pathNode);
                        pathNode = pathNode.PathParent;
                    }

                    Clear();
                    return node.PathNumber;
                }
                else
                {
                    ignoranceList.Add(node);
                    foreach (var tempNode in node.LinkNodeList)
                    {
                        if (!tempNode.IsPath) continue;
                        if (ignoranceList.Contains(tempNode)) continue;
                        if (noodeQueue.Contains(tempNode)) continue;

                        tempNode.PathNumber = node.PathNumber + 1;
                        tempNode.PathParent = node;
                        noodeQueue.Enqueue(tempNode);
                    }
                }

            }

            Clear();
            return -1;
        }

        private static void Clear()
        {
            noodeQueue?.Clear();
            ignoranceList?.Clear();
            noodeQueue = null;
            ignoranceList = null;
        }

        //是否移动1步到达了目标
        private static bool IsAchieve(INode<T> start, INode<T> expect)
        {
            return start.Equals(expect);
        }
    }
}
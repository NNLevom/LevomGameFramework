using NodePath;
using System.Collections.Generic;

namespace NodePath
{
    public interface INode<T> where T : class
    {
        /// <summary>
        /// 只供寻路时用
        /// </summary>
        int PathNumber { get; set; }
        /// <summary>
        /// 只供寻路时用
        /// </summary>
        INode<T> PathParent { get; set; }

        /// <summary>
        /// 相连接的节点
        /// </summary>
        List<INode<T>> LinkNodeList { get; set; }

        /// <summary>
        /// 当前节点是否可以寻路
        /// </summary>
        bool IsPath { get; set; }

        void SetLinkNodeList(List<INode<T>> linkNodeList);

        void AddLinkNode(INode<T> linkNode);

        bool IsLinkNode(INode<T> node);

        void SetIsPath(bool isPath);
    }
}

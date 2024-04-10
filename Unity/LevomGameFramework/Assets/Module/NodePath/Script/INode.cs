using NodePath;
using System.Collections.Generic;

namespace NodePath
{
    public interface INode<T> where T : class
    {
        /// <summary>
        /// ֻ��Ѱ·ʱ��
        /// </summary>
        int PathNumber { get; set; }
        /// <summary>
        /// ֻ��Ѱ·ʱ��
        /// </summary>
        INode<T> PathParent { get; set; }

        /// <summary>
        /// �����ӵĽڵ�
        /// </summary>
        List<INode<T>> LinkNodeList { get; set; }

        /// <summary>
        /// ��ǰ�ڵ��Ƿ����Ѱ·
        /// </summary>
        bool IsPath { get; set; }

        void SetLinkNodeList(List<INode<T>> linkNodeList);

        void AddLinkNode(INode<T> linkNode);

        bool IsLinkNode(INode<T> node);

        void SetIsPath(bool isPath);
    }
}

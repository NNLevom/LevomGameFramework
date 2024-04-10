using System.Collections.Generic;

namespace NodePath
{
    public class Node<T> : INode<T> where T : class
    {
        public int PathNumber { get; set; }
        public INode<T> PathParent { get; set; }
        public List<INode<T>> LinkNodeList { get; set; }
        public bool IsPath { get; set; }

        public void AddLinkNode(INode<T> linkNode)
        {
            LinkNodeList ??= new List<INode<T>>();
            LinkNodeList.Add(linkNode);
        }
        public void SetLinkNodeList(List<INode<T>> linkNodeList)
        {
            this.LinkNodeList = linkNodeList;
        }

        public bool IsLinkNode(INode<T> node)
        {
            if (LinkNodeList == null || node == null) return false;
            return LinkNodeList.Contains(node);
        }

        public void SetIsPath(bool isPath)
        {
            IsPath = isPath;
        }
    }
}
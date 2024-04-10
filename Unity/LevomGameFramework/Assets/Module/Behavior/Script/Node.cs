using System;

namespace BehaviorTree
{
    [Serializable]
    public abstract class Node 
    {

        public delegate NodeStates NodeReturn();

        protected NodeStates m_NodeStates;

        public NodeStates nodeStates
        {
            get => m_NodeStates;
        }

        public Node() { }

        public abstract NodeStates Evaluate();

    }

    public enum NodeStates
    {
        Failure,
        Success,
        Running
    }
}
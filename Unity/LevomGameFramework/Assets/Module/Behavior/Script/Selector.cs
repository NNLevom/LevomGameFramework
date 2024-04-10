using System.Collections.Generic;

namespace BehaviorTree
{
    /// <summary>
    /// 选择器，遍历到有Success 直接返回Success，否则返回返回Failure
    /// </summary>
    public class Selector : Node
    {
        protected List<Node> m_nodes;

        public Selector(List<Node> nodes)
        {
            m_nodes = nodes;
        }

        public override NodeStates Evaluate()
        {
            if (m_nodes == null)
            {
                m_NodeStates = NodeStates.Success;
                return m_NodeStates;
            }

            foreach (var node in m_nodes)
            {
                switch (node.Evaluate())
                {
                    case NodeStates.Failure:
                        continue;

                    case NodeStates.Success:
                        m_NodeStates = NodeStates.Success;
                        return m_NodeStates;

                    case NodeStates.Running:
                        m_NodeStates = NodeStates.Running;
                        return m_NodeStates;

                    default: continue;
                }
            }

            m_NodeStates = NodeStates.Failure;
            return m_NodeStates;
        }
    }
}
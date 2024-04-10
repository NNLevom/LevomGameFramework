using System.Collections.Generic;

namespace BehaviorTree
{
    /// <summary>
    /// 序列器 全部 Success 则返回Success ，否则 Failure /
    /// </summary>
    public class Sequence : Node
    {
        private List<Node> m_nodes;

        public Sequence(List<Node> nodes)
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

            bool isAnyChildRunning = false;

            foreach (var node in m_nodes)
            {
                switch (node.Evaluate())
                {
                    case NodeStates.Failure:
                        m_NodeStates = NodeStates.Failure;
                        return m_NodeStates;

                    case NodeStates.Success:
                        continue;

                    case NodeStates.Running:
                        isAnyChildRunning = true;
                        continue;

                    default:
                        m_NodeStates = NodeStates.Success;
                        return m_NodeStates;
                }
            }

            m_NodeStates = isAnyChildRunning ? NodeStates.Running : NodeStates.Success;
            return m_NodeStates;
        }
    }
}
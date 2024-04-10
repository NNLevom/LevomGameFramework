
namespace BehaviorTree
{
    /// <summary>
    /// 反选器，遍历到有Failure 直接返回Success ，否则返回Failure
    /// </summary>
    public class Inverter : Node
    {
        private Node m_node;

        public Node node
        {
            get => m_node;
        }

        public Inverter(Node node)
        {
            m_node = node;
        }

        public override NodeStates Evaluate()
        {
            if (m_node == null)
            {
                m_NodeStates = NodeStates.Success;
                return m_NodeStates;
            }
            switch (node.Evaluate())
            {
                case NodeStates.Failure:
                    m_NodeStates = NodeStates.Success;
                    return m_NodeStates;

                case NodeStates.Success:
                    m_NodeStates = NodeStates.Failure;
                    return m_NodeStates;

                case NodeStates.Running:
                    m_NodeStates = NodeStates.Running;
                    return m_NodeStates;
            }

            m_NodeStates = NodeStates.Success;
            return m_NodeStates;
        }
    }
}
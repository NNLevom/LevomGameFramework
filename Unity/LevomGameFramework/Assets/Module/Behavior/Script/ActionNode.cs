
namespace BehaviorTree
{
    /// <summary>
    /// Ö´ÐÐ½Úµã
    /// </summary>
    public class ActionNode : Node
    {
        public delegate NodeStates ActionNodeDelegate();

        private ActionNodeDelegate m_action;

        public ActionNode(ActionNodeDelegate action)
        {
            m_action = action;
        }


        public override NodeStates Evaluate()
        {
            if (m_action == null)
            {
                m_NodeStates = NodeStates.Success;
                return m_NodeStates;
            }

            switch (m_action())
            {
                case NodeStates.Success:
                    m_NodeStates = NodeStates.Success;
                    return m_NodeStates;

                case NodeStates.Failure:
                    m_NodeStates = NodeStates.Failure;
                    return m_NodeStates;

                case NodeStates.Running:
                    m_NodeStates = NodeStates.Running;
                    return m_NodeStates;

                default:
                    m_NodeStates = NodeStates.Failure;
                    return m_NodeStates;
            }

        }
    }
}
using System.Collections.Generic;
using System;
using UnityEngine;

namespace BehaviorTree
{
    public class DemoBehaviorTree : MonoBehaviour
    {
        protected Selector rootNode;

        private void Start()
        {
            List<Node> list = new()
            {
                new ActionNode(OnAct1),
                new ActionNode(OnAct2),
                new ActionNode(OnAct3),
                new ActionNode(OnAct4),
                new ActionNode(OnAct5),
            };

            rootNode = new Selector(list);

            rootNode.Evaluate();
        }



        protected  NodeStates OnAct1()
        {
            Log.Info("OnAct1 Failure");
            return NodeStates.Failure;
        }

        protected NodeStates OnAct2()
        {
            Log.Info("OnAct2 Failure");
            return NodeStates.Failure;
        }

        protected NodeStates OnAct3()
        {
            Log.Info("OnAct3 Success");
            return NodeStates.Success;
        }

        protected NodeStates OnAct4()
        {
            Log.Info("OnAct4 Success");
            return NodeStates.Success;
        }

        protected NodeStates OnAct5()
        {
            Log.Info("OnAct5 Success");
            return NodeStates.Success;
        }

    }
}
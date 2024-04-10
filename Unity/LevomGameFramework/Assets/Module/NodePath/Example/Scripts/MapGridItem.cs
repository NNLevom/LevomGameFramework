using UnityEngine;
using TMPro;

namespace NodePath
{

    public class MapGridItem : NodeMono<MapGridItem>
    {
        private TextMeshPro txIndex;
        public int id;

        public void Init(int id, bool isObstacle)
        {
            this.id = id;
            txIndex = transform.Find("TxIndex").GetComponent<TextMeshPro>();
            txIndex.text = id.ToString();
            SetIsPath(!isObstacle);
            if (isObstacle) GetComponent<MeshRenderer>().material.color = Color.grey;
            gameObject.name = id.ToString();
        }


    }
}

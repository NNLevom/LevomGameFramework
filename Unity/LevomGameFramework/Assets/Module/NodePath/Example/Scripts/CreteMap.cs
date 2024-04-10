using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using System.IO;
using static UnityEditor.Timeline.Actions.MenuPriority;
using static UnityEditor.Progress;
namespace NodePath
{
    public class CreteMap : MonoBehaviour
    {
        [Range(1, 9)]
        public int countX, countZ;
        public float space;
        public GameObject preMapGrid;

        private Dictionary<int, MapGridItem> dicMapGridItem;

        void Start()
        {
            dicMapGridItem = new Dictionary<int, MapGridItem>();
            CreateMap(preMapGrid.transform.localScale, countX, countZ, space);
        }

        public void CreateMap(Vector3 sizeMapGrid, int countX, int countZ, float space)
        {
            dicMapGridItem.Clear();
            int indexX = 1, indexZ = 1;

            while (countX > indexX)
            {
                indexZ = 1;
                while (countZ > indexZ)
                {
                    var localPos = new Vector3(indexX * (sizeMapGrid.x + space), 0, indexZ * (sizeMapGrid.z + space));
                    var tf = Instantiate(preMapGrid, transform).transform;
                    tf.localPosition = localPos;

                    var mapGridItem = tf.GetOrAddComponent<MapGridItem>();
                    var id = indexX * 10 + indexZ;
                    mapGridItem.Init(id, UnityEngine.Random.Range(0, 1.0f) > 0.75f);
                    dicMapGridItem.Add(id, mapGridItem);
                    indexZ++;
                }
                indexX++;
            }

            indexX = 1;

            while (countX > indexX)
            {
                indexZ = 1;
                while (countZ > indexZ)
                {
                    var id = indexX * 10 + indexZ;
                    dicMapGridItem.TryGetValue(id, out var item);
                    if (!item) continue;

                    id = indexX * 10 + indexZ + 1;
                    dicMapGridItem.TryGetValue(id, out var itemLinkNode);
                    AddItem(item, itemLinkNode);

                    id = indexX * 10 + indexZ - 1;
                    dicMapGridItem.TryGetValue(id, out itemLinkNode);
                    AddItem(item, itemLinkNode);

                    id = (indexX + 1) * 10 + indexZ;
                    dicMapGridItem.TryGetValue(id, out itemLinkNode);
                    AddItem(item, itemLinkNode);

                    id = (indexX - 1) * 10 + indexZ;
                    dicMapGridItem.TryGetValue(id, out itemLinkNode);
                    AddItem(item, itemLinkNode);


                    indexZ++;
                }
                indexX++;
            }

        }

        private void AddItem(MapGridItem item, MapGridItem add)
        {
            if (add != null && item != null)
            {
                item.AddLinkNode(add);
                Debug.DrawLine(item.transform.position + Vector3.one * -0.15f, add.transform.position + Vector3.one * -0.15f, Color.gray, 1000000);
            }
        }


        public TMP_InputField inputStar, inputEnd;
        private List<INode<MapGridItem>> pathNodeList;

        public void OnClickBtnPath()
        {
            int.TryParse(inputStar.text, out var startID);
            int.TryParse(inputEnd.text, out var endID);

            dicMapGridItem.TryGetValue(startID, out var startItem);
            dicMapGridItem.TryGetValue(endID, out var endItem);

            if (startItem == null || endItem == null)
            {
                Log.Error($"·Ç·¨ID,startID:{startID} , endID:{endID}");
                return;
            }

            //  var code = NodePath<MapGridItem>.Path(startItem, endItem, out pathNodeList);

            StartCoroutine(NodePath<MapGridItem>.Path(startItem, endItem, (code, pathNodeList) =>
            {
                this.pathNodeList = pathNodeList;
                if (code > 0)
                {
                    Log.Info($"Path Succ");
                    if (pathNodeList == null)
                    {
                        Log.Error($"pathNodeList == null");
                        return;
                    }

                }
                else
                {
                    Log.Info($"Path Error:{code}");
                }
            }));

        }


        private void Update()
        {
            if (pathNodeList != null && pathNodeList.Count > 1)
            {
                for (int i = 1; i < pathNodeList.Count; i++)
                {
                    var item1 = pathNodeList[i - 1] as MapGridItem;
                    var item2 = pathNodeList[i] as MapGridItem;
                    Debug.DrawLine(item1.transform.position, item2.transform.position, Color.red);
                }
            }

        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphView : MonoBehaviour
{
    public GameObject nodeViewPrefab;
    public Color baseColor = Color.white;
    public Color wallColor = Color.black;
    public NodeView[,] nodeViews;

    // Start is called before the first frame update
    public void Init ( Graph graph )
    {
        nodeViews = new NodeView[graph.Width, graph.Height];
        foreach ( var node in graph.nodes )
        {
            GameObject instance = Instantiate(nodeViewPrefab, node.position, Quaternion.identity);
            NodeView nodeView = instance.GetComponent<NodeView>();
            if ( nodeView == null )
            {
                Debug.LogError("Nodeview is missing");
                return;
            }

            nodeView.Init(node);

            nodeViews[node.xIndex, node.yIndex] = nodeView;

            if ( node.nodeType == NodeType.Blocked )
            {
                nodeView.ColorNode(wallColor);
            }

            else if ( node.nodeType == NodeType.Open )
            {
                nodeView.ColorNode(baseColor);
            }


        }

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    Node m_startNode;
    Node m_endNode;
    Graph m_graph;
    GraphView m_graphView;

    Queue<Node> m_frontierNodes;
    List<Node> m_exploredNodes;
    List<Node> m_pathNodes;

    public Color startColor = Color.green;
    public Color endColor = Color.red;
    public Color frontierColor = Color.magenta;
    public Color exploredColor = Color.gray;
    public Color pathColor = Color.cyan;

    public void Init ( Graph graph, GraphView graphView, Node startNode, Node endNode )
    {
        if ( startNode.nodeType == NodeType.Blocked || endNode.nodeType == NodeType.Blocked )
        {
            Debug.LogError("Start or End node is blocked.");
            return;
        }

        m_graph = graph;
        m_graphView = graphView;
        m_startNode = startNode;
        m_endNode = endNode;


        NodeView startNodeView = graphView.nodeViews[startNode.xIndex, startNode.yIndex];
        NodeView endNodeView = graphView.nodeViews[endNode.xIndex, endNode.yIndex];

        m_frontierNodes = new Queue<Node>();
        m_frontierNodes.Enqueue(startNode);
        m_exploredNodes = new List<Node>();
        m_pathNodes = new List<Node>();


        for ( int yIndex = 0; yIndex < graph.Height; yIndex++ )
        {
            for ( int xIndex = 0; xIndex < graph.Width; xIndex++ )
            {
                graph.nodes[xIndex, yIndex].Reset();
            }
        }

        startNodeView.ColorNode(startColor);
        endNodeView.ColorNode(endColor);

        ShowColors();
    }

    public IEnumerator SearchRoutine ( float timeStep = 0.5f )
    {
        while ( true )
        {
            if ( m_frontierNodes.Count == 0 )

            {
                break;
            }

            Node currentNode = m_frontierNodes.Dequeue();

            foreach ( var node in currentNode.neighbors )
            {
                if ( !m_frontierNodes.Contains(node) && !m_exploredNodes.Contains(node) )
                {
                    m_frontierNodes.Enqueue(node);
                    node.previous = currentNode;
                }
            }

            m_exploredNodes.Add(currentNode);

            ShowColors();

            m_graphView.nodeViews[currentNode.xIndex, currentNode.yIndex].SetArrow();

            yield return new WaitForSeconds(timeStep);

        }

        ShowColors();
    }

    private void ShowColors ()
    {
        if ( m_exploredNodes != null && m_exploredNodes.Count > 0 )
        {
            foreach ( Node node in m_exploredNodes )
            {
                if ( node != m_startNode && node != m_endNode )
                {
                    m_graphView.nodeViews[node.xIndex, node.yIndex].ColorNode(exploredColor);
                }
            }
        }

        if ( m_frontierNodes != null && m_frontierNodes.Count > 0 )
        {
            foreach ( Node node in m_frontierNodes )
            {
                if ( node != m_startNode && node != m_endNode )
                {
                    m_graphView.nodeViews[node.xIndex, node.yIndex].ColorNode(frontierColor);
                }
            }
        }
    }
}

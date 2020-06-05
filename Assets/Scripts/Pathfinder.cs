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
    public Color pathArrowColor = Color.yellow;

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

            if ( currentNode == m_endNode )
            {
                m_pathNodes = GetPath(currentNode);
            }

            foreach ( var node in currentNode.neighbors )
            {
                if ( !m_frontierNodes.Contains(node) && !m_exploredNodes.Contains(node) )
                {
                    m_frontierNodes.Enqueue(node);
                    node.previous = currentNode;
                }
            }

            m_exploredNodes.Add(currentNode);


            m_graphView.nodeViews[currentNode.xIndex, currentNode.yIndex].SetArrow();

            ShowColors();

            yield return new WaitForSeconds(timeStep);

        }

        ShowColors();
    }

    private void ShowColors ()
    {
        foreach ( var node in m_graph.nodes )
        {
            if ( node == m_endNode )
            {
                m_graphView.nodeViews[node.xIndex, node.yIndex].ColorNode(endColor);

                // Path has found
                if ( m_pathNodes.Contains(node) )
                {
                    m_graphView.nodeViews[node.xIndex, node.yIndex].ColorArrow(pathArrowColor);
                }
            }

            else if ( node == m_startNode )
            {
                m_graphView.nodeViews[node.xIndex, node.yIndex].ColorNode(startColor);
            }

            else if ( m_pathNodes.Contains(node) )
            {
                m_graphView.nodeViews[node.xIndex, node.yIndex].ColorNode(pathColor);
                m_graphView.nodeViews[node.xIndex, node.yIndex].ColorArrow(pathArrowColor);
            }

            else if ( m_frontierNodes.Contains(node) )
            {
                m_graphView.nodeViews[node.xIndex, node.yIndex].ColorNode(frontierColor);
            }

            else if ( m_exploredNodes.Contains(node) )
            {
                m_graphView.nodeViews[node.xIndex, node.yIndex].ColorNode(exploredColor);
            }
        }
    }

    private List<Node> GetPath ( Node endNode )
    {
        List<Node> path = new List<Node>();
        if ( endNode == null )
        {
            return path;
        }


        Node currentNode = endNode;

        while ( currentNode.previous != null )
        {
            path.Add(currentNode);
            currentNode = currentNode.previous;
        }

        return path;
    }
}

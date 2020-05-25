using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    Node[,] nodes;
    List<Node> walls = new List<Node>();

    int[,] m_mapData;
    int m_width;
    int m_height;

    public static readonly Vector2[] Directions = {
        new Vector2(1,0),
        new Vector2(0,1),
        new Vector2(-1, 0),
        new Vector2(0, -1),
        new Vector2(1, 1),
        new Vector2(1, -1),
        new Vector2(-1, 1),
        new Vector2(-1, -1),
    };

    public void Init ( int[,] mapData )
    {
        m_mapData = mapData;
        m_width = mapData.GetLength(0);
        m_height = mapData.GetLength(1);
        nodes = new Node[m_width, m_height];
        walls = new List<Node>();

        for ( int i = 0; i < m_height; i++ )
        {
            for ( int j = 0; j < m_width; j++ )
            {
                NodeType type = (NodeType)mapData[j, i];
                Node newNode = new Node(j, i, type);
                nodes[j, i] = newNode;

                if ( type == NodeType.Blocked )
                {
                    walls.Add(newNode);
                }
            }
        }


        for ( int y = 0; y < m_height; y++ )
        {
            for ( int x = 0; x < m_width; x++ )
            {
                nodes[x, y].neighbors = GetNeighbours(x, y);
            }
        }
    }

    public bool IsWithinBounds ( int x, int y )
    {
        return (x >= 0 && x <= m_width && y >= 0 && y <= m_height);
    }

    List<Node> GetNeighbours ( int x, int y, Node[,] nodesArray, Vector2[] directions )
    {
        List<Node> neighbours = new List<Node>();

        foreach ( var dir in directions )
        {
            int xPos = (int)dir.x + x;
            int yPos = (int)dir.y + y;

            if ( IsWithinBounds(xPos, yPos) && nodesArray[xPos, yPos] != null &&
                nodesArray[xPos, yPos].nodeType != NodeType.Blocked )
            {
                neighbours.Add(nodesArray[xPos, yPos]);
            }
        }

        return neighbours;
    }

    List<Node> GetNeighbours ( int x, int y )
    {
        return GetNeighbours(x, y, nodes, Directions);
    }

}

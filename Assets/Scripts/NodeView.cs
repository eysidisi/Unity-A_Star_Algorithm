using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeView : MonoBehaviour
{
    public GameObject tile;
    public GameObject arrow;

    Node m_node;

    [Range(0, 0.5f)]
    public float sideOffset;

    public void Init ( Node node )
    {
        if ( tile != null && node != null )
        {
            gameObject.name = "Node (" + node.xIndex + ", " + node.yIndex + ")";
            gameObject.transform.position = node.position;
            tile.transform.localScale = new Vector3(1 - sideOffset, 1, 1 - sideOffset);
            arrow.SetActive(false);
            m_node = node;
        }
    }

    void ColorNode ( Color color, GameObject go )
    {
        if ( go != null )
        {
            Renderer goRenderer = go.GetComponent<Renderer>();

            if ( goRenderer != null )
            {
                goRenderer.material.color = color;
            }
        }
    }

    public void ColorNode ( Color color )
    {
        ColorNode(color, tile);
    }

    public void SetArrow ()
    {
        if ( m_node.previous != null )
        {
            arrow.SetActive(true);
            Vector3 relativePos = m_node.previous.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            arrow.transform.rotation = rotation;
        }
    }
}

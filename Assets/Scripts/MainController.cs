using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MainController : MonoBehaviour
{
    GraphView graphView;
    public Graph graph;
    public MapData mapData;
    int[,] mapValues;
    public Pathfinder pathfinder;

    public int startX = 0;
    public int startY = 0;

    public int endX = 0;
    public int endY = 2;

    public float searcRoutineTimeStep = 0.5f;

    void Start ()
    {
        if ( graph != null && mapData != null && pathfinder != null )
        {
            graphView = graph.gameObject.GetComponent<GraphView>();

            if ( graphView == null )
            {
                Debug.LogError("Graphview is null");
                return;
            }

            mapValues = mapData.MakeMap();
            graph.Init(mapValues);
            graphView.Init(graph);

            if ( graph.IsWithinBounds(startX, startY) && graph.IsWithinBounds(endX, endY) )
            {
                pathfinder.Init(graph, graphView, graph.nodes[startX, startY],
                     graph.nodes[endX, endY]);

                StartCoroutine(pathfinder.SearchRoutine(searcRoutineTimeStep));
            }
            else
            {
                Debug.LogError("start and end points are out of boundry");
                return;
            }

        }
    }

    // Update is called once per frame
    void Update ()
    {

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData : MonoBehaviour
{
    public string mapName;
    int width;
    int height;
    TextAsset mapText;

    public int[,] MakeMap ()
    {
        mapText = Resources.Load(mapName) as TextAsset;

        string mapStr = mapText.text;

        string[] stringSeparators = new string[] { "\r\n" };

        string[] mapValues = mapStr.Split(stringSeparators, System.StringSplitOptions.None);

        Array.Reverse(mapValues);

        width = mapValues[0].Length;
        height = mapValues.Length;

        int[,] map = new int[width, height];
        for ( int y = 0; y < height; y++ )
        {
            for ( int x = 0; x < width; x++ )
            {
                map[x, y] = (int)Char.GetNumericValue(mapValues[y], x);
            }
        }

        return map;

    }
}

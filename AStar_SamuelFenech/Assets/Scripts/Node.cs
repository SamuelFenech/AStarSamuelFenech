using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Node
{
    // This is the node class
    public bool walkable;

    public Vector3 worldPos;
    public int gCost;
    public int hCost;
    public int gridX;
    public int gridY;
    public Node parent;
    // Tile Prefab for visuals along with the text variables
    public GameObject tile;
    public TMP_Text fCostTxt;
    public TMP_Text hCostTxt;
    public TMP_Text gCostTxt;
    public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY, GameObject _tile, TMP_Text _fCostTxt, TMP_Text _hCostTxt, TMP_Text _gCostTxt)
    {
        walkable = _walkable;
        worldPos = _worldPos;
        gridX = _gridX;
        gridY = _gridY;  
        tile = _tile;
        fCostTxt = _fCostTxt;
        hCostTxt = _hCostTxt;
        gCostTxt = _gCostTxt;
    }

//fCost is the value of gCost + hCost
    public int fCost
    {
        get{
            return gCost + hCost;
        }
    }

    
}

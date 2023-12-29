using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Pathfinding : MonoBehaviour
{
    public Transform seeker, target;
    Grid grid;

    bool targetFound = false;


    void Awake()
    {
        grid = GetComponent<Grid>();
    }

    void Start()
    {
        Node startNode = grid.NodeFromWorldPoint(seeker.transform.position);
		Node targetNode = grid.NodeFromWorldPoint(target.transform.position);
        StartCoroutine(FindPath(seeker.position,target.position));
    }

    void Update()
    {
        
    }
    //A coroutine which is used at the start of the script. This method finds the tile with the lowest Fcost in the neighbouring nodes
    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = grid.NodeFromWorldPoint(startPos);
		Node targetNode = grid.NodeFromWorldPoint(targetPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        while (openSet.Count > 0 && !targetFound) {
            
            Node currentNode = openSet[0];
            
            for(int i = 1; i < openSet.Count; i++)   
            {
                if(openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if(currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                targetFound = true;
                yield return null;
            }

            foreach(Node neighbour in grid.GetNeighbours(currentNode))
            {
                if(!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }
                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if(newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) 
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if(!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }

            foreach(Node n in openSet)
            {
                Debug.Log(n.fCost);
                n.fCostTxt.text = n.fCost.ToString();
                n.hCostTxt.text = n.hCost.ToString();
                n.gCostTxt.text = n.gCost.ToString();
                n.tile.GetComponent<SpriteRenderer>().color = Color.cyan;
            }
          
            
            yield return new WaitForSeconds(0.5f);
        }  
        
    }
    // Once a path is found in the coroutine, all the saved nodes will be coloured green.
    void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        grid.path = path;

        foreach(Node tile in path)
        {
            tile.tile.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }
    // A method to get the distance between both nodes.
    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if(dstX > dstY)
            return 14*dstY + 10* (dstX - dstY);
        return 14*dstX + 10 * (dstY -dstX);
    } 
}
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Grid : MonoBehaviour
{
    public Transform player;
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public GameObject tile;
    public float nodeRadius;
    Node[,] grid;
    float nodeDiameter;
    int gridSizeX, gridSizeY;

    void Start()
    {
        nodeDiameter = nodeRadius*2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x/nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y/nodeDiameter);
        CreateGrid();

        
    }

    // A grid is created with each tile representing a seperate node.
    // All tiles have there gCost hCost and fCost being displayed.
    void CreateGrid()
    {
        grid = new Node[gridSizeX,gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.forward * gridWorldSize.y/2;
        for(int x = 0; x < gridSizeX; x++)
        {
            for(int y=0; y<gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));

                GameObject newTile = Instantiate(tile, worldPoint, Quaternion.Euler(new Vector3(90, 0, 0)));
                TMP_Text fCostTxt = newTile.transform.GetChild(0).transform.GetChild(0).GetComponent<TMP_Text>();
                TMP_Text gCostTxt = newTile.transform.GetChild(0).transform.GetChild(1).GetComponent<TMP_Text>();
                TMP_Text hCostTxt = newTile.transform.GetChild(0).transform.GetChild(2).GetComponent<TMP_Text>();
                grid[x,y] = new Node(walkable, worldPoint, x, y, newTile, fCostTxt, hCostTxt, gCostTxt);
            }
        }
        

        
    }

    // A method to return the node based of the Seeker position
    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x/2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y/2) / gridWorldSize.y;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX-1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY-1) * percentY);

        return grid[x,y];

    }

    public List<Node> path;
    void OnDrawGizmos()
    {
        //        Instantiate(visualBlock, transform.position, Quaternion.identity);
        if(grid != null)
        {
            Node playerNode = NodeFromWorldPoint(player.position);
            foreach(Node n in grid)
            {
                
                n.tile.GetComponent<SpriteRenderer>().color = (n.walkable)?Color.white:Color.red;

                if(playerNode == n)
                {
                    n.tile.GetComponent<SpriteRenderer>().color = Color.gray;
                }
                if(path != null)
                {
                    if(path.Contains(n))
                    {
                        n.tile.GetComponent<SpriteRenderer>().color = Color.green ;
                        // n.tile.GetComponent<Image>().color = new Color (0, 1, 0, 1); 
                        
                    }
                }
                Gizmos.DrawCube(n.worldPos, Vector3.one * (nodeDiameter-.1f));
                

                
            }
        }
    }
    
  //This method returns a list of the neighbouring Nodes for the given node
    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

		for (int x = -1; x <= 1; x++) 
        {
			for (int y = -1; y <= 1; y++) 
            {
				if (x == 0 && y == 0)
					continue;
 
				int checkX = node.gridX + x;
				int checkY = node.gridY + y;

				if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) {
					neighbours.Add(grid[checkX,checkY]);
                    
				}
			}
		}
    	return neighbours;
    }

   
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum DirectionType { FourPath, EightPath };
public class PathFinding
{
    public const float DIAGONAL_MOVE_COST = 1.4f, STRAIGHT_MOVE_COST = 1f;
    private GridMap grid;

    [SerializeField] public DirectionType direction;
    public GridMap Grid { get => grid; set => grid = value; }
    public PathFinding(int width, int height, int cellSize, Vector3 position)
    {
        grid = new GridMap(width, height, cellSize, position);
    }

    //Return a list of path 
    public void FindPath(int startX, int startY, int endX, int endY)
    {
        Node startNode = grid.GetValue(startX, startY);
        Node targetNode = grid.GetValue(endX, endY);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node node = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost() <= node.fCost() )
                {
                    if (openSet[i].H < node.H)
                        node = openSet[i];
                }
            }

            openSet.Remove(node);
            closedSet.Add(node);

            if (node == targetNode)
            {
                RetracePath(startNode,targetNode);
                return;
            }

            foreach (Node neighbour in FindNeighber(node))
            {
                if (!neighbour.IsWalkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                float newCostToNeighbour = node.G + CalculateDistanceCos(node, neighbour);
                if (newCostToNeighbour < neighbour.G|| !openSet.Contains(neighbour))
                {
                    neighbour.G = newCostToNeighbour;
                    neighbour.H = CalculateDistanceCos(neighbour, targetNode);
                    neighbour.CameFromNode = node;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }
    }
    //Reverse calculate node when generate path done
    void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.CameFromNode;
        }
        path.Reverse();

        grid.path = path;

    }
    public float CalculateDistanceCos(Node start, Node end)
    {
        int xDis = Mathf.Abs(start.X - end.X);
        int yDis = Mathf.Abs(start.Y - end.Y);
        int remaining = Mathf.Abs(xDis - yDis);
        return DIAGONAL_MOVE_COST * Mathf.Min(xDis, yDis) + STRAIGHT_MOVE_COST * remaining;
    }
    private List<Node> FindNeighber(Node currnentNode)
    {
        List<Node> neighber = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = currnentNode.X + x;
                int checkY = currnentNode.Y + y;
                if (checkX >= 0 && checkX < grid.Width&& checkY >= 0 && checkY < grid.Height)
                {
                    neighber.Add(grid.GetValue(checkX, checkY));
                }
            }
        }

        return neighber;
    }
    public Node GetLowestFCostNode(List<Node> nodeList)
    {
        Node lowestNode = nodeList[0];
        foreach (Node i in nodeList)
        {
            if (i.fCost() < lowestNode.fCost())
            {
                lowestNode = i;
            }
        }
        return lowestNode;
    }


}

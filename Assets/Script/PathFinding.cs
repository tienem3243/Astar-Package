using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum DirectionType { FourPath, EightPath };
public class PathFinding
{
    private const float DIAGONAL_MOVE_COST = 1.4f, STRAIGHT_MOVE_COST = 1f;
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
        if (!targetNode.IsWalkable)
        {
            grid.path.Clear();
            return;

        }
        //init 2 set of A star
        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = FindBestFCost(openSet);

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }

            foreach (Node neighbour in grid.FindNeighber(currentNode))
            {
                //Brickwall or has been checked will be ignore
                if (!neighbour.IsWalkable || closedSet.Contains(neighbour))
                {
                    closedSet.Add(neighbour);
                    continue;
                }
                //tentative neigbour node and add to openlist which need to be check
                TentativeNeigbour(targetNode, openSet, currentNode, neighbour);
            }
        }
    }

    private void TentativeNeigbour(Node targetNode, List<Node> openSet, Node node, Node neighbour)
    {
        float tentativeGcost = node.G + CalculateDistanceCos(node, neighbour);
        if (tentativeGcost < neighbour.G || !openSet.Contains(neighbour))
        {
            neighbour.G = tentativeGcost;
            neighbour.H = CalculateDistanceCos(neighbour, targetNode);
            neighbour.CameFromNode = node;

            if (!openSet.Contains(neighbour))
                openSet.Add(neighbour);
        }
    }

    private static Node FindBestFCost(List<Node> openSet)
    {
        Node node = openSet[0];
        for (int i = 1; i < openSet.Count; i++)
        {
            if (openSet[i].F <= node.F)
            {
                if (openSet[i].H < node.H)
                    node = openSet[i];
            }
        }

        return node;
    }

    
    void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.CameFromNode;
        }
        path.Reverse();//Reverse list to find form start to end

        grid.path = path;

    }
    public float CalculateDistanceCos(Node start, Node end)
    {
        int xDis = Mathf.Abs(start.X - end.X);
        int yDis = Mathf.Abs(start.Y - end.Y);
        int remaining = Mathf.Abs(xDis - yDis);
        return DIAGONAL_MOVE_COST * Mathf.Min(xDis, yDis) + STRAIGHT_MOVE_COST * remaining;
    }
    


}

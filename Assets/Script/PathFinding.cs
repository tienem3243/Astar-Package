using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    private List<Node> openList, closeList;
    private CustomGrid grid;

    public CustomGrid Grid { get => grid; set => grid = value; }
    public PathFinding(int width,int height)
    {
        grid = new CustomGrid(width, height,2, new Vector2(0,0));
    }
    public List<Node> FindPath(int startX, int startY, int endX, int endY)
    {
        Node startNode = Grid.GetValue(startX, startY);
        Node endNode = Grid.GetValue(endX, endY);
        openList = new List<Node>() { startNode };
        closeList = new List<Node>();
        //init
        for (int i = 0; i < Grid.Width; i++)
        {
            for (int j = 0; j < Grid.Height; j++)
            {
                Node node = Grid.GetValue(i, j);
                node.G = int.MaxValue;
                node.CalculateFCost();
                node.CameFromNode = null;
            }
        }
        startNode.G = 0;
        startNode.H = CalculateDistanceCos(startNode, endNode);
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            Node currentNode = GetLowestFCostNode(openList);
            if (currentNode == endNode)
            {
                return (CalculatePathNode(endNode));
            }

            openList.Remove(currentNode);
            closeList.Add(currentNode);
            foreach (Node i in FindNeighber(currentNode))
            {
                //Ignore which has already in close list be cause it was checked
                if (closeList.Contains(i)) continue;
                if (!i.IsWalkable)
                {
                    closeList.Add(i);
                    continue;
                }
                float tentativeGCost = currentNode.G + CalculateDistanceCos(currentNode, i);

                if (tentativeGCost < i.G)
                {
                    i.CameFromNode = currentNode;
                    i.G = tentativeGCost;
                    i.H = CalculateDistanceCos(i, endNode);
                    i.CalculateFCost();
                    //add to OpenList new node
                    if (!openList.Contains(i)) { openList.Add(i); }
                }
                
            }
        }
        //out of not open list
        return null;
    }
    //focus this
    public List<Node> CalculatePathNode(Node endNode)
    {
        List<Node> path = new List<Node>();
        path.Add(endNode);
        Node currentNode = endNode;
        while (currentNode.CameFromNode != null)
        {
            path.Add(currentNode);
            currentNode = currentNode.CameFromNode;

        }
        path.Reverse();
        return path;
    }
    public float CalculateDistanceCos(Node start, Node end)
    {
        int xDis = Mathf.Abs(start.X - end.X);
        int yDis = Mathf.Abs(start.Y - end.Y);
        return xDis + yDis;
    }
    public List<Node> FindNeighber(Node currnentNode)
    {
        List<Node> neighber = new List<Node>();
        //Left
        if (currnentNode.X - 1 >= 0) neighber.Add(Grid.GetValue(currnentNode.X - 1, currnentNode.Y));
        //Right
        if (currnentNode.X + 1 < Grid.Width) neighber.Add(Grid.GetValue(currnentNode.X + 1, currnentNode.Y));
        //Up
        if (currnentNode.Y + 1 < Grid.Height) neighber.Add(Grid.GetValue(currnentNode.X, currnentNode.Y+1));
        //Down
        if (currnentNode.Y - 1 >= 0) neighber.Add(Grid.GetValue(currnentNode.X , currnentNode.Y-1));
        return neighber;
    }
    public Node GetLowestFCostNode(List<Node> nodeList)
    {
        Node lowestNode = nodeList[0];
        foreach (Node i in nodeList)
        {
            if (i.F < lowestNode.F)
            {
                lowestNode = i;
            }
        }
        return lowestNode;
    }


}

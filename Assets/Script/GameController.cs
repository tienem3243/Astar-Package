using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public PathFinding pathFinding;
    public Moverment gameMoverment;
   
  
    private void Update()
    {
        //move
        MoveFollowPath();
        SetObtacle();
        DrawPath(gameMoverment.moveQueue.ToArray());

    }

    private void SetObtacle()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pathFinding.Grid.GetXY(mouseWorldPos, out int x, out int y);
            Node node = new Node(x, y);
            node.IsWalkable = !pathFinding.Grid.GetValue(x, y).IsWalkable;
            pathFinding.Grid.SetValue(x, y, node);
        }
    }

    private void MoveFollowPath()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StopMove();
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pathFinding.Grid.GetXY(mouseWorldPos, out int endX, out int endY);
            pathFinding.Grid.GetXY(gameMoverment.player.transform.position, out int startX, out int startY);
            pathFinding.FindPath(startX, startY, endX, endY);
            List<Node> path = pathFinding.Grid.path;
            foreach (Node i in path)
            {
                Vector2 worldPos = pathFinding.Grid.GetWorldPosition(i.X, i.Y);
                gameMoverment.moveQueue.Enqueue(new Vector2(worldPos.x + pathFinding.Grid.CellSize / 2, worldPos.y + pathFinding.Grid.CellSize / 2));
            }

            StartCoroutine(gameMoverment.MoveQueue());
        }
    }

    private void StopMove()
    {
        gameMoverment.moveQueue.Clear();
        gameMoverment.isMoving = false;
        gameMoverment.StopAllCoroutines();
    }

    public void DrawPath(Vector2[] listPos)
    {
        for (int i = 0; i < listPos.Length - 1; i++)
        {
            Debug.DrawLine(new Vector3(listPos[i].x, listPos[i].y), new Vector3(listPos[i + 1].x, listPos[i + 1].y), Color.green);
        }
    }

}




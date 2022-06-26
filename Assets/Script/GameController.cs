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
        if (Input.GetMouseButtonDown(0))
        {
            MoveFollowPath();
        }
        DrawPath(gameMoverment.moveQueue.ToArray());

    }

    private void MoveFollowPath()
    {
       
            StopMove();
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            List<Node> path = pathFinding.getPath(gameMoverment.player.transform.position, mouseWorldPos);
            foreach (Node i in path)
            {
                Vector2 worldPos = pathFinding.Grid.GetWorldPosition(i.X, i.Y);
                gameMoverment.moveQueue.Enqueue(new Vector2(worldPos.x + pathFinding.Grid.CellSize / 2, worldPos.y + pathFinding.Grid.CellSize / 2));
            }

            StartCoroutine(gameMoverment.MoveQueue());
        
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




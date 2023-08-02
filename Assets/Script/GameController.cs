using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public PathFinding pathFinding;
    public UnitController selection;


    private void Update()
    {
        //move
       
        if (Input.GetMouseButtonDown(0))
        {
            if (selection.selectedUnits.Count > 0)
              
                selection.selectedUnits.ForEach(x =>
                {

                    MoveFollowPath(x.Movement);
                    DrawPath(x.Movement.moveQueue.ToArray());
                });

        }



    }

    private void MoveFollowPath(UnitMovement move)
    {

        StopMove(move);
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Debug.Log(pathFinding);
        Debug.Log(pathFinding.GetPath(move.transform.position, mouseWorldPos));
        List<Node> path = new List<Node>(pathFinding.GetPath(move.transform.position, mouseWorldPos));
        foreach (Node i in path)
        {
            Vector2 worldPos = pathFinding.Grid.GetWorldPosition(i.X, i.Y);
            move.moveQueue.Enqueue(new Vector2(worldPos.x + pathFinding.Grid.CellSize / 2, worldPos.y + pathFinding.Grid.CellSize / 2));
        }

        StartCoroutine(move.MoveQueue());

    }

    private void StopMove(UnitMovement move)
    {
        move.moveQueue.Clear();
        move.isMoving = false;
        move.StopAllCoroutines();
    }

    public void DrawPath(Vector2[] listPos)
    {
        for (int i = 0; i < listPos.Length - 1; i++)
        {
            Debug.DrawLine(new Vector3(listPos[i].x, listPos[i].y), new Vector3(listPos[i + 1].x, listPos[i + 1].y), Color.green);
        }
    }

}




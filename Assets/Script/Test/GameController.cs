using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public PathFinding pathFinding;
    public Moverment gameController;
    public Canvas gameBoard;
    int iz=0;
    private void Start()
    {
        pathFinding = new PathFinding(9, 9);
        gameController.Grid = pathFinding.Grid;
        gameBoard.GetComponent<RectTransform>().sizeDelta = new Vector2(9*gameController.Grid.CellSize, 9* gameController.Grid.CellSize);
        for(int i = 0; i < gameController.Grid.Height - 1; i++)
        {
            Node node = new Node(5, i);
            node.IsWalkable = false;
            gameController.Grid.SetValue(5, i, node);
        }
        for (int i = gameController.Grid.Height; i >0; i--)
        {
            Node node = new Node(3, i);
            node.IsWalkable = false;
            gameController.Grid.SetValue(3, i, node);
        }

    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)&&!gameController.isMoving)
        {
           
            gameController.moveQueue.Clear();
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pathFinding.Grid.GetXY(mouseWorldPos, out int endX, out int endY);
            pathFinding.Grid.GetXY(gameController.target.transform.position, out int startX, out int startY);
            Debug.Log(startX+" "+startY);
            List<Node> path = pathFinding.FindPath(startX,startY,endX,endY);
            foreach (Node i in path)
            {
                
                Vector2 worldPos = pathFinding.Grid.GetWorldPosition(i.X, i.Y);
                gameController.moveQueue.Enqueue(new Vector2(worldPos.x + 1, worldPos.y + 1));
             
            }
           
            StartCoroutine( gameController.MoveQueue());
        }
        DrawPath(gameController.moveQueue.ToArray());
    }
    public void DrawPath(Vector2[] listPos)
    {
        for (int i = 0; i < listPos.Length- 1; i++)
        {
            Debug.DrawLine(new Vector3(listPos[i].x, listPos[i].y), new Vector3(listPos[i + 1].x, listPos[i + 1].y),Color.green);
        }
    }
    
}

      
        

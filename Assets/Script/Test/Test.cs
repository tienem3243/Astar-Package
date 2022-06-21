using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    CustomGrid grid;
   public bool isMoving;
   public GameObject ball;
    public Queue<Vector2> moveQueue;
    public Vector2 currentNode;

    
    private void Start()
    {
        grid = new CustomGrid(9,12, 2, transform.position);

        moveQueue = new Queue<Vector2>();
    }
    private void Update()
    {
        int x, y;
        grid.GetXY(gameObject.transform.position, out x, out y);
            if (isMoving&&new Vector2(x,y)!=currentNode)
            {
                  currentNode=new Vector2(x,y);
                 HeadPointer(gameObject);
            }
            //move
            if (Input.GetMouseButton(0) && !isMoving)
            {
                Vector2 des = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 onBoardPos = GetObjectPosOnBoard(des);
                moveQueue.Enqueue(onBoardPos);

            }
       
    }
    [ContextMenu("testMove")]
    public void MoveTest()
    {
        if(moveQueue.Count>=0&&!isMoving)
        StartCoroutine(MoveQueue());
    }
    public Vector2 GetObjectPosOnBoard(Vector2 des)
    {
        int x, y;
        grid.GetXY(des, out x, out y);
      
        if (x >= 0 || y >= 0 || x < grid.Width || y < grid.Height)
        {
            Vector2 desf = grid.DebugArray[x, y].gameObject.transform.position;
            Debug.Log(desf.x +" "+ desf.y);
            return desf;
        }
        else
            return Vector2.zero;
       
    }
    public IEnumerator Move(GameObject target,Vector3 destination)
    {
        isMoving = true;
        while (target.transform.position != destination)
        {
            target.transform.position = Vector2.MoveTowards(target.transform.position, destination, Time.deltaTime*20);
            yield return null;
        }
        isMoving = false;
       
    }
    public IEnumerator MoveQueue()
    {
        yield return new WaitForSeconds(1);
        while (moveQueue.Count != 0)
        {
           StartCoroutine( Move(gameObject, moveQueue.Dequeue()));
              
                yield return new WaitUntil(()=>!isMoving);
        }
    }
    public void HeadPointer(GameObject target)
    {
        int x, y;
        grid.GetXY(target.transform.position, out x, out y);
        grid.SetValue(x, y, grid.GetValue(x, y) + 1);
        grid.DebugArray[x, y].color = new Color(grid.GetValue(x, y)/5, 1/grid.GetValue(x, y) , 0);
    }
}

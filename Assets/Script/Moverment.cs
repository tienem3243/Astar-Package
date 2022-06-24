using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moverment : MonoBehaviour
{
    CustomGrid grid;
   public bool isMoving;
    public Queue<Vector2> moveQueue;
    public GameObject target;
 

    public Vector2 currentNode;

    public CustomGrid Grid { get => grid; set => grid = value; }

    private void Start()
    {
        moveQueue = new Queue<Vector2>();
    }
    private void Update()
    {
        int x, y;
        Grid.GetXY(gameObject.transform.position, out x, out y);
            if (isMoving&&new Vector2(x,y)!=currentNode)
            {
                  currentNode=new Vector2(x,y);
                // HeadPointer(gameObject);
            }
            //move
     
       
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
        Grid.GetXY(des, out x, out y);
      
        if (x >= 0 || y >= 0 || x < Grid.Width || y < Grid.Height)
        {
            Vector2 desf = Grid.DebugArray[x, y].gameObject.transform.position;
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
            target.transform.position = Vector2.MoveTowards(target.transform.position, destination, Time.deltaTime*50);
            yield return null;
        }
        isMoving = false;
       
    }
    public IEnumerator MoveQueue()
    {
       
        while (moveQueue.Count != 0)
        {
           StartCoroutine( Move(target, moveQueue.Dequeue()));
              
                yield return new WaitUntil(()=>!isMoving);
        }
    }
    public void HeatPointer(GameObject target)
    {
        int x, y;
        Grid.GetXY(target.transform.position, out x, out y);
        Node n = new Node(0, Grid.GetValue(x, y).H + 1, 0);
        Grid.SetValue(x, y, n);
        Grid.DebugArray[x, y].color = new Color(Grid.GetValue(x, y).H/5, 1/Grid.GetValue(x, y).H , 0);
    }
}

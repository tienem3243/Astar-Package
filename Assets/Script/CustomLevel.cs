using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomLevel : MonoBehaviour
{
    [SerializeField] public string KEY_DATA = "custumdata";
    GridMap grid;
    public int width, height;
    public BallMap ballMap;
    List<Ball> balls = new List<Ball>();
    public GameObject ball;
    private void Start()
    {
        grid = new GridMap(9, 9, 2, transform.position);
        ballMap = new BallMap();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            grid.GetXY(mouseWorldPos, out int x, out int y);
            if (x < grid.Width && y < grid.Height&&x>=0&&y>=0)
            {
                GameObject ballObj = Instantiate(ball);
                Ball a = ballObj.GetComponent<Ball>();
                a.color = Color.red;
                a.x = x;
                a.y = y;
                ballObj.transform.position = grid.GetWorldPosition(x, y) + Vector3.one;
                ballMap.balls.Add(a);
                foreach (Ball i in ballMap.balls)
                {
                    Debug.Log(i.x + " " + i.y);
                }   
            }


        }
    }
    public void Save()
    {
        foreach(Ball i in ballMap.balls)
        {
            Debug.Log(JsonUtility.ToJson(i));
           
        }
    
       
    }
}

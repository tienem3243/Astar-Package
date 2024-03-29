using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GridMap : MonoBehaviourSingleton<GridMap>
{
    [SerializeField] private int width, height;
    [SerializeField] private float cellSize;
    private Node[,] gridArray;
    private TextMesh[,] debugArray;
    [SerializeField] private Vector3 basePos;
    public List<Node> path=new List<Node>();
    [SerializeField] LayerMask maskCollider;
    
    public int Width { get => width; set => width = value; }
    public int Height { get => height; set => height = value; }
    public float CellSize { get => cellSize; set => cellSize = value; }
    public Node[,] GridArray { get => gridArray; set => gridArray = value; }

    public Vector3 BasePos { get => basePos; set => basePos = value; }
    public TextMesh[,] DebugArray { get => debugArray; set => debugArray = value; }



    private void OnDrawGizmos()
    {
        DebugVisible();
        DrawGrid();
    }
    private void Awake()
    {
        InitGrid();

    }

    public GridMap(int width, int height, int cellSize, Vector3 basePos)
    {
        this.Width = width;
        this.Height = height;
        this.CellSize = cellSize;
        this.BasePos = basePos;
        gridArray = new Node[width, height];
        DebugArray = new TextMesh[width, height];
        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
            {
                GridArray[i, j] = new Node(i, j);
            }
    }
    private void InitGrid()
    {
        gridArray = new Node[width, height];
        DebugArray = new TextMesh[width, height];
        for (int i = 0; i < gridArray.GetLength(0); i++)
            for (int j = 0; j < gridArray.GetLength(1); j++)
            {
                gridArray[i, j] = new Node(i, j);
            }
    }
    public void Scan()
    {

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                Vector3 nodePos = GetWorldPosition(x, y);
                Collider2D col = Physics2D.OverlapCircle(nodePos+ Vector3.one * cellSize / 2, cellSize/2,maskCollider);
                if (col != null)
                {
                    Color lowAlphaRed = new Color(1, 0, 0, 0.3f);
                    Gizmos.color = lowAlphaRed;
                    if (gridArray != null)
                    {
                        gridArray[x, y].IsWalkable = false;
                        //detect constructs
                        if (col.gameObject.CompareTag("Construct"))
                        {
                            gridArray[x, y].IsConstruct = true;
                            Debug.Log(col);
                        }
                    }
                 

                }
                else
                {
                    Color lowAlphaGreen = new Color(0, 1, 0, 0.3f);
                    Gizmos.color = lowAlphaGreen;
                }
                //Drawn node
             
                Gizmos.DrawCube(GetWorldPosition(x, y)+Vector3.one*cellSize/2, Vector3.one*cellSize/2);
            }

    }
    public void DebugVisible()
    {

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                Vector3 nodePos = GetWorldPosition(x, y);
              
                if (gridArray == null) return;
                if (!GetValue(x,y).IsWalkable)
                {
                    Color lowAlphaRed = new Color(1, 0, 0, 0.3f);
                    Gizmos.color = lowAlphaRed;
               


                }
                else
                {
                    Color lowAlphaGreen = new Color(0, 1, 0, 0.3f);
                    Gizmos.color = lowAlphaGreen;
                }
                //Drawn node

                Gizmos.DrawCube(GetWorldPosition(x, y) + Vector3.one * cellSize / 2, Vector3.one * cellSize / 2);
            }

    }
    private void DrawGrid()
    {
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {

                Gizmos.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1));
                Gizmos.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y));
            }
        Gizmos.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height));
        Gizmos.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height));
    }
    //convert board pos to world pos
    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * CellSize + BasePos;
    }
    //converworld to board
    public void GetXY(Vector3 worldPos, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPos.x - BasePos.x) / CellSize);
        y = Mathf.FloorToInt((worldPos.y - BasePos.y) / CellSize);
        x = Mathf.Clamp(x,0, width);
        y = Mathf.Clamp(y, 0,height);
    }
    public void SetValue(int x, int y, Node value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
            //TODO need make visual debug better
            if (!gridArray[x, y].IsWalkable)
                DebugArray[x, y].text = "X";
            else DebugArray[x, y].text = "O";
        }

    }

    public void SetValue(Vector3 worldPos, Node value)
    {
        int x, y;
        GetXY(worldPos, out x, out y);
        SetValue(x, y, value);
    }
    public Node GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        return null;
    }
    public Node GetValue(Vector3 worldPos)
    {
        int x, y;
        GetXY(worldPos, out x, out y);
        return GetValue(x, y);
    }
    public List<Node> FindNeighber(Node currnentNode)
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
                if (checkX >= 0 && checkX < Width && checkY >= 0 && checkY < Height)
                {
                    neighber.Add(gridArray[checkX, checkY]);
                }
            }
        }

        return neighber;
    }

}

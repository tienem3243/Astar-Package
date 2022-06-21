using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astar : MonoBehaviour
{
    CustomGrid grid;
    public Transform origin;
    // Start is called before the first frame update
    void Start()
    {
        grid = new CustomGrid(9, 9, 2, origin.position);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 s = Camera.main.ScreenToWorldPoint( Input.mousePosition);
        grid.SetValue(s, grid.GetValue(s)+1);
        Debug.Log(s.x + " " + s.y);
    }
}

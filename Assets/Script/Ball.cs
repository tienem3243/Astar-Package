using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] public Color color;
    [SerializeField] public int x;
    [SerializeField] public int y;

    public Ball(Color color, int x, int y)
    {
        this.color = color;
        this.x = x;
        this.y = y;
    }
}

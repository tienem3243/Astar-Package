using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    public bool isMoving;
    public Queue<Vector2> moveQueue;



    public Vector3 currentNode;
    
    private void Start()
    {
        moveQueue = new Queue<Vector2>();
    }

    public IEnumerator MoveToTarget(GameObject target, Vector3 destination)
    {
        isMoving = true;
        while (target.transform.position != destination)
        {
            target.transform.position = Vector2.MoveTowards(target.transform.position, destination, Time.deltaTime * 50);
            yield return null;
        }
        isMoving = false;

    }
    public IEnumerator MoveQueue()
    {
        while (moveQueue.Count != 0)
        {
            StartCoroutine(MoveToTarget(gameObject, moveQueue.Dequeue()));

            yield return new WaitUntil(() => !isMoving);
        }

    }
    public void FollowQueue()
    {
        StartCoroutine(MoveQueue());
    }
}

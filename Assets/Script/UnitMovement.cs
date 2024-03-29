using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class UnitMovement : MonoBehaviour
{
    public bool isMoving;
    public Queue<Vector2> moveQueue;
    public Vector3 currentNode;
    Tween tween;
    private void Start()
    {
        moveQueue = new Queue<Vector2>();
    }

    private void Update()
    {
        if (Physics2D.OverlapCircleAll(transform.position, 0.2f).Length>1&&!isMoving)
        {
            Vector2 des = (Vector2)transform.position + new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            transform.position = Vector2.MoveTowards(transform.position, des, Time.deltaTime *2);
        }
    }

    public IEnumerator MoveToTarget(GameObject target, Vector3 destination)
    {
    
        isMoving = true;
        while (target.transform.position != destination)
        {
            target.transform.position = Vector2.MoveTowards(target.transform.position, destination, Time.deltaTime * 30);
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
        tween.Kill();
        StartCoroutine(MoveQueue());
    }
}

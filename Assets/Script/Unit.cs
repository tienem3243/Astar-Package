using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] UnitMovement movement;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] Timer timer;
    [SerializeField] float lifeTime=5;
    [SerializeField] Soul soul;

    public Soul Soul { get => soul; set => soul = value; }

    private void Start()
    {
        timer.StartTimer(lifeTime,()=> Death());
    }

    private void Death()
    {
        Soul obj= Instantiate(soul, transform.position,Quaternion.identity);
        soul.gameObject.SetActive(true);
        Destroy(gameObject, 0);
    }
    public int Kill()
    {
        Death();
        return soul.Power;
    }
}

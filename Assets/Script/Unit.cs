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
    public UnitMovement Movement { get => movement;  }

    private void Start()
    {
        timer.StartTimer(lifeTime,()=> Death());
    }
 
    private void Death()
    {
        Soul obj= Instantiate(soul, transform.position,Quaternion.identity);
        soul.gameObject.SetActive(true);
       
    }
    public int Kill()
    {
        Destroy(gameObject, 0);
       
        return soul.Power;
    }

    private void OnDestroy()
    {
        UnitController.Instance.allUnits.Remove(this);
    }
}

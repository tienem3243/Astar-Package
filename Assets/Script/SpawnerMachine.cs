using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnerMachine : MonoBehaviour
{
    [Tooltip("time between each spawn")]
    public float SpawnRate = 10;
    [SerializeField] private Unit minion;
    public bool isEnable;
    private void Start()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        while (true)
        {
            if (isEnable)
            {
                Vector2 targetPos = (Vector2)transform.position + new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));
                 GridMap.Instance.GetXY(targetPos, out int x, out int y);
                Vector2 normalizePos=GridMap.Instance.GetWorldPosition(x, y);
                Unit unit= Instantiate(minion, normalizePos, Quaternion.identity );
                unit.transform.parent = transform;
                UnitController.Instance.allUnits.Add(unit);
            }
            yield return new WaitForSeconds(SpawnRate);
        }
    }
}

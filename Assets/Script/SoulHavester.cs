using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulHavester : MonoBehaviour
{
    private int storagePower=5;
    [SerializeField]private int storageQueue;
    private List<Unit> inQueueUnits= new List<Unit>();
    public int StoragePower{ get => StoragePower; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Unit"))
        {
            inQueueUnits.Add(collision.GetComponent<Unit>());
            if (inQueueUnits.Count >= storageQueue)
            {
                ConvertUnitToEnergy();
            }
        }

    }
    private void ConvertUnitToEnergy()
    {
        if (inQueueUnits.Count > 0)
        {
            inQueueUnits.ForEach(x =>storagePower+= x.Kill());
            inQueueUnits.Clear();
        }
    }
}

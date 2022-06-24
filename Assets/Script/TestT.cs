using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestT : MonoBehaviour
{
    public int[] a = new int[] { 1, 2, 3, 4, 5 };
    public int off;
    [ContextMenu("test")]
    public void test()
    {
        a = MoveByOffset(a, off);
    }
    public int[] MoveByOffset(int[] array, int offset)
    {
        int[] offArray = new int[array.Length];
        int move = Mathf.Abs(offset);
        move = move % array.Length;
        if (offset > 0)
        {
            //  offset %= array.Length;
            for (int i = 0; i < array.Length; i++)
            {
                offArray[(i + move) % array.Length] = a[i];
            }
            return offArray;
        }
        if (offset < 0)
        {
            //offset %= -array.Length;
            for (int i = 0; i < array.Length; i++)
            {
                offArray[(array.Length + i - move) % array.Length] = a[i];
            }
            return offArray;
        }
        return array;
    }

    public int LargestChildArray(int[] a)
    {
        List<int> sorted = new List<int>(a);
        List<int> length = new List<int>();
        List<int> childArr = new List<int>();
        string s = "";
        //sort to format child array unit alway inscrease
        sorted.Sort();
        //handle 
        for (int i = 0; i < sorted.Count; i++)
        {
            childArr.Add(sorted[i]);

            for (int j = i + 1; j < sorted.Count; j++)
            {
                if (Mathf.Abs(sorted[i] - sorted[j]) <= 1)
                {
                    childArr.Add(sorted[j]);
                }
            }

            length.Add(childArr.Count);
            childArr.Clear();

        }
        //sort and get highest lenght
        length.Sort();
        int res = length[length.Count - 1];
        return res;
    }
}

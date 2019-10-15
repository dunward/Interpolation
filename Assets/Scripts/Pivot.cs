using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pivot : IEnumerable
{
    private int position = -1;

    public List<float> grids = new List<float>();

    public float this[int index]
    {
        get
        {
            return grids[index];
        }
        set
        {
            grids[index] = value;
        }
    }

    public void Add(float v)
    {
        grids.Add(v);
    }

    public int Count
    {
        get
        {
            return grids.Count;
        }
    }

    #region Iterator Interface
    public IEnumerator GetEnumerator()
    {
        for (int i = 0; i < grids.Count; i++)
        {
            yield return grids[i];
        }
    }
        #endregion
}
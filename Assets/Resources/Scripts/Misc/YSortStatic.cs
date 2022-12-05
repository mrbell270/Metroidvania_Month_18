using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YSortStatic : MonoBehaviour
{
    void Start()
    {
        GetComponent<SpriteRenderer>().sortingOrder = transform.GetSortingOrder();
    }

    public void Recalibrate()
    {
        GetComponent<SpriteRenderer>().sortingOrder = transform.GetSortingOrder();
    }
}

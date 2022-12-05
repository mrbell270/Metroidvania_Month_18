using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YSortDynamic : MonoBehaviour
{

    int baseSortingOrder;
    [SerializeField]
    SortableSprite[] sortableSprites;
    [SerializeField]
    Transform baseSortingPoint;

    private void Start()
    {
        if (baseSortingPoint == null)
        {
            baseSortingPoint = transform;
        }
    }
    void Update()
    {
        baseSortingOrder = transform.GetSortingOrder(baseSortingPoint.localPosition.y);
        foreach(SortableSprite ss in sortableSprites)
        {
            ss.spriteRenderer.sortingOrder = baseSortingOrder + ss.relativeOrder;
        }
    }

    [Serializable]
    public struct SortableSprite
    {
        public SpriteRenderer spriteRenderer;
        public int relativeOrder;
    }
}

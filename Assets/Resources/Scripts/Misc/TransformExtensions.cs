
using UnityEngine;

public static class TransformExtensions
{
    public static int GetSortingOrder(this Transform transform, float offset = 0f)
    {
        return -Mathf.RoundToInt(1000 * (transform.position.y + offset));
    }
}

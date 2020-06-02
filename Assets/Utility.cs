using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    public static Vector3 GetMouseWorldPosition(Camera worldCamera, Vector3 mousePosition)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(mousePosition);
        worldPosition.z = 0;
        return worldPosition;
    }

    public static Vector3 GetMouseWorldPosition()
    {
        return GetMouseWorldPosition(Camera.main, Input.mousePosition);
    }

    public static List<T> ShuffleList<T>(List<T> list)
    {
        int count = list.Count;
        for (var i = 0; i < count - 1; ++i)
        {
            int r = UnityEngine.Random.Range(i, count);
            T tmp = list[i];
            list[i] = list[r];
            list[r] = tmp;
        }
        return list;
    }

    public static T ReturnRandom<T>(T[] array)
    {
        if (array != null && array.Length > 0)
        {
            return array[Random.Range(0, array.Length)];
        }
        Debug.LogWarning("Array empty");
        return default;
    }
}

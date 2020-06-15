using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

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

    public static T ReturnRandom<T>(List<T> list)
    {
        if (list != null && list.Count > 0)
        {
            return list[Random.Range(0, list.Count)];
        }
        Debug.LogWarning("List empty");
        return default;
    }


    /// <summary>
    //	This makes it easy to create, name and place unique new ScriptableObject asset files.
    /// </summary>
    public static T CreateAsset<T>(string pathFolderName, string assetName) where T : ScriptableObject
    {
        T asset = ScriptableObject.CreateInstance<T>();

        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (path == "")
        {
            path = "Assets";
            if (pathFolderName != null)
            {
                path += "/" + pathFolderName;
            }
        }
        else if (Path.GetExtension(path) != "")
        {
            path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
        }

        string assetPathAndName = "";

        if (assetName == "")
        {
            assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New " + typeof(T).ToString() + ".asset");
        }
        else
        {
            assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/" + assetName + ".asset");
        }

        AssetDatabase.CreateAsset(asset, assetPathAndName);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
        return asset;
    }

    public static float PercentageToFactor(int wholePercentage)
    {
        return (float)wholePercentage / 100;
    }

    public static int FactorToPercentage(float floatPercentage)
    {
        return Mathf.RoundToInt(floatPercentage * 100);
    }

    public static string FactorToPercentageText(float floatPercentage)
    {
        int percentage = FactorToPercentage(floatPercentage);
        return percentage.ToString() + "%";
    }
}

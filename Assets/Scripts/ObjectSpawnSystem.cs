using UnityEngine;

public enum ObjectType { BarrelOfAle }

public class ObjectSpawnSystem : MonoBehaviour
{
    [SerializeField] HexObject[] objects = null;
    [SerializeField] Transform objectParent = null;

    #region Singleton
    public static ObjectSpawnSystem instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Another instance of : " + instance.ToString() + " was tried to be instanced, but was destroyed from gameobject: " + this.transform.name);
            GameObject.Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
    #endregion

    public void SpawnObject(ObjectType type, HexCell cellToSpawnOn)
    {
        GameObject objToSpawn = objects[(int)type].gameObject;
        HexObject spawnedObject = Instantiate(objToSpawn).GetComponent<HexObject>();
        spawnedObject.transform.SetParent(objectParent);
        spawnedObject.Location = cellToSpawnOn;
    }
}

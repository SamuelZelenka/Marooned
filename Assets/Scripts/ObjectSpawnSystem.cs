using UnityEngine;


public class ObjectSpawnSystem : MonoBehaviour
{
    [SerializeField] HexObject[] objects = null;
    [SerializeField] ActiveHexObject[] activeObjects = null;

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

    public HexObject SpawnNormalObject(int objectIndex, HexCell cellToSpawnOn)
    {
        GameObject objToSpawn = objects[objectIndex].gameObject;
        HexObject spawnedObject = Instantiate(objToSpawn).GetComponent<HexObject>();
        spawnedObject.transform.SetParent(objectParent);
        spawnedObject.Location = cellToSpawnOn;
        return spawnedObject;
    }

    public ActiveHexObject SpawnActiveObject(int objectIndex, HexCell cellToSpawnOn, int changePerTurn, Character connectedToCharacter)
    {
        GameObject objToSpawn = activeObjects[objectIndex].gameObject;
        ActiveHexObject spawnedObject = Instantiate(objToSpawn).GetComponent<ActiveHexObject>();
        spawnedObject.transform.SetParent(objectParent);
        spawnedObject.Location = cellToSpawnOn;
        spawnedObject.SetupObject(changePerTurn, connectedToCharacter);
        return spawnedObject;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{

    [System.Serializable]
    public class PoolCategory
    {
        public string name;
        public List<GameObject> _object = new List<GameObject>();
    }

    public PoolCategory[] pooledObjects;
    private bool spawnNewObj = false;

    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    private void Awake()
    {
        for (int i = 0; i < pooledObjects.Length; i++)
            for (int y = 0; y < pooledObjects[i]._object.Count; y++)
            {
                pooledObjects[i]._object[y].transform.position = new Vector3(0, -200, 0);
                pooledObjects[i]._object[y].SetActive(false);
            }
    }

    private GameObject GetObjectFromPool(int _IndexOne, int _IndexTwo)
    {
        return pooledObjects[_IndexOne]._object[_IndexTwo];
    }

    public void PoolObject(int _Category, Transform spawnPos)
    {

        for (int i = 0; i < pooledObjects[_Category]._object.Count; i++)
        {
            if (!pooledObjects[_Category]._object[i].activeInHierarchy)
            {
                pooledObjects[_Category]._object[i].transform.position = spawnPos.position;
                pooledObjects[_Category]._object[i].SetActive(true);
                pooledObjects[_Category]._object[i].transform.rotation = spawnPos.rotation;
                spawnNewObj = false;
                break;
            }
            else
                spawnNewObj = true;
        }
        if (spawnNewObj)
        {
            GameObject NewelyInstantiated = (GameObject)Instantiate(pooledObjects[_Category]._object[0]);
            NewelyInstantiated.SetActive(true);
            NewelyInstantiated.transform.position = spawnPos.position;
            NewelyInstantiated.transform.rotation = spawnPos.rotation;
            pooledObjects[_Category]._object.Add(NewelyInstantiated.gameObject);
        }
    }
}

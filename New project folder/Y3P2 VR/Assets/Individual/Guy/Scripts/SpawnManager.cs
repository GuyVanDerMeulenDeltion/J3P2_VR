using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnManager : MonoBehaviourPunCallbacks {

    public static SpawnManager spawnManager;

    [System.Serializable]
    public struct ItemSpawns {
        public Transform _Pos;
        public GameObject _Item;
    }

    [System.Serializable]
    public struct Entity {
        public Transform[] _Pos;
        public GameObject _Entity;
    }

    public List<ItemSpawns> items = new List<ItemSpawns>();
    public List<Entity> entities = new List<Entity>();

    private void Awake() {
        if (spawnManager != null) Destroy(this);
        spawnManager = this;
    }

    public void SpawnScenery() {
        SpawnItems();
        SpawnEntites();
    }

    private void SpawnItems() {
        foreach (ItemSpawns _Spawn in items) {
            if (PhotonNetwork.IsConnected)
                PhotonNetwork.InstantiateSceneObject(_Spawn._Item.name, _Spawn._Pos.position, Quaternion.identity);
            else
                Instantiate(_Spawn._Item, _Spawn._Pos.position, Quaternion.identity);
        }
    }

    private void SpawnEntites() {
        foreach (Entity _Spawn in entities) {
            foreach (Transform _Pos in _Spawn._Pos) {
                if (PhotonNetwork.IsConnected)
                    PhotonNetwork.Instantiate(_Spawn._Entity.name, _Pos.position, Quaternion.identity);
                else
                    Instantiate(_Spawn._Entity, _Pos.position, Quaternion.identity);
            }
        }
    }
}

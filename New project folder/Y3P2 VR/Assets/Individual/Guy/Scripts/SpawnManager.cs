using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnManager : MonoBehaviourPunCallbacks {

    public static SpawnManager spawnManager;
    public static List<Enemy> enemies;

    [SerializeField] private GameObject spawnParticles;

    [System.Serializable]
    public struct ItemSpawns {
        public string itemType;
        public Transform[] _Pos;
        public GameObject _Item;
    }

    [System.Serializable]
    public struct Entity {
        public Transform _Pos;
        public GameObject _Enemy;
    }

    [System.Serializable]
    public struct EntityChunk
    {
        public Entity[] entitiesToSpawn;
    }

    public List<ItemSpawns> items = new List<ItemSpawns>();
    public List<EntityChunk> entitiesToSpawn = new List<EntityChunk>();

    private void Awake() {
        if (spawnManager != null) Destroy(this);
        spawnManager = this;
    }

    private void Start() {
        if (PhotonNetwork.IsMasterClient)
            SpawnScenery();
    }

    public void SpawnScenery() {
        SpawnItems();
    }

    internal void SpawnItems() {
        foreach (ItemSpawns _Spawn in items) {
            if (_Spawn._Item != null) {
                foreach (Transform _Pos in _Spawn._Pos) {
                    if (PhotonNetwork.IsConnected)
                        PhotonNetwork.InstantiateSceneObject(_Spawn._Item.name, _Pos.position, _Pos.rotation);
                    else
                        Instantiate(_Spawn._Item, _Pos.position, Quaternion.identity);
                }
            }
        }
    }

    internal void SpawnEntites(int _Chunk) {
        EntityChunk _ToBeSpawned = entitiesToSpawn[_Chunk];

        foreach (Entity _Entity in _ToBeSpawned.entitiesToSpawn)
        {
            if (_Entity._Enemy && _Entity._Pos) //If there is a enemy to spawn and a position to spawn it at.
            {
                if (PhotonNetwork.IsConnected)
                {
                    PhotonNetwork.Instantiate(_Entity._Enemy.name, _Entity._Pos.position, _Entity._Pos.rotation);
                    photonView.RPC("SetParticles", RpcTarget.All, _Entity._Pos.position, _Entity._Pos.eulerAngles);
                }
                else
                {
                    Instantiate(_Entity._Enemy, _Entity._Pos.position, _Entity._Pos.rotation);
                    SetParticles(_Entity._Pos.position, _Entity._Pos.rotation.eulerAngles);
                }
            }
        }
    }

    [PunRPC]
    private void SetParticles(Vector3 _Pos, Vector3 _Eulers)
    {
        Instantiate(spawnParticles, _Pos, Quaternion.Euler(_Eulers));
    }
}

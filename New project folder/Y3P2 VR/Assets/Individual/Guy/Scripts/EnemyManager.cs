using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using System;

[RequireComponent(typeof(PhotonView))]
public class EnemyManager : MonoBehaviourPunCallbacks {

    public static EnemyManager enemyManager;

    internal void SetEnemyTotalHit(int viewID) {
        throw new NotImplementedException();
    }

    private void Awake() {
        if (enemyManager != null) Destroy(this);
        enemyManager = this;
    }

    public void SetEnemyTotalHit(int _i, int _Hit, Vector3 _Velocity) {
        if (PhotonNetwork.IsConnected)
            photonView.RPC("GetEnemyTotalHit", RpcTarget.MasterClient, _i, _Hit, _Velocity);
        else
            GetEnemyTotalHit( _i, _Hit, _Velocity);
    }

    public void SetNewTarget(int _i) {
        if (PhotonNetwork.IsConnected)
            photonView.RPC("GetNewTarget", RpcTarget.MasterClient, _i);
        else
            GetNewTarget(_i);
    }

    public void SetEnemyAnimation(int _i, bool _State) {
        if (PhotonNetwork.IsConnected)
            photonView.RPC("GetEnemyAnimation", RpcTarget.AllBuffered, _i , _State);
        else
            GetEnemyAnimation(_i, _State);
    }

    public void SetEnemyHealth(int _i, float _maxHealth, float _health) {
        if (PhotonNetwork.IsConnected)
            photonView.RPC("GetEnemyHealth", RpcTarget.AllBuffered, _i, _maxHealth, _health);
        else
            GetEnemyHealth(_i, _maxHealth, _health);
    }

    public void SetEnemyHitsplash(Vector3 _Pos, int _Damage) {
        if (PhotonNetwork.IsConnected)
            photonView.RPC("GetEnemyHitsplash", RpcTarget.All, _Pos, _Damage);
        else
            GetEnemyHitsplash(_Pos, _Damage);
    }
 
    public void SetEnemyDeath(int _i, Vector3 _SplashPos) {
        if (PhotonNetwork.IsConnected)
            photonView.RPC("GetEnemyDeath", RpcTarget.AllBuffered, _i, _SplashPos);
        else
            GetEnemyDeath(_i, _SplashPos);
    }

    public void SetEnemyRagdoll(int _i, bool _State) {
        if (PhotonNetwork.IsConnected)
            photonView.RPC("GetEnemyRagdoll", RpcTarget.AllBuffered, _i, _State);
        else
            GetEnemyRagdoll(_i, _State);
    }

    public void SetEnemyMessage(Vector3 _Pos, string _Message) {
        if (PhotonNetwork.IsConnected)
            photonView.RPC("GetEnemyMessage", RpcTarget.All, _Pos, _Message);
        else
            GetEnemyMessage(_Pos, _Message);
    }

    public void SetEnemyHit(int _i, Vector3 _Velocity, float _Damage) {
        if (PhotonNetwork.IsConnected)
            photonView.RPC("GetEnemyHit", RpcTarget.MasterClient, _i, _Velocity, _Damage);
        else
            GetEnemyHit(_i, _Velocity, _Damage);
    }

    [PunRPC]
    private void GetNewTarget(int _i) {
        foreach (PhotonView _View in PhotonNetwork.PhotonViews)
            if (_View.ViewID == _i && _View.gameObject.tag == "Player") {
                Enemy.players.Add(_View.transform.GetChild(2).transform);
                return;
            }
    }

    [PunRPC]
    private void GetEnemyAnimation(int _i, bool _State) {
        foreach(PhotonView _View in PhotonNetwork.PhotonViews) {
            if(_View.ViewID == _i) {
                _View.GetComponent<Animator>().SetBool("Walk", _State);
                return;
            }
        }
    }

    [PunRPC]
    private void GetEnemyHealth(int _i, float _maxHealth, float _health) {
        foreach(PhotonView _View in PhotonNetwork.PhotonViews) {
            if(_View.ViewID == _i) {
                Image[] _HealthUI = _View.GetComponentsInChildren<Image>();
                foreach(Image _Image in _HealthUI) {
                    if(_Image.gameObject.tag == "Health") {
                        _Image.fillAmount = _health / _maxHealth;
                    }
                }
            }
        }
    }

    [PunRPC]
    private void GetEnemyTotalHit(int _ViewID, int _Hit, Vector3 _Velocity) {
        foreach(PhotonView _View in PhotonNetwork.PhotonViews) {
            if(_ViewID == _View.ViewID) {
                _View.GetComponent<Enemy>().GetDamaged(_Hit, _Velocity);
                return;
            }
        }
    }

    [PunRPC]
    private void GetEnemyHitsplash(Vector3 _Pos, int _Damage) {
        GameObject _Hitsplash = (GameObject)Instantiate(Resources.Load("Hitsplash"), _Pos, Quaternion.identity);
        _Hitsplash.GetComponentInChildren<TextMeshProUGUI>().text = _Damage.ToString();
    }

    [PunRPC]
    private void GetEnemyDeath(int _i, Vector3 _SplashPos) {
        GameObject _KOMessage = (GameObject)Instantiate(Resources.Load("Hitsplash"), _SplashPos, transform.rotation);
        _KOMessage.GetComponentInChildren<TextMeshProUGUI>().text = "KO'd!";
        _KOMessage.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
        foreach(PhotonView _View in PhotonNetwork.PhotonViews) {
            if(_View.ViewID == _i) {
                Destroy(_View.transform.GetChild(2));
                return;
            }
        }
    }

    [PunRPC]
    private void GetEnemyRagdoll(int _i, bool _State) {
        foreach(PhotonView _View in PhotonNetwork.PhotonViews) {
            if(_View.ViewID == _i) {
                _View.GetComponent<KekDoll>().doRagdoll = _State;

                if (!PhotonNetwork.IsMasterClient)
                    _View.GetComponent<PhotonTransformView>().enabled = !_State;
            }
        }
    }

    [PunRPC]
    private void GetEnemyMessage(Vector3 _Pos, string _Message) {
        GameObject _Hitsplash = (GameObject)Instantiate(Resources.Load("Hitsplash"), _Pos, Quaternion.identity);
        _Hitsplash.GetComponentInChildren<TextMeshProUGUI>().text = _Message;
        _Hitsplash.GetComponentInChildren<TextMeshProUGUI>().color = Color.magenta;
    }

    [PunRPC]
    private void GetEnemyHit(int _i, Vector3 _Velocity, float Damage) {
        foreach(PhotonView _View in PhotonNetwork.PhotonViews) {
            if(_View.ViewID == _i && _View.transform.tag == "Enemy") {
                _View.GetComponent<Rigidbody>().velocity = _Velocity;
                _View.GetComponent<Enemy>().health -= Damage;
                return;
            }
        }
    }
}

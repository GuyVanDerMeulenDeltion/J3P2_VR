using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using UnityEngine;
using Photon.Pun;

public class InteractionManager : MonoBehaviourPunCallbacks
{
    public static InteractionManager intManager;

    public GameObject leftHandSpawnPos { get { return (transform.GetChild(0).gameObject); } }
    public GameObject rightHandSpawnPos { get { return (transform.GetChild(1).gameObject); } }

    private void Awake() {
        intManager = this;
        print(intManager);
    }

    public void PickObjectNetwork(int _View, int pickUpObject, int hasItem, bool itemStatus) {
        if (PhotonNetwork.IsConnected)
            photonView.RPC("PickObject", RpcTarget.AllBuffered, _View, pickUpObject, hasItem, itemStatus);
        else
            PickObject(_View, pickUpObject, hasItem, itemStatus);
    }

    public void DropObjectNetwork(int _View, GameObject throwable, SteamVR_Behaviour_Pose trackedObj) {
        if (PhotonNetwork.IsConnected)
            photonView.RPC("ThrowObject", RpcTarget.AllBuffered, _View, throwable, trackedObj);
        else
            ThrowObject(_View, throwable, trackedObj);
    }

    [PunRPC]
    public void PickObject(int _View, int pickUpObject, int hasItem, bool itemStatus)
    {
        GameObject _Hand = null;
        GameObject _PickedupObject = null;
        GameObject _HasItem = null;

        foreach (PhotonView _view in PhotonNetwork.PhotonViews) {
            if (_view.ViewID == _View) {
                _Hand = _view.gameObject;
                break;
            }
        }

        foreach (PhotonView _view in PhotonNetwork.PhotonViews) {
            if (_view.ViewID == pickUpObject) {
                _PickedupObject = _view.gameObject;
                break;
            }
        }

        foreach (PhotonView _view in PhotonNetwork.PhotonViews) {
            if (_view.ViewID == hasItem && itemStatus == true) {
                _HasItem = _view.gameObject;
                break;
            }
        }

        print(_PickedupObject +" is pickedup");
        print(_Hand + " is hand");

        if (_HasItem == null)
        {
            _PickedupObject.transform.SetParent(_Hand.transform);
            _PickedupObject.GetComponent<Rigidbody>().isKinematic = true;
            _PickedupObject.GetComponent<Rigidbody>().useGravity = false;
            if (_PickedupObject.GetComponent<Interactables>() != null)
                _PickedupObject.GetComponent<Interactables>().enabled = true;
            _PickedupObject.GetComponent<Transform>().position = _Hand.transform.position;
            _PickedupObject.GetComponent<Transform>().rotation = _Hand.transform.rotation;
            _Hand.GetComponent<Controller>().item = _PickedupObject;
        }
    }

    [PunRPC]
    public void ThrowObject(int _View, GameObject throwable, SteamVR_Behaviour_Pose trackedObj)
    {
        GameObject _Hand = null;

        foreach (PhotonView _view in PhotonNetwork.PhotonViews) {
            if (_view.ViewID == _View) {
                _Hand = _view.gameObject;
                break;
            }
        }

        if (throwable != null && _Hand != null)
        {
            _Hand.GetComponent<Controller>().item = null;
            throwable.transform.SetParent(null);
            if (throwable.GetComponent<Interactables>() != null)
                throwable.GetComponent<Interactables>().enabled = false;
            throwable.GetComponent<Rigidbody>().isKinematic = false;
            throwable.GetComponent<Rigidbody>().useGravity = true;
            throwable.GetComponent<Rigidbody>().velocity = trackedObj.GetVelocity();
            throwable.GetComponent<Rigidbody>().angularVelocity = trackedObj.GetAngularVelocity();
        }
    }
}

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

    private void Awake()
    {
        intManager = this;
    }

    public override void OnEnable()
    {
        intManager = this;
    }

    public void PickObjectNetwork(int _View, int pickUpObject, int hasItem, bool itemStatus) {
        if (PhotonNetwork.IsConnected)
            photonView.RPC("PickObject", RpcTarget.MasterClient, _View, pickUpObject, hasItem, itemStatus);
        else
            PickObject(_View, pickUpObject, hasItem, itemStatus);
    }

    public void DropObjectNetwork(int _View, int throwable) {
        if (PhotonNetwork.IsConnected)
            photonView.RPC("ThrowObject", RpcTarget.MasterClient, _View, throwable);
        else
            ThrowObject(_View, throwable);
    }

    private PhotonView GetView(int _ID)
    {
        PhotonView _NewView = null;
        foreach(PhotonView _View in PhotonNetwork.PhotonViews)
        {
            if (_View.ViewID == _ID)
            {
                _NewView = _View;
                break;
            }
        }

        return _NewView;
    }

    [PunRPC]
    public void PickObject(int _View, int pickUpObject, int hasItem, bool itemStatus)
    {
        GameObject _Hand = GetView(_View).gameObject;
        GameObject _PickedupObject = GetView(pickUpObject).gameObject;
        GameObject _HasItem = null;
        if (GetView(hasItem) != null)
            _HasItem = GetView(hasItem).gameObject;

        if (PhotonNetwork.IsMasterClient)
        {
            if (_HasItem == null)
            {
                    _PickedupObject.transform.SetParent(_Hand.transform);
                    _PickedupObject.GetComponent<Rigidbody>().isKinematic = true;
                    _PickedupObject.GetComponent<Rigidbody>().useGravity = false;

                    if (_PickedupObject.GetComponent<Interactables>() != null)
                        _PickedupObject.GetComponent<Interactables>().enabled = true;

                _PickedupObject.GetComponent<Transform>().position = _Hand.transform.position;
                _PickedupObject.GetComponent<Transform>().rotation = _Hand.transform.rotation;

                if (_Hand.GetPhotonView().IsMine)
                    _Hand.GetComponent<Controller>().item = _PickedupObject;
            }
        }

        GetView(pickUpObject).TransferOwnership(_Hand.GetPhotonView().Owner);
    }

    [PunRPC]
    public void ThrowObject(int _View, int _throwable)
    {
        GameObject _Hand = null;
        GameObject _Throwable = null;
        SteamVR_Behaviour_Pose _TrackedObj = null;

        foreach (PhotonView _view in PhotonNetwork.PhotonViews) {
            if (_view.ViewID == _View) {
                _Hand = _view.gameObject;
                _TrackedObj = _view.GetComponent<SteamVR_Behaviour_Pose>();
                break;
            }
        }

        foreach (PhotonView _view in PhotonNetwork.PhotonViews) {
            if (_view.ViewID == _throwable) {
                _Throwable = _view.gameObject;
                break;
            }
        }

        if (_Throwable != null && _Hand != null)
        {
            if(_Hand.GetPhotonView().IsMine)
            _Hand.GetComponent<Controller>().item = null;

            if (PhotonNetwork.IsMasterClient) {

                if (_Throwable.GetComponent<Interactables>() != null)
                    _Throwable.GetComponent<Interactables>().enabled = false;

                _Throwable.transform.SetParent(null);
                _Throwable.GetComponent<Rigidbody>().isKinematic = false;
                _Throwable.GetComponent<Rigidbody>().useGravity = true;
                _Throwable.GetComponent<Rigidbody>().velocity = _TrackedObj.GetVelocity();
                _Throwable.GetComponent<Rigidbody>().angularVelocity = _TrackedObj.GetAngularVelocity();
            }
        }
    }
}

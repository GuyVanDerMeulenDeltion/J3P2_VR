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
            photonView.RPC("PickObject", RpcTarget.AllBuffered, _View, pickUpObject, hasItem, itemStatus);
        else
            PickObject(_View, pickUpObject, hasItem, itemStatus);
    }

    public void DropObjectNetwork(int _View, int throwable) {
        if (PhotonNetwork.IsConnected)
            photonView.RPC("ThrowObject", RpcTarget.AllBuffered, _View, throwable);
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

        if(_PickedupObject.GetComponentInParent<Controller>()) {
            if (_PickedupObject.GetComponent<Sword>())
                _PickedupObject.GetComponent<Sword>().enabled = true;

            if (_PickedupObject.GetComponent<Shield>())
                _PickedupObject.GetComponent<Shield>().enabled = true;

            if (!_PickedupObject.GetComponentInParent<Controller>().gameObject.GetPhotonView().IsMine)
            {
                Controller _Cont = _PickedupObject.GetComponentInParent<Controller>();
                ThrowObject(_Cont.gameObject.GetPhotonView().ViewID, pickUpObject);
            }
        }

        _PickedupObject.GetComponent<Rigidbody>().isKinematic = true;

        if (GetView(_View).IsMine ) {
        GameObject _HasItem = null;
        if (GetView(hasItem) != null)
            _HasItem = GetView(hasItem).gameObject;

        GetView(pickUpObject).TransferOwnership(_Hand.GetPhotonView().Owner);

            if (_HasItem == null)
            {
                _PickedupObject.transform.SetParent(_Hand.transform);

                if (_PickedupObject.GetComponent<Interactables>() != null)
                    _PickedupObject.GetComponent<Interactables>().enabled = true;

                _PickedupObject.GetComponent<Transform>().position = _Hand.transform.position;
                _PickedupObject.GetComponent<Transform>().rotation = _Hand.transform.rotation;
                _Hand.GetComponent<Controller>().item = _PickedupObject;
            }       
        }
    }

    [PunRPC]
    public void ThrowObject(int _View, int _throwable)
    {
        GameObject _Hand = GetView(_View).gameObject;
        SteamVR_Behaviour_Pose _TrackedObj = _Hand.GetComponent<SteamVR_Behaviour_Pose>();
        GameObject _Throwable = null;
        if (GetView(_throwable) != null)
        {
            _Throwable = GetView(_throwable).gameObject;
        }

        if (_Throwable != null && _Hand != null)
        {
            if (_Hand.GetPhotonView().IsMine)
            {
                _Hand.GetComponent<Controller>().item = null;
                if (_Throwable.GetComponent<Sword>())
                    _Throwable.GetComponent<Sword>().enabled = false;

                if (_Throwable.GetComponent<Shield>())
                    _Throwable.GetComponent<Shield>().enabled = false;
            }
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

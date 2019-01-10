using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Teleport : MonoBehaviour {

    private Transform playerHead;
    [Header("Debugging:")]
    [SerializeField]private bool canTeleport = false;

    [Header("Reticle Settings:")]
    [SerializeField]private Vector3 reticleOffset = Vector3.zero;

    [Header("Teleport Settings:")]
    [SerializeField]private GameObject reticle;
    [SerializeField]private float maxRange = 100f;
    [SerializeField]private LayerMask teleportMask;
    public float teleportLerpSpeed = 5;

    [Header("Line Settings:")]
    [SerializeField]private Vector2 lineWidth;
    [SerializeField]private Vector2 textureOffset;
    [SerializeField]private Material lineMaterial;
    [SerializeField]private Texture canTeleportImage;
    [SerializeField]private Texture cannotTeleportImage;
    [SerializeField]private int lineVertexCount = 10;

    [Header("Controllers")]
    [SerializeField] public GameObject leftController;
    [SerializeField] public GameObject rightController;

    #region References
    private RaycastHit hitInfo;
    private LineRenderer thisLine;
    #endregion


    private void Awake() {
        thisLine = GetComponent<LineRenderer>();
        thisLine.SetWidth(lineWidth.x, lineWidth.y);
        thisLine.positionCount = lineVertexCount;
        thisLine.material = lineMaterial;
        playerHead = transform.root.GetComponentInChildren<Camera>().transform;
        this.enabled = false;
    }

    private void OnDisable() {
        thisLine.enabled = false;
        reticle.SetActive(false);
    }

    private void OnEnable() {
        thisLine.enabled = true;
        reticle.SetActive(true);
    }

    //Main function of this script;
    internal void TeleportPlayer() {
        if(canTeleport == true) {
            Vector3 _OldReticlePosition = reticle.transform.position;
            float _NewHeightDecrement = playerHead.transform.position.y - transform.root.position.y;
            Vector3 _Offset = transform.root.position - playerHead.transform.position;
            Vector3 _Plus = reticle.transform.position + _Offset;
            Vector3 _Final = new Vector3(_Plus.x, _Plus.y + _NewHeightDecrement, _Plus.z);
            Controller.currentPos = _Final;
            reticle.transform.position = _OldReticlePosition;
        }
    }

    private void Update() {
        UpdateTextureOffset(); 
        CheckForHits();
    }

    private void UpdateTextureOffset() {
        thisLine.material.mainTextureOffset -= textureOffset * Time.deltaTime;
    }


    private void CheckForHits() {
            if (Physics.Raycast(transform.position, transform.forward, out hitInfo, maxRange ,teleportMask)) {
                reticle.transform.position = hitInfo.point;
                reticle.SetActive(true);
                canTeleport = true;
                thisLine.SetPositions(new Vector3[] { transform.position, hitInfo.point });
                thisLine.material.mainTexture = canTeleportImage;
        } else {
            thisLine.SetPositions(new Vector3[] { transform.position, transform.forward * 5 + transform.position });
            reticle.SetActive(false);
                canTeleport = false;
            thisLine.material.mainTexture = cannotTeleportImage;
        }
        }
    }

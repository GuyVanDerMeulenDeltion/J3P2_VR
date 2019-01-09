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
    [SerializeField]private int lineVertexCount = 10;
    [SerializeField]private Vector3 arcOffset = new Vector3(0, 10, 0);

    [Header("Controllers")]
    [SerializeField] public GameObject leftController;
    [SerializeField] public GameObject rightController;

    [Header("Ray Settings:")]
    [SerializeField]private float radius = 5.0f;
    [SerializeField]private int segments = 12;
    [SerializeField]private float curveAmount = 360.0f;

    private Vector3 arcStatistics = Vector3.one;
    private Vector3 reticleEulers;
    private float calcAngle;
    private List<Vector3> nodes;

    private int radiusMax = 10;
    #region References
    private RaycastHit hitInfo;
    private LineRenderer thisLine;
    #endregion


    private void Awake() {
        thisLine = GetComponent<LineRenderer>();
        reticleEulers = reticle.transform.eulerAngles;
        thisLine.SetWidth(lineWidth.x, lineWidth.y);
        thisLine.positionCount = lineVertexCount;
        thisLine.material = lineMaterial;
        playerHead = transform.root.GetComponentInChildren<Camera>().transform;
    }

    public void Start() {
        nodes = new List<Vector3>();
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
        DrawRayReticle();
        DrawLine();
        UpdateTextureOffset();
        arcStatistics = transform.position - arcOffset;
        CalculatePoints();
        DrawLines();    // just for testing    
        CheckForHits();
    }

    private void UpdateTextureOffset() {
        thisLine.material.mainTextureOffset -= textureOffset * Time.deltaTime;
    }

    private void DrawLine() {
        if (canTeleport == false)
        {
            thisLine.positionCount = 2;
            thisLine.SetPositions(new Vector3[] { transform.position, transform.forward * 1.25f + transform.position });
            return;
        }

        thisLine.positionCount = lineVertexCount;
        float _Distance = Vector3.Distance(transform.position, reticle.transform.position);

        //Sets the array length to the line vertex count;
        Vector3[] _Points = new Vector3[lineVertexCount];
        float _Angles = ReturnAngles(transform.position, reticle.transform.position); //The alpha of the equation;
        float Change_Theta = _Angles / (lineVertexCount - 1); //A constantly modified value to differ the output of the final 'theta';
        _Points[0] = transform.position;
        _Points[_Points.Length - 1] = reticle.transform.position;

        //Changes the theta and calculates the new position afterwards;
        for (int i = 1; i < _Points.Length - 1; i++) {
            float _Theta = (i - 1) * Change_Theta;
                _Points[i] = CalculateFinalPos(transform.position, arcStatistics, reticle.transform.position, _Angles, _Theta);
        }

        //Sets the array of newfound positions into the linerenderer;
        thisLine.SetPositions(_Points);
    }

    //Calculates the newely found vertex position;
    private Vector3 CalculateFinalPos(Vector3 _Begin, Vector3 _Middle, Vector3 _End, float _Alpha, float _Theta) {
        return _Middle + (Mathf.Sin(_Alpha - _Theta) * (_Begin - _Middle) + Mathf.Sin(_Theta) * (_End - _Middle)) / Mathf.Sin(_Alpha);
    }
    
    //Calculates the angles of the alpha;
    private float ReturnAngles(Vector3 _Start, Vector3 _End) {
        Vector3 _First = arcStatistics - _Start;
        Vector3 _Second = _End - arcStatistics;
        float _DotProduct = Vector3.Dot(_First, _Second);

        return _DotProduct / (Vector3.Magnitude(_First) * Vector3.Magnitude(_Second)); //Returns the Alpha of the equation;

    }

    private void DrawRayReticle() {
        reticle.transform.eulerAngles = reticleEulers;

        if(Physics.Raycast(reticle.transform.GetChild(0).transform.position, Vector3.down, out hitInfo, maxRange, teleportMask)) {
            reticle.transform.position = new Vector3(reticle.transform.position.x, hitInfo.point.y, reticle.transform.position.z);
            reticle.GetComponent<MeshRenderer>().enabled = enabled;
            return;
        }
            reticle.GetComponent<MeshRenderer>().enabled = false;
    }

    private void CalculatePoints() {
        nodes.Clear();
        calcAngle = 0;

        // Calculate Arc on Y-Z    
        for (int i = 0; i < segments + 1; i++) {
            float posY = Mathf.Cos(calcAngle * Mathf.Deg2Rad) * radius;
            float posZ = Mathf.Sin(calcAngle * Mathf.Deg2Rad) * radius;
            nodes.Add(transform.position + (transform.up * posY) + (transform.forward * posZ));
            calcAngle += curveAmount / (float)segments;
        }
    }

    private void CheckForHits() {
        radius = radiusMax - (transform.eulerAngles.x * transform.eulerAngles.x / 360);
        arcOffset.y = radiusMax - (transform.eulerAngles.x * transform.eulerAngles.x / 360);
        radius = Mathf.Clamp(radius, 0 ,radiusMax);

        for (int i = 0; i < nodes.Count - 1; i++) {
            if (Physics.Linecast(nodes[i], nodes[i + 1], out hitInfo, teleportMask)) {
                reticle.transform.position = hitInfo.point;
                reticle.SetActive(true);
                canTeleport = true;
                break;
            } else {
                reticle.SetActive(false);
                canTeleport = false;
            }
        }
    }

    private void DrawLines() {
        for (int i = 0; i < nodes.Count - 1; i++) {
            Debug.DrawLine(nodes[i], nodes[i + 1], Color.red, 1.5f);
        }
    }
}

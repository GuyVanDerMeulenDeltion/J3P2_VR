using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.AI;
using TMPro;

public class Enemy : MonoBehaviourPunCallbacks {

    public static bool stopAttacking = false;

    public static float timeTillTargetCheck = 0.2f;
    public static float restoreTimer = 4;

    [Header("Presets:")]
    [SerializeField]private float maxHealth = 100;
    [SerializeField]private GameObject deadPrefab;

    [Header("Enemy Hit Settings:")]
    [SerializeField]private float maxHitTimer = 2;

    [Header("NPC Statistics:")]
    [SerializeField]public float health;

    [Header("UI:")]
    [SerializeField]private GameObject healthUI;
    [SerializeField]private Image healthFill;
    [SerializeField]private PlayerManager currentTarget;

    #region Private references
    private MeshRenderer thisRenderer;
    private Animator thisAnim;
    private List<Transform> players;
    private NavMeshAgent thisAgent;
    private Rigidbody thisBody;
    private float hitTimer;
    private bool isAggro = false;
    [SerializeField]private UnityEvent gotHitEvent;
    #endregion

    private bool hit = true;
    private bool started = false;

    public void Update() {
        if(stopAttacking == true) {
            thisAgent.enabled = false;
            this.enabled = false;
        }

        if(Input.GetButtonDown("Fire1")) {
            GetDamaged(3, transform.position, 1);
        }
    }

    private bool CheckIfGrounded() {
        RaycastHit _Hit;

        if(Physics.Raycast(transform.position, new Vector3(0, -1, 0), out _Hit, 1f)) {
            return true;
        }

        return false;
    }

    private void OnTriggerEnter(Collider _O) {
        if (_O.GetComponent<PlayerManager>()) {
            if (isAggro == false) {
                isAggro = true;
                healthUI.SetActive(true);
                AggroEnemy();
                return;
            }
        }
    }

    private void OnTriggerStay(Collider _O) {
        if (currentTarget != null && hit == false) {
            if (_O.gameObject == currentTarget.gameObject) {
                if (PlayerManager.thisPlayer.died == false) {
                    thisAgent.enabled = false;
                    if (hitTimer > 0) {
                        hitTimer -= Time.deltaTime;
                    } else {
                        Attack();
                        hitTimer = maxHitTimer;
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider _O) {
        if (_O.GetComponent<PlayerManager>() && hit == false && stopAttacking == false) {
            thisAgent.enabled = true;
        }
    }

    protected virtual void Attack() {
        PlayerManager.thisPlayer.SetDeath();
        stopAttacking = true;
    }

    private void OnCollisionEnter(Collision collision) {
        if (hit == true && isAggro == true)
            gotHitEvent.Invoke();
    }

    //Sets references
    private void Awake() {
        if (PhotonNetwork.IsConnected && !photonView.IsMine)
            this.enabled = false;

        thisAnim = GetComponent<Animator>();
        thisAgent = GetComponent<NavMeshAgent>();
        thisBody = GetComponent<Rigidbody>();
        thisRenderer = GetComponent<MeshRenderer>();
    }

    //First frame
    private void Start() {
        //healthUI.SetActive(false);
        health = maxHealth;
        hitTimer = maxHitTimer;
    }

    private void AggroEnemy() {
        thisRenderer.enabled = true;
        thisAnim.enabled = true;
    }

    public void StartEnemy() {
        hit = false;
        thisAnim.enabled = false;
        thisAgent.enabled = true;
        StartCoroutine(AssignTargetTimer());
    }

    //Timer before the enemy will be searching for a new target
    private IEnumerator AssignTargetTimer() {
        yield return new WaitForSeconds(timeTillTargetCheck);
        SetTarget();
        StartCoroutine(AssignTargetTimer());
    }

    //Sets new target for the enemy
    private void SetTarget() {
        if (PhotonNetwork.IsConnected) {
            photonView.RPC("CollectPlayers", RpcTarget.MasterClient);
            SetNewTargetTransform(ChooseTarget(players));
            return;
        }

            SetNewTargetTransform(PlayerManager.thisPlayer.transform);
    }

    //Returns all the transforms of all players ingame
    [PunRPC]
    private void CollectPlayers() {
        players.Add(PlayerManager.thisPlayer.transform);
    }

    //Checks between all the players which player is the closest to the enemy
    private Transform ChooseTarget(List<Transform> _Players) {
        float _ClosestDistance = Mathf.Infinity;
        Transform _ClosestPlayer = null;

        if (_Players != null) {
            foreach (Transform _Player in _Players) {
                if (Vector3.Distance(transform.position, _Player.transform.position) < _ClosestDistance) {
                    _ClosestPlayer = _Player;
                    _ClosestDistance = Vector3.Distance(transform.position, _Player.transform.position);
                }
            }
        }

        if (_ClosestPlayer != null)
            return _ClosestPlayer.transform;
        else
            return null;
    }

    //Sets the new target of the enemy based on the overloaded transform
    private void SetNewTargetTransform(Transform _Target) {
        if (hit == false && _Target != null) {
            if(thisAgent.isActiveAndEnabled)
            thisAgent.SetDestination(_Target.position);
            currentTarget = _Target.GetComponent<PlayerManager>();
            players = null;
        }
    }

    //Function handles the hit that has been applied to the enemy
    public void GetDamaged(int _Hit, Vector3 _HitPos, float _Force) {
        if (hit == false) {
            hit = true;
            health -= _Hit;
            CheckDeath();
            if (PhotonNetwork.IsConnected)
                photonView.RPC("SetHitVisuals", RpcTarget.All, _Hit, _HitPos);
            else
                SetHitVisuals(_Hit, _HitPos);
            thisAgent.enabled = false;
            thisBody.AddForce(_HitPos * _Force, ForceMode.Impulse);
        }
    }

    public void CheckDeath() {
        if(health <= 0) {
            if (PhotonNetwork.IsConnected == true) {
                photonView.RPC("Die", RpcTarget.All);
            } else
                Die();
        }
    }

    [PunRPC]
    public void Die() {
        Instantiate(deadPrefab, transform.position, transform.rotation);
        GameObject _KOMessage = (GameObject)Instantiate(Resources.Load("Hitsplash"), transform.position + new Vector3(0, 1, 0), transform.rotation);
        _KOMessage.GetComponentInChildren<TextMeshProUGUI>().text = "KO'd!";
        _KOMessage.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
        Destroy(gameObject);
    }

    //Sets the restore timer
    public void Restore() {
        StartCoroutine(RestoreTimer());
    }

    //Restores after a while
    private IEnumerator RestoreTimer() {
        yield return new WaitForSeconds(restoreTimer);
        if (CheckIfGrounded()) {
            hit = false;
            thisAgent.updatePosition = transform;
            thisAgent.enabled = true;
            yield break;
        }

        StartCoroutine(RestoreTimer());
    }

    [PunRPC]
    private void SetHitVisuals( int _Hit, Vector3 _HitPos) {
        healthFill.fillAmount = health / maxHealth;
        GameObject _Hitsplash = (GameObject)Instantiate(Resources.Load("Hitsplash"), _HitPos, Quaternion.identity);
        _Hitsplash.GetComponentInChildren<TextMeshProUGUI>().text = _Hit.ToString();
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.AI;
using TMPro;
using System;

public class Enemy : MonoBehaviourPunCallbacks {

    public static List<Transform> players = new List<Transform>();

    public static float timeTillTargetCheck = 0.2f;
    public static float restoreTimer = 4;

    [Header("Presets:")]
    [SerializeField]private float maxHealth = 500;

    [Header("Enemy Hit Settings:")]
    [SerializeField]private float maxHitTimer = 2;
    [SerializeField]private LayerMask floor; 

    [Header("NPC Statistics:")]
    [SerializeField]public float health;

    [Header("Target Info:")]
    [SerializeField]private Transform currentTarget;

    #region Private references
    private NavMeshAgent thisAgent;
    private Rigidbody thisBody;
    private float hitTimer;
    //[SerializeField]private UnityEvent gotHitEvent;
    #endregion

    private bool hit = false;
    private bool started = false;
    private bool inRange = false;
    private float agentSpeed;

    public void Update() {
        Attacking();

        /*if (Input.GetButtonDown("Fire1")) {
            GetDamaged(50, Vector3.one);
        }

        if (Input.GetButtonDown("Fire2")) {
            StartEnemy();
        }*/
    }

    private void Attacking() {
         if(inRange == true && hit == false) {
                if (hitTimer > 0) {
                    hitTimer -= Time.deltaTime;
                } else {
                    Attack(currentTarget.transform.parent.GetComponent<PhotonView>().ViewID);
                    hitTimer = maxHitTimer;
                }
             }
        }

    private void Attack(int _View) {
        //Does the big attack;
        print("Attack");
        EnemyManager.enemyManager.SetEnemyMessage(transform.position + new Vector3(0, 1, 0), gameObject.name + " has attacked " +currentTarget.name+"!");
    }

    #region Checks
    private bool CheckIfGrounded() {
        RaycastHit _Hit;

        if(Physics.BoxCast(transform.position + (Vector3.up * 2), new Vector3(0.2f, 0.2f, 0.2f), Vector3.down * 100, out _Hit ,Quaternion.identity, floor)) {
            return true;
        }

        return false;
    }

    private bool CheckIfTarget(GameObject _Object) {
        if (currentTarget != null && started == true && hit == false)
            if (_Object == currentTarget.gameObject)
                return true;

        return false;
    }

    private void OnTriggerEnter(Collider _O) {
        if (_O.GetComponent<PlayerManager>() && started == false) {
            StartEnemy();
        }

        if (CheckIfTarget(_O.gameObject) == false)
            return;

        thisAgent.speed = 0;
        thisAgent.velocity = Vector3.zero;
        EnemyManager.enemyManager.SetEnemyAnimation(photonView.ViewID, false);
        inRange = true;
    }

    private void OnTriggerExit(Collider _O) {
        if(currentTarget != null)
          if(_O.gameObject == currentTarget.gameObject)
                inRange = false;

        if (CheckIfTarget(_O.gameObject)) {
                thisAgent.speed = agentSpeed;
                inRange = false;
                EnemyManager.enemyManager.SetEnemyAnimation(photonView.ViewID, true);
            }
        }
    #endregion

    #region Functions related to setting references and starting
    //Sets references
    private void Awake() {
        thisAgent = GetComponent<NavMeshAgent>();
        thisBody = GetComponent<Rigidbody>();

        if (PhotonNetwork.IsConnected) {
            if (!PhotonNetwork.IsMasterClient) {

                thisBody.constraints = RigidbodyConstraints.FreezeAll;
                Destroy(thisAgent);
                Destroy(this);
            }
        }
    }


    private void Start() {
        EnemyManager.enemyManager.SetEnemyRagdoll(photonView.ViewID, true);
        health = maxHealth;
        hitTimer = maxHitTimer;
        agentSpeed = thisAgent.speed;

        if (EnemyManager.enemyManager.aggroAllOnStart == true) {
            StartEnemy();
        }
    }
    #endregion

    public void StartEnemy() {
        hit = false;
        EnemyManager.enemyManager.SetEnemyRagdoll(photonView.ViewID, false);
        StartCoroutine(StartNavigation());
    }

    private IEnumerator StartNavigation() {
        if (started == true)
            yield break;

        started = true;
        yield return new WaitForSeconds(2);
        thisAgent.enabled = true;
        StartCoroutine(AssignTargetTimer());
        EnemyManager.enemyManager.SetEnemyAnimation(photonView.ViewID, true);
    }

    //Timer before the enemy will be searching for a new target
    private IEnumerator AssignTargetTimer() {
        yield return new WaitForSeconds(timeTillTargetCheck);
        StartCoroutine(AssignTargetTimer());
        if (inRange == true) yield break;
        SetTarget();
    }

    //Sets new target for the enemy
    private void SetTarget() {
        if (PlayerManager.thisPlayer == null) return;

            if (PhotonNetwork.IsConnected) {
            SetNewTargetTransform(ChooseTarget(players));
            return;
        }

            SetNewTargetTransform(PlayerManager.thisPlayer.transform);
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
        if (hit == false && _Target != null && inRange == false) {
            if (thisAgent.isActiveAndEnabled) {
                thisAgent.SetDestination(_Target.position);
                currentTarget = _Target;
            }
        }
    }

    //Function handles the hit that has been applied to the enemy
    public void GetDamaged(int _Hit, Vector3 _Velocity) {

        //EnemyManager.enemyManager.SetEnemyHit(photonView.ViewID, _Velocity, GetComponent<Rigidbody>().angularVelocity, _Hit);
        EnemyManager.enemyManager.SetEnemyHitsplash(transform.position + new Vector3(0, 1, 0), _Hit);
        EnemyManager.enemyManager.SetEnemyHealth(photonView.ViewID, maxHealth, health);

        if (hit == false && started == true) {
            hit = true;
            thisAgent.enabled = false;
            StopCoroutine(RestoreTimer());
            StartCoroutine(RestoreTimer());
            EnemyManager.enemyManager.SetEnemyRagdoll(photonView.ViewID, true);
        }

        CheckDeath();

    }

    public void CheckDeath() {
        if (health <= 0) {
            Die();
            return;
        }

        if (started == false)
            StartEnemy();
    }

    public void Die() {
        EnemyManager.enemyManager.SetEnemyDeath(photonView.ViewID, transform.position + new Vector3(0, 1, 0));
        EnemyManager.enemyManager.SetEnemyRagdoll(photonView.ViewID, true);
        thisBody.velocity *= 10;
        Destroy(thisAgent);
        Destroy(this);


    }

    //Restores after a while
    private IEnumerator RestoreTimer() {
        yield return new WaitForSeconds(restoreTimer);
        if (CheckIfGrounded()) {
            EnemyManager.enemyManager.SetEnemyRagdoll(photonView.ViewID, false);
            EnemyManager.enemyManager.SetEnemyAnimation(photonView.ViewID, false);
            yield return new WaitForSeconds(1.5f);
            hit = false;
            thisAgent.enabled = true;
            thisAgent.updatePosition = transform;
            if (inRange == false) {
                thisAgent.speed = agentSpeed;
                EnemyManager.enemyManager.SetEnemyAnimation(photonView.ViewID, true);
            } else {
                thisAgent.speed = 0;
                thisAgent.velocity = Vector3.zero;
                EnemyManager.enemyManager.SetEnemyAnimation(photonView.ViewID, false);
            }
            yield break;
        }

        print("Failed to find collision");
        StartCoroutine(RestoreTimer());
    }

    private void SetHitVisuals( int _Hit, Vector3 _HitPos) {
        EnemyManager.enemyManager.SetEnemyHealth(photonView.ViewID, maxHealth, health);
        EnemyManager.enemyManager.SetEnemyHitsplash(_HitPos, _Hit);
    }
}

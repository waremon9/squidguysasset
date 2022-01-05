using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public CapsuleCollider PlayerCapsule;
    
    //MovementComponent script reference
    public MoveComponent MoveComp;
    [SerializeField] private Rigidbody RigidComp;
    [SerializeField] private Animator AnimComp;

    [SerializeField] private ParticleSystem CollisionParticle;

    //username script for ui
    public PlayerUsername playerUsername;

    //player id (may be unused)
    [SerializeField] private int identifiant;
    public int Identifiant
    {
        get { return identifiant; }
        set { identifiant = value; }
    }

    //twitch username of the player
    [SerializeField] private string playerName;
    public string Name
    {
        get { return playerName; }
        set { playerName = value; }
    }

    //player coordiantes on the grid
    [SerializeField] private Vector2Int coordinates;
    public Vector2Int Coordinates
    {
        get { return coordinates; }
        set { coordinates = value; }
    }

    //coordinate before moving
    [SerializeField] private Vector2Int lastCoordinates;
    public Vector2Int LastCoordinates
    {
        get { return lastCoordinates; }
        set { lastCoordinates = value; }
    }

    //coordinate after moving while ignoring colision
    [SerializeField] private Vector2Int futurCoordinates;
    public Vector2Int FuturCoordinates
    {
        get { return futurCoordinates; }
        set { futurCoordinates = value; }
    }

    //player dead state
    [SerializeField] private bool isDead = false;
    public bool IsDead
    {
        get { return isDead; }
    }

    /// <summary>
    /// Player is dead. Set dead state, disable animator, activate ragdoll (sort-of).
    /// </summary>
    public void Die()
    {
        RigidComp.isKinematic = false;
        AnimComp.enabled = false;
        isDead = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown("q"))
        {
            Die();
        }
    }

    //All animation trigger
    #region AnimationTrigger

        /// <summary>Set trigger to activate "Wave" animation.</summary>
        public void AnimatorWaveTrigger()
        {
            AnimComp.SetTrigger("Wave");
        }
        /// <summary>Set trigger to activate "Push" animation.</summary>
        public void AnimatorPushTrigger()
        {
            AnimComp.SetTrigger("Push");
        }
        /// <summary>Set trigger to activate "Jump" animation and set the speed to correspond to the movement duration.</summary>
        public void AnimatorJumpTrigger()
        {
            AnimComp.SetFloat("JumpSpeed", 1f / MoveComp.MoveDuration);
            AnimComp.SetTrigger("Jump");
        }
        /// <summary>Set trigger to activate "GetPushed" animation and set the speed to correspond to the movement duration.</summary>
        public void AnimatorGetPushedTrigger()
        {
            AnimComp.SetFloat("JumpSpeed", (1f / MoveComp.MoveDuration) * 1.2f);
            AnimComp.SetTrigger("GetPushed");
        }
        /// <summary>Set trigger to activate "Victory" animation.</summary>
        public void Win()
        {
            AnimComp.SetTrigger("Victory");
        }

    #endregion
    
    /// <summary>
    /// Instantiate collision particle system.
    /// </summary>
    /// <param name="list">Players we collide with.</param>
    public void CreateParticleCollide(List<Player> list)
    {
        foreach (Player other in list)
        {
            Instantiate(CollisionParticle, (other.transform.position + transform.position) / 2, Quaternion.LookRotation(other.transform.position - transform.position));
        }
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Bullet>() && !MoveComp.IsCoroutineMoveRunning)
        {
            StartCoroutine(BulletCollideCoroutine(other));
        }
    }

    private IEnumerator BulletCollideCoroutine(Collider other)
    {
        LastCoordinates = futurCoordinates = coordinates;
        MoveComp.Move(other.GetComponent<Bullet>().direction, MoveOrigin.Push);
        other.GetComponent<Renderer>().enabled = false;
        other.enabled = false;
        yield return StartCoroutine(MoveComp.MoveVisualCoroutine());
        Destroy(other.gameObject);
    }

    public PlayerData GetPlayerData()
    {
        PlayerData pData = new PlayerData(this);
        return pData;
    }

    public void SetPlayerData(PlayerData pData)
    {
        Name = pData.username;
        Coordinates = pData.position;
        playerUsername.SetUsernameColor(pData.usernameColor);
        playerUsername.SetUsernameText(pData.username);
    }
}

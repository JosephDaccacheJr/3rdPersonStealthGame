using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Guard_Controller : Character_Base
{

    [Header("AI References")]
    public NavMeshAgent nav;
    public GameObject eyePoint;

    [Header("AI Settings")]
    public LayerMask eyeLayer;
    public List<GameObject> patrolPoints = new List<GameObject>();
    public float walkSpeed, runSpeed;
    internal int patrolIndex;

    internal Quaternion startRotation;

    // States
    internal INPCState currentState;
    internal NPCState_Idle stateIdle = new NPCState_Idle();
    internal NPCState_Chase stateChase = new NPCState_Chase();
    internal NPCState_Search stateSearch = new NPCState_Search();
    // State Timers
    public float timerChase, timerSearch;
    internal float currentStateTimer;
    internal float playerPosAwarenessTimer;
    internal float searchPointTimer;
    // State Flags 
    internal bool heardNoise;

    internal bool playerIsSeen, informedOfPlayer;
    internal Vector3 playerLastSeenPosition;

    float _angleToPlayer
    {
        get
        {
                Vector3 dir = GameManager.instance.player.transform.position - transform.position;
                return Vector3.Angle(dir, transform.forward);
            
        }
    }

    float _distanceFromPlayer
    {
        get
        {
            return Vector3.Distance(transform.position, GameManager.instance.player.transform.position);
        }
    }

    public override void Start()
    {
        base.Start();
        GameManager.instance.guards.Add(this);
        currentState = stateIdle;
        startRotation = Quaternion.identity;

    }

    public override void Update()
    {
        base.Update();
        currentStateTimer -= Time.deltaTime;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        PlayerInSight();
        currentState = currentState.DoState(this);
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
        AnimationControl();
    }


    void AnimationControl()
    {
        float mag = nav.velocity.magnitude;
        anim.SetFloat("Speed", mag);
    }

    public void AlertGuard(Vector3 pos)
    {
        if(currentState != stateChase)
        {
            heardNoise = true;
            playerLastSeenPosition = pos;
        }
        
    }

    void PlayerInSight()
    {

        // Distances to try
        // 10 In Shadow
        // 100 in light

        if (GameManager.instance.playerCon.currentState != GameManager.instance.playerCon.stateAlive)
        {
            playerIsSeen = false;
            return;
        }
        RaycastHit hit;
        Vector3 dir = (GameManager.instance.player.transform.position + new Vector3(0f,0.5f,0f)) - eyePoint.transform.position;
        Debug.DrawRay(eyePoint.transform.position, dir);

        float distNeeded = 5;

        Debug.DrawRay(eyePoint.transform.position, dir, Color.red);
        if (GameManager.instance.playerCon.isInShadow)
        {
            distNeeded = 1;
        }

        if (_angleToPlayer <= 60
        && Physics.Raycast(eyePoint.transform.position, dir, out hit, 100f, eyeLayer)
        && _distanceFromPlayer <= distNeeded
        && hit.collider.tag == "Player")
        {
            playerIsSeen = true;
        }
        else
        {
            playerIsSeen = false;
        }
    }

    public void AlertedByAnotherGuard(Guard_Controller otherGuard)
    {
        if(currentState != stateChase)
        {
            playerLastSeenPosition = otherGuard.playerLastSeenPosition;
            currentStateTimer = timerChase;
            playerPosAwarenessTimer = 2f;
            informedOfPlayer = true;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        switch(other.gameObject.tag)
        {
            case "Player":
                if (currentState == stateChase)
                {
                    GameManager.instance.playerCon.KillPlayer();
                }
                break;
            case "Door":
                other.gameObject.GetComponent<Doorway>().forceOpenDoor();
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Door":
                other.gameObject.GetComponent<Doorway>().forceCloseDoor();
                break;
        }
    }

}

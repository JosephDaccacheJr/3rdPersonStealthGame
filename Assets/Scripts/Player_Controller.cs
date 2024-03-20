using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : Character_Base
{
    [Header("Player Stats")]
    public int health = 100;

    

    [Header("Player References")]
    public GameObject cameraGimbal;
    public GameObject cameraStartPosition;
    public GameObject objectivePointer;
    CharacterController _con;

    [Header("Audio Prefabs")]
    public GameObject sfxWhistle;

    // Rotation controls
    Quaternion _targetRotation;

    float _cameraStartingZ;

    internal float inX, inY, inX2, inY2;
    internal float speedMod = 1f;
    int _lightCount;

    private float _cameraShiftX;
    private float _cameraShiftXStart;

    // Visibility controllers
    public bool isInShadow
    {
        get { return _isInShadow; }
        private set { _isInShadow = value; }
    }

    private bool _isInShadow;
    List<GameObject> _lights = new List<GameObject>();


    // States
    internal IPlayerStates currentState;
    internal playerState_Alive stateAlive = new playerState_Alive();
    internal playerState_Dead stateDead = new playerState_Dead();

    // Timers
    internal float whistleTimer;

    public override void Start()
    {
        base.Start();
        currentState = stateAlive;
        _con = GetComponent<CharacterController>();
        _cameraStartingZ = GameManager.instance.camera.transform.localPosition.z;
        _cameraShiftX = _cameraShiftXStart = GameManager.instance.camera.transform.localPosition.x;
    }

    public override void Update()
    {
        base.Update();
        currentState = currentState.DoState(this);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        currentState = currentState.DoStateFixed(this);
        CheckVisibility();
        PointAtObjective();
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
        AnimationControl();
    }

    private void OnTriggerEnter(Collider other)
    {
        switch(other.tag)
        {
            case "Light":
                _lights.Add(other.gameObject);
                break;
            case "Item":
                other.gameObject.GetComponent<Item_Base>().GetItem();
                break;
            case "Door":
                other.gameObject.GetComponent<Doorway>().openDoor();
                break;
            case "Exit":
                if(GameManager.instance.gotObjective)
                {
                    GameManager.instance.PlayerWins();
                }
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        switch (other.tag)
        {
            case "Light":
                _lights.Remove(other.gameObject);
                break;
            case "Door":
                other.gameObject.GetComponent<Doorway>().closeDoor();
                break;
        }
    }

    internal void ReadInputs()
    {
        inX = Input.GetAxis("Horizontal");
        inY = Input.GetAxis("Vertical");

        inX2 = Input.GetAxis("CameraX") + Input.GetAxis("Mouse X");
        inY2 = Input.GetAxis("CameraY") + Input.GetAxis("Mouse Y");
        speedMod = Input.GetButton("Run") ? 2f : 1f;
        if (Input.GetButtonDown("MakeSound") && whistleTimer <= 0f)
        {
            whistleTimer = 1f;
            GameObject whistleSound = Instantiate(sfxWhistle, transform.position, Quaternion.identity);
            Destroy(whistleSound, 3f);
            MakeNoise(5f);
        }
        if(Input.GetButtonDown("ShowObjective"))
        {
            objectivePointer.SetActive(!objectivePointer.activeInHierarchy);
        }

        if(Input.GetButtonDown("SwitchCamera"))
            _cameraShiftX = _cameraShiftX == _cameraShiftXStart ? -_cameraShiftXStart : _cameraShiftXStart;
    }

    internal void Movement()
    {

        float yMove = !_con.isGrounded ? -1f : 0f;
        Camera camera = Camera.main;
        Vector3 forward = camera.transform.forward;
        Vector3 right = camera.transform.right;

        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 targetDirection = forward;

        Vector3 mov = (forward * inY + right * inX) + new Vector3(0f, _con.isGrounded ? 0f : -9.8f, 0f) * Time.deltaTime;
        
        _con.Move(mov * Time.deltaTime * moveSpeed * speedMod);

        _targetRotation = Quaternion.LookRotation(new Vector3(mov.x, 0f, mov.z),Vector3.up);
           
        Quaternion nextRotation = Quaternion.Lerp(characterBody.transform.rotation, _targetRotation, 10f * Time.deltaTime);
        if(_con.velocity.magnitude > 0f)
            characterBody.transform.rotation = nextRotation;

    }

    void AnimationControl()
    {
        float mag = _con.velocity.magnitude;
        anim.SetFloat("Speed", mag > 0 ? 0.5f * speedMod : 0f);
    }

    internal void CameraControl()
    {
        Vector3 camStart = cameraStartPosition.transform.localPosition;
        cameraStartPosition.transform.localPosition = new Vector3(Mathf.Lerp(camStart.x, _cameraShiftX,Time.deltaTime * 8f), camStart.y, camStart.z);
        Vector3 cam = GameManager.instance.camera.transform.localPosition;
        GameManager.instance.camera.transform.localPosition = new Vector3(Mathf.Lerp(cam.x, _cameraShiftX, Time.deltaTime * 8f), cam.y, cam.z);

        RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(0f, 0.5f, 0f), cameraStartPosition.transform.position - transform.position, out hit))
        {
            if (hit.distance <= Mathf.Abs(_cameraStartingZ) + 0.5f && hit.collider.gameObject.tag != "Player")
            {
                Vector3 pos = GameManager.instance.camera.transform.localPosition;
                float moveY = 0.6f + (0.2f * ((1.67f + pos.z) / 1.67f));
                GameManager.instance.camera.transform.localPosition = new Vector3(pos.x, moveY, -hit.distance + 0.6f);
            }
            else
            {
                ResetCamera();
            }
        }
        else
        {
            ResetCamera();
        }

        Vector3 eul = cameraGimbal.transform.localEulerAngles;
        cameraGimbal.transform.localEulerAngles = new Vector3(Mathf.Clamp(eul.x + (inY2 * 100f * GameManager.instance.cameraSpeed * Time.deltaTime), 10f, 50f), eul.y + (inX2 * 100f * GameManager.instance.cameraSpeed * Time.deltaTime), eul.z);
    }

    void ResetCamera()
    {
        Vector3 pos = GameManager.instance.camera.transform.localPosition;
        float zMove = Mathf.MoveTowards(pos.z, _cameraStartingZ, Time.deltaTime * 40f);
        GameManager.instance.camera.transform.localPosition = new Vector3(pos.x, 0.6f, zMove);
    }

    public void KillPlayer()
    {
        health = 0;
        anim.SetTrigger("Kill");
        _con.enabled = false;
        GameManager.instance.PlayerHasDied();
    }

    public void MakeNoise(float soundDist)
    {
        foreach (Guard_Controller guard in GameManager.instance.guards)
        {
            if (Vector3.Distance(transform.position, guard.transform.position) <= soundDist)
            {
                guard.AlertGuard(transform.position);
            }
        }
    }

    public void CheckVisibility()
    {
        int _numLightsOnPlayer = 0;
        foreach (GameObject l in _lights)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + new Vector3(0f, 0.5f, 0f), l.transform.position - transform.position, out hit))
            {
                if (hit.collider.gameObject.name == "LightObject" && hit.collider.gameObject.transform.parent.gameObject == l)
                {
                    _numLightsOnPlayer++;
                }
            }

        }
        _isInShadow = (_numLightsOnPlayer <= 0);
    }

    public void PointAtObjective()
    {
        Vector3 pos = GameManager.instance.GetObjectiveZone();
        objectivePointer.transform.LookAt(new Vector3(pos.x, transform.position.y, pos.z));
    }
}

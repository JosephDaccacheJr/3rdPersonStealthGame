using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public enum key { regular, red };
    [Header("Game Settings")]
    public float cameraSpeed;

    [Header("References")]
    public GameObject player;
    public GameObject camera;
    public GameObject objective;
    public GameObject exitZone;
    public Player_Controller playerCon;

    [Header("Game States")]
    public bool gotObjective;
    public bool gotRedKey;

    [Header("UI Elements")]
    public Image lightGem;
    public Image lightGemLit;
    public GameObject textGameOver;
    public GameObject textMissionComplete;
    public GameObject imageRedKey;

    [Header("NPC List")]
    public List<Character_Base> characters = new List<Character_Base>();
    public List<Guard_Controller> guards = new List<Guard_Controller>();

    int _currentCharacterID;

    private void Awake()
    {
        if (GameManager.instance == null)
            GameManager.instance = this;


        Time.timeScale = 1f;

        player = GameObject.FindGameObjectWithTag("Player");
        playerCon = player.GetComponent<Player_Controller>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (playerCon.isInShadow)
        {
            lightGemLit.color = new Color(1f, 1f, 1f, Mathf.MoveTowards(lightGemLit.color.a, 0f, Time.deltaTime * 3f));
        }
        else
        {
            lightGemLit.color = new Color(1f, 1f, 1f, Mathf.MoveTowards(lightGemLit.color.a, 1f, Time.deltaTime * 3f));
        }
    }

    public void PlayerHasDied()
    {
        textGameOver.SetActive(true);
        Invoke("ResetLevel", 5f);
    }

    public void PlayerWins()
    {
        textMissionComplete.SetActive(true);
        Invoke("ResetLevel", 3f);
    }

    void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public int GetNewCharacterID()
    {
        _currentCharacterID++;
        return _currentCharacterID;
    }

    public Vector3 GetObjectiveZone()
    {
        if (gotObjective)
            return exitZone.transform.position;
        else
            return objective.transform.position;
    }

    public void GetKey(key key)
    {
        switch (key)
        {
            case key.red:
                gotRedKey = true;
                imageRedKey.SetActive(true);
                break;
        }
    }
}

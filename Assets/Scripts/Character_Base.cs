using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Base : MonoBehaviour
{
    public int characterID;
    [Header("Character Stats")]
    public float moveSpeed;

    [Header("References")]
    public Animator anim;
    public GameObject characterBody;

    public virtual void Start()
    {
        characterID = GameManager.instance.GetNewCharacterID();
        GameManager.instance.characters.Add(this);   
    }
    public virtual void Update()
    {
        
    }

    public virtual void FixedUpdate()
    {

    }

    public virtual void LateUpdate()
    {
        
    }
}

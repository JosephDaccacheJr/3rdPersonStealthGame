using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent_CharacterBase : MonoBehaviour
{
    public AudioSource stepSound;
    public List<AudioClip> stepList = new List<AudioClip>();
    public Character_Base parent;

    string _characterType;

    Player_Controller _playerCon;
    Guard_Controller _guardCon;

    private void Start()
    {
        if (parent.GetComponent<Player_Controller>() != null)
        {
            _characterType = "Player";
            _playerCon = parent.GetComponent<Player_Controller>();
        }
        else if(parent.GetComponent<Guard_Controller>() != null)
        {
            _characterType = "Guard";
            _guardCon = parent.GetComponent<Guard_Controller>();
        }
    }
    public void Step()
    {
        stepSound.Stop();
        stepSound.clip = stepList[Random.Range(0, stepList.Count)];
        stepSound.Play();
        switch (_characterType)
        {
            case "Player":
                if(_playerCon.speedMod > 1)
                    _playerCon.MakeNoise(2);
                break;
        }
    }


}

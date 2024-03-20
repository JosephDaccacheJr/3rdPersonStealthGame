using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerState_Dead : IPlayerStates
{
    public IPlayerStates DoState(Player_Controller player)
    {
        player.ReadInputs();
        return player.stateDead;
    }

    public IPlayerStates DoStateFixed(Player_Controller player)
    {
        player.CameraControl();
        return player.currentState;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerState_Alive : IPlayerStates
{
    public IPlayerStates DoState(Player_Controller player)
    {
        player.whistleTimer -= Time.deltaTime;
        player.ReadInputs();
        if (player.health > 0)
            return player.stateAlive;
        else
        {
            player.whistleTimer = 1f;
            return player.stateDead;
        }
    }

    public IPlayerStates DoStateFixed(Player_Controller player)
    {
        player.Movement();
        player.CameraControl();
        return player.currentState;
    }
}

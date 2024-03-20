using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerStates
{
    IPlayerStates DoState(Player_Controller player);
    IPlayerStates DoStateFixed(Player_Controller player);
}

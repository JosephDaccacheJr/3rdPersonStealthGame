using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCState_Chase : INPCState
{
    public INPCState DoState(Guard_Controller npc)
    {
        npc.nav.speed = npc.runSpeed;
        npc.nav.angularSpeed = 240;
        if (npc.informedOfPlayer) npc.informedOfPlayer = false;

        foreach (Guard_Controller guard in GameManager.instance.guards)
        {
            if (guard.gameObject != npc.gameObject)
            {
                float dist = Vector3.Distance(npc.transform.position, guard.transform.position);
                if(dist <= 5f)
                {
                    guard.AlertedByAnotherGuard(npc);
                }
            }
        }

        if (npc.playerIsSeen)
        {
            npc.playerPosAwarenessTimer = 2f;
            npc.currentStateTimer = npc.timerChase;
        }

        if(npc.playerPosAwarenessTimer > 0)
        {
            npc.playerLastSeenPosition = GameManager.instance.player.transform.position;
            npc.nav.destination = npc.playerLastSeenPosition;
        }

        npc.playerPosAwarenessTimer -= Time.deltaTime;

        if (GameManager.instance.playerCon.health <= 0)
        {
            return npc.stateIdle;
        }
        else if (npc.currentStateTimer <= 0)
        {
            npc.currentStateTimer = npc.timerSearch;
            npc.searchPointTimer = 0f;
            return npc.stateSearch;
        }
        else
            return npc.stateChase;
        
    }

}

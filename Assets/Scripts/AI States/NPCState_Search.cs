using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCState_Search : INPCState
{
    public INPCState DoState(Guard_Controller npc)
    {
        npc.nav.speed = npc.walkSpeed;

        if(npc.searchPointTimer <= 0)
        {
            npc.nav.destination = npc.playerLastSeenPosition + new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f));
            npc.searchPointTimer = 4f;
        }

        if(npc.heardNoise)
        {
            npc.nav.destination = npc.playerLastSeenPosition;
            npc.currentStateTimer = npc.timerSearch;
            npc.searchPointTimer = 5f;
            npc.heardNoise = false;
        }

        npc.searchPointTimer -= Time.deltaTime;

        if (npc.currentStateTimer <= 0)
            return npc.stateIdle;
        else if (npc.playerIsSeen || npc.informedOfPlayer)
            return npc.stateChase;
        else
            return npc.stateSearch;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCState_Idle : INPCState
{
    public INPCState DoState(Guard_Controller npc)
    {
        npc.nav.speed = npc.walkSpeed;
        npc.nav.angularSpeed = 240;

        Movement(npc);


        if (npc.playerIsSeen)
        {
            npc.currentStateTimer = npc.timerChase;
            return npc.stateChase;
        }
        else if (npc.informedOfPlayer)
        {
            return npc.stateChase;
        }
        else if (npc.heardNoise)
        {
            npc.nav.destination = npc.playerLastSeenPosition;
            npc.currentStateTimer = npc.timerSearch;
            npc.searchPointTimer = 5f;
            npc.heardNoise = false;
            return npc.stateSearch;
        }
        else
            return npc.stateIdle;
        
    }

    void Movement(Guard_Controller npc)
    {
        if (npc.patrolPoints.Count != 0)
        {
            npc.nav.destination = npc.patrolPoints[npc.patrolIndex].transform.position;
            // TODO: See if remaining distance doesn't work anymore?
            if (Vector3.Distance(npc.transform.position, npc.nav.destination) <= npc.nav.stoppingDistance)
            {
                npc.patrolIndex = (npc.patrolIndex == npc.patrolPoints.Count - 1 ? 0 : npc.patrolIndex + 1);
            }
        }
    }

    void ResumeRotation(Guard_Controller npc)
    {
        npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation, npc.startRotation, Time.deltaTime * 10f);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doorway : MonoBehaviour
{
    public GameObject doorWay;
    public GameManager.key doorType;
    bool _openDoor;
    int _InDoorCount;
    BoxCollider _boxCollider;

    private void Start()
    {
        _boxCollider = doorWay.GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if (_InDoorCount > 0)
        {
            if(_boxCollider.enabled)
            {
                _boxCollider.enabled = false;
            }
            doorWay.transform.localPosition = new Vector3(0f, Mathf.MoveTowards(doorWay.transform.localPosition.y, -0.49f, Time.deltaTime * 4f), 0f);
        }
        else
        {
            if (_boxCollider.enabled)
            {
                _boxCollider.enabled = true;
            }
            doorWay.transform.localPosition = new Vector3(0f, Mathf.MoveTowards(doorWay.transform.localPosition.y, 0.5f, Time.deltaTime * 4f), 0f);
        }
    }


    public void openDoor()
    {
        switch(doorType) 
        {
            case GameManager.key.regular:
                _InDoorCount++;
                break;
            case GameManager.key.red:
                if(GameManager.instance.gotRedKey) { _InDoorCount++; }
                break;
        }
    }

    public void closeDoor()
    {
        switch (doorType)
        {
            case GameManager.key.regular:
                _InDoorCount--;
                break;
            case GameManager.key.red:
                if (GameManager.instance.gotRedKey) { _InDoorCount--; }
                break;
        }
    }

    public void forceOpenDoor()
    {
        _InDoorCount++;
    }

    public void forceCloseDoor()
    {
        _InDoorCount--;
    }
}

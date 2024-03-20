using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Objective : Item_Base
{
    public override void GetItem()
    {
        GameManager.instance.gotObjective = true;
        base.GetItem();
    }
}

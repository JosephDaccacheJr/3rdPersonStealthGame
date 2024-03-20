using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Key : Item_Base
{
    public GameManager.key keyType;
    public override void GetItem()
    {
        GameManager.instance.GetKey(keyType);
        base.GetItem();
    }
}

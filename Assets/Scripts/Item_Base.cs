using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Base : MonoBehaviour
{
    public virtual void GetItem()
    {
        gameObject.SetActive(false);
    }
}

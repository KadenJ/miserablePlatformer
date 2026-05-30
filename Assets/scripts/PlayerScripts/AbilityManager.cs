using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public bool itemHeld = false;
    public GameObject heldItem;
    public float throwPower = 300;

    public GameObject getHeldItem()
    {
        return heldItem;
    }

    public void setHeldItem(GameObject _heldItem)
    {
        heldItem = _heldItem;
        itemHeld = true;
    }
    public void setHeldItem()
    {
        heldItem = null;
        itemHeld = false;
    }

}

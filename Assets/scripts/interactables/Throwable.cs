using UnityEngine;

public class Throwable : MonoBehaviour, IInteractables
{

    bool IInteractables.canInteract => true;


    public void Interact()
    {
        Destroy(gameObject);
        //print("interact with + " + gameObject.name);
    }
}

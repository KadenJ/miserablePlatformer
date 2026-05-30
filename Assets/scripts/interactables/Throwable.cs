using System;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour, IInteractables
{

    bool IInteractables.canInteract => true;

    AbilityManager am;
    [SerializeField] private string objectName;
    [SerializeField] private int dmg;
    [SerializeField] private int throwPower;


    // every throwable will contain a list of attributes
    //sprite
    //dmg
    //throw power


    public void Interact()
    {
        //take throwable gameObject and store offScreen
        am = GameObject.Find("AbilityManager").GetComponent<AbilityManager>();
        am.setHeldItem(this.gameObject);

        this.gameObject.transform.position = Vector3.zero;
        

        //give attributes to abilityManager to hold onto
    }

    //create script to launch forward
}


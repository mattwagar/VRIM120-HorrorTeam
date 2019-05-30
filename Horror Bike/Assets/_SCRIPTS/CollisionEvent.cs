using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEvent : MonoBehaviour
{
    public Animator eventAnim;

    public void ActivateEvent()
    {
        if(eventAnim != null)
            eventAnim.Play("event");
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.name == "Bike")
            ActivateEvent();
    }

}

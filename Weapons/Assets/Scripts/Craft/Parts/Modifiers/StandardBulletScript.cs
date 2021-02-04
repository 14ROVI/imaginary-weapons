using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModApi.Common;
using ModApi.Craft.Parts;
using Assets.Scripts.Craft.Parts;

public class StandardBulletScript : MonoBehaviour
{
    private Vector3 PreviousPosition;
    

    private void Start() {
        PreviousPosition = transform.position - transform.forward;
    }


    void FixedUpdate() {
        if (transform.position - PreviousPosition == new Vector3(0, 0, 0)){
            Destroy(gameObject, 0.5f);
        }
        else{
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(transform.position - PreviousPosition), 0.2f);
        }
        PreviousPosition = transform.position;
    }


    void OnTriggerEnter(Collider other)
    {
        var part = other.transform.GetComponentInParent<PartScript>();
        if (part != null){
            part.BodyScript.ExplodePart(part, 1f, 0);
            Destroy(gameObject);
        }
        
    }
}
namespace Assets.Scripts.Craft.Parts.Modifiers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using ModApi.Craft.Parts;
    using ModApi.GameLoop.Interfaces;
    using UnityEngine;

    public class explosionScript : PartModifierScript<explosionData>
    {
        private void Start() {
            PartScript.PartDestroyed += Explode;
        }

        void Explode(object sender) {
            if (Game.InFlightScene){
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, Data.explosionPower);
                foreach(Collider hitCollider in hitColliders){
                    var part = hitCollider.transform.GetComponentInParent<PartScript>();
                    if (part != null){
                        RaycastHit hit;
                        var exposed = false;
                        if (Physics.Raycast(transform.position, (part.transform.position-transform.position), out hit)) {
                            if (hit.collider == part.PrimaryCollider) {
                                exposed = true;
                            }
                        }
                        if (exposed) {
                            part.BodyScript.RigidBody.AddExplosionForce(Data.explosionPower*500, transform.position, Data.explosionPower, 10F);
                        }
                    }
                }
            }
        }
    }
}
namespace Assets.Scripts.Craft.Parts.Modifiers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using ModApi.Common;
    using ModApi.Craft.Parts;
    using ModApi.GameLoop.Interfaces;
    using UnityEngine;

    public class missileScript : PartModifierScript<missileData>
    {

        private ParticleSystem missileTrail;
        private ParticleSystem missileRocket;
        private string missileInput;
        private ModApi.Craft.ICraftScript oldCraft;
        private Vector3 targetPosition;
        private float power = 600f;
        private float burnTime = 5f;
        private float timer = 0f;
        private bool fired = false;

        

        private void Start() {
            if (Game.InFlightScene && !PartScript.Disconnected){
                missileTrail = transform.Find("missile trail").gameObject.GetComponent<ParticleSystem>();
                missileRocket = transform.Find("missile rocket").gameObject.GetComponent<ParticleSystem>();
                var main = missileTrail.GetComponent<ParticleSystem>().main;
                main.customSimulationSpace = PartScript.CraftScript.Transform;
                oldCraft = PartScript.CraftScript;
            }
        }

        private void Update(){
            missileInput = Data.missileInput;
            power = Data.missilePower;
            burnTime = Data.burnTime;

            if (Game.InFlightScene){

                if (fired){
                    timer += Time.deltaTime * Convert.ToSingle(Game.Instance.FlightScene.TimeManager.CurrentMode.TimeMultiplier);

                    if (timer > burnTime * 1.5f){
                        PartScript.BodyScript.ExplodePart(PartScript, 1000f, 10);
                        return;
                    } else if (timer < burnTime){
                        var multiplier = Convert.ToSingle(Game.Instance.FlightScene.TimeManager.CurrentMode.TimeMultiplier);
                        var force = transform.forward * power * multiplier / 3f;
                        PartScript.BodyScript.RigidBody.AddForce(force - PartScript.BodyScript.RigidBody.velocity * multiplier);
                    } else if (missileTrail.isPlaying){
                        missileTrail.Stop();
                        missileRocket.Stop();
                    }
                    
                    if(Data.guided){
                        if (oldCraft.FlightData.NavSphereTarget != null){
                            targetPosition = oldCraft.FlightData.NavSphereTarget.Position.ToVector3();
                            var currestPosition = PartScript.CraftScript.FlightData.Position.ToVector3();
                            var mult = ((targetPosition - currestPosition).magnitude / PartScript.BodyScript.RigidBody.velocity.magnitude) * 0.9f;
                            var toDir = targetPosition - currestPosition + (oldCraft.FlightData.NavSphereTarget.Velocity.ToVector3() - PartScript.CraftScript.CraftNode.Velocity.ToVector3() - PartScript.CraftScript.GravityForce * PartScript.BodyScript.Data.Mass / 0.9f) * mult;
                            var newDirection = Vector3.RotateTowards(transform.forward, toDir, 1f * Time.deltaTime, 0f);
                            transform.rotation = Quaternion.LookRotation(newDirection);

                            if ((targetPosition - currestPosition).magnitude < 30f){
                                PartScript.BodyScript.ExplodePart(PartScript, 1000f, 10);
                            }
                        }
                    }
                }
                
                try{
                    if(PartScript.Data.Activated && !fired){
                        fired = true;
                        PartScript.BodyScript.RigidBody.interpolation = RigidbodyInterpolation.Interpolate;
                        missileTrail.Play();
                        missileRocket.Play();
                    }

                    else if (PartScript.CraftScript.ActiveCommandPod.IsPlayerControlled){
                        if (UnityEngine.Input.GetMouseButton(0) && missileInput == "Left Click"){
                            PartScript.Activate();
                        }
                        else if (UnityEngine.Input.GetMouseButton(1) && missileInput == "Right Click"){
                            PartScript.Activate();
                        }
                        else if (UnityEngine.Input.GetKey("space") && missileInput == "Space"){
                            PartScript.Activate();
                        }
                        else if (UnityEngine.Input.GetKey("return") && missileInput == "Return"){
                            PartScript.Activate();
                        }
                        else if (UnityEngine.Input.GetKey(KeyCode.F) && missileInput == "F"){
                            PartScript.Activate();
                        }
                    }
                } catch {}
            }
        }
    }
}
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

    public class laserScript : PartModifierScript<laserData>
    {
        private GameObject laserBeam;
        private LineRenderer laserRenderer;
        private float observedPower;
        private string LaserInput;
        private Dictionary<PartScript, float> heatDict = new Dictionary<PartScript, float>();


        void Start() {
            laserBeam = transform.Find("laser beam").gameObject;
            laserRenderer = laserBeam.GetComponent<LineRenderer>();
        }


        void FixedUpdate(){
            laserRenderer.enabled = false;
            LaserInput = Data.LaserInput;

            if (Game.InFlightScene){
                if (PartScript.CraftScript.ActiveCommandPod.IsPlayerControlled && PartScript.Data.Activated){
                    if (LaserInput == "Activation Group"){
                        Damage();
                    }
                    else if (UnityEngine.Input.GetMouseButton(0) && LaserInput == "Left Click"){
                        Damage();
                    }
                    else if (UnityEngine.Input.GetMouseButton(1) && LaserInput == "Right Click"){
                        Damage();
                    }
                    else if (UnityEngine.Input.GetKey("space") && LaserInput == "Space"){
                        Damage();
                    }
                    else if (UnityEngine.Input.GetKey("return") && LaserInput == "Return"){
                        Damage();
                    }
                    else if (UnityEngine.Input.GetKey(KeyCode.F) && LaserInput == "F"){
                        Damage();
                    }
                }
            }
            
            foreach(KeyValuePair<PartScript, float> entry in heatDict.ToArray())
            {
                heatDict[entry.Key] -= 0.5f * Time.deltaTime;
                if (entry.Value < 0.5f){
                    heatDict.Remove(entry.Key);
                }
            }
        }


        void Damage(){
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit)){
                if(hit.collider){
                    laserRenderer.SetPosition(1, transform.InverseTransformPoint(hit.point));

                    observedPower = ((Data.power*Data.power) / (hit.distance/100)) * Time.deltaTime;

                    if (observedPower > 0.1){
                        var part = hit.collider.gameObject.transform.GetComponentInParent<PartScript>();
                        if (part != null){
                            if (heatDict.ContainsKey(part)){
                                heatDict[part] += observedPower;
                            }
                            else heatDict.Add(part, observedPower);

                            if (heatDict[part] > part.Data.LoadedMass){
                                part.BodyScript.ExplodePart(part, part.Data.LoadedMass, 0);
                                heatDict.Remove(part);
                            }
                        }
                    }
                }
            } else laserRenderer.SetPosition(1, Vector3.forward * 5000);

            laserRenderer.enabled = true;
            PartScript.BatteryFuelSource.RemoveFuel(Data.power * Time.deltaTime);
        }
    }
}
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

    public class StandardGunScript : PartModifierScript<StandardGunData>
    {
        
        private GameObject GunParent;
        private GameObject GunBody;
        private GameObject GunBarrel;
        private GameObject GunAudio;
        private AudioSource GunAudioSource;
        private ParticleSystem smoke;
        private ParticleSystem flash;

        private GameObject BulletStart;
        private GameObject Bullet;
        private GameObject TempBullet;
        private bool AllowFire = true;
        private float Accuracy;
        private int TracerCount = 0;
        private Material TracerMaterial;

        private float Caliber;
        private float Length;
        private float BulletSpeed;
        private string GunInput;
        private float Force;
        private float BulletMass = 15f;


        private void Start() {
            GunParent = transform.Find("better gun").gameObject;
            GunBody = GunParent.transform.Find("body").gameObject;
            GunBarrel = GunParent.transform.Find("barrel").gameObject;
            BulletStart = GunBarrel.transform.Find("bullet start point").gameObject;
            GunAudio = GunBarrel.transform.Find("gun audio").gameObject;
            GunAudioSource = GunAudio.GetComponent<AudioSource>();
            smoke = GunBarrel.transform.Find("muzzle smoke").gameObject.GetComponent<ParticleSystem>();
            flash = GunBarrel.transform.Find("muzzle flash").gameObject.GetComponent<ParticleSystem>();

            Bullet = Mod.Instance.ResourceLoader.LoadAsset<GameObject>("Assets/Content/Craft/Parts/Prefabs/Standard Bullet.prefab");
            TracerMaterial = Mod.Instance.ResourceLoader.LoadAsset<Material>("Assets/Materials/TracerMaterial.mat");
        }


        // Update is called once per frame
        void Update()
        {
            Length = Data.Length;
            Caliber = Data.Caliber;
            GunInput = Data.GunInput;
            BulletSpeed = Data.BulletSpeed;

            if(Caliber > 90){
                GunBody.transform.localScale = new Vector3(Caliber, Caliber, 1f+Length/26f);
                GunParent.transform.localPosition = new Vector3(0, 0, 0);
            }
            else{
                GunBody.transform.localScale = new Vector3(Caliber, Caliber, 0.25f+(Length/26f)+(Caliber/120f));
                GunParent.transform.localPosition = new Vector3(0, 0, (0.8f*Caliber)/90f - 0.8f);
            }
            GunBarrel.transform.localScale = new Vector3(Caliber, Caliber, Length);

            GunAudioSource.pitch = 1f - (1f*(Caliber-12f))/(2f*(152f-12f));

            if (Game.InFlightScene && AllowFire){
                if (PartScript.CraftScript.ActiveCommandPod.IsPlayerControlled && PartScript.Data.Activated){
                    if (GunInput == "Activation Group"){
                        StartCoroutine("Fire");
                    }
                    else if (UnityEngine.Input.GetMouseButton(0) && GunInput == "Left Click"){
                        StartCoroutine("Fire");
                    }
                    else if (UnityEngine.Input.GetMouseButton(1) && GunInput == "Right Click"){
                        StartCoroutine("Fire");
                    }
                    else if (UnityEngine.Input.GetKey("space") && GunInput == "Space"){
                        StartCoroutine("Fire");
                    }
                    else if (UnityEngine.Input.GetKey("return") && GunInput == "Return"){
                        StartCoroutine("Fire");
                    }
                    else if (UnityEngine.Input.GetKey(KeyCode.F) && GunInput == "F"){
                        StartCoroutine("Fire");
                    }
                }
            }
            
        }


        private System.Collections.IEnumerator Fire(){
            AllowFire = false;

            GunAudioSource.PlayOneShot(GunAudioSource.clip);
            smoke.Play();
            flash.Play();

            BulletMass = 7.5f * (Caliber/90f)*(Caliber/90f)*(Caliber/90f);

            TempBullet = BulletPooler.SharedInstance.GetPooledObject();
            TempBullet.transform.position = BulletStart.transform.position;
            TempBullet.transform.rotation = Quaternion.LookRotation(BulletStart.transform.forward);
            TempBullet.SetActive(true);
            // TempBullet = Instantiate(Bullet, BulletStart.transform.position, Quaternion.LookRotation(BulletStart.transform.forward));
            TempBullet.transform.localScale = new Vector3 (Caliber, Caliber, Caliber);
            Rigidbody rb = TempBullet.GetComponent<Rigidbody>();
            rb.mass = BulletMass;
            rb.velocity = BulletStart.transform.forward * BulletSpeed + PartScript.BodyScript.RigidBody.GetPointVelocity(transform.position);
            Accuracy = (Length-5f)*(Length-5f)*((Caliber*Caliber)/1000f);
            rb.AddForce(new Vector3(UnityEngine.Random.Range(-Accuracy, Accuracy), UnityEngine.Random.Range(-Accuracy, Accuracy), 0));
            Destroy(TempBullet, 30);

            if(Data.Tracers != -1f){
                if(TracerCount == 0){
                    TempBullet.GetComponentInChildren<MeshRenderer>().material = TracerMaterial;
                }
                TracerCount += 1;
                if(TracerCount > Data.Tracers){
                    TracerCount = 0;
                }
            }

            if(Data.Recoil == -1f){
                PartScript.BodyScript.RigidBody.AddForce(-gameObject.transform.forward*BulletMass*BulletSpeed*0.5f);
            } else{
                PartScript.BodyScript.RigidBody.AddForce(-gameObject.transform.forward*Data.Recoil);
            }

            if(Data.RateMultiplier){
                yield return new WaitForSeconds(1f/(Data.RateOfFire/10f));
            } else{
                yield return new WaitForSeconds(1f/Data.RateOfFire);
            }

            AllowFire = true;
        }
    }
}
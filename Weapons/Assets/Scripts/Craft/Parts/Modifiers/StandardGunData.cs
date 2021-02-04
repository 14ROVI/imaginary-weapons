namespace Assets.Scripts.Craft.Parts.Modifiers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;
    using ModApi.Craft.Parts;
    using ModApi.Craft.Parts.Attributes;
    using UnityEngine;

    [Serializable]
    [DesignerPartModifier("StandardGun")]
    [PartModifierTypeId("ImaginaryWeapons.StandardGun")]
    public class StandardGunData : PartModifierData<StandardGunScript>
    {
        [DesignerPropertyToggleButton(Label="Rate of Fire /10")]
        public bool RateMultiplier = false;

        [DesignerPropertySlider(MinValue = 1f, MaxValue = 50f, NumberOfSteps = 50, Label = "Rate Of Fire (rps)")]
        public float RateOfFire = 50f;

        [DesignerPropertySlider(MinValue = 400f, MaxValue = 1200f, NumberOfSteps = 41, Label = "Bullet Speed (m/s)")]
        public float BulletSpeed = 800f;

        [DesignerPropertySlider(MinValue = 12f, MaxValue = 152f, NumberOfSteps = 141, Label = "Caliber (mm)")]
        public float Caliber = 90f;

        [DesignerPropertySlider(MinValue = 0.5f, MaxValue = 5f, NumberOfSteps = 46, Label = "Length (m)")]
        public float Length = 2.6f;

        [DesignerPropertySlider(MinValue = -1f, MaxValue = 10f, NumberOfSteps = 12, Label = "Rounds per Tracer", Tooltip="-1 means no tracer rounds")]
        public float Tracers = -1f;


        [DesignerPropertySpinner("F", "Activation Group", "Space", "Return", "Left Click", "Right Click", Label= "input")]
        public string GunInput = "F";


        public float Recoil = -1f;
    }
}
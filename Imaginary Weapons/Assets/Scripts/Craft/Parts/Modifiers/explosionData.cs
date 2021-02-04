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
    [DesignerPartModifier("explosion")]
    [PartModifierTypeId("ImaginaryWeapons.explosion")]
    public class explosionData : PartModifierData<explosionScript>
    {
        [DesignerPropertySlider(MinValue = 10f, MaxValue = 100f, NumberOfSteps = 19, Label = "Explosion power")]
        public float explosionPower = 30f;
    }
}
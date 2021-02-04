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
    [DesignerPartModifier("laser")]
    [PartModifierTypeId("ImaginaryWeapons.laser")]
    public class laserData : PartModifierData<laserScript>
    {
        [DesignerPropertySlider(MinValue = 1f, MaxValue = 100f, NumberOfSteps = 100, Label = "Power")]
        public float power = 50f;

        [DesignerPropertySpinner("F", "Activation Group", "Space", "Return", "Left Click", "Right Click", Label= "input")]
        public string LaserInput = "F";
    }
}
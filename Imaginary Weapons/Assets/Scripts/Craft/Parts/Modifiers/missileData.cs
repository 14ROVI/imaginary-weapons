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
    [DesignerPartModifier("missile")]
    [PartModifierTypeId("ImaginaryWeapons.missile")]
    public class missileData : PartModifierData<missileScript>
    {
        [DesignerPropertyToggleButton(Label="Guided")]
        public bool guided = false;

        [DesignerPropertySpinner("Activation Group", "F", "Space", "Return", "Left Click", "Right Click", Label= "Input")]
        public string missileInput = "Activation Group";

        [DesignerPropertySlider(MinValue = 100f, MaxValue = 5000f, NumberOfSteps = 50, Label = "Missile power")]
        public float missilePower = 600f;

        [DesignerPropertySlider(MinValue = 1f, MaxValue = 20f, NumberOfSteps = 20, Label = "Burn Time (s)")]
        public float burnTime = 5f;
    }
}
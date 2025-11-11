using System;
using UnityEngine;


namespace MS.Data
{
    [Serializable]
    public class SkillSettingData
    {
        public string skillName;
        public string description;
        public float coefficient;
        public int manaCost;
        public string elementType;
    }
}

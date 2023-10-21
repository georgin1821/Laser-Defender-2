using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skills : Singleton<Skills>
{
    public int id;
    public ShipChaos.ChaosSkils chaosSkils;
    public ShipEnia.EniaSkills eniaSkills;

    public bool[] ChaosShipSkills;
    public bool[] EniaSkills;
    public List<bool[]> skills;

    private void InitializeShips()
    {

    }

    [System.Serializable]
    public class ShipChaos
    {
        public string name;
        public bool[] unlockedSkill;
        public enum ChaosSkils
        {
            SKILL1,
            SKILL2,
            SKILL3,
            SKILL4,
        }

    }

    public class ShipEnia
    {
        public enum EniaSkills
        {
            SKILL1,
            SKILL2,
            SKILL3,
            SKILL4,

        }
    }
}

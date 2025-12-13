using UnityEngine;

namespace DriftRacer.Data
{
    [CreateAssetMenu(fileName = "New Car", menuName = "DriftRacer/Car Data")]
    public class CarData : ScriptableObject
    {
        [Header("Car Info")]
        public string carName = "New Car";
        public Sprite carSprite;
        public int unlockCost = 1000;
        public bool isUnlockedByDefault = false;

        [Header("Base Stats")]
        [Range(5f, 20f)] public float maxSpeed = 10f;
        [Range(2f, 15f)] public float acceleration = 5f;
        [Range(50f, 200f)] public float turnRate = 100f;
        [Range(0.5f, 2f)] public float brakePower = 1f;

        [Header("Drift Stats")]
        [Range(0.01f, 0.3f)] public float driftFriction = 0.1f;
        [Range(50f, 150f)] public float driftTurnRate = 80f;
        [Range(1f, 5f)] public float minDriftSpeed = 2f;
        [Range(10f, 45f)] public float minDriftAngle = 15f;

        [Header("Physics")]
        [Range(0.5f, 3f)] public float mass = 1f;
        [Range(0.1f, 1f)] public float normalFriction = 0.5f;
        [Range(0.5f, 0.99f)] public float velocityDamping = 0.9f;

        [Header("Upgrade System")]
        public int maxUpgradeLevel = 5;

        [System.Serializable]
        public class UpgradeStats
        {
            public float speedIncrease = 1f;
            public float accelerationIncrease = 0.5f;
            public float handlingIncrease = 10f;
            public float driftControlIncrease = 0.02f;
        }

        public UpgradeStats upgradePerLevel = new UpgradeStats();

        [Header("Visual")]
        public Color primaryColor = Color.white;
        public Color secondaryColor = Color.gray;

        public CarData GetUpgradedStats(int engineLevel, int tireLevel, int brakeLevel, int handlingLevel)
        {
            CarData upgraded = Instantiate(this);

            upgraded.maxSpeed += engineLevel * upgradePerLevel.speedIncrease;
            upgraded.acceleration += engineLevel * upgradePerLevel.accelerationIncrease;
            upgraded.turnRate += handlingLevel * upgradePerLevel.handlingIncrease;
            upgraded.driftTurnRate += handlingLevel * upgradePerLevel.handlingIncrease;
            upgraded.driftFriction = Mathf.Max(0.01f, driftFriction - (tireLevel * upgradePerLevel.driftControlIncrease));
            upgraded.brakePower += brakeLevel * 0.2f;

            return upgraded;
        }
    }
}

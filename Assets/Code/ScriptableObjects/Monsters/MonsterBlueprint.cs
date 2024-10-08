using UnityEngine;

namespace Vampire
{
    public class MonsterBlueprint : ScriptableObject
    {
        [Header("Stats")]
        public new string name; 
        public float hp;
        public float atk;
        public float recovery;
        public float armor;
        public float atkspeed;
        public float movespeed;
        public float acceleration;
        [Header("Drops")]
        public LootTable<GemType> gemLootTable;
        public LootTable<CoinType> coinLootTable;
        [Header("Animation")]
        public Sprite[] walkSpriteSequence;
        public float walkFrameTime;
    }
}

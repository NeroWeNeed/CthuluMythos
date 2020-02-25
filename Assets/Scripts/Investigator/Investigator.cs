using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CMythos
{


    [CreateAssetMenu(fileName = "Investigator", menuName = "Cthulhu Mythos/Investigator", order = 0)]
    public class Investigator : ScriptableObject
    {
        [SerializeField]
        private int baseMovement;

        public int BaseMovement => baseMovement;

        [SerializeField]
        private string characterName;

        public string CharacterName => characterName;

        [SerializeField]
        private string lore;

        public string Lore => lore;

        [SerializeField]
        private string language;

        public string Language => language;

        [SerializeField]
        private string[] phobias;

        public string[] Phobias => phobias;

        [SerializeField]
        private int maxHealth;

        public int MaxHealth => maxHealth;

        [SerializeField]
        private int maxSanity;

        public int MaxSanity => maxSanity;

        [SerializeField]
        private int maxCards;

        public int MaxCards => maxCards;

        [SerializeField]
        private int power;

        public int Power => power;

    }
}
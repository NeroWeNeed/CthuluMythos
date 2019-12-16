

using UnityEngine;
using CMythos.Events;
namespace CMythos
{
    [CreateAssetMenu(fileName = "Card", menuName = "Cthulhu Mythos/Card", order = 0)]
    public class Card : ScriptableObject
    {
        public const int CARD_WIDTH = 128;
        public const int CARD_HEIGHT = 256;
        [SerializeField]
        private CardPlayEvent cardPlayEvent;

        public CardPlayEvent CardPlayEvent
        {
            get => cardPlayEvent;
        }

        [SerializeField]
        private CardActiveEvent cardActiveEvent;

        public CardActiveEvent CardActiveEvent
        {
            get => cardActiveEvent;
        }

        [SerializeField]
        private CardDiscardEvent cardDiscardEvent;

        public CardDiscardEvent CardDiscardEvent
        {
            get => cardDiscardEvent;
        }
        [SerializeField]
        private CardDrawEvent cardDrawEvent;

        public CardDrawEvent CardDrawEvent
        {
            get => cardDrawEvent;
        }


        [SerializeField]
        public string Title;

        [SerializeField]
        public string Description;

        [SerializeField]
        public CardType Type;

        [SerializeField]
        public Texture2D graphic;

        private void Start()
        {
            if (cardPlayEvent == null)
                cardPlayEvent = new CardPlayEvent();
        }

    }
}
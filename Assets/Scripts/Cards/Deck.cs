using System.Collections.Generic;
using UnityEngine;

namespace CMythos
{
    public class Deck
    {
        private readonly Stack<DeckCard> cards;

        public Deck()
        {
            cards = new Stack<DeckCard>();
            InitializeDeck();
        }
        private void InitializeDeck()
        {
            UnityEngine.Object[] cards = Resources.LoadAll("Cards", typeof(Card));
            this.cards.Clear();
            for (int i = 0; i < 100; i++)
            {
                this.cards.Push(new DeckCard(cards[UnityEngine.Random.Range(0, cards.Length - 1)]));
            }
        }
        public Card Draw()
        {
            if (cards.Count > 0)
            {
                return cards.Pop().Initiate();
            }
            else
                return null;
        }


    }
    public class DeckCard
    {
        private UnityEngine.Object cardBase;

        public DeckCard(Object cardBase)
        {
            this.cardBase = cardBase;
        }

        public Card Initiate()
        {
            return UnityEngine.Object.Instantiate(cardBase) as Card;
        }
    }

}
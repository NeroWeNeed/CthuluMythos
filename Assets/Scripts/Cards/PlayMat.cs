

using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

namespace CMythos
{

    public class PlayMat
    {
        private Dictionary<CardType, PlayMatPile> piles = Enum.GetValues(typeof(CardType)).Cast<CardType>().Select(x => new PlayMatPile(x)).ToDictionary(x =>
            {
                return x.Type;
            });
        public Dictionary<CardType, PlayMatPile> Piles
        {
            get => piles;
        }
        private Dictionary<CardType, PlayMatPile> discardPiles = Enum.GetValues(typeof(CardType)).Cast<CardType>().Select(x => new PlayMatPile(x)).ToDictionary(x =>
            {
                return x.Type;
            });
        public Dictionary<CardType, PlayMatPile> DiscardPiles
        {
            get => discardPiles;
        }

        public PlayMatPile GetPile(CardType type)
        {
            return piles[type];
        }
        public PlayMatPile GetDiscardPile(CardType type)
        {
            return discardPiles[type];
        }
        public void Discard(Card card)
        {
            discardPiles[card.Type].Cards.Push(card);
        }
        public void Play(Card card)
        {
            piles[card.Type].Cards.Push(card);

        }


    }
    public class PlayMatPile
    {
        public CardType Type
        {
            get;
            private set;
        }

        public Stack<Card> Cards
        {
            get;
            private set;
        }

        public PlayMatPile(CardType type)
        {
            Type = type;
            Cards = new Stack<Card>();
        }
        public Card GetTopCard()
        {
            if (Cards.Count > 0)
                return Cards.Peek();
            else
                return null;
        }
    }
}
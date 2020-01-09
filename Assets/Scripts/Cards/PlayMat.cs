

using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

namespace CMythos
{

    public class PlayMat
    {
        public Dictionary<CardType, PlayMatPile> Piles { get; } = Enum.GetValues(typeof(CardType)).Cast<CardType>().Select(x => new PlayMatPile(x)).ToDictionary(x =>
                                                                             {
                                                                                 return x.Type;
                                                                             });
        public Dictionary<CardType, PlayMatPile> DiscardPiles { get; } = Enum.GetValues(typeof(CardType)).Cast<CardType>().Select(x => new PlayMatPile(x)).ToDictionary(x =>
                                                                                    {
                                                                                        return x.Type;
                                                                                    });

        public PlayMatPile GetPile(CardType type)
        {
            return Piles[type];
        }
        public PlayMatPile GetDiscardPile(CardType type)
        {
            return DiscardPiles[type];
        }
        public void Discard(Card card)
        {
            
            DiscardPiles[card.Type].Cards.Push(card);
            
        }
        public void Play(Card card)
        {
            Piles[card.Type].Cards.Push(card);

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
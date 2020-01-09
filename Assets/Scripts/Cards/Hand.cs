
using System.Collections.Generic;

namespace CMythos
{
    public class Hand
    {
        private const int maxCards = 7;
        public static int MAX_CARDS { get => maxCards; }

        private List<Card> cards;


        public int Count { get => Cards.Count; }
        public bool IsFull { get => cards.Count < MAX_CARDS; }

        public IReadOnlyList<Card> Cards { get; private set; }
        public Hand()
        {
            cards = new List<Card>();
            Cards = cards.AsReadOnly();

        }
        public void Fill(Deck deck)
        {
            
            while (Count < MAX_CARDS) {
                Draw(deck);
            }
        }


        public Card Draw(Deck deck)
        {
            Card card = deck.Draw();
            if (card != null)
            {
                cards.Add(card);
                return card;
            }
            return null;
        }
        public int IndexOf(Card card)
        {
            return cards.IndexOf(card);
        }
        public bool Play(Card card, PlayMat playMat)
        {
            if (cards.Contains(card))
            {
                playMat.Play(card);
                cards.Remove(card);
                return true;
            }
            return false;
        }
        public bool Play(int index, PlayMat playMat)
        {
            if (index < Count && index >= 0)
            {
                playMat.Play(cards[index]);
                cards.RemoveAt(index);
                return true;
            }
            return false;
        }
        public bool Discard(Card card, PlayMat playMat)
        {
            if (cards.Contains(card))
            {
                playMat.Discard(card);
                cards.Remove(card);
                return true;
            }
            return false;
        }
        public bool Discard(int index, PlayMat playMat)
        {
            if (index < Count && index >= 0)
            {
                playMat.Discard(cards[index]);
                cards.RemoveAt(index);
                return true;
            }
            return false;
        }


    }
}
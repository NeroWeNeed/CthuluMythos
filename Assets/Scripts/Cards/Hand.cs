using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.UI;

namespace CMythos
{

    [RequireComponent(typeof(Canvas), typeof(GraphicRaycaster))]
    public class Hand : MonoBehaviour
    {
        private const int cardsPerHand = 7;
        public static int CARDS_PER_HAND { get => cardsPerHand; }

        private const int cardsPerDeck = 40;
        public static int CARDS_PER_DECK { get => cardsPerDeck; }
        [SerializeField]
        private float pileSpacing = 5.0f;

        [SerializeField]
        private float pileHorizontalMargin = 10.0f;


        private List<Card> cardsInHand;
        private Stack<Card> cardsInDeck;
        private List<CardRenderer> cardRenderers;
        private bool deckInitialized = false;
        private Canvas canvas;
        private void Start()
        {
            canvas = GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            cardsInHand = new List<Card>(CARDS_PER_HAND);
            cardsInDeck = new Stack<Card>();
            cardRenderers = new List<CardRenderer>(CARDS_PER_HAND);
            CardRenderer[] childCardRenderers = GetComponentsInChildren<CardRenderer>();
            GameObject obj;
            float lastX = 0.0f;
            CardRenderer renderer;
            for (int i = 0; i < CARDS_PER_HAND; i++)
            {
                if (i < childCardRenderers.Length)
                    renderer = childCardRenderers[i];

                else
                {
                    obj = new GameObject($"Card Renderer {i}", typeof(CardRenderer));
                    obj.transform.SetParent(transform, false);
                    renderer = obj.GetComponent<CardRenderer>();
                }



                renderer.transform.position = new Vector3(Card.CARD_WIDTH + pileSpacing + lastX, 0, 0);
                lastX += Card.CARD_WIDTH + pileSpacing;

                cardRenderers.Add(renderer);
            }


        }
        public Card DrawCard()
        {
            int index = 0;
            if (!IsHandFull() && !IsDeckEmpty())
            {
                Card card;
                card = cardsInDeck.Pop();
                cardsInHand.Add(card);
                Debug.Log(cardsInHand.IndexOf(card));
                UpdateCardRenderers();


                Debug.Log("Hand size is " + cardsInHand.Count);
                return card;
            }
            else
                return null;
        }
        private void UpdateCardRenderers()
        {
            for (int i = 0; i < cardRenderers.Count; i++)
            {
                if (i < cardsInHand.Count)
                    cardRenderers[i].Card = cardsInHand[i];
                else
                    cardRenderers[i].Card = null;
            }
        }
        public bool IsHandFull()
        {
            return cardsInHand.Count >= CARDS_PER_HAND;
        }
        public bool IsDeckEmpty()
        {
            return cardsInDeck.Count == 0;
        }
        public bool Discard(Card card, PlayMat playMat)
        {
            int index = cardsInHand.IndexOf(card);
            if (index >= 0)
            {
                cardsInHand.RemoveAt(index);
                UpdateCardRenderers();
                playMat.Discard(card);
                Debug.Log("Hand size is " + cardsInHand.Count);
                return true;
            }
            return false;
        }
        public bool Play(Card card, PlayMat playMat)
        {
            int index = cardsInHand.IndexOf(card);
            if (index >= 0)
            {
                cardsInHand.RemoveAt(index);
                UpdateCardRenderers();
                playMat.Play(card);
                return true;
            }
            return false;
        }
        public void InitializeDeck()
        {
            if (!deckInitialized)
            {
                deckInitialized = true;
                
                Card[] cards = AssetDatabase.LoadAllAssetsAtPath("Assets/Resources/Cards") as Card[];
                cardsInDeck.Clear();
                for (int i = 0; i < 100; i++)
                {
                    
                    cardsInDeck.Push(Instantiate(cards[UnityEngine.Random.Range(0,cards.Length-1)]));
                }
            }

        }
        private bool played = false;
        private void Update()
        {

            if (Input.GetKey(KeyCode.L))
            {
                if (!played)
                {
                    played = true;
                    Debug.Log("Playing...");
                    GetComponentInParent<GameBoardPlayer>().Discard(cardsInHand.First());
                }
            }
            else
                played = false;
        }
    }

}
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.UI;

namespace CMythos
{

    [RequireComponent(typeof(Canvas), typeof(GraphicRaycaster), typeof(PlayerViewUIRefreshable))]
    public class HandRenderer : MonoBehaviour
    {
        private const int cardsPerHand = 7;
        public static int CARDS_PER_HAND { get => cardsPerHand; }

        private const int cardsPerDeck = 40;
        public static int CARDS_PER_DECK { get => cardsPerDeck; }
        [SerializeField]
        private float pileSpacing = 5.0f;

        [SerializeField]
        private float pileHorizontalMargin = 10.0f;

        private List<CardRenderer> cardRenderers;
        private bool deckInitialized = false;
        private Canvas canvas;

        public PlayerViewUI PlayerViewUI { get; set; }
        public Hand Hand
        {
            get => PlayerViewUI.CurrentPlayer.Hand;
        }
        public Deck Deck
        {
            get => PlayerViewUI.CurrentPlayer.Deck;
        }
        public GameBoardPlayer Player
        {
            get => PlayerViewUI.CurrentPlayer;
        }

        public bool LeftControl { get; private set; }

        public void UpdateCardRenderers()
        {
            PlayerViewUI = GetComponentInParent<PlayerViewUI>();
            for (int i = 0; i < cardRenderers.Count; i++)
            {
                if (i < Hand.Count)
                    cardRenderers[i].Card = Hand.Cards[i];
                else
                    cardRenderers[i].Card = null;
            }
        }
        public bool Discard(Card card, PlayMat playMat)
        {

            if (Hand.Discard(Hand.IndexOf(card), playMat))
            {
                UpdateCardRenderers();
                return true;
            }
            return false;

        }

        public bool Discard(int index, PlayMat playMat)
        {
            if (Hand.Discard(index, playMat))
            {
                UpdateCardRenderers();
                return true;
            }
            return false;

        }
        public bool Play(Card card, PlayMat playMat)
        {
            if (Hand.Play(card, playMat))
            {
                UpdateCardRenderers();
                return true;
            }
            return false;
        }
        public bool Play(int index, PlayMat playMat)
        {
            if (Hand.Play(index, playMat))
            {
                UpdateCardRenderers();
                return true;
            }
            return false;
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
                LeftControl = true;
            if (Input.GetKeyUp(KeyCode.LeftControl))
                LeftControl = false;
        }
        public void Initialize()
        {

            canvas = GetComponent<Canvas>();
            PlayerViewUI = GetComponentInParent<PlayerViewUI>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
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

                renderer.GetComponent<Button>().onClick.AddListener(renderer.Interact);
            }

        }

    }

}
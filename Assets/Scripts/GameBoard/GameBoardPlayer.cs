using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CMythos
{
    [RequireComponent(typeof(TurnManagable), typeof(GameBoardEntity))]
    public class GameBoardPlayer : MonoBehaviour
    {


        private TurnManagable turnManagable;
        private GameBoardEntity entity;
        private PlayMat playMat;
        private Hand hand;

        [SerializeField]
        private PlayMatRenderer playMatRenderer;

        [SerializeField]
        private float health;

        public float Health
        {
            get => health;
            set => health = value;
        }
        [SerializeField]
        private float sanity;

        public float Sanity
        {
            get => sanity;
            set => sanity = value;
        }

        void Start()
        {
            turnManagable = GetComponent<TurnManagable>();
            entity = GetComponent<GameBoardEntity>();
            entity.InfoInitializer = FindAsylum;
            playMat = new PlayMat();
            playMatRenderer = GetComponentInChildren<PlayMatRenderer>();
            if (playMatRenderer == null)
            {
                GameObject obj = new GameObject($"{transform.name} PlayMat Renderer", typeof(PlayMatRenderer));
                obj.transform.parent = transform;
                playMatRenderer = obj.GetComponent<PlayMatRenderer>();
                playMatRenderer.Init(playMat);
            }
            if (hand == null)
            {
                GameObject obj = new GameObject($"{transform.name} Hand", typeof(Hand));
                obj.transform.parent = transform;
                hand = obj.GetComponent<Hand>();
            }

            turnManagable.TurnStartedEvent.AddListener(InitTurn);
            turnManagable.GetTurnManager().RoundCycleStartEvent.AddListener(Initialize);

        }
        private void Initialize()
        {
            Debug.Log("Initiating...");
            hand.InitializeDeck();
            while (hand.DrawCard() != null)
            {

            }
        }


        private void InitTurn()
        {
            Debug.Log("My Turn");
            foreach (var item in playMat.Piles.Values)
            {
                foreach (var item2 in item.Cards)
                {
                    item2.CardActiveEvent.Invoke(this, item2);
                }
            }
        }

        private GameBoardEntityInfo FindAsylum(GameBoard gameBoard)
        {
            Debug.Log("SEARCHING FOR ASYLUM");
            foreach (var item in gameBoard.tiles)
            {


                if (item.Effect != null && item.Effect.tag == "Asylum")
                {
                    Vector3Int coords = gameBoard.GetCoordinates(item);
                    Debug.Log($"Starting at {coords}");
                    Debug.Log($"Direction {GetComponentInParent<GameBoardManager>().PathOutDirection(coords)?.Opposite() ?? GameBoardEntityDirection.NORTH}");
                    return new GameBoardEntityInfo
                    {
                        coordinates = coords,
                        direction = GetComponentInParent<GameBoardManager>().PathOutDirection(coords)?.Opposite() ?? GameBoardEntityDirection.NORTH
                    };

                }

            }
            return new GameBoardEntityInfo
            {
                coordinates = new Vector3Int(-1, -1, -1)
            };
        }
        private bool toggled = false;
        private void Update()
        {

            if (Input.GetKey(KeyCode.Space) && !toggled)
            {
                toggled = true;
                Debug.Log($"Player Tile: {GetComponentInParent<GameBoardManager>().GetCoordinates(entity)}");
            }
            else
            {
                toggled = false;
            }

        }


        //General Commands
        public void DrawCard()
        {
            Card card = hand.DrawCard();
            if (card != null)
            {
                card.CardDrawEvent.Invoke(this, card);
            }
        }
        public bool IsHandFull()
        {
            return hand.IsHandFull();
        }
        public void Discard(Card card)
        {
            if (hand.Discard(card, playMat))
                card.CardDiscardEvent.Invoke(this, card);
            playMatRenderer.Refresh();
        }
        public void Play(Card card)
        {
            if (hand.Play(card, playMat))
                card.CardPlayEvent.Invoke(this, card);
            playMatRenderer.Refresh();
        }
    }
}
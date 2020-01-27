using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CMythos
{
    [RequireComponent(typeof(TurnManagable), typeof(GameBoardEntity), typeof(Initializable))]
    public class GameBoardPlayer : MonoBehaviour
    {


        private TurnManagable turnManagable;
        private GameBoardEntity entity;
        private PlayMat playMat;
        public PlayMat PlayMat
        {
            get => playMat;
        }
        public Hand Hand { get; private set; }

        public Deck Deck { get; private set; }


        private PlayMatRenderer PlayMatRenderer { get => GetComponentInParent<GameBoardManager>()?.PlayMatRenderer; }

        [SerializeField]
        private float health = 10.0f;

        public float Health
        {
            get => health;
            set => health = value;
        }
        [SerializeField]
        private float sanity = 100.0f;

        public float Sanity
        {
            get => sanity;
            set => sanity = value;
        }

        public void Awake()
        {

            turnManagable = GetComponent<TurnManagable>();
            entity = GetComponent<GameBoardEntity>();
            entity.InfoInitializer = FindAsylum;
            playMat = new PlayMat();
            Hand = new Hand();
            Deck = new Deck();
            Hand.Fill(Deck);

            turnManagable.TurnStartedEvent.AddListener(InitTurn);


        }

        public Vector3Int GetCoordinates()
        {
            return transform.parent.GetComponentInParent<GameBoardManager>().GetInformation(entity).coordinates;
        }
        public Vector3Int SetCoordinates(Vector3Int coordinates)
        {
            return transform.parent.GetComponentInParent<GameBoardManager>().GetInformation(entity).coordinates = coordinates;
        }

        public GameBoardManager GetGameBoardManager()
        {
            return transform.parent.GetComponentInParent<GameBoardManager>();
        }

        private void InitTurn()
        {
            Debug.Log($"{name}'s turn");
            GetComponentInParent<GameBoardManager>()?.PlayMatRenderer?.Refresh(playMat);
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

            foreach (var item in gameBoard.tiles)
            {
                if (item.Effect != null && item.Effect.tag == "Asylum")
                {
                    Vector3Int coords = gameBoard.GetCoordinates(item);
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


        //General Commands
        public Card DrawCard()
        {

            Card card = Hand.Draw(Deck);
            if (card != null)
            {
                card.CardDrawEvent.Invoke(this, card);
            }
            return card;
        }
        public bool IsHandFull()
        {
            return Hand.IsFull;
        }
        public void Discard(Card card)
        {
            if (Hand.Discard(card, playMat))
                card.CardDiscardEvent.Invoke(this, card);
            PlayMatRenderer.Refresh(playMat);
        }
        public void Discard(int index)
        {
            Card card = Hand.Cards[index];
            if (Hand.Discard(index, playMat))
            {

                PlayMatRenderer.Refresh(playMat);
                card.CardDiscardEvent.Invoke(this, card);

            }

        }
        public void Play(Card card)
        {
            if (Hand.Play(card, playMat))
                card.CardPlayEvent.Invoke(this, card);
            PlayMatRenderer.Refresh(playMat);
        }
        public void Play(int index)
        {
            Card card = Hand.Cards[index];
            if (Hand.Play(index, playMat))
            {

                PlayMatRenderer.Refresh(playMat);
                card.CardPlayEvent.Invoke(this, card);

            }

        }
        public void EndTurn()
        {
            turnManagable.EndTurn();
        }
    }
}
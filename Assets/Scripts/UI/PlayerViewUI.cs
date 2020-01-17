using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using CMythos.Events;

namespace CMythos
{
    [RequireComponent(typeof(Canvas))]
    public class PlayerViewUI : MonoBehaviour
    {
        [SerializeField]
        private PlayerChangedEvent playerChanged;

        public PlayerChangedEvent PlayerChanged
        {
            get => playerChanged;
        }


        [SerializeField]
        private GameBoardPlayer currentPlayer;

        public GameBoardPlayer CurrentPlayer
        {
            get => currentPlayer;
            set
            {
                currentPlayer = value;
            }
        }
        [SerializeField]
        private DiceShooter diceShooter;
        public DiceShooter DiceShooter
        {
            get => diceShooter;
            set => diceShooter = value;
        }
        [SerializeField]
        public HandRenderer HandRenderer;
        [SerializeField]
        private GameBoardManager manager;

        public GameBoardManager GameBoardManager
        {
            get => manager;
            set => manager = value;
        }
        private void Awake()
        {
            if (playerChanged == null)
                playerChanged = new PlayerChangedEvent();
        }


        public void Init()
        {

            GameBoardManager.AmbiguousDirectionEvent.AddListener(() =>
            {
                Transform child;
                DirectionSelector selector;
                bool disablePlayerActions = false;
                foreach (var item in GameObject.FindGameObjectsWithTag("DirectionSelect"))
                {


                    for (int i = 0; i < item.transform.childCount; i++)
                    {
                        child = item.transform.GetChild(i);
                        selector = child.GetComponent<DirectionSelector>();

                        if (selector != null && GameBoardManager.CanMove(CurrentPlayer.GetCoordinates(), GameBoardManager.GetDirection(CurrentPlayer.GetComponent<GameBoardEntity>(), selector.Direction)))
                        {
                            child.gameObject.SetActive(true);
                            disablePlayerActions = true;
                        }

                    }


                }



            });
            GameBoardManager.AmbiguousDirectionSolvedEvent.AddListener(() =>
            {

                foreach (var item in GameObject.FindGameObjectsWithTag("DirectionSelect"))
                {
                    for (int i = 0; i < item.transform.childCount; i++)
                    {
                        item.transform.GetChild(i).gameObject.SetActive(false);
                    }


                }
                

            });
            if (HandRenderer == null)
            {
                HandRenderer = GetComponentInChildren<HandRenderer>();
                if (HandRenderer == null)
                {
                    GameObject obj = new GameObject("Hand Renderer", typeof(HandRenderer));
                    obj.transform.SetParent(transform);
                    HandRenderer = obj.GetComponent<HandRenderer>();

                }
            }
            HandRenderer.Initialize();
        }
        public void SetCurrentPlayer(TurnManagable turnManagable)
        {

            currentPlayer = turnManagable.GetComponent<GameBoardPlayer>();
            GetComponentInChildren<PlayMatPileRenderer>()?.Refresh(currentPlayer.PlayMat);
            HandRenderer.UpdateCardRenderers();
            foreach (var item in GetComponentsInChildren<PlayerViewUIRefreshable>())
            {
                if (item.Refresher != null)
                    item.Refresher.Invoke(currentPlayer);
            }
        }


        public void DrawCard(GameBoardPlayer player)
        {
            Debug.Log("Drawing for player " + player.name);
            if (player?.DrawCard() != null)
                GetComponentInChildren<HandRenderer>().UpdateCardRenderers();
        }
        public void EndTurn(GameBoardPlayer player)
        {
            player?.EndTurn();
        }
        public void Move(GameBoardPlayer player)
        {
            DiceShootCallback diceShootCallback = player.GetComponentInChildren<DiceShootCallback>();
            if (diceShootCallback == null)
            {
                GameObject obj = new GameObject("Dice Shooter Callback", typeof(DiceShootCallback));
                obj.transform.parent = player.transform;
                diceShooter.Shoot(obj.GetComponent<DiceShootCallback>().Callback, manager.MovementDice);



            }
        }

        public void SignalNorthDirection(GameBoardPlayer player)
        {
            DiceShootCallback callback = player.GetComponentInChildren<DiceShootCallback>();
            if (callback == null)
                return;
            callback.Choose(GameBoardEntityDirection.NORTH);
        }
        public void SignalSouthDirection(GameBoardPlayer player)
        {
            DiceShootCallback callback = player.GetComponentInChildren<DiceShootCallback>();
            if (callback == null)
                return;
            callback.Choose(GameBoardEntityDirection.SOUTH);
        }
        public void SignalEastDirection(GameBoardPlayer player)
        {
            DiceShootCallback callback = player.GetComponentInChildren<DiceShootCallback>();
            if (callback == null)
                return;
            callback.Choose(GameBoardEntityDirection.EAST);
        }
        public void SignalWestDirection(GameBoardPlayer player)
        {
            DiceShootCallback callback = player.GetComponentInChildren<DiceShootCallback>();
            if (callback == null)
                return;
            callback.Choose(GameBoardEntityDirection.WEST);
        }
        public void SignalFowardDirection(GameBoardPlayer player)
        {
            DiceShootCallback callback = player.GetComponentInChildren<DiceShootCallback>();
            if (callback == null)
                return;
            callback.Choose(player.GetComponentInParent<GameBoardManager>().GetInformation(player.GetComponent<GameBoardEntity>()).direction);
        }
        public void SignalLeftDirection(GameBoardPlayer player)
        {
            DiceShootCallback callback = player.GetComponentInChildren<DiceShootCallback>();
            if (callback == null)
                return;
            callback.Choose(player.GetComponentInParent<GameBoardManager>().GetInformation(player.GetComponent<GameBoardEntity>()).direction.Left());
        }
        public void SignalRightDirection(GameBoardPlayer player)
        {
            DiceShootCallback callback = player.GetComponentInChildren<DiceShootCallback>();
            if (callback == null)
                return;
            callback.Choose(player.GetComponentInParent<GameBoardManager>().GetInformation(player.GetComponent<GameBoardEntity>()).direction.Right());
        }
    }


}
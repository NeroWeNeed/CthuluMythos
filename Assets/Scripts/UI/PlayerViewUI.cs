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
        private GameBoardManager manager;

        public GameBoardManager GameBoardManager
        {
            get => manager;
            set => manager = value;
        }


        private void Start()
        {
            if (playerChanged == null)
                playerChanged = new PlayerChangedEvent();
        }

        public void SetCurrentPlayer(TurnManagable turnManagable)
        {

            currentPlayer = turnManagable.GetComponent<GameBoardPlayer>();
            GetComponentInChildren<PlayMatPileRenderer>()?.UpdatePile(currentPlayer.PlayMat);
            foreach (var item in GetComponentsInChildren<PlayerViewUIAction>())
            {
                item.Refresh();
            }

        }


        public void DrawCard(GameBoardPlayer player)
        {
            Debug.Log("Drawing...");
            player?.DrawCard();
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
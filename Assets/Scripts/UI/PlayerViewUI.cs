using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace CMythos
{
    [RequireComponent(typeof(Canvas))]
    public class PlayerViewUI : MonoBehaviour
    {


        [SerializeField]
        private GameBoardPlayer currentPlayer;

        public GameBoardPlayer CurrentPlayer
        {
            get => currentPlayer;
            set
            {
                currentPlayer = value;
                if (currentPlayer != null)
                    currentPlayer.CanRoll = true;
                movingSum = 0;
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


        //Flags
        private int movingSum = 0;
        private bool ambiguousPath = false;


        public void MoveCurrentPlayer()
        {
            if (currentPlayer != null && movingSum == 0 && currentPlayer.CanRoll)
            {
                Debug.Log("Shooting...");
                currentPlayer.CanRoll = false;
                diceShooter.Shoot(DiceShootCallback, manager.MovementDice);

            }

        }
        public void EndCurrentPlayerTurn()
        {
            Debug.Log(movingSum);
            if (currentPlayer != null && movingSum == 0)
            {
                Debug.Log("Ending...");
                currentPlayer.GetComponent<TurnManagable>().EndTurn();
            }
        }
        private void DiceShootCallback(Dictionary<Dice, string> values)
        {
            int sum = values.Values.Sum(x => System.Convert.ToInt32(x));
            foreach (var item in values.Keys)
            {
                Destroy(item.gameObject);
            }
            movingSum = GameBoardManager.TryMove(currentPlayer.GetComponent<GameBoardEntity>(), sum);

            if (movingSum != 0)
            {
                ambiguousPath = true;
            }
            


        }
    }


}

namespace CMythos
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class DiceShootCallback : MonoBehaviour
    {

        private int movingSum = 0;



        public void Callback(Dictionary<Dice, string> values)
        {
            int sum = values.Values.Sum(x => System.Convert.ToInt32(x));
            foreach (var item in values.Keys)
            {
                Destroy(item.gameObject);
            }
            
            GameBoardManager manager = GetComponentInParent<GameBoardManager>();
            GameBoardPlayer player = GetComponentInParent<GameBoardPlayer>();
            sum += player.BaseMovement;
            movingSum = manager.TryMove(player.GetComponent<GameBoardEntity>(), sum);

            if (movingSum <= 0)
            {
                manager.AmbiguousDirectionSolvedEvent.Invoke();

                Destroy(this.gameObject);
            }
            else
            {
                GetComponentInParent<GameBoardManager>()?.AmbiguousDirectionEvent?.Invoke();
            }
        }
        public void Choose(GameBoardEntityDirection direction)
        {
            if (movingSum > 0)
            {
                GameBoardManager manager = GetComponentInParent<GameBoardManager>();
                GameBoardPlayer player = GetComponentInParent<GameBoardPlayer>();
                movingSum = manager.TryMove(player.GetComponent<GameBoardEntity>(), movingSum, direction);
                
                if (movingSum <= 0)
                {
                    manager.AmbiguousDirectionSolvedEvent.Invoke();

                    Destroy(this.gameObject);
                }
                else
                {
                    GetComponentInParent<GameBoardManager>()?.AmbiguousDirectionEvent?.Invoke();
                }
            }
            else
            {
                GetComponentInParent<GameBoardManager>()?.AmbiguousDirectionSolvedEvent.Invoke();
                Destroy(this.gameObject);
            }
        }


    }
}

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

            movingSum = GetComponentInParent<GameBoardManager>().TryMove(GetComponentInParent<GameBoardPlayer>().GetComponent<GameBoardEntity>(), sum);

            if (movingSum == 0)
            {
                Destroy(this.gameObject);
            }
            else {
                GetComponentInParent<GameBoardManager>()?.AmbiguousDirectionEvent?.Invoke();
            }
        }
        public void Choose(GameBoardEntityDirection direction)
        {
            if (movingSum != 0)
            {
                movingSum = GetComponentInParent<GameBoardManager>().TryMove(GetComponentInParent<GameBoardPlayer>().GetComponent<GameBoardEntity>(), movingSum,direction);

                if (movingSum == 0)
                {
                    GetComponentInParent<GameBoardManager>()?.AmbiguousDirectionSolvedEvent?.Invoke();
                    Destroy(this.gameObject);
                }
                else {
                    GetComponentInParent<GameBoardManager>()?.AmbiguousDirectionEvent?.Invoke();
                }
            }
        }


    }
}
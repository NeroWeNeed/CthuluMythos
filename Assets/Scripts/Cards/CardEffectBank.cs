using System.Linq;
using UnityEditor;
using UnityEngine;
namespace CMythos
{


    public class CardEffectBank : MonoBehaviour
    {
        public void HealRollCard(GameBoardPlayer player, Card card)
        {

            player.GetGameBoardManager().DiceShooter.Shoot(x =>
            {
                int sum = x.Select(a =>
                {
                    if (a.Value == null)
                    {
                        
                        return int.Parse(a.Key.ApproximateValue());
                    }
                    else
                    {
                        return int.Parse(a.Value);
                    }
                }).Sum();
                Debug.Log("Healing by " + sum);
                player.Health = Mathf.Min(100,player.Health+sum);
                foreach (var item in x.Keys)
                {
                    Destroy(item.gameObject);
                }
            }, Resources.Load<Dice>("Dice/Dice4"), 
            Resources.Load<Dice>("Dice/Dice6"), 
            Resources.Load<Dice>("Dice/Dice8"),
            Resources.Load<Dice>("Dice/Dice4"), 
            Resources.Load<Dice>("Dice/Dice4")
            );
        }
    }
}
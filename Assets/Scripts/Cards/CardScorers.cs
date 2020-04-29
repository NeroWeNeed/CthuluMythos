using System.Linq;
using UnityEngine;

namespace CMythos
{
    
    
    public class CardScorers : MonoBehaviour {
        public void PointsPerTurnOnMat(GameBoardPlayer player, Card card)
        {
            Debug.Log("Testing here");
            if (player.PlayMat.Piles.Any(x => x.Value.Cards.Count > 0 && x.Value.Cards.Peek() == card))
                player.Score += 10f;
        }
    }
    
}
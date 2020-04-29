using System.Collections.Generic;
using UnityEngine;
namespace CMythos
{

    public class ScoreKeeper : MonoBehaviour
    {
        [SerializeField]
        private TurnManager turnManager;

        private void Awake()
        {
            turnManager.RoundEndEvent.AddListener(SumPoints);
        }

        private void SumPoints(List<TurnManagable> managables)
        {
            Debug.Log("Summing...");
            foreach (var managable in managables)
            {

                var player = managable.gameObject.GetComponent<GameBoardPlayer>();
                if (player == null)
                    continue;
                Debug.Log(managable);
                foreach (var card in player.Hand)
                {

                    card.CardScoreEvent.Invoke(player, card);
                }
                foreach (var card in player.PlayMat.Piles)
                {
                    if (card.Value.Cards.Count > 0)
                        card.Value.Cards.Peek().CardScoreEvent.Invoke(player, card.Value.Cards.Peek());
                }
                foreach (var card in player.PlayMat.DiscardPiles)
                {
                    if (card.Value.Cards.Count > 0)
                        card.Value.Cards.Peek().CardScoreEvent.Invoke(player, card.Value.Cards.Peek());
                }
                GameConfiguration.standings[player.name] = player.Score;

            }
        }
    }
}
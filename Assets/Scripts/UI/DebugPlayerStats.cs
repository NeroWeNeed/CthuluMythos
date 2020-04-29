using UnityEngine;
using UnityEngine.UI;

namespace CMythos
{

    [RequireComponent(typeof(PlayerViewUIRefreshable), typeof(RectTransform))]
    public class DebugPlayerStats : MonoBehaviour
    {
        private Text coordinateText, healthText, sanityText, nameText, turnText;
        private GameBoardPlayer targetPlayer;

        
        private void Awake()
        {
            //GetComponent<RectTransform>().position = new Vector3(100,200,0);
            GameObject playerName = new GameObject("Player Name");
            nameText = configureText(playerName, 0);
            GameObject coordinates = new GameObject("Player Coordinates");
            coordinateText = configureText(coordinates, 1);
            GameObject health = new GameObject("Player Health");
            healthText = configureText(health, 2);
            GameObject sanity = new GameObject("Player Sanity");
            sanityText = configureText(sanity, 3);
            GameObject currentTurn = new GameObject("Current Turn");
            turnText = configureText(currentTurn, 4);

            GetComponent<PlayerViewUIRefreshable>().Refresher = Refresh;
        }
        private Text configureText(GameObject obj, int num)
        {

            obj.transform.SetParent(transform, false);

            Text objText = obj.AddComponent<Text>();

            objText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            objText.color = Color.black;

            var rect = objText.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(0f, 1f);
            rect.anchoredPosition = new Vector2(rect.sizeDelta.x / 2f, rect.sizeDelta.y / -2f);
            obj.transform.position += new Vector3(20, -20 * num * GetComponentInParent<Canvas>().scaleFactor, 0);

            return objText;
        }
        private void Refresh(GameBoardPlayer player)
        {
            targetPlayer = player;
        }
        private void Update()
        {
            if (targetPlayer != null)
            {
                nameText.text = $"Name: {targetPlayer.name}";
                coordinateText.text = $"Tile: {targetPlayer.GetCoordinates().ToString()}";
                healthText.text = $"Health: {targetPlayer.Health}";
                sanityText.text = $"Sanity: {targetPlayer.Sanity}";
                turnText.text = $"Current Turn: {targetPlayer.GetComponentInParent<TurnManager>().CurrentTurn}";
            }
        }
    }

}
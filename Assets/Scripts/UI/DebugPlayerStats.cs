using UnityEngine;
using UnityEngine.UI;

namespace CMythos
{

    [RequireComponent(typeof(PlayerViewUIRefreshable), typeof(RectTransform))]
    public class DebugPlayerStats : MonoBehaviour
    {
        private Text coordinateText, healthText, sanityText;
        private GameBoardPlayer targetPlayer;
        private void Start()
        {
            //GetComponent<RectTransform>().position = new Vector3(100,200,0);

            GameObject coordinates = new GameObject("Player Coordinates");
            coordinateText = configureText(coordinates, 0);
            GameObject health = new GameObject("Player Health");
            healthText = configureText(health, 1);
            GameObject sanity = new GameObject("Player Sanity");
            sanityText = configureText(sanity, 2);
            GetComponent<PlayerViewUIRefreshable>().Refresher = Refresh;
        }
        private Text configureText(GameObject obj, int num)
        {

            obj.transform.SetParent(transform, false);
            obj.transform.position += new Vector3(20, -20 * num, 0);
            Text objText = obj.AddComponent<Text>();
            objText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            objText.color = Color.black;
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
                coordinateText.text = $"Tile: {targetPlayer.GetCoordinates().ToString()}";
                healthText.text = $"Health: {targetPlayer.Health}";
                sanityText.text = $"Sanity: {targetPlayer.Sanity}";
            }
        }
    }

}
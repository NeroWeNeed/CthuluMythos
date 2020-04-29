using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.RectTransform;
using static UnityEngine.UI.ContentSizeFitter;

namespace CMythos
{
    [RequireComponent(typeof(Canvas))]
    public class ResultsScreen : MonoBehaviour
    {

        private void Start()
        {
            var offset = -20f;
            foreach (var player in GameConfiguration.standings.OrderBy(x => x.Value).ToList())
            {
                Debug.Log(player);
                GameObject obj = new GameObject($"{player}'s Results");
                obj.transform.SetParent(transform, false);
                var text = obj.AddComponent<Text>();
                var fit = obj.AddComponent<ContentSizeFitter>();
                
                text.text = $"{player.Key}'s Score: {player.Value}";
                text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                text.color = Color.black;

                var rect = text.GetComponent<RectTransform>();             
                
                rect.anchorMin = new Vector2(0.5f, 1f);
                rect.anchorMax = new Vector2(0.5f, 1f);
                rect.anchoredPosition = new Vector2(rect.sizeDelta.x / 2f, rect.sizeDelta.y / -2f);

                obj.transform.position += new Vector3(0, offset * GetComponentInParent<Canvas>().scaleFactor, 0);
                offset -= text.rectTransform.rect.height;
            }
        }
    }
}
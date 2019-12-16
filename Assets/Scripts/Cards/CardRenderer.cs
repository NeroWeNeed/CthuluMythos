
using UnityEngine;
using UnityEngine.UI;

namespace CMythos
{

    [RequireComponent(typeof(RawImage))]
    public class CardRenderer : MonoBehaviour
    {

        [SerializeField]
        private Card card;
        public Card Card
        {
            get => card;
            set
            {
                card = value;
                UpdateRender();
            }
        }
        private RawImage rawImage;
        private void Start()
        {
            rawImage = GetComponent<RawImage>();
            rawImage.rectTransform.sizeDelta = new Vector2(Card.CARD_WIDTH, Card.CARD_HEIGHT);
            UpdateRender();
        }
        private void UpdateRender()
        {
            Debug.Log("Updating...");
            if (card == null)
            {
                rawImage.enabled = false;
            }
            else
            {

                rawImage.enabled = true;
                rawImage.texture = card.graphic;
            }


        }
        
        
    }

}

using UnityEngine;
using UnityEngine.UI;

namespace CMythos
{

    [RequireComponent(typeof(RawImage), typeof(Button))]
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
            Initialize();
        }
        public void Initialize()
        {
            rawImage = GetComponent<RawImage>();
            rawImage.rectTransform.sizeDelta = new Vector2(Card.CARD_WIDTH, Card.CARD_HEIGHT);

            UpdateRender();
        }
        private void UpdateRender()
        {
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
        public void Discard()
        {
            if (Card != null)
            {
                Hand hand = GetComponentInParent<Hand>();
                int pos = 0;
                foreach (var item in hand.GetComponentsInChildren<CardRenderer>())
                {
                    if (item == this)
                    {
                        hand.player.Discard(pos);
                    }
                    else
                        pos++;
                }

            }
        }



    }

}
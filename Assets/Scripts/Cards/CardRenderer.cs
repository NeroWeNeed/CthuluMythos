
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
        private void Awake()
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
                HandRenderer handRenderer = GetComponentInParent<HandRenderer>();
                int pos = System.Array.IndexOf(handRenderer.GetComponentsInChildren<CardRenderer>(), this);
                if (pos != -1)
                {

                    handRenderer.Player.Discard(pos);
                    handRenderer.UpdateCardRenderers();
                }

            }
        }
        public void Play()
        {
            if (Card != null)
            {
                HandRenderer handRenderer = GetComponentInParent<HandRenderer>();
                int pos = System.Array.IndexOf(handRenderer.GetComponentsInChildren<CardRenderer>(), this);
                if (pos != -1)
                {

                    handRenderer.Player.Play(pos);
                    handRenderer.UpdateCardRenderers();
                }

            }
        }
        public void Interact()
        {
            if (GetComponentInParent<HandRenderer>().LeftControl)
            {
                Discard();
            }
            else  {
                Play();
            }
        }



    }

}
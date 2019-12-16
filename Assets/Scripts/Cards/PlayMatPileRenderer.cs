using UnityEngine;
using UnityEngine.UI;

namespace CMythos
{


    [RequireComponent(typeof(RawImage))]
    public class PlayMatPileRenderer : MonoBehaviour
    {

        [SerializeField]
        private PlayMat playMat;
        private bool isDiscardPile = false;
        public bool IsDiscardPile
        {
            get => isDiscardPile;
            set
            {
                isDiscardPile = value;
                UpdatePile(playMat, pile);
            }
        }

        public PlayMat PlayMat
        {
            get => playMat;
            set
            {
                playMat = value;
                UpdatePile(playMat, pile);
            }
        }

        [SerializeField]
        private CardType pile;

        public CardType Pile
        {
            get => pile;
            set
            {
                pile = value;
                UpdatePile(playMat, pile);
            }
        }

        public PlayMatPile CurrentPile { get; private set; }
        private RawImage rawImage;
        private void Start()
        {
            rawImage = GetComponent<RawImage>();
            rawImage.rectTransform.sizeDelta = new Vector2(Card.CARD_WIDTH, Card.CARD_HEIGHT);

        }

        private void UpdatePile(PlayMat playMat, CardType pile)
        {
            if (IsDiscardPile)
                CurrentPile = playMat?.GetDiscardPile(pile);
            else
                CurrentPile = playMat?.GetPile(pile);
        }
        public void Refresh()
        {

            if (CurrentPile.Cards.Count > 0)
            {
                rawImage.texture = CurrentPile.Cards.Peek().graphic;
            }
        }



    }
}
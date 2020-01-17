using UnityEngine;
using System;
using System.Linq;

namespace CMythos
{
    [RequireComponent(typeof(Canvas))]
    public class PlayMatRenderer : MonoBehaviour
    {

        [SerializeField]
        private float pileSpacing = 5.0f;

        [SerializeField]
        private float pileHorizontalMargin = 10.0f;

        [SerializeField]
        private float pileVerticalMargin = 10.0f;

        [SerializeField]
        private Texture2D texture;
        [SerializeField]
        private float scale;

        public float Width { get; private set; }

        public float Height { get; private set; }

        public void Init()
        {

            PlayMatPileRenderer[] renderers = GetComponentsInChildren<PlayMatPileRenderer>();
            GameObject obj;
            GameObject obj2;
            PlayMatPileRenderer pileRenderer, discardPileRenderer;
            float lastX = 0.0f;
            var index = 0;
            foreach (var item in Enum.GetValues(typeof(CardType)).Cast<CardType>())
            {
                if (renderers.All(x => x.PileType != item))
                {
                    obj = new GameObject($"{item} Pile Renderer", typeof(PlayMatPileRenderer));
                    obj2 = new GameObject($"{item} Discard Pile Renderer", typeof(PlayMatPileRenderer));
                    pileRenderer = obj.GetComponent<PlayMatPileRenderer>();
                    discardPileRenderer = obj2.GetComponent<PlayMatPileRenderer>();
                    pileRenderer.PileType = item;
                    discardPileRenderer.PileType = item;
                    discardPileRenderer.IsDiscardPile = true;


                    if (index == 0)
                    {

                        obj2.transform.localPosition = new Vector3(0, 0, 0);

                        obj.transform.localPosition = new Vector3(0, Card.CARD_HEIGHT + 20, 0);
                        lastX = Card.CARD_WIDTH + pileSpacing;
                    }
                    else
                    {
                        obj2.transform.localPosition = new Vector3(lastX, 0, 0);
                        obj.transform.localPosition = new Vector3(lastX, Card.CARD_HEIGHT + 20, 0);
                        lastX += Card.CARD_WIDTH + pileSpacing;
                    }
                    obj.transform.SetParent(transform, false);
                    obj2.transform.SetParent(transform, false);


                }
                index++;

            }
            Width = lastX;
            Height = Card.CARD_HEIGHT * 2 + 20;
        }

        public float ApproximateWidth()
        {
            var index = 0;
            float lastX = 0.0f;
            foreach (var item in Enum.GetValues(typeof(CardType)).Cast<CardType>())
            {
                if (index == 0)
                    lastX = Card.CARD_WIDTH + pileSpacing;
                else
                    lastX += Card.CARD_WIDTH + pileSpacing;
                    index++;
            }
            return (lastX-(Card.CARD_WIDTH + pileSpacing))*transform.localScale.x;
        }
        public void Refresh(PlayMat playMat)
        {

            foreach (var item in GetComponentsInChildren<PlayMatPileRenderer>())
            {
                item.Refresh(playMat);
            }
        }
    }
}


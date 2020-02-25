using UnityEditor;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

namespace CMythos
{


    public class InvestigatorSelector : MonoBehaviour
    {
        private int index;
        public int Index
        {
            get => index; set
            {
                index = value % investigators.Length;
                UpdateInvestigatorView();
            }
        }

        private Investigator[] investigators;


        private void Start()
        {
            investigators = Resources.LoadAll("Investigators", typeof(Investigator)).Select(x => Instantiate(x) as Investigator).ToArray();
            UpdateInvestigatorView();
        }

        private void UpdateInvestigatorView()
        {
            foreach (var item in GameObject.FindObjectsOfType<InvestigatorSelectorText>())
            {
                item.UpdateText(investigators[index]);
            }
        }
        public void Next()
        {
            Index++;
        }
        public void Push() {
            GameConfiguration.investigators.Enqueue(investigators[Index]);
        }

        public void StartGame() {
            SceneManager.LoadScene(1);
        }
    }
}
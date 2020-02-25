using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace CMythos
{

    [RequireComponent(typeof(Text))]
    public class InvestigatorSelectorText : MonoBehaviour
    {
        [SerializeField]
        private string fieldName;
        private Text textComponent;
        private void Awake()
        {
            textComponent = GetComponent<Text>();
        }

        public void UpdateText(Investigator investigator)
        {
            foreach (var item in investigator.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (item.Name == fieldName)
                {
                    if (item.FieldType.IsArray)
                    {
                        textComponent.text = "";
                        foreach (var entry in item.GetValue(investigator) as object[])
                        {
                            textComponent.text += entry.ToString() + '\n';
                            
                        }

                    }
                    else
                    {
                        textComponent.text = item.GetValue(investigator).ToString();
                    }

                    break;
                }
            }
        }
    }
}
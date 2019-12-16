using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class PlayerCoordinateTracker : MonoBehaviour
{
    public void UpdateText(string value)
    {
        GetComponent<Text>().text = value;

    }
}
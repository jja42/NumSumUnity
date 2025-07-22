using TMPro;
using UnityEngine;
using static NumberGen;

public class GridEntity : MonoBehaviour
{
    public Num number;
    public Vector2 position;
    public TextMeshProUGUI text;
    public bool interactible;

    public void InitText()
    {
        text.text = number.value.ToString();
    }

    public void OnClick()
    {
        if (interactible)
        {
            print("Number: " + number.value + " Position: " + position + " Is Valid: " + number.is_valid);
        }
    }
}

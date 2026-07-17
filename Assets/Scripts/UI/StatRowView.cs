using TMPro;
using UnityEngine;

public class StatRowView : MonoBehaviour
{
    [SerializeField] private TMP_Text _label;
    [SerializeField] private TMP_Text _value;

    public void Set(string label, string value)
    {
        _label.text = label;
        _value.text = value;
    }
}
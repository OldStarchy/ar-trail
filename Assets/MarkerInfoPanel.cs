using UnityEngine;
using UnityEngine.UI;

public class MarkerInfoPanel : MonoBehaviour
{
    public MarkerListPanel MarkerListPanel;
    public Text IdTextBox;
    private int _markerNumber;
    public int MarkerNumber
    {
        get
        {
            return _markerNumber;
        }
        set
        {
            _markerNumber = value;
            IdTextBox.text = "" + value;
        }
    }

    public void SetHighlighted(Color? highlight)
    {
        GetComponent<Image>().color = highlight ?? Color.white;
    }

    public void FindMarker()
    {
        MarkerListPanel.FindMarker(_markerNumber);
    }

    public void DeleteMarker()
    {
        MarkerListPanel.RemoveMarker(this);
    }
}
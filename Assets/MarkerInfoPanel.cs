using UnityEngine;
using UnityEngine.UI;

public class MarkerInfoPanel : MonoBehaviour
{
    public MarkerListPanel MarkerListPanel;
    public Text IdTextBox;
    public int MarkerNumber;
    private int _routeNumber;
    public int RouteNumber
    {
        get
        {
            return _routeNumber;
        }
        set
        {
            _routeNumber = value;
            if (value < 0) {
                IdTextBox.text = "-";
            } else {
                IdTextBox.text = (value + 1).ToString();
            }
        }
    }

    public void SetHighlighted(Color? highlight)
    {
        GetComponent<Image>().color = highlight ?? Color.white;
    }

    public void FindMarker()
    {
        MarkerListPanel.FindMarker(MarkerNumber);
    }

    public void DeleteMarker()
    {
        MarkerListPanel.RemoveMarker(this);
    }

    public void ToggleInCource()
    {
        MarkerListPanel.ToggleInRoute(this);
    }
}
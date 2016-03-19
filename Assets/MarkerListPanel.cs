using UnityEngine;
using System.Collections;

public class MarkerListPanel : MonoBehaviour {
    public DestinationManager DestinationManager;
    public MarkerInfoPanel InfoPanel;
    public TouchListener TouchListener;
    int highlightedIndex = -1;
    MarkerInfoPanel deletingIndex;

    public GameObject DeletePanel;
    public GameObject OtherPanel;

    void OnEnable()
    {
        ReloadChildren();
    }

    public void ToggleInRoute(MarkerInfoPanel mip)
    {
        int r = DestinationManager.ToggleInRoute(mip.MarkerNumber);
        if (r == -1)
            ReloadChildren();
        else
            mip.RouteNumber = r;
    }

    public void ReloadChildren()
    {
        foreach (Transform go in transform)
        {
            Destroy(go.gameObject);
        }

        for (int i = 0; i < DestinationManager.DestinationCount; i++)
        {
            MarkerInfoPanel mip = GameObject.Instantiate<MarkerInfoPanel>(InfoPanel);
            mip.gameObject.transform.SetParent(this.transform);
            
            mip.RouteNumber = DestinationManager.GetDestination(i).RouteIndex;
            mip.transform.localScale = Vector3.one;
            mip.MarkerListPanel = this;
            mip.MarkerNumber = i;
            if (i == highlightedIndex)
                mip.SetHighlighted(Color.green);
        }
    }

    public void FindMarker(int index)
    {
        highlightedIndex = index;
        TouchListener.ShowFindMarkerUi(index);
    }

    public void RemoveMarker(MarkerInfoPanel mip)
    {
        if (deletingIndex != null)
            deletingIndex.SetHighlighted(null);
        deletingIndex = mip;
        mip.SetHighlighted(Color.red);
        DeletePanel.SetActive(true);
        OtherPanel.SetActive(false);
    }

    public void ActuallyDelete()
    {
        DeletePanel.SetActive(false);
        OtherPanel.SetActive(true);
        DestinationManager.RemoveMarker(deletingIndex.MarkerNumber);
        if (deletingIndex.MarkerNumber == highlightedIndex)
            highlightedIndex = -1;
        else if (deletingIndex.MarkerNumber < highlightedIndex)
            highlightedIndex--;
        ReloadChildren();
        deletingIndex = null;
    }

    public void DontActuallyDelete()
    {
        DeletePanel.SetActive(false);
        OtherPanel.SetActive(true);
        deletingIndex.SetHighlighted(highlightedIndex == deletingIndex.MarkerNumber ? Color.green : (Color?)null);
        deletingIndex = null;
    }
}

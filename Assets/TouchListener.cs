using UnityEngine;
using System.Collections;
using System;

public class TouchListener : MonoBehaviour {
    [Flags]
    public enum UiObject : int
    {
        STARTER_BUTTON = 1,
        MAIN = 2,
        MARKER_LIST = 4,
        FIND_MARKER = 8,
    }

    public enum UiMode
    {
        HIDDEN = UiObject.STARTER_BUTTON,
        MAIN = UiObject.MAIN,
        MARKER_LIST = UiObject.MARKER_LIST,
        FIND_MARKER = UiObject.FIND_MARKER,
    }

    public void HideUi()
    {
        SetUiMode(UiMode.HIDDEN);
    }

    public void ShowMainMenu()
    {
        SetUiMode(UiMode.MAIN);
    }

    public void ShowMarkerList()
    {
        SetUiMode(UiMode.MARKER_LIST);
    }

    public void ShowFindMarkerUi(int index)
    {
        SetUiMode(UiMode.FIND_MARKER);
        dm.PointTo(index);
    }

    public DestinationManager dm;
    bool touched;
    int show = -1;
    int showTime = 120;
    private UiMode currentMode = UiMode.HIDDEN;
    public GameObject UiCanvas;
    public GameObject UiButtonStarter;

    public GameObject UiMarkersPanel;
    public GameObject UiFindMarkerPanel;

    private int timer;
    private int doubleTapTime = 60;
    private bool prevTouched;

	// Use this for initialization
	void Start () {
        SetUiMode(UiMode.HIDDEN);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Taparuni()
    {
        Debug.Log("taparuni" + timer);
        if (timer > 0)
        {
            SetUiMode(UiMode.MAIN);
            timer = -1;
        }
        else {
            timer = doubleTapTime;
        }
    }

    void FixedUpdate()
    {
        //dm.ResetLocation();
        // Application.Quit();

        if (timer >= 0)
            timer--;
        if (show == 0)
            SetUiMode(UiMode.HIDDEN);
        if (show >= 0)
            show--;
    }

    private bool checkBit(UiObject obj)
    {
        return ((int)currentMode & (int)obj) != 0;
    }

    public void SetUiMode(UiMode mode)
    {
        if (mode == UiMode.MAIN)
            show = showTime;
        else
            show = -1;

        currentMode = mode;
        
        UiButtonStarter.SetActive(checkBit(UiObject.STARTER_BUTTON));
        UiCanvas.SetActive(checkBit(UiObject.MAIN));
        UiMarkersPanel.SetActive(checkBit(UiObject.MARKER_LIST));
        UiFindMarkerPanel.SetActive(checkBit(UiObject.FIND_MARKER));
    }
}

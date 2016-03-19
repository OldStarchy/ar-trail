using UnityEngine;
using System.Collections;

public class UIMachine : MonoBehaviour {
    public GameObject BoringStuff;
    public TextMesh InterestingStuff_probably_not_interesting_actually;
    public GameObject ParentOfTheThingRUUUN;
    private int timer = -1;
    public void ShowBoringStuff()
    {
        BoringStuff.SetActive(true);
        ParentOfTheThingRUUUN.SetActive(false);
    }

    public void ShowInstructions()
    {
        BoringStuff.SetActive(false);
        ParentOfTheThingRUUUN.SetActive(true);
    }

    public void OmedetouToTheUserPlux()
    {
        BoringStuff.SetActive(false);
        ParentOfTheThingRUUUN.SetActive(true);
        InterestingStuff_probably_not_interesting_actually.text = "終わり\nおめでとう";
        timer = 240;
    }

    void FixedUpdate()
    {
        if (timer >= 0)
            timer--;
        if (timer == 0)
            ShowBoringStuff();
    }
}

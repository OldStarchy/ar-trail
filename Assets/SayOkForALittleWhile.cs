using UnityEngine;
using UnityEngine.UI;

public class SayOkForALittleWhile : MonoBehaviour {
    public int HowLong;
    public string NormalText;

    private int timer;

	// Update is called once per frame
	void FixedUpdate () {
        if (timer >= 0)
            timer--;
        
        if (timer == 0)
            GetComponent<Text>().text = NormalText;
	}

    public void ShowOk(string text)
    {
        GetComponent<Text>().text = text;
        timer = HowLong;
    }
}

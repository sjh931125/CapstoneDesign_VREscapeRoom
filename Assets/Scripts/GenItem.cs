using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenItem : MonoBehaviour {
    
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }

    public void GetThis(GameObject hand)
    {
        transform.SetParent(hand.transform);
        transform.localPosition = Vector3.zero;
        transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    public string GetTag()
    {
        return tag;
    }
}

using UnityEngine;
using System.Collections;

public class CollisionHandler : MonoBehaviour {

    public static GameObject LastTappedObject;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider otherObject)
    {
        // detect object tap
        GameObject focusedObject = otherObject.gameObject; // InteractibleManager.Instance.FocusedGameObject;
        if (LastTappedObject != null)
        {
            Debug.Log("BEFORE:Last Tapped Object: " + LastTappedObject.GetComponent<Renderer>().material.color.ToString());
        } else
        {
            Debug.Log("BEFORE:Last Tapped Object is null");
        }

        if (LastTappedObject != null && focusedObject != null &&
            LastTappedObject.GetComponent<Renderer>().material.color == focusedObject.GetComponent<Renderer>().material.color)
        {
            Debug.Log("Matched: " + LastTappedObject.GetComponent<Renderer>().material.color.ToString());
            Debug.Log("Matched: " + focusedObject.GetComponent<Renderer>().material.color.ToString());
            EnableGravity(focusedObject);
            EnableGravity(LastTappedObject);
        }
        
        Debug.Log("Collided with: " + focusedObject.GetComponent<Renderer>().material.color.ToString());
        if (focusedObject != null)
        {
            LastTappedObject = focusedObject;
        }
        Debug.Log("AFTER:Last Tapped Object: " + LastTappedObject.GetComponent<Renderer>().material.color.ToString());

        //
        this.gameObject.SetActive(false);

        //
        if (focusedObject != null)
        {
            focusedObject.SendMessageUpwards("OnSelect");
        }
    } 

    private static void EnableGravity(GameObject focusedObject)
    {
        var rb = focusedObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = true;
        }
    }
}

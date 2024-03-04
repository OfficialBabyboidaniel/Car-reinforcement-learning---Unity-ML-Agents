using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointSingle : MonoBehaviour
{
    // Start is called before the first frame update
    private TrackCheckpoints trackCheckpoints;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Car")
        {
            if (trackCheckpoints == null)
            {
                Debug.Log("trackcheckpoints was null: " + trackCheckpoints);
            }
            else
            {

                trackCheckpoints.CarThroughCheckpoint(this, other.transform);
                Debug.Log("checkpoint reached");
            }
        }
    }

    public void SetTrackCheckpoints(TrackCheckpoints track)
    {
        trackCheckpoints = track;

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackCheckpoints : MonoBehaviour
{
    public event EventHandler OnPlayerCorrectCheckpoint;
    public event EventHandler OnplayerWrongCheckpoint;

    [SerializeField] private List<Transform> carTransformList;
    private List<CheckPointSingle> checkPointSingleList;
    private List<int> nextCheckpointSingleIndexList;
    private void Awake()
    {
        Transform checkpointsTransform = transform.Find("Positve_reward_checkpoints");
        checkPointSingleList = new List<CheckPointSingle>();

        foreach (Transform checkpointSingleTransform in checkpointsTransform)
        {
            CheckPointSingle checkPointSingle = checkpointSingleTransform.GetComponent<CheckPointSingle>();
            checkPointSingle.SetTrackCheckpoints(this);

            checkPointSingle.SetTrackCheckpoints(this);
            checkPointSingleList.Add(checkPointSingle);
            //Debug.Log(checkpointSingleTransform);
        }

        nextCheckpointSingleIndexList = new List<int>();
        foreach (Transform carTransform in carTransformList)
        {
            nextCheckpointSingleIndexList.Add(0);
        }

    }

    public void CarThroughCheckpoint(CheckPointSingle checkPointSingle, Transform carTransform)
    {
        int nextCheckpointSingleIndex = nextCheckpointSingleIndexList[carTransformList.IndexOf(carTransform)];
        if (checkPointSingleList.IndexOf(checkPointSingle) == nextCheckpointSingleIndex)
        {
            Debug.Log("Correct");
            nextCheckpointSingleIndexList[carTransformList.IndexOf(carTransform)] = (nextCheckpointSingleIndex + 1) % checkPointSingleList.Count;
            OnPlayerCorrectCheckpoint?.Invoke(this, EventArgs.Empty);
            // correcct chjeckpoint
        }
        else
        {
            Debug.Log("wrong checkpoint");
            OnplayerWrongCheckpoint?.Invoke(this, EventArgs.Empty);
            //wrong checkpoint
        }

        //Debug.Log(checkPointSingle.transform.name);
    }
}

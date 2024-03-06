using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackCheckpoints : MonoBehaviour
{
    public class CarCheckpointEventArgs : EventArgs
    {
        public Transform carTransform;
    }
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
            //Debug.Log("checkPointSingle: " + checkPointSingle);
            checkPointSingleList.Add(checkPointSingle);
            //Debug.Log(checkpointSingleTransform);
        }

        nextCheckpointSingleIndexList = new List<int>();
        foreach (Transform carTransform in carTransformList)
        {

            nextCheckpointSingleIndexList.Add(0);
        }

    }


    private void Update()
    {
        Debug.Log(nextCheckpointSingleIndexList[carTransformList.IndexOf(carTransformList[0])]);
    }

    public void CarThroughCheckpoint(CheckPointSingle checkPointSingle, Transform carTransform)
    {
        int nextCheckpointSingleIndex = nextCheckpointSingleIndexList[carTransformList.IndexOf(carTransform)];
        Debug.Log(nextCheckpointSingleIndexList[carTransformList.IndexOf(carTransform)]);
        Debug.Log("nextCheckpointSingleIndex: " + nextCheckpointSingleIndex);
        //Debug.Log("checkPointSingleList.IndexOf(checkPointSingle) : " + checkPointSingleList.IndexOf(checkPointSingle));
        if (checkPointSingleList.IndexOf(checkPointSingle) == nextCheckpointSingleIndex)
        {
            Debug.Log("Correct checkpoint");
            nextCheckpointSingleIndexList[carTransformList.IndexOf(carTransform)] = (nextCheckpointSingleIndex + 1) % checkPointSingleList.Count;
            CarCheckpointEventArgs carCheckpointEventArgs = new CarCheckpointEventArgs();
            carCheckpointEventArgs.carTransform = carTransform;
            OnPlayerCorrectCheckpoint?.Invoke(this, carCheckpointEventArgs);
            // correcct chjeckpoint
        }
        else
        {
            Debug.Log("wrong checkpoint");
            CarCheckpointEventArgs carCheckpointEventArgs = new CarCheckpointEventArgs();
            carCheckpointEventArgs.carTransform = carTransform;
            OnplayerWrongCheckpoint?.Invoke(this, carCheckpointEventArgs);
            //wrong checkpoint
        }

        //Debug.Log(checkPointSingle.transform.name);
    }

    public void ResetCheckpoints(Transform transform)
    {
        int carIndex = carTransformList.IndexOf(transform);
        if (carIndex != -1)
        {
            // Reset the next checkpoint index for the specified car
            nextCheckpointSingleIndexList[carIndex] = 0;
        }
        else
        {
            Debug.LogError("Car not found in the carTransformList.");
        }
    }


    // den här är fel
    public CheckPointSingle GetNextCheckpoint(Transform carTransform)
    {
        int carIndex = carTransformList.IndexOf(carTransform);

        if (carIndex != -1)
        {
            int nextCheckpointIndex = nextCheckpointSingleIndexList[carIndex];

            // Check if the car is on the last checkpoint
            if (nextCheckpointIndex == checkPointSingleList.Count - 1)
            {
                // Reset to checkpoint zero (first checkpoint)
                nextCheckpointSingleIndexList[carIndex] = 0;
                return checkPointSingleList[0];
            }
            else
            {
                // return  the next checkpoint
                return checkPointSingleList[nextCheckpointIndex + 1];
            }
        }
        else
        {
            Debug.LogError("Car not found in the carTransformList.");
            return null;
        }
    }

}

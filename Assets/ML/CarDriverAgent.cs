using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System;

public class CarDriverAgent : Agent
{
    [SerializeField] private CarController ControlledCar;

    [SerializeField] private Transform StartPosition;

    [SerializeField] private TrackCheckpoints trackCheckpoints;

    private void Awake()
    {
        ControlledCar = GetComponent<CarController>();
    }

    private void Start()
    {
        trackCheckpoints.OnPlayerCorrectCheckpoint += TrackCheckpoints_OnCarCorrectCheckpoint;
        trackCheckpoints.OnplayerWrongCheckpoint += TrackCheckpoints_OnCarWrongCheckpoint;
    }

    private void TrackCheckpoints_OnCarCorrectCheckpoint(object sender, EventArgs e)
    {

        if ((e as TrackCheckpoints.CarCheckpointEventArgs).carTransform == transform)
        {
            Debug.Log("Added + 1 reward for going through checkpoint");
            AddReward(1f);
        }
    }
    private void TrackCheckpoints_OnCarWrongCheckpoint(object sender, EventArgs e)
    {


        if ((e as TrackCheckpoints.CarCheckpointEventArgs).carTransform == transform)
        {
            Debug.Log("Added - 1 reward for going through wrong checkpoint");
            AddReward(-1f);
        }

    }

    public override void OnEpisodeBegin()
    {
        //base.OnEpisodeBegin();
        Debug.Log("OnEpisodeBegin");
        transform.position = StartPosition.position;
        transform.forward = StartPosition.forward;
        trackCheckpoints.ResetCheckpoints(transform);
        ControlledCar.StopCarMovement();
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        //base.CollectObservations(sensor);
        Vector3 checkpointForward = trackCheckpoints.GetNextCheckpoint(transform).transform.forward;
        float directionDot = Vector3.Dot(transform.forward, checkpointForward);
        sensor.AddObservation(directionDot);
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        /* UserControl userControl = GetComponent<UserControl>();
        ControlledCar.UpdateControls(userControl.Horizontal, userControl.Vertical, userControl.Brake); */

        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");

        /*     int forwardAction = 0;

            if (Input.GetKey(KeyCode.W))
            {
                forwardAction = 1;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                forwardAction = 2;
            }

            int turnAction = 0;

            if (Input.GetKey(KeyCode.D))
            {
                // Right turn action
                turnAction = 1;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                // Left turn action
                turnAction = 2;
            }

            ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
            discreteActions[0] = forwardAction;
            discreteActions[1] = turnAction; */
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        //   float forwardAmount = 0f;
        //  float turnAmount = 0f;
        //  Debug.Log("actions.DiscreteActions[0]: " + actions.DiscreteActions[0]);
        //  Debug.Log("actions.DiscreteActions[1]: " + actions.DiscreteActions[1]); 
        //  switch (actions.DiscreteActions[0])
        //  {
        //      case 0: forwardAmount = 0f; break;
        //      case 1: forwardAmount = +1f; break;
        //      case 2: forwardAmount = -1f; break;
        //  }
        //  switch (actions.DiscreteActions[1])
        //  {
        //      case 0: turnAmount = 0f; break;
        //      case 1: turnAmount = +1f; break;
        //      case 2: turnAmount = -1f; break;
        //  } 

        // Debug.Log("actions.DiscreteActions[0]: " + actions.DiscreteActions[0]);
        // Debug.Log("actions.DiscreteActions[1]: " + actions.DiscreteActions[1]);    
        ControlledCar.UpdateControls(actions.ContinuousActions[0], actions.ContinuousActions[1], false);
        //base.OnActionReceived(actions);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "NegativeWall")
        {
            Debug.Log("hit negative wall");
            AddReward(-0.5f);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "NegativeWall")
        {
            Debug.Log("inside negative wall");
            AddReward(-0.1f);
        }
    }
}

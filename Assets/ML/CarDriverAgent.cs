using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class CarDriverAgent : Agent
{
    [SerializeField] private CarController ControlledCar;

    [SerializeField] private Transform StartPosition;


    public override void OnEpisodeBegin()
    {
        base.OnEpisodeBegin();
        transform.position = StartPosition.position;
    }
    public override void CollectObservations(VectorSensor sensor)
    {

        base.CollectObservations(sensor);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        base.OnActionReceived(actions);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Checkpoint")
        {
            if (ControlledCar.CurrentSpeed < 0 && ControlledCar.CarDirection == 1)
            {
                Debug.Log("hit checkpoint");
                AddReward(1f);
            }
        }
        if (other.gameObject.tag == "NegativeWall")
        {
            Debug.Log("hit negative wall");
            AddReward(-1f);
        }
    }
}

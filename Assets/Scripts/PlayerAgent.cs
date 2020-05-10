using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAgents;
using MLAgents.Sensors;

public class PlayerAgent : Agent
{
    Rigidbody rBody;
    private int count;
    public Text countText;
    private int episode_count;
    public Text episodeText;
    // Start is called before the first frame update
    void Start()
    {
        rBody = transform.GetComponent<Rigidbody>();
        episode_count = 0;

    }

    public Transform Target;
    // Call this method when Episode Begin
    public override void OnEpisodeBegin()
    {

        episode_count += 1;
        episodeText.text = "Episode: " + episode_count.ToString();

        count = 0;
        countText.text = "Scores: " + count.ToString();

        SetReward(0.0f);

        if (transform.position.y < 0)
        {
            // If the Agent fell, zero its momentum
            rBody.angularVelocity = Vector3.zero;
            rBody.velocity = Vector3.zero;
            transform.position = new Vector3(0, 0.5f, 0);
        }

        // Move the target to a new spot
        int x = Random.Range(-9, 9);
        int z = Random.Range(-9, 9);
        Target.position = new Vector3(x, 0.8f, z);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Target and Agent positions
        sensor.AddObservation(Target.position);
        sensor.AddObservation(transform.position);

        // Agent velocity
        sensor.AddObservation(rBody.velocity.x);
        sensor.AddObservation(rBody.velocity.z);
    }

    public float speed = 10;
    public override void OnActionReceived(float[] vectorAction)
    {
        // Actions, size = 2
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = vectorAction[0];
        controlSignal.z = vectorAction[1];
        rBody.AddForce(controlSignal * speed);

        // Rewards
        float distanceToTarget = Vector3.Distance(transform.position, Target.position);

        //AddReward(-0.001f);
        // Reached target
        if (distanceToTarget < 1.0f)
        {
            AddReward(1.0f);
            count += 1;
            countText.text = "Scores: " + count.ToString();

            // Move the target to a new spot
            int x = Random.Range(-9, 9);
            int z = Random.Range(-9, 9);
            Target.position = new Vector3(x, 0.8f, z);
        }

        // Fell off platform
        if (transform.position.y < 0)
        {
            AddReward(-1.0f);
            EndEpisode();
        }

    }

    public override float[] Heuristic()
    {
        var action = new float[2];
        action[0] = Input.GetAxis("Horizontal");
        action[1] = Input.GetAxis("Vertical");
        return action;
    }
}

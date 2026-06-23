using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SeekPlayer", story: "Agent seeks Player at [speed]", category: "Action", id: "62656fcac3bd4a9f7b6058770607f477")]
public partial class SeekPlayerAction : Action
{
    [SerializeReference] public BlackboardVariable<float> Speed;

    private Transform player;
    private Transform self;

    protected override Status OnStart()
    {
        player = PlayerController.Intance.transform;
        self = GameObject.transform;

        if (player == null)
        {
            Debug.Log("Player não encontrado!");
            return Status.Failure;
        } 

        Debug.Log("Player encontrado!");
        return Status.Running;
                    
    }

    protected override Status OnUpdate()
    {
        if (player == null)
        {
            return Status.Failure;
        }

        Vector3 direction = (player.position - self.position).normalized;
        self.position += direction * Speed * Time.deltaTime;

        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}


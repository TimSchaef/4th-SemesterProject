using System.Collections;
using UnityEngine;

public class InteractionObjective : MonoBehaviour
{
    [Header("Objective Data")]
    [SerializeField] private SO_ObjectiveWave objectiveWave;

    [Header("Arena")]
    [SerializeField] private DoorAnimation[] doors;
    [SerializeField] private Transform[] objectiveSpawnPoints;

    [Header("Cooldown")]
    [SerializeField] private float cooldownTime = 60f;

    private bool onCooldown;
    public Animator animator;

    public SO_ObjectiveWave ObjectiveWave => objectiveWave;
    public Transform[] SpawnPoints => objectiveSpawnPoints;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    
    public void Interact(EnemySpawner spawner)
    {
        if (onCooldown)
        {
            Debug.Log(cooldownTime);
            return;
        }

        spawner.StartObjectiveWave(this);
    }

    public void CloseDoors()
    {
        foreach (var door in doors)
            door.CloseDoor();
    }

    public void OpenDoors()
    {
        foreach (var door in doors)
            door.OpenDoor();
    }

    public void StartCooldown()
    {
        StartCoroutine(CooldownRoutine());
    }

    private IEnumerator CooldownRoutine()
    {
        onCooldown = true;

        yield return new WaitForSeconds(cooldownTime);

        animator.SetTrigger("isInactive");
        onCooldown = false;
    }
}
using System.Collections;
using UnityEngine;

public class InteractionObjective : MonoBehaviour
{
    [Header("Objective Data")]
    [SerializeField] private SO_ObjectiveWave objectiveWave;

    [Header("Arena")]
    [SerializeField] private GameObject[] doors;
    [SerializeField] private Transform[] objectiveSpawnPoints;

    [Header("Cooldown")]
    [SerializeField] private float cooldownTime = 60f;

    private bool onCooldown;

    public SO_ObjectiveWave ObjectiveWave => objectiveWave;
    public Transform[] SpawnPoints => objectiveSpawnPoints;

    public void Interact(EnemySpawner spawner)
    {
        if (onCooldown)
            return;

        spawner.StartObjectiveWave(this);
    }

    public void CloseDoors()
    {
        foreach (GameObject door in doors)
            door.SetActive(false);
    }

    public void OpenDoors()
    {
        foreach (GameObject door in doors)
            door.SetActive(true);
    }

    public void StartCooldown()
    {
        StartCoroutine(CooldownRoutine());
    }

    private IEnumerator CooldownRoutine()
    {
        onCooldown = true;

        yield return new WaitForSeconds(cooldownTime);

        onCooldown = false;
    }
}
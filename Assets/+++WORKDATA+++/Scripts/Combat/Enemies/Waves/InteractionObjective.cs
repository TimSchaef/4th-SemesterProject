using System.Collections;
using TMPro;
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
    [SerializeField] private Canvas cooldownCanvas;
    [SerializeField] private TextMeshProUGUI cooldownText;

    private bool onCooldown;
    public Animator animator;

    public SO_ObjectiveWave ObjectiveWave => objectiveWave;
    public Transform[] SpawnPoints => objectiveSpawnPoints;

    void Awake()
    {
        animator = GetComponentInParent<Animator>();
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

        animator.SetTrigger("isInactive");

        while (cooldownTime > 0)
        {
            cooldownTime--;

            if (cooldownCanvas.enabled)
                cooldownText.text = cooldownTime.ToString();

            yield return new WaitForSeconds(1f);
        }

        onCooldown = false;
    }

    public void ShowCooldownCanvas()
    {
        cooldownCanvas.enabled = true;
        cooldownText.text = cooldownTime.ToString();
    }

    public void HideCooldownCanvas()
    {
        cooldownCanvas.enabled = false;
    }
}
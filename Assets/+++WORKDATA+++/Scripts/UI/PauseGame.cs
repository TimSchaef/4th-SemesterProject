using UnityEngine;
using UnityEngine.InputSystem;

public class PauseGame : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Pause Game");
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        { 
            //Time.timeScale = 0f;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
        }

        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            //Time.timeScale = 1f;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}

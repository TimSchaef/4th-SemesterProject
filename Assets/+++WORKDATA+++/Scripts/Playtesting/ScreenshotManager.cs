using UnityEngine;
using System.IO;

public class ScreenshotManager : MonoBehaviour
{
    [SerializeField] private KeyCode screenshotKey = KeyCode.P;
    [SerializeField] private string folderName = "Screenshots";

    void Update()
    {
        // Überprüft, ob die Taste 'P' gedrückt wurde
        if (Input.GetKeyDown(screenshotKey))
        {
            TakeScreenshot();
        }
    }

    void TakeScreenshot()
    {
        // Pfad zum Hauptverzeichnis deines Projekts (über dem Assets-Ordner)
        string projectRoot = Path.Combine(Application.dataPath, "..");
        
        // Zielordner-Pfad (Hauptverzeichnis + "Screenshots")
        string targetFolder = Path.Combine(projectRoot, folderName);
        targetFolder = Path.GetFullPath(targetFolder);

        // Falls der Ordner noch nicht existiert, erstelle ihn automatisch
        if (!Directory.Exists(targetFolder))
        {
            Directory.CreateDirectory(targetFolder);
            Debug.Log($"<b><color=orange>[Screenshot]</color></b> Ordner wurde erstellt unter: {targetFolder}");
        }

        // Erstellt den Dateinamen mit Zeitstempel
        string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        string fileName = $"Screenshot_{timestamp}.png";
        
        // Kombiniert den Ordnerpfad mit dem Dateinamen
        string fullPath = Path.Combine(targetFolder, fileName);

        // Nimmt den Screenshot auf
        ScreenCapture.CaptureScreenshot(fullPath);

        // Gibt den genauen Speicherort in der Konsole aus
        Debug.Log($"<b><color=green>[Screenshot]</color></b> Gespeichert im Screenshots-Ordner: <color=yellow>{fullPath}</color>");
    }
}
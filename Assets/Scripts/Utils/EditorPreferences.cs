using System.IO;
using UnityEngine;

namespace Game.GamePlay.Root {
    public class EditorPreferences : MonoBehaviour {
#if UNITY_EDITOR
        private void Update() {
            if(Input.GetKeyDown(KeyCode.F)) { //Making a screenshot
                string fileName = $"Screenshot_{System.DateTime.Now:yyyy-MM-dd_HH-mm-ss}.png";
                string fullPath = Path.Combine(Application.persistentDataPath, fileName);
                ScreenCapture.CaptureScreenshot(fullPath);
            
                Debug.Log($"Screenshot saved at this path: {fullPath}");
            }
        }
#endif
    }
}
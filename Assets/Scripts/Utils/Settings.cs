using UnityEngine;

namespace Utils {
    public class Settings {
        public static float volume;
        public static Resolution res;
        public static int qualityLvl;
        public static bool isFullScreen;

        public Settings() {
            res = new Resolution {
                height = 1080,
                width = 1920
            };

            volume = 1f;
            qualityLvl = 0;
            isFullScreen = true;
        }
    }
}
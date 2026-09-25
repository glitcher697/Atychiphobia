using System;
using System.Collections;
using System.Collections.Generic;
using EntryPoint;
using EntryPoint.EntryPoints;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utils;

namespace Game.MainMenu {
    public class SettingMenu : MonoBehaviour {
        [SerializeField] private Slider _slider;
        [SerializeField] private TMP_Dropdown ResDropDown;
        [SerializeField] private TMP_Dropdown qualityDropDown;
        //[SerializeField] private Toggle FullScreenToggle;

        private bool _isFullScreen;
        private Resolution[] _allResolutions;
        private List<Resolution> SelectedResolutionList = new List<Resolution>();
        private int _indexSelectedRes;

        public void ChangeResolution() {
            _indexSelectedRes = ResDropDown.value;
            Screen.SetResolution(SelectedResolutionList[_indexSelectedRes].width,
                SelectedResolutionList[_indexSelectedRes].height,
                _isFullScreen);

            Settings.res = SelectedResolutionList[_indexSelectedRes];
        }

        public void ChangeQuality(int index) {
            QualitySettings.SetQualityLevel(index);
            Settings.qualityLvl = index;
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.F11)) {
                _isFullScreen = !_isFullScreen;
                Settings.isFullScreen = _isFullScreen;

                Screen.SetResolution(SelectedResolutionList[_indexSelectedRes].width,
                    SelectedResolutionList[_indexSelectedRes].height,
                    _isFullScreen);

            }

            AudioListener.volume = _slider.value;
            Settings.volume = _slider.value;
        }

        public void InitSettings() {
            ChangeQuality(Settings.qualityLvl);
            
            Screen.SetResolution(Settings.res.width,
                Settings.res.height,
                Settings.isFullScreen);

            _isFullScreen = Settings.isFullScreen;

            _slider.value = Settings.volume;
            AudioListener.volume = Settings.volume;
        }
        
        private void Start() {
            _allResolutions = Screen.resolutions;
            List<string> resolutionStringList = new List<string>();
            string newRes;
            foreach (Resolution res in _allResolutions) {
                newRes = res.width.ToString() + "x" + res.height.ToString();
                if (!resolutionStringList.Contains(newRes)) {
                    resolutionStringList.Add(newRes);
                    SelectedResolutionList.Add(res);
                }
            }
            
            ResDropDown.AddOptions(resolutionStringList);
            
            int id = 0;
            foreach (var i in Screen.resolutions) {
                if (i.height == Settings.res.height &
                    i.width == Settings.res.width)
                    break;
                id++;
            }
            ResDropDown.value = id;
            
            qualityDropDown.value = Settings.qualityLvl;
        }
    }
}
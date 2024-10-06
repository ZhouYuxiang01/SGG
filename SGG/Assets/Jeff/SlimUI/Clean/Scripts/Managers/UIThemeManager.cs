// www.SlimUI.com
// Copyright (c) 2019 - 2023 SlimUI. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

// JOIN THE DISCORD FOR SUPPORT OR CONVERSATION: https://discord.gg/7cK4KBf


// HOW TO USE ///////////////////////////

// If you're building a new menu using the prefabbed content, use this script somehwere in your scene to access the Theme Editor!
//
// Just add it to an empty game object and set the theme you want to use. All UI elements in your scene that have the 'ThemedUIElement'
// script attached will automatically update their style settings to match your theme settings.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlimUI.Clean{
    public class UIThemeManager : MonoBehaviour
    {
        public enum Theme { custom1, custom2, custom3 };

        [Header("Theme Settings")]
        public Theme theme;
        int themeIndex;
        public ThemedUIData themeController;

        void Start(){
            SetThemeColors();
        }

        void SetThemeColors(){
			if (theme == Theme.custom1){
				themeController.currentColor = themeController.custom1.graphic1;
				themeController.textColor = themeController.custom1.text1;
				themeIndex = 0;
			}else if (theme == Theme.custom2){
				themeController.currentColor = themeController.custom2.graphic2;
				themeController.textColor = themeController.custom2.text2;
				themeIndex = 1;
			}else if (theme == Theme.custom3){
				themeController.currentColor = themeController.custom3.graphic3;
				themeController.textColor = themeController.custom3.text3;
				themeIndex = 2;
			}
		}

        public void ChangeTheme(int index){
            if(index == 0){
                theme = Theme.custom1;
            }else if(index == 1){
                theme = Theme.custom2;
            }else if(index == 2){
                theme = Theme.custom3;
            }

            SetThemeColors();
        }
    }
}
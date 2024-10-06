using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlimUI.Clean{
    public class WebsiteButton : MonoBehaviour
    {
        public void OpenSite(){
            Application.OpenURL("https://www.slimui.com/");
        }
    }
}
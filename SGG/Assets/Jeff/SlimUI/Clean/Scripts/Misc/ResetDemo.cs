using UnityEngine;
using UnityEngine.SceneManagement;

namespace SlimUI.Clean
{
	public class ResetDemo : MonoBehaviour{
		public string sceneName;

		void Update(){
			if (Input.GetKeyDown("r")){
				SceneManager.LoadScene(sceneName);
			}
		}
	}
}
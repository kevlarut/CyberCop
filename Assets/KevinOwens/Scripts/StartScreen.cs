using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
 
public class StartScreen : MonoBehaviour {
     
    public string NextSceneName = "Headquarters";

	void Update()
    {
     	if(Input.anyKey)
     	{
            LoadScene(NextSceneName);    
     	}
	}

    void LoadScene(string sceneName)
    {        
        SceneManager.LoadSceneAsync(sceneName);
    } 
}
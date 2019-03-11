using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
 
public class LevelExit : MonoBehaviour {
     
    public string NextSceneName = "Headquarters";

	void OnTriggerEnter2D(Collider2D collider)
    {
     	if(collider.tag == "Player")
     	{
            LoadScene(NextSceneName);    
     	}
	}

    void LoadScene(string sceneName)
    {        
        SceneManager.LoadSceneAsync(sceneName);
    } 
}
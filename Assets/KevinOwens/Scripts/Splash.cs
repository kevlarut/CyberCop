using UnityEngine;

namespace Assets.Scripts
{
    public class Splash : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {        
			var lifetime = 0.35f;
			Destroy(gameObject, lifetime); 
        }

        // Update is called once per frame
        void Update()
        {       
        }
    }
}
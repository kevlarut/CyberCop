 using UnityEngine;
 using System.Collections;
 
 [RequireComponent(typeof(Renderer))]
 public class Parallax : MonoBehaviour {
     
    public float Multiplier = 0.1f;
    public Transform Target;

    private float _previousTargetX;
    private Renderer _renderer;
    private float _textureOffset = 0;

    void Start() {
        _previousTargetX = Target.position.x;
        _renderer = GetComponent<Renderer>();
        _renderer.material.SetTextureOffset("_MainTex",  new Vector2(0,0));
    }

    void Update () 
    {
        if (Target.position.x != _previousTargetX) {
            transform.position = new Vector3(Target.transform.position.x,  transform.position.y, 0);

            var newTextureOffset = _textureOffset + (Target.position.x - _previousTargetX) * Multiplier;

            _renderer.material.SetTextureOffset("_MainTex",  new Vector2(newTextureOffset,0));
            _previousTargetX = Target.position.x;
            _textureOffset = newTextureOffset;
        }
    }
 }
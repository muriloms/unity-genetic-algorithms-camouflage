using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNA : MonoBehaviour
{
    // Gene for color
    public float r;
    public float g;
    public float b;
    public float s;

    private bool _dead = false;
    public float timeToDie = 0;

    private SpriteRenderer _renderer;
    private Collider2D _collider2D;
    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _collider2D = GetComponent<Collider2D>();
        
        _renderer.color = new Color(r,g,b);
        transform.localScale = new Vector3(s,s,s);
    }

    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        _dead = true;
        //timeToDie = PopulationManager.elapsed;

        _renderer.enabled = false;
        _collider2D.enabled = false;
    }
}

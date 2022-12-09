using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMovement : MonoBehaviour
{
    public GameObject Barrera;
    public AudioClip Explosion;

    private Rigidbody2D Rb;
    private float Distance;

    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Distance = Barrera.transform.position.x - transform.position.x;

        // if ( Distance < 0.02f )
        // {
        //     Boom();
        // }
    }

    // FixedUpdate() es una función que se llama cada 20ms
    private void FixedUpdate() {
        Rb.velocity = new Vector2(6, 0);
    }

    private void Boom() {
        Debug.Log( Distance );
        Camera.main.GetComponent<AudioSource>().PlayOneShot(Explosion);
    }

    // private void OnCollisionEnter2D( Collision2D collision ) {
    //     BarreraScript barrera = collision.collider.GetComponent<BarreraScript>();
    //
    //     if ( barrera != null ) {
    //         barrera.Hit();
    //     }
    //
    //     DestroyGhost();
    // }

    // Collider 2D Is Trigger = true
    private void OnTriggerEnter2D( Collider2D collision ) {
        BarreraScript barrera = collision.GetComponent<BarreraScript>();

        if ( barrera != null ) {
            Boom();
            barrera.Hit();
        }

        DestroyGhost();
    }

    public void DestroyGhost() {
        Destroy( gameObject );
    }
}

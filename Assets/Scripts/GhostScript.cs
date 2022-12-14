using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostScript : MonoBehaviour {

    // Variables globales de tipo audio del prefab
    public AudioClip[] audioClips;
    AudioClip audioSpawn, audioBoom;

    // Variables físicas de velocidad del prefab
    float speed;
    Vector3 velocity;

    // Variable global del Animator del prefab
    Animator animator;

    // Setters de inicio de las variables globales
    // Antes del primer frame (Inicio del play)
    void Start() {
        speed = 6f;
        // Vector3.right = Vector3(1, 0, 0)
        velocity = Vector3.right * speed;

        animator = GetComponent<Animator>();

        Spawn();
    }

    // En cada frame ...
    void Update() {

    }

    // FixedUpdate() es una función que se llama 50 veces de forma Konstante
    // O sea, cada 20 ms o 0.02 segundos
    void FixedUpdate()
    {
        // Usando la instancia del patrón Singleton
        if( GameManager.instance.GameOver ) { return; }

        // Dentro de este método Time.deltaTime = Time.fixedDeltaTime = 0.02
        Vector3 movement = velocity * Time.deltaTime;
        transform.position += movement;
    }

    // private void OnCollisionEnter2D( Collision2D other ) {
    //     BarreraScript player = other.collider.GetComponent<BarreraScript>();
    //
    //     if ( player != null ) {
    //         player.Hit();
    //         Explotar();
    //         velocity = Vector3.zero;
    //     }
    //
    //     // DestroyGhost();
    // }

    void OnTriggerEnter2D( Collider2D collision )
    {
        // bool player = collision.gameObject.tag == "Player";
        BarreraScript barrera = collision.GetComponent<BarreraScript>();

        if ( barrera != null ){
            // Punto para la barrera
            GameManager.instance.BarreraHit();
            velocity = Vector3.zero;

            Explotar();
        }
    }

    void Spawn() {
        audioSpawn = audioClips[0];
        Camera.main.GetComponent<AudioSource>().PlayOneShot(audioSpawn);
    }

    void Explotar() {
        // Transición a la animación Explosion
        animator.SetBool("exploding", true);

        audioBoom = audioClips[1];
        Camera.main.GetComponent<AudioSource>().PlayOneShot(audioBoom);
    }

    void DestroyGhost() {
        Destroy( gameObject );
    }
}

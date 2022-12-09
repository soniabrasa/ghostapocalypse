using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostScript : MonoBehaviour {

    public AudioClip[] audioClips;
    AudioClip audioSpawn, audioBoom;

    float speed = 6f;
    Vector3 velocity;

    void Start() {
        // Vector3.right = Vector3(1, 0, 0)
        velocity = Vector3.right * speed;

        Spawn();
    }

    void Update() {

    }

    // FixedUpdate() es una función que se llama 50 veces/s de forma constante
    // O sea, cada 20 ms o 0.02 segundos
    void FixedUpdate()
    {
        // Dentro de este método Time.deltaTime = Time.fixedDeltaTime = 0.02
        Vector3 movement = velocity * Time.deltaTime;
        transform.position += movement;

        // PositionX();
    }

    // public float PositionX() {
    //     return this.transform.position.x;
    // }

    private void OnTriggerEnter2D( Collider2D collision )
    {
        // bool player = collision.gameObject.tag == "Player";
        BarreraScript player = collision.GetComponent<BarreraScript>();

        if ( player != null ){
            // Punto para el jugador
            player.Hit();

            Explotar();
            DestroyGhost();
        }
    }

    void Hit() { }

    private void Spawn() {
        audioSpawn = audioClips[0];
        Camera.main.GetComponent<AudioSource>().PlayOneShot(audioSpawn);
    }

    private void Explotar() {
        audioBoom = audioClips[1];
        Camera.main.GetComponent<AudioSource>().PlayOneShot(audioBoom);
    }

    public void DestroyGhost() {
        Destroy( gameObject );
    }
}

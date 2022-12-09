using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject ghostPrefab;
    public GameObject gameOverPrefab;
    public GameObject vidasPrefab;
    public GameObject barrera;

    public Transform[] spawnPoints;
    public Transform[] marcadorPoints;
    public Transform[] board;

    private GameObject ghostClone;
    private GameObject vidaClone;
    private List<GameObject> ghostClones = new List<GameObject>();
    // private List<GameObject> vidasClones = new List<GameObject>();
    // private GameObject[] vidasClones = new GameObject[4];
    private int enemigos, neutralizados, perdidos, vidas;
    private float borderTop, borderRight, borderBottom;
    private bool play;
    private string nameGhost, nameVida;

    // Al inicio del juego
    void Start() {
        enemigos = 0;
        neutralizados = 0;
        perdidos = 0;
        vidas = 4;
        play = true;

        nameGhost = "Ghost_";
        nameVida = "Vida_";
        // LimpiarMarcadores();

        borderTop = board[0].position.y;
        borderRight = board[1].position.x;
        borderBottom = board[2].position.y;

        InicioMarcador ();
    }

    // En cada frame
    void Update() {

        if ( vidas > 0 ) {
            // Probabilidad del 0.001
            if ( Random.Range(0f, 1f) < 0.001f ) {
                SpawnGhost();
            }

            UpdateGhosts();

            if ( barrera != null ) {
                neutralizados = barrera.GetComponent<BarreraScript>().Puntuacion();
            }
        }

        else {
            FinJuego();
            // Parar toda la escena
            Time.timeScale = 0;
        }
    }

    void InicioMarcador () {
        // marcadorX = [-0.75f, -0.25f, 0.25f, 0.75f];

        for ( int i = 0; i < vidas; i++ ) {
            Transform marcadorPoint = marcadorPoints[i];
            Vector3 posicion = marcadorPoint.position;
            Quaternion rotacion = Quaternion.identity;

            vidaClone = Instantiate( vidasPrefab, posicion, rotacion );
            vidaClone.name = nameVida + i;
        }
    }

    // Método para instanciar un gosthPrefab
    // en uno de los 5 neutralizados de espaneo al azar
    private void SpawnGhost()
    {
        // Random.Range devuelve un valor de un rango
        // que incluye el 1er parámetro [0]
        // pero excluye el segundo [5]

        // n resulta un entero al azar entre 0 y 4
        int n = Random.Range(0, spawnPoints.Length);

        // Uno de los 5 objetos del array spawPoints
        Transform spawnPoint = spawnPoints[n];

        // Los objetos de tipo Transform tienen 3 propiedades
        // position, rotation y scale

        Vector3 posicion = spawnPoint.position;

        // Quaternion.identity
        // Obtiene un cuaternión que no representa nunguna rotación.
        // Cuaternión cuyos valores son (0, 0, 0, 1)

        Quaternion rotacion = Quaternion.identity;

        // Instantiate( prefab, Vector3, rotacion );
        ghostClone = Instantiate( ghostPrefab, posicion, rotacion );
        ghostClone.name = nameGhost + enemigos;
        enemigos++;

        ghostClones.Add( ghostClone );

    }

    void UpdateGhosts()
    {
        for ( int i=0; i < ghostClones.Count; i++ ) {
            GameObject clon = ghostClones[i];

            if ( clon != null ) {

                float x = clon.transform.position.x;
                float y = clon.transform.position.y;

                // print( "Position X GhostClone_" + enemigos + ": " + x );

                if ( x > borderRight ) {
                    ghostClones.RemoveAt(i);
                    Destroy( clon );
                    RestarVida();
                }
            }
        }
    }

    void RestarVida() {
        vidas--;
        print("vidas: " + vidas );

        string vidaPrefabName = nameVida + vidas;

        GameObject vida = GameObject.Find(vidaPrefabName);
        Destroy( vida );
    }

    void ErrorEnemigo() {
        Debug.Log("Enemigo perdido");
    }

    void FinJuego() {
        // Quaternion rotacion = Quaternion.identity;
        if ( play ) {
            Instantiate( gameOverPrefab);
            // Destroy( barrera );
            play = false;
        }
    }

    void OnGUI()
    {
        GUI.skin.label.fontSize = 40;

        GUI.Label( new Rect(10, 1000, 400, 100),
            "Fantasmas: " + enemigos
        );

        GUI.Label( new Rect(400, 1000, 500, 100),
            "Neutralizados: " + neutralizados
        );

        GUI.Label( new Rect(800, 1000, 500, 100),
            "Perdidos: " + perdidos
        );

        GUI.Label( new Rect(1500, 1000, 500, 100),
            "Vidas: " + vidas
        );
    }
}

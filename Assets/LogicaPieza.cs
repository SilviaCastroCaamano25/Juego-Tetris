using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogicaPieza : MonoBehaviour
{
    //El tiempo que transcurre cuando cae una pieza y el tiempo que ha pasado para que otra pieza caiga
    private float tiempoAnterior;
    public float tiempoCaida = 0.8f;

    //Marco los límites
    public static int alto = 20;
    public static int ancho = 10;

    //Punto de rotación de la pieza
    public Vector3 puntoRotacion;

    //La posición para saber si se ha formado una línea
    private static Transform[,] grid = new Transform[ancho, alto];

    //Para hacer la puntuacion
    public static int puntuacion = 0;

    //Para añadir dificultad al juego
    public static int nivel = 0;


    // Start is called before the first frame update
    void Start()
    {

    }//Llave Start

    // Update is called once per frame
    void Update()
    {
        //Las teclas que se van a utilizar para mover las piezas
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //Para que al apretar la flecha izquierda se mueva una posición hacia la izquierda
            transform.position += new Vector3(-1, 0, 0);

            if (!Limites())
            {
                transform.position -= new Vector3(-1, 0, 0);
            }

        }
        
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            //Para que al apretar la flecha derecha se mueva una posición hacia la derecha
            transform.position += new Vector3(1, 0, 0);

            if (!Limites())
            {
                transform.position -= new Vector3(1, 0, 0);
            }

        }

        //Para que la pieza vaya callendo
        if (Time.time - tiempoAnterior > (Input.GetKey(KeyCode.DownArrow) ? tiempoCaida / 20 : tiempoCaida))
        {
            transform.position += new Vector3(0, -1, 0);

            if (!Limites())
            {
                transform.position -= new Vector3(0, -1, 0);

                AñadirAlGrid();
                RevisarLineas();

                this.enabled = false;

                //Una vez que se desactive una pieza cree otra
                FindObjectOfType<LogicaGeneradorPieza>().NuevaPieza();

            }

            tiempoAnterior = Time.time;
        }

        //Para rotar las piezas del tetris
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            transform.RotateAround(transform.TransformPoint(puntoRotacion), new Vector3(0, 0, 1), 90);

            //Una vez rotado, poder devolverlo a la posición original
            if (!Limites()) {
                transform.RotateAround(transform.TransformPoint(puntoRotacion), new Vector3(0, 0, 1), -90);
            }
        }

        AumentarNivel();
        AumentarDificultad();


    }//Llave Update

    //Para marcar los límites
    bool Limites()
    {
        foreach (Transform hijo in transform)
        {
            int X = Mathf.RoundToInt(hijo.transform.position.x);
            int Y = Mathf.RoundToInt(hijo.transform.position.y);

            if (X < 0 || X >= ancho || Y < 0 || Y >= alto)
            {
                return false;
            }

            if (grid[X, Y] != null)
            {
                return false;
            }

        }

        return true;

    }//Llave Limites


    //Añadir una pieza al juego
    void AñadirAlGrid() {
        foreach (Transform hijo in transform) {
            int X = Mathf.RoundToInt(hijo.transform.position.x);
            int Y = Mathf.RoundToInt(hijo.transform.position.y);

            grid[X, Y] = hijo;

            if (Y >= 19) {
                puntuacion = 0;
                nivel = 0;
                tiempoCaida = 0.8f;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

        }
    }

    //Eliminar las filas completadas
    void RevisarLineas() {
        for (int i = alto -1; i >=0; i--)
        {
            if (TieneLinea(i)) {
                BorrarLinea(i);
                BajarLinea(i);
            }
        }
    }
    
    //Va ha revisar las filas
    bool TieneLinea(int i) {
        for (int j = 0; j < ancho; j++)
        {
            if (grid[j, i] == null) {
                return false;
            }
        }

        puntuacion += 100;
        Debug.Log(puntuacion);

        return true;
    }

    //Va ha borrar las filas completas
    void BorrarLinea(int i) {
        for (int j = 0; j < ancho; j++)
        {
            Destroy(grid[j, i].gameObject);
            grid[j, i] = null;
        }
    }

    //Va ha bajas las filas superiores a las borradas
    void BajarLinea(int i) {
        for (int z = i; z < alto; z++)
        {
            for (int j = 0; j < ancho; j++)
            {
                if (grid[j, z] != null) {
                    grid[j, z - 1] = grid[j, z];
                    grid[j, z] = null;
                    grid[j, z - 1].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    }

    void AumentarNivel() {
        switch (puntuacion) {
            case 1500:
                nivel = 1;
                break;

            case 3000:
                nivel = 2;
                break;

        }

    }

    void AumentarDificultad()
    {
        switch (nivel)
        {
            case 1:
                tiempoCaida = 0.4f;
                break;

            case 2:
                tiempoCaida = 0.2f;
                break;
        }
    }

}
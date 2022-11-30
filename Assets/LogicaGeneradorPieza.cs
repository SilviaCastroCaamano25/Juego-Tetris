using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicaGeneradorPieza : MonoBehaviour
{
    //Donde voy a guardar las diferentes piezas
    public GameObject[] piezas;


    // Start is called before the first frame update
    void Start()
    {
        NuevaPieza();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Crea las piezas
    //Quaternion.identity es para que la rotación sea 0
    public void NuevaPieza() {
        Instantiate(piezas[Random.Range(0, piezas.Length)], transform.position, Quaternion.identity);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField]
    DatosEnemigo datos;

    [SerializeField]
    Datos[] datos2;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"Hola me llamo {datos.nombre}");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [System.Serializable]
    struct Datos
    {
        public string otroNombre;
        public string otraVida;
        public string otraNoseq;
    }
}

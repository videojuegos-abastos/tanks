using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DatosEnemigo", menuName = "ScriptableObjects/DatosEnemigo", order = 1)]
public class DatosEnemigo : ScriptableObject
{
    
    public string nombre;
    public float vida;
    public float daño;

}

using System;
using UnityEngine;

[Serializable]
public class DialogoNodo
{
    public bool Jugador;
  
    [TextArea]
    public string Mensaje;

    [TextArea]
    public string[] opciones;
    public int[] siguientes;
    
}
using UnityEngine;
using System.Collections.Generic;

public class Persistencia : MonoBehaviour
{
    [Tooltip("Dá um nome único para este objeto. Ex: 'Player' ou 'CanvasUI'")]
    public string idUnico;

    private static Dictionary<string, Persistencia> instancias = new Dictionary<string, Persistencia>();

    private void Awake()
    {
        // Se já existir um objeto com este ID (quando voltas a uma cena onde já tinhas estado)
        if (instancias.ContainsKey(idUnico))
        {
            // Se não sou eu, destruo-me para não haver clones
            if (instancias[idUnico] != this)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            // Se for o original, guardo-o e torno-o imortal!
            instancias.Add(idUnico, this);
            DontDestroyOnLoad(gameObject);
        }
    }
}
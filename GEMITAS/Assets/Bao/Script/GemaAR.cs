using UnityEngine;

public class GemaAR : MonoBehaviour
{
    private JuegoAR juego;

    public void Configurar(JuegoAR nuevoJuego)
    {
        juego = nuevoJuego;
    }

    private void OnMouseDown()
    {
        if (juego != null)
        {
            juego.RecogerGema();
        }

        Destroy(gameObject);
    }
}
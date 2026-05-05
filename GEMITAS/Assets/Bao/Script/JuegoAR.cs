using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;

public class JuegoAR : MonoBehaviour
{
   
    public ARPlaneManager planeManager;

  
    public AudioSource audioEfectos;
    public AudioClip sonidoGema;
    public GameObject gemaPrefab;

    
    public TMP_Text textoPlanos;
    public TMP_Text textoJuego;
    public GameObject botonCrear;

    private int planosHorizontales;
    private int planosVerticales;

    private int gemasTotales;
    private int gemasRecogidas;
    private float tiempoRestante;

    private bool juegoEmpezado = false;
    private bool partidaTerminada = false;

    void Start()
    {
        gemasTotales = DatosPartida.gemasHorizontales + DatosPartida.gemasVerticales;
        tiempoRestante = DatosPartida.tiempo;

        textoPlanos.gameObject.SetActive(true);
        textoJuego.gameObject.SetActive(false);
        botonCrear.SetActive(true);
    }

    void Update()
    {
        if (partidaTerminada) return;

        ContarPlanos();

        if (!juegoEmpezado)
        {
            textoPlanos.text =
                "ENCUENTRA PLANOS\n" +
                "VERTICALES: " + planosVerticales + "/" + DatosPartida.gemasVerticales + "\n" +
                "HORIZONTAL: " + planosHorizontales + "/" + DatosPartida.gemasHorizontales;
        }
        else
        {
            tiempoRestante -= Time.deltaTime;

            textoJuego.text =
                "Gemas encontradas: " + gemasRecogidas + "/" + gemasTotales + "\n" +
                "Tiempo: " + Mathf.CeilToInt(tiempoRestante);

            if (tiempoRestante <= 0)
            {
                Perder();
            }

            if (gemasRecogidas >= gemasTotales)
            {
                Ganar();
            }
        }
    }

    void ContarPlanos()
    {
        planosHorizontales = 0;
        planosVerticales = 0;

        foreach (ARPlane plano in planeManager.trackables)
        {
            if (plano.alignment == PlaneAlignment.HorizontalUp ||
                plano.alignment == PlaneAlignment.HorizontalDown)
            {
                planosHorizontales++;
            }

            if (plano.alignment == PlaneAlignment.Vertical)
            {
                planosVerticales++;
            }
        }
    }

    public void CrearGemas()
    {
        if (planosHorizontales < DatosPartida.gemasHorizontales ||
            planosVerticales < DatosPartida.gemasVerticales)
        {
            return;
        }

        juegoEmpezado = true;

        textoPlanos.gameObject.SetActive(false);
        textoJuego.gameObject.SetActive(true);
        botonCrear.SetActive(false);

        int creadasHorizontal = 0;
        int creadasVertical = 0;

        foreach (ARPlane plano in planeManager.trackables)
        {
            if ((plano.alignment == PlaneAlignment.HorizontalUp ||
                 plano.alignment == PlaneAlignment.HorizontalDown) &&
                 creadasHorizontal < DatosPartida.gemasHorizontales)
            {
                CrearGemaEnPlano(plano);
                creadasHorizontal++;
            }

            if (plano.alignment == PlaneAlignment.Vertical &&
                creadasVertical < DatosPartida.gemasVerticales)
            {
                CrearGemaEnPlano(plano);
                creadasVertical++;
            }
        }
    }

    void CrearGemaEnPlano(ARPlane plano)
    {
        Vector3 posicion = plano.center;
        posicion.y += 0.05f;

        GameObject gema = Instantiate(gemaPrefab, posicion, Quaternion.identity);

        GemaAR scriptGema = gema.GetComponent<GemaAR>();
        scriptGema.Configurar(this);
    }

    public void RecogerGema()
    {
        if (!juegoEmpezado) return;

        gemasRecogidas++;

        if (audioEfectos != null && sonidoGema != null)
        {
            audioEfectos.PlayOneShot(sonidoGema);
        }
    }

    void Ganar()
    {
        partidaTerminada = true;
        textoJuego.text = "HAS GANADO OSTIA\nTodas las gemas encontraditas";
    }

    void Perder()
    {
        partidaTerminada = true;
        textoJuego.text = "HAS PERDIDO MALO\nSe te ha acabado el tiempo";
    }

    public void Salir()
    {
        SceneManager.LoadScene("Menu");
    }
}
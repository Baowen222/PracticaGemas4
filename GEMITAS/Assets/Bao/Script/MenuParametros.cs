using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuParametros : MonoBehaviour
{
    public Slider sliderTiempo;
    public Slider sliderHorizontal;
    public Slider sliderVertical;
    public Toggle toggleOclusion;

    public TMP_Text textoTiempo;
    public TMP_Text textoHorizontal;
    public TMP_Text textoVertical;

    void Update()
    {
        textoTiempo.text = "Tiempo búsqueda: " + sliderTiempo.value;
        textoHorizontal.text = "Gemas en horizontal: " + sliderHorizontal.value;
        textoVertical.text = "Gemas en vertical: " + sliderVertical.value;
    }

    public void Comenzar()
    {
        DatosPartida.tiempo = sliderTiempo.value;
        DatosPartida.gemasHorizontales = (int)sliderHorizontal.value;
        DatosPartida.gemasVerticales = (int)sliderVertical.value;
        DatosPartida.oclusion = toggleOclusion.isOn;

        SceneManager.LoadScene("JuegoAR");
    }
}
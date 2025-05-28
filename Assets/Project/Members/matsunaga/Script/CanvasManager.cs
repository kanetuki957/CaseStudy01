using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public GameObject canvas1;
    public GameObject canvas2;
    public GameObject canvas3;
    public GameObject canvas4;

    void Start()
    {
        if (canvas1 != null) canvas1.SetActive(true);
        if (canvas2 != null) canvas2.SetActive(false);
        if (canvas3 != null) canvas3.SetActive(false);
        if (canvas4 != null) canvas4.SetActive(false);
    }

    public void ShowCanvas1()
    {
        if (canvas1 != null) canvas1.SetActive(true);
        if (canvas2 != null) canvas2.SetActive(false);
        if (canvas3 != null) canvas3.SetActive(false);
        if (canvas4 != null) canvas4.SetActive(false);
    }

    public void ShowCanvas2()
    {
        if (canvas1 != null) canvas1.SetActive(false);
        if (canvas2 != null) canvas2.SetActive(true);
        if (canvas3 != null) canvas3.SetActive(false);
        if (canvas4 != null) canvas4.SetActive(false);
    }

    public void ShowCanvas3()
    {
        if (canvas1 != null) canvas1.SetActive(false);
        if (canvas2 != null) canvas2.SetActive(false);
        if (canvas3 != null) canvas3.SetActive(true);
        if (canvas4 != null) canvas4.SetActive(false);
    }

    public void ShowCanvas4()
    {
        if (canvas1 != null) canvas1.SetActive(false);
        if (canvas2 != null) canvas2.SetActive(false);
        if (canvas3 != null) canvas3.SetActive(false);
        if (canvas4 != null) canvas4.SetActive(true);
    }
}

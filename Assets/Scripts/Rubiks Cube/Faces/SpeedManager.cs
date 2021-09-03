using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

static class SpeedManager
{
    public static void Change(float newSpeed)
    {
        // Change rotation speed
        Face.rotatingSpeed = newSpeed;

        // Change speed on the slider
        GameObject.Find("Speed slider").GetComponent<Slider>().value = newSpeed;
    }

    public static void Reset()
    {
        Change(Face.DEFAULT_ROTATING_SPEED);
    }
}
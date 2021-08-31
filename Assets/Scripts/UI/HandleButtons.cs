using UnityEngine;
using UnityEngine.UI;

public class HandleButtons : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        RubiksCube rubiksCube = GameObject.Find("Rubiks cube").GetComponent<RubiksCube>();

        GameObject.Find("R").GetComponent<Button>().onClick.AddListener(delegate { rubiksCube.Manipulate("R"); });
        GameObject.Find("Ri").GetComponent<Button>().onClick.AddListener(delegate { rubiksCube.Manipulate("Ri"); });
        GameObject.Find("L").GetComponent<Button>().onClick.AddListener(delegate { rubiksCube.Manipulate("L"); });
        GameObject.Find("Li").GetComponent<Button>().onClick.AddListener(delegate { rubiksCube.Manipulate("Li"); });
        GameObject.Find("B").GetComponent<Button>().onClick.AddListener(delegate { rubiksCube.Manipulate("B"); });
        GameObject.Find("Bi").GetComponent<Button>().onClick.AddListener(delegate { rubiksCube.Manipulate("Bi"); });
        GameObject.Find("D").GetComponent<Button>().onClick.AddListener(delegate { rubiksCube.Manipulate("D"); });
        GameObject.Find("Di").GetComponent<Button>().onClick.AddListener(delegate { rubiksCube.Manipulate("Di"); });
        GameObject.Find("F").GetComponent<Button>().onClick.AddListener(delegate { rubiksCube.Manipulate("F"); });
        GameObject.Find("Fi").GetComponent<Button>().onClick.AddListener(delegate { rubiksCube.Manipulate("Fi"); });
        GameObject.Find("U").GetComponent<Button>().onClick.AddListener(delegate { rubiksCube.Manipulate("U"); });
        GameObject.Find("Ui").GetComponent<Button>().onClick.AddListener(delegate { rubiksCube.Manipulate("Ui"); });

        GameObject.Find("Shuffle button").GetComponent<Button>().onClick.AddListener(delegate { rubiksCube.Shuffle(); });
        GameObject.Find("Solve button").GetComponent<Button>().onClick.AddListener(delegate { rubiksCube.Solve(); });
        GameObject.Find("Test button").GetComponent<Button>().onClick.AddListener(delegate { rubiksCube.Test(); });
    }
}

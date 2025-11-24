using MS.Manager;
using MS.Mode;
using UnityEngine;
using UnityEngine.UI;

public class TestUIController : MonoBehaviour
{
    [SerializeField] private Button BtnTest;


    void Start()
    {
        BtnTest.onClick.AddListener(()
            =>
        {
            GameManager.Instance.ChangeMode(new SurvivalMode());

            Debug.Log("BtnTest");
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

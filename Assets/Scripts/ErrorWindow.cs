using System.Collections;
using TMPro;
using UnityEngine;

public class ErrorWindow : MonoBehaviour
{
    public float ScaleDuration;

    [SerializeField] private TMP_Text _title;
    [SerializeField] private TMP_Text _message;
    [SerializeField] private TMP_Text _buttonLabel;



    public void DisplayError(string message, string title = "Error", string button = "Ok")
    {
        _title.text = title;
        _message.text = message;
        _buttonLabel.text = button;

        //StartCoroutine(ScaleWindow(1f));
        transform.localScale = Vector3.one;
    }

    public void HideErrorWindow()
    {
        //StartCoroutine(ScaleWindow(0f));
        transform.localScale = Vector3.zero;
    }

    //private IEnumerator ScaleWindow(float scale)
    //{
    //    var timer = ScaleDuration;

    //    var targetScale = scale * Vector3.one;

    //    while (timer > 0f)
    //    {
    //        transform.localScale = Vector3.MoveTowards(transform.localScale, targetScale, Time.deltaTime / ScaleDuration);

    //        timer -= Time.deltaTime;
    //        yield return null;
    //    }
    //}

}

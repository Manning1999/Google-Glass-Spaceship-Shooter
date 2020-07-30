using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearTimer : MonoBehaviour
{

    [SerializeField]
    float timeTilDisappear = 0.5f;

    Vector3 positionToStayIn = Vector3.zero;

    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(Timer());
    }

    public void SetLocation(Vector3 location)
    {
        positionToStayIn = location;
    }

    private void Update()
    {
        transform.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(positionToStayIn);
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(timeTilDisappear);
        gameObject.SetActive(false);
    }
}

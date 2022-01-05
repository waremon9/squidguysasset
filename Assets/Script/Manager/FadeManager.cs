using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FadeManager : MySingleton<FadeManager>
{
    [SerializeField] private GameObject SquidPrefab;
    [SerializeField] private GameObject Circle;
    [SerializeField] private float NbSquid;
    private List<GameObject> Squids = new List<GameObject>();
    [SerializeField] private Image image;
    [SerializeField] private  float fadeSpeed = 0.5f;
    [SerializeField] private float SpeedSquid =150f;
    [SerializeField] private float SpeedCircle =1/60f;
    [SerializeField] private TMP_Text title;

    public override bool DoDestroyOnLoad { get; }

    private void Start()
    {
        SpawnSquids();
        title.color = new Color(title.color.r, title.color.g, title.color.b, 0);
        foreach (var squid in Squids)
        {
            Image imSquid = squid.GetComponent<Image>();
            squid.GetComponent<Image>().color = new Color(imSquid.color.r, imSquid.color.g, imSquid.color.b, 0);
        }
    }
    private void Update()
    {
        RotateSquids();
        if (Input.GetKeyDown("f"))
        {
          
            
            StartCoroutine(FadeIn());
        }
    }

    public IEnumerator FadeIn()
    {
        float tempFade = fadeSpeed * Time.deltaTime;
        while (!IsOpaque())
        {
            image.color += new Color(0, 0, 0, tempFade);
            title.color += new Color(0, 0, 0, tempFade);

            foreach (var squid in Squids)
            {
                squid.GetComponent<Image>().color += new Color(0, 0, 0, tempFade);
            }
            yield return new WaitForEndOfFrame();
        }
    }
    public IEnumerator FadeOut()
    {
        yield return new WaitUntil(IsOpaque);
        yield return new WaitForSecondsRealtime(1f);
        float tempFade = fadeSpeed * Time.deltaTime;
        while (!IsTransparent())
        {
            image.color -= new Color(0, 0, 0, tempFade);
            title.color -= new Color(0, 0, 0, tempFade);
            foreach (var squid in Squids)
            {
                squid.GetComponent<Image>().color -= new Color(0, 0, 0, tempFade);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public bool IsOpaque()
    {
        return image.color.a >= 1;
    }
    private bool IsTransparent()
    {
        return image.color.a <= 0;
    }

    private void SpawnSquids()
    {
        

        for (int i = 0; i < NbSquid; i++)
        {

            Transform tempTransform = Circle.transform;
            GameObject squid = Instantiate(SquidPrefab, Circle.transform);
            squid.transform.position = Circle.transform.position + new Vector3(
                Mathf.Cos(2 * Mathf.PI * i / NbSquid) * 300  ,
                Mathf.Sin(2 * Mathf.PI * i / NbSquid)*150,
                0f) ;

            Squids.Add(squid );
        }
    }

    private void RotateSquids()
    {
        int i = 0;
        foreach (var squid in Squids)
        {
            squid.transform.eulerAngles +=new Vector3(0, 0, Time.deltaTime*SpeedSquid);
            squid.transform.position = Circle.transform.position + new Vector3(
                Mathf.Cos(2 * Mathf.PI * i / NbSquid + Time.time * SpeedCircle) * 300 +50* Mathf.Cos(5*Time.time * SpeedCircle + 2 * Mathf.PI * i / NbSquid)
                , Mathf.Sin(2 * Mathf.PI * i / NbSquid + Time.time * SpeedCircle ) * 150 + 25* Mathf.Cos(5* Time.time * SpeedCircle + 2 * Mathf.PI * i / NbSquid)
                , 0f);
            i++;
        }
        //Circle.transform.eulerAngles += new Vector3(0, 0, Time.deltaTime*SpeedCircle);
    }

}



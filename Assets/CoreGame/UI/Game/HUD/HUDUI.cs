using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDUI : UIElement
{
    [SerializeField] private GameObject baraldPortrait;
    [SerializeField] private GameObject illonaPortrait;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectBaraldPortrait()
    {
        baraldPortrait.SetActive(true);
        illonaPortrait.SetActive(false);
    }

    public void SelectIlonaPortrait()
    {
        baraldPortrait.SetActive(false);
        illonaPortrait.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallEssencePotion : MonoBehaviour
{

    [SerializeField] private float essenceInBottle = 10f;

    private EssenceLogic essenceLogic;

    private bool potionUsed = false;

    // Start is called before the first frame update
    void Start()
    {
        essenceLogic = GetComponent<EssenceLogic>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
           potionUsed = true;
        }
        EssencePotionMath();
    }

    void EssencePotionMath()
    {
        if (potionUsed) 
        {
            essenceLogic.currentEssence += essenceInBottle;
            potionUsed = false;
        }
    }
}

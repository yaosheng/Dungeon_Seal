using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public HealthLine healthLine;
    public Image gameoverUI;
    public Text bloodText;
    public Text coinText;
    public int coinNumber;
    public Transform healthlineParent;
    public Text atkText;
    public Text defenseText;

    void Start( )
    {
        gameoverUI.gameObject.SetActive(false);
        ShowCoinUI( );
    }

    public HealthLine GetHelathLine( )
    {
        HealthLine hl = Instantiate(healthLine) as HealthLine;
        hl.transform.SetParent(healthlineParent);
        return hl;
    }

    public void ShowGameOverUI( )
    {
        gameoverUI.gameObject.SetActive(true);
    }

    public void ShowCoinUI( )
    {
        coinText.text = coinNumber.ToString( );
    }

    public void ShowEquipUI(Hero hero)
    {
        atkText.text = hero.data.atk.ToString();
        defenseText.text = hero.data.defense.ToString( );
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenuBuild : MonoBehaviour
{
	public Text buildName;
	public Text labelProd;
	public Text labelCount;

	public void SetData(DC_Build data, int valueProd, int valueCount)
	{
		buildName.text = data.name;
		labelProd.text = "+" + (data.prod + valueProd);
		labelCount.text = (data.count + valueCount).ToString();
	}
}

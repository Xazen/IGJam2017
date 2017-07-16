using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BudgetText : MonoBehaviour
{
	public Text BudgetTextUI;
	public Text BudgetChangedText;
	private GameController _gameController;

	[Inject]
	public void Inject(GameController gameController)
	{
		_gameController = gameController;
	}

	public void Start()
	{
		BudgetChangedText.gameObject.SetActive(false);
		BudgetTextUI.text = _gameController.Budget.ToString();
		_gameController.OnBudgetChanged += OnBudgetChanged;
	}

	private void OnBudgetChanged(int newBudget, int deltabudget)
	{
		BudgetTextUI.text = newBudget.ToString();

		BudgetChangedText.color = (deltabudget < 0) ? Color.red : Color.green;
		string calcChar = (deltabudget < 0) ? "" : "+";
		BudgetChangedText.text = calcChar + deltabudget.ToString();
		BudgetChangedText.gameObject.SetActive(true);
		StartCoroutine(HideBudgetChanged());
	}

	private IEnumerator HideBudgetChanged()
	{
		yield return new WaitForSeconds(2.0f);
		BudgetChangedText.gameObject.SetActive(false);
	}
}

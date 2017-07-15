using UnityEngine;

public class Police : Unit
{
	public int AttackDamage = 10;
	
	public override void OnUnitSpawned()
	{
		
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag.Equals(TagConstants.Enemy))
		{
			Rioter rioter = other.gameObject.GetComponent<Rioter>();
			if (rioter != null)
			{
				rioter.TryTakeDamage(10);
			}
		}
	}
}

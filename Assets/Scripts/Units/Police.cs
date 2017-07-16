using UnityEngine;

public class Police : Unit
{
	public int AttackDamage = 10;
	
	public override void OnUnitSpawned()
	{
		
	}

	protected override void OnTriggerStay(Collider other)
	{
		base.OnTriggerStay(other);
		Rioter rioter = other.gameObject.GetComponent<Rioter>();
		if (rioter != null && !Destroyed)
		{
			if (!IsSpawned())
			{
				return;
			}
			rioter.TryTakeDamage(AttackDamage);
		}
	}
}

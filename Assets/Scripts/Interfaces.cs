using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Damageable {
	/// <summary>
	/// Takes damage.
	/// </summary>
	/// <returns><c>true</c>, if entity died, <c>false</c> otherwise.</returns>
	/// <param name="damage">Damage in hitpoints.</param>
	bool TakeDamage(int damage);
}

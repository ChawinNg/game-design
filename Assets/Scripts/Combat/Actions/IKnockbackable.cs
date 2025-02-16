using System.Collections;
using UnityEngine;

public interface IKnockbackable
{
  IEnumerator OnTakingKnockback(Vector3 force, float second);
}

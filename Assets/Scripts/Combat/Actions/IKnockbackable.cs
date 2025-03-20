using System.Collections;
using UnityEngine;

public interface IKnockbackable
{
  void TakingKnockback(Vector3 force, float second);

  IEnumerator OnTakingKnockback(Vector3 force, float second);
}

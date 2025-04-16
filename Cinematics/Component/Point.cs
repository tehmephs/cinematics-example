using Assets.Scripts.Data;
using UnityEngine;

public class Point : IdentityComponent<PointId>, IPoint
{
	public PointId Identity => this.identity;
	public Vector3 position => this.transform.position;

	public static implicit operator Vector3(Point point) => point.transform.position;
}

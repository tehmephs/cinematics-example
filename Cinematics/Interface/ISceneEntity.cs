using UnityEngine;

public interface ISceneEntity : INamedEntity
{
	Transform transform { get; }
	Vector3 position { get; }
}

public interface ISceneEntity<TIDentity> : ISceneEntity
	where TIDentity : Identity
{
	TIDentity Identity { get; }
}

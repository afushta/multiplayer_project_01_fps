// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 1.0.46
// 

using Colyseus.Schema;

public partial class Player : Schema {
	[Type(0, "ref", typeof(Vector3_NO))]
	public Vector3_NO position = new Vector3_NO();

	[Type(1, "ref", typeof(Vector3_NO))]
	public Vector3_NO velocity = new Vector3_NO();

	[Type(2, "ref", typeof(Vector3_NO))]
	public Vector3_NO rotation = new Vector3_NO();

	[Type(3, "ref", typeof(Vector3_NO))]
	public Vector3_NO angularVelocity = new Vector3_NO();

	[Type(4, "number")]
	public float speed = default(float);

	[Type(5, "boolean")]
	public bool isCrouching = default(bool);
}


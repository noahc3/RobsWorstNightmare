using UnityEngine;
using System.Collections;

//<summary>
//Ball movement controlls and simple third-person-style camera
//</summary>
public class RollerBall : MonoBehaviour {

	public GameObject Wall = null;
	public GameObject ViewCamera = null;
	public AudioClip JumpSound = null;
	public AudioClip HitSound = null;
	public AudioClip CoinSound = null;
	public AudioClip GlitchSound = null;

	public int BoostWhenTransformingAmount = 4;
	public int HeightBoostAmount = 2;
	
	private Rigidbody mRigidBody = null;
	private AudioSource mAudioSource = null;
	private bool mFloorTouched = false;
	
	

	void Start () {
		mRigidBody = GetComponent<Rigidbody> ();
		mAudioSource = GetComponent<AudioSource> ();
	}

	void FixedUpdate () {
		if (mRigidBody != null) {
			if (Input.GetButton ("Horizontal")) {
				mRigidBody.AddTorque(Vector3.back * Input.GetAxis("Horizontal")*10);
			}
			if (Input.GetButton ("Vertical")) {
				mRigidBody.AddTorque(Vector3.right * Input.GetAxis("Vertical")*10);
			}
			
			
		}
		if (ViewCamera != null) {
			Vector3 direction = (Vector3.up*2+Vector3.back)*2;
			RaycastHit hit;
			Debug.DrawLine(transform.position,transform.position+direction,Color.red);
			if(Physics.Linecast(transform.position,transform.position+direction,out hit)){
				ViewCamera.transform.position = hit.point;
			}else{
				ViewCamera.transform.position = transform.position+direction;
			}
			ViewCamera.transform.LookAt(transform.position);
		}
	}

	void GlitchWall(GameObject oldWall)
	{
		var glitchWalls = GameObject.FindGameObjectsWithTag("GlitchWall");
		var wall = glitchWalls[Random.Range(0, glitchWalls.Length)];
		var wallLocation = wall.transform.position;

		if (wall.transform.rotation.y == -90)
		{
			wallLocation.x += BoostWhenTransformingAmount;
		}else if (wall.transform.rotation.y == 90)
		{
			wallLocation.x -= BoostWhenTransformingAmount;
		}else if (wall.transform.rotation.y == -180)
		{
			wallLocation.z -= BoostWhenTransformingAmount;
		}else if (wall.transform.rotation.y == 0)
		{
			wallLocation.z += BoostWhenTransformingAmount;
		}

		//Boost the ball up incase it falls underground
		wallLocation.y += HeightBoostAmount;
		
		//Replace the wall that was touched
		var test = Instantiate(Wall, oldWall.transform.position, oldWall.transform.rotation) as GameObject;
		test.transform.parent = oldWall.transform.parent;
		Destroy(oldWall as GameObject);
		
		//and replace the wall that the ball is teleported too

		var repWall = Instantiate(Wall, wall.transform.position, wall.transform.rotation) as GameObject;
		repWall.transform.parent = wall.transform.parent;
		Destroy(wall as GameObject);

		
		mRigidBody.position = wallLocation;
		Debug.Log("I have glitched");
	}

	void OnCollisionEnter(Collision coll){
		
		if (coll.gameObject.tag.Equals("GlitchWall"))
		{
			mAudioSource.PlayOneShot(GlitchSound);
			GlitchWall(coll.gameObject);
		}
		
		if (coll.gameObject.tag.Equals ("Floor")) {
			mFloorTouched = true;
			if (mAudioSource != null && HitSound != null && coll.relativeVelocity.y > .5f) {
				mAudioSource.PlayOneShot (HitSound, coll.relativeVelocity.magnitude);
			}
		} else {
			if (mAudioSource != null && HitSound != null && coll.relativeVelocity.magnitude > 2f) {
				mAudioSource.PlayOneShot (HitSound, coll.relativeVelocity.magnitude);
			}
		}

	}

	void OnCollisionExit(Collision coll){
		if (coll.gameObject.tag.Equals ("Floor")) {
			mFloorTouched = false;
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag.Equals ("Coin")) {
			if(mAudioSource != null && CoinSound != null){
				mAudioSource.PlayOneShot(CoinSound);
			}
			Destroy(other.gameObject);
		}
	}
}
	
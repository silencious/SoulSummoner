using UnityEngine;
using System.Collections;

public class PCBehaviour : LiveBehaviour
{
	public Animator anim;
	public Rigidbody rb;

	// camera relative
	public Camera firstPersonCamera;
	public Camera thirdPersonCamera;
	private Camera currentCamera;

	// physics relative
	public float jumpSpeed;
	public bool onGround;

	// gameplay relative
	Elements reserve;		// elements the PC has collected
	float castDistance = 2.0f;
	float maxClickDistance = 10.0f;
	float minMoveSpeed = 0.1f;

	float minLeftPressTime = 1.0f;
	float minRightPressTime = 1.0f;
	System.Diagnostics.Stopwatch leftPressTimer;
	System.Diagnostics.Stopwatch rightPressTimer;
	public Vector3 focus;

	UIDisplay ui;
	RouteMap routeMap;
	AudioSource musicSource;
	AudioSource soundSource;
	AudioClip bgm;
	AudioClip absorbClip;
	AudioClip summonClip;

	protected override void Start ()
	{
		// init gameplay relative parameters
		soulName = "PC";
		base.Start ();
		reserve = Elements.zero;	// empty initial element reserve

		// init moving parameters
		anim = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody> ();
		moveSpeed = 15.0f;
		jumpSpeed = 10.0f;
		onGround = true;

		// init cameras
		if(firstPersonCamera==null)
			firstPersonCamera = transform.Find ("FirstPersonCamera").GetComponent<Camera> ();
		if(thirdPersonCamera==null)
			thirdPersonCamera = transform.Find ("ThirdPersonCamera").GetComponent<Camera> ();
		firstPersonCamera.enabled = false;
		thirdPersonCamera.enabled = true;
		currentCamera = thirdPersonCamera;

		leftPressTimer = System.Diagnostics.Stopwatch.StartNew();
		leftPressTimer.Reset ();
		rightPressTimer = System.Diagnostics.Stopwatch.StartNew();
		rightPressTimer.Reset ();

		ui = GameObject.Find ("UI").GetComponent<UIDisplay> ();
		ui.UpdatePCElements (elements);
		ui.UpdatePCReserve (reserve);
		ui.UpdatePCHPCeil (hp);
		ui.UpdatePCHP (hp);

		musicSource = transform.Find ("MusicSource").GetComponent<AudioSource> ();
		soundSource = transform.Find ("SoundSource").GetComponent<AudioSource> ();
		absorbClip = Resources.Load ("Sound/Absorb") as AudioClip;
		summonClip = Resources.Load("Sound/Summon") as AudioClip;
		SetMusic (bgm);
	}

	protected override void FixedUpdate(){
		//HandleMove ();
		if(waypoints==null || waypoints.Count==0){
			return;
		}
		DoRoute();
		var routeMap = RouteMap.GetCurrentInstance ();
		if(!routeMap.Passable(transform.position)){
			transform.Translate(rb.velocity * (-Time.fixedDeltaTime));
		}
	}

	protected override void OnCollisionEnter(Collision other){
		//Debug.Log ("collision enter");
		base.OnCollisionEnter (other);
		/*if (other.gameObject.CompareTag ("Terrain")) {
			onGround = true;
			//anim.SetBool ("jumping", false);
		}*/
	}

	protected override void Update (){
		MouseEvents ();
		KeyEvents ();
	}

	void HandleMove(){
		float vertical = Input.GetAxis ("Vertical");
		float horizontal = Input.GetAxis ("Horizontal");

		Vector3 forward = thirdPersonCamera.transform.forward;
		forward.y = 0;

		forward.Normalize ();
		Vector3 rightward = Vector3.Cross (Vector3.up, forward);

		Vector3 velocity = moveSpeed * (forward * vertical+rightward*horizontal);

		if (Mathf.Abs(vertical)+Mathf.Abs(horizontal) >= minMoveSpeed) {
			// set direction of character
			transform.forward = velocity;
			waypoints.Clear ();
		}


		//anim.SetFloat ("velocity", velocity.magnitude);
		if (Input.GetKey (KeyCode.Space) && onGround) {
			velocity.y = jumpSpeed;
			onGround = false;
			//anim.Play("Jump");
			//anim.SetBool("jumping", true);
		} else {
			velocity.y = rb.velocity.y;
		}

		// set velocity of rigidbody
		rb.velocity =velocity;
	}

	void MouseEvents(){
		// hold left
		if(Input.GetMouseButtonUp(0) && leftPressTimer.IsRunning){
			//Debug.Log ("left:"+leftPressTimer.Elapsed.TotalSeconds+" sec");
			if(leftPressTimer.Elapsed.TotalSeconds<minLeftPressTime){
				// click to route
				var p = MousedPoint ();
				if((p-transform.position).magnitude<=maxRouteDis){
					focus = p;
					dm.ReRouteLives ();
				}
			}else{
				// hold left mouse button, summon/cast
				Summon ();				
			}
			leftPressTimer.Reset();
		}
		if(Input.GetMouseButtonDown(0)){
			leftPressTimer.Start();
		}

		// hold right
		if(Input.GetMouseButtonUp(1) && rightPressTimer.IsRunning){
			//Debug.Log ("right:"+rightPressTimer.Elapsed.TotalSeconds+" sec");
			if(rightPressTimer.Elapsed.TotalSeconds<minRightPressTime){
				// click to route
				var p = MousedPoint ();
				if((p-transform.position).magnitude<=maxRouteDis){
					dm.RouteTo (this, p);
				}
			}else{
				// hold right mouse button, absorb
				Absorb ();
			}
			rightPressTimer.Reset();
		}
		if(Input.GetMouseButtonDown(1)){
			rightPressTimer.Start();
		}
	}

	void KeyEvents(){
		if (Input.GetKeyDown (KeyCode.Tab)) {
			// switch camera
			if (currentCamera == firstPersonCamera) {
				firstPersonCamera.enabled = false;
				thirdPersonCamera.enabled = true;
				currentCamera = thirdPersonCamera;
			} else {
				thirdPersonCamera.enabled = false;
				firstPersonCamera.enabled = true;
				currentCamera = firstPersonCamera;
			}
		}
	}

	public void UpdateHP(){
		ui.UpdatePCHP (hp);
		if(hp<=0){
			HandleDeath ();
		}
	}

	void HandleDeath(){
		
	}

	string GetCurrentCandidate(){
		return "LiveToken";
	}

	void Summon(){
		Ray ray = currentCamera.ScreenPointToRay (Input.mousePosition);
		var d = ray.direction;
		d.y = 0.0f;
		d = d.normalized * castDistance;
		var candidate = GetCurrentCandidate ();
		if(reserve.over(data.GetElementsByName(candidate))){
			PlaySound (summonClip);
			dm.Summon (candidate, transform.position+d, ref reserve);
			ui.UpdatePCReserve (reserve);
		}else{
			// test
			PlaySound (summonClip);
			dm.Summon (candidate, transform.position+d, ref reserve);
		}
	}

	void Absorb(){
		var obj = MousedObject ();
		if(obj!=null){			
			var soul = obj.GetComponent<SoulBehaviour> ();
			// only non-live soul can be absorbed
			if (soul != null && !(soul is LiveBehaviour)) {
				//Debug.Log ("Absorb " + soul.soulName);
				// absorb the soul: add its elements to pc's reserve, destroy soul obj
				PlaySound (absorbClip);
				reserve = Elements.min (reserve + soul.elements, elements);
				ui.UpdatePCReserve (reserve);
				Destroy (soul.gameObject);
			}
		}
	}

	void PlaySound(AudioClip clip){
		soundSource.clip = clip;
		soundSource.Play ();
	}

	public void SetMusic(AudioClip clip){
		bgm = clip;
		if(bgm!=null && musicSource!=null){
			musicSource.loop = true;
			musicSource.clip = bgm;
			musicSource.Play ();
		}
	}

	Vector3 MousedPoint(){
		Ray ray = currentCamera.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit)) {
			return hit.point;
		}
		return new Vector3 (Mathf.NegativeInfinity, Mathf.NegativeInfinity, Mathf.NegativeInfinity);
	}

	GameObject MousedObject(){
		Ray ray = currentCamera.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit)) {
			if (hit.distance <= maxClickDistance) {
				return hit.collider.gameObject;
			}
		}
		return null;
	}

}

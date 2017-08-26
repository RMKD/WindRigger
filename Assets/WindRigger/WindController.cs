using UnityEngine;

namespace WindRigger{

	/*
	WindRigger is a simple audio effect to increase spatial awareness for head mounted displays by mimicking the difference in sound 
	between your left ear and right ear depending on which is oriented to the wind.

	Usage:
		* add the WindRig prefab as a child of the part of your camera rig that tracks neck rotation
		* attach select a GameObject to identify the windSource (it probably makes sense to use an Empty)
		* adjust the audio effects in SetAudioEffects as needed, debug visually by linking DrawLine to effects
	*/

	public class WindController : MonoBehaviour {

		public GameObject windSource;
		public GameObject leftAudioParent;
		public GameObject rightAudioParent;

		private AudioSource leftAudio;
		private AudioSource rightAudio;

		private float angleToSource = 0;
		private float rotationRadian = 0;
		private float proximityRight = 0;
		private float proximityLeft = 0;

		void Start () {
			leftAudio = leftAudioParent.GetComponent<AudioSource> ();
			rightAudio = rightAudioParent.GetComponent<AudioSource> ();
			leftAudio.panStereo = -1;
			rightAudio.panStereo = 1;
		}

		void Update () {
			angleToSource = Mathf.Atan2 (windSource.transform.position.x - transform.position.x,
				windSource.transform.position.z - transform.position.z) + Mathf.PI; //produces values [0,2PI]
			rotationRadian = transform.eulerAngles.y / 180f * Mathf.PI; // produces values [0, 2PI]

			proximityRight = NormalizeSin (rotationRadian - angleToSource);
			proximityLeft = 1-proximityRight;

			SetAudioEffects ();

			// in the editor's Scene view, draw lights that correspond with the audio volume
			Debug.DrawLine (transform.position, windSource.transform.position);
			Debug.DrawLine (leftAudioParent.transform.position, windSource.transform.position, new Color(0,leftAudio.volume,leftAudio.volume));
			Debug.DrawLine (rightAudioParent.transform.position, windSource.transform.position, new Color(0,rightAudio.volume,rightAudio.volume));
		}

		void SetAudioEffects(){
			leftAudio.volume = ScaleNormalizedFloat(proximityLeft, 0.3f, 1f);
			leftAudio.pitch = ScaleNormalizedFloat (proximityLeft, 1, 3f);
			leftAudio.dopplerLevel =  ScaleNormalizedFloat(proximityLeft, 0.0f, 5f);

			rightAudio.volume = ScaleNormalizedFloat(proximityRight, 0.3f, 1f);
			rightAudio.pitch = ScaleNormalizedFloat(proximityRight, 1, 3f);
			rightAudio.dopplerLevel = ScaleNormalizedFloat(proximityRight, 0.0f, 5f);
		}

		float NormalizeSin(float f){
			/* normalizes sin function from [-1, 1] to [0,1] */
			return (1 + Mathf.Sin(f))/2f;
		}

		float ScaleNormalizedFloat(float f, float min, float max){
			/* scales value f [0.0, 1.0] to [min, max] */
			return min + f * (max-min);
		}
	}
}

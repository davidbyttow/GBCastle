using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static GameManager global { get; private set; }

	[SerializeField] private AudioClip music;
	private float restartDelay = 0;

	void Awake() {
		if (global != null) {
			throw new System.Exception("More than one GameManager present in scene");
		}
		global = this;
	}

	void Start() {
		StartMusic();
	}

	void StartMusic() {
		//if (music && Music.inst) {
		//	Music.inst.MaybePlay(music);
		//}
	}

	void Update() {
		if (restartDelay > 0) {
			restartDelay -= Time.deltaTime;
			if (restartDelay <= 0) {
				Restart();
			}
		}
		if (Input.GetKeyDown(KeyCode.Tab)) {
			Restart();
		}
	}

	public void QueueRestart() {
		restartDelay = 1.5f;
	}

	private void Restart() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void ExitLevel() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
}

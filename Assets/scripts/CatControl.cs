using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CatControl : MonoBehaviour {

	public int rotateRate = 10;
	public float upSpeed = 10;
    public GameObject scoreMgr;
	public GameObject gameoverPanel;

    public AudioClip jumpUp;
    public AudioClip hit;
    public AudioClip score;

    public bool inGame = false;

	private bool dead = false;
	private bool landed = false;

    private Sequence CatSequence;

    // Use this for initialization
    void Start () {

        float catOffset = 0.05f;
        float catTime = 0.3f;
        float catStartY = transform.position.y;

        catequence = DOTween.Sequence();

        catSequence.Append(transform.DOMoveY(catStartY + catOffset, catTime).SetEase(Ease.Linear))
            .Append(transform.DOMoveY(catStartY - 2 * catOffset, 2 * catTime).SetEase(Ease.Linear))
            .Append(transform.DOMoveY(catStartY, catTime).SetEase(Ease.Linear))
            .SetLoops(-1);
    }

	// Update is called once per frame
	void Update () {
        if (!inGame)
        {
            return;
        }
        catSequence.Kill();

		if (!dead)
		{
			if (Input.GetButtonDown("Fire1"))
			{
                JumpUp();

			}

		}

		if (!landed)
		{
			float v = transform.GetComponent<Rigidbody2D>().velocity.y;

			float rotate = Mathf.Min(Mathf.Max(-90, v * rotateRate + 60), 30);

			transform.rotation = Quaternion.Euler(0f, 0f, rotate);
		}
		else
		{
			transform.GetComponent<Rigidbody2D>().rotation = -90;
		}
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.name == "land" || other.name == "pipe_up" || other.name == "pipe_down")
		{
            if (!dead)
            {
                GameObject[] objs = GameObject.FindGameObjectsWithTag("movable");
                foreach (GameObject g in objs)
                {
                    g.BroadcastMessage("GameOver");
                }

                GetComponent<Animator>().SetTrigger("die");
                AudioSource.PlayClipAtPoint(hit, Vector3.zero);
            }



			if (other.name == "land")
			{
				transform.GetComponent<Rigidbody2D>().gravityScale = 0;
				transform.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

				landed = true;
			}
		}


        if (other.name == "pass_trigger")
        {
            scoreMgr.GetComponent<ScoreMgr>().AddScore();
            AudioSource.PlayClipAtPoint(score, Vector3.zero);
        }


	}

    public void JumpUp()
    {
        transform.GetComponent<Rigidbody2D>().velocity = new Vector2(0, upSpeed);
        AudioSource.PlayClipAtPoint(jumpUp, Vector3.zero);
    }

	public void GameOver()
	{
		dead = true;
		StartCoroutine(waitforGo());
	}
	IEnumerator waitforGo()
	{
		yield return new WaitForSeconds(1);
		gameoverPanel.SetActive(true);
		Time.timeScale = 0;
	}

}

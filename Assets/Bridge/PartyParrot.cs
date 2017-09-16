using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.Experimental.UIElements.Image;

public class PartyParrot : MonoBehaviour
{

	public Texture2D[] frames;
	public float framesPerSecond = 10;
	
	private RawImage image;


	void Start()
	{
		image = gameObject.GetComponent<RawImage>();
	}
	
	void Update ()
	{
		int index = (int)(Time.unscaledTime * framesPerSecond);
		index = index % frames.Length;
		image.texture = frames[index];
	}
}

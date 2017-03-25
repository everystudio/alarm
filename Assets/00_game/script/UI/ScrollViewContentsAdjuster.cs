using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewContentsAdjuster : MonoBehaviour {

	// 私用のため縦のみ対応

	public int target_index;

	[SerializeField]
	private float offset_height;
	public float height_interval;

	[SerializeField]
	private RectTransform contents;

	[SerializeField]
	private ScrollRect scrollRect;

	public void StartAdjust()
	{
	}

	private void reset<T>()
	{
		T[] list = contents.gameObject.GetComponentsInChildren<T>();
		if (0 < list.Length)
		{
			height_interval = contents.sizeDelta.y / list.Length;
		}
		else
		{
			height_interval = 0.0f;
		}
	}





}

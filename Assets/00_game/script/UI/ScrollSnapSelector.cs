using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollSnapSelector : ScrollRect
{
	public int target_index;

	[SerializeField]
	private float offset_height;
	[SerializeField]
	private float height_interval;
	[SerializeField]
	private RectTransform contents;

	private int contents_num;

	public void SetText( string _strTail)
	{
		Text[] list = contents.gameObject.GetComponentsInChildren<Text>();
		int num = 0;
		foreach( Text t in list)
		{
			t.text = string.Format("{0:D2}{1}", num, _strTail);
			num += 1;
		}
	}

	public override void OnBeginDrag(PointerEventData eventData)
	{
		base.OnBeginDrag(eventData);
		reset<Text>();
	}

	public void SetIndex(int _iIndex)
	{
		reset<Text>();
		target_index = _iIndex;
		move();
	}

	private void move()
	{
		if (height_interval == 0.0f)
		{
			Invoke("retrymove", 0.02f);
		}
		else {
			contents.transform.localPosition = new Vector3(
				contents.transform.localPosition.x,
				offset_height + target_index * height_interval,
				0.0f);
		}
	}

	private void retrymove()
	{
		reset<Text>();
		move();
	}



	public override void OnEndDrag(PointerEventData eventData)
	{
		base.OnEndDrag(eventData);

		//Debug.LogError(contents.gameObject.transform.localPosition.y);

		if( contents.gameObject.transform.localPosition.y < offset_height)
		{
			target_index = 0;
			//Debug.LogError("a");
		}
		else if(offset_height + contents_num * height_interval < contents.gameObject.transform.localPosition.y)
		{
			target_index = contents_num - 1;
			//Debug.LogError("b");
		}
		else
		{
			//Debug.LogError("c");
			float start_pos = offset_height - height_interval * 0.5f;
			float pos_y = contents.gameObject.transform.localPosition.y;
			for( int i = 0; i < contents_num; i++)
			{
				start_pos += height_interval;
				if ( pos_y < start_pos)
				{
					target_index = i;
					break;
				}
			}
			SetIndex(target_index);
		}
	}

	private void reset<T>()
	{
		T[] list = contents.gameObject.GetComponentsInChildren<T>();

		contents_num = list.Length;
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

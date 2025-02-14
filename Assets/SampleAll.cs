using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Emptybraces.TweenTest
{
	public class SampleAll : MonoBehaviour
	{
		public GameObject _template;
		public Transform _topParent;
		[Range(0.01f, 2)] public float Duration = 1;
		public float Interval = 1;
		List<(RectTransform handle, Text elapsed)> _handles = new();
		IEnumerator Start()
		{
			var names = Enum.GetNames(typeof(Tween.Easing));
			for (int i = 0, l = names.Length; i < l; ++i)
			{
				var g = Instantiate(_template, _template.transform.parent);
				g.name = g.GetComponentInChildren<Text>().text = names[i];
				_handles.Add((g.GetComponentInChildren<Slider>().handleRect, g.transform.GetChild(2).GetComponent<Text>()));
				if ((Tween.Easing)i == Tween.Easing.Linear || (Tween.Easing)i == Tween.Easing.Swing)
					g.transform.SetParent(_topParent);
			}
			Destroy(_template.gameObject);

			while (true)
			{
				var start_time = Time.time;
				var end_time = Time.time + Duration;
				while (true)
				{
					var t = Mathf.InverseLerp(start_time, end_time, Time.time);
					for (int i = 0; i < _handles.Count; ++i)
					{
						// _sliders[i].value = Tween.Ease((Easing)i, 0, 1, t);
						var value = Tween.Ease((Tween.Easing)i, 0, 1, t);
						_handles[i].handle.anchorMin = new Vector2(value, 0f);
						_handles[i].handle.anchorMax = new Vector2(value, 1f);
						_handles[i].handle.anchoredPosition = Vector2.zero;
						_handles[i].handle.GetComponent<Image>().color = value < 0f || 1f < value ? Color.red : Color.green;
						_handles[i].elapsed.text = $"{value:f4}";
						_handles[i].elapsed.color = Color.Lerp(Color.white, Color.green, value);
					}
					if (end_time <= Time.time)
						break;
					yield return null;
				}
				yield return new WaitForSeconds(Interval);
			}
		}

		void Update()
		{
			// Debug.Log(Tween.Ease(Easing.InElastic, 0, 1, Time.time % 1f) + ": " + Time.time % 1f);
		}
	}
}

using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Emptybraces.TweenTest
{
	public class SampleRunner : MonoBehaviour
	{
		[Range(0.01f, 5)] public float Duration = 1;
		public float Interval = 1;
		public Tween.Easing StartEasing;
		public Text Display;
		public RectTransform Current;
		async Task Start()
		{
			var names = System.Enum.GetNames(typeof(Tween.Easing));
			int idx = (int)StartEasing;
			while (true)
			{
				var duration = Random.Range(0.5f, 2.0f);
				var runner = new Tween.Runner(duration, (Tween.Easing)idx);
				var start = Random.Range(-1f, 1f);
				var end = Random.Range(1f, 2f);
				var elapsed = 0f;
				while (runner.Run(start, end, out var v))
				// while (runner.Run01(out var result))
				{
					var vi = (v - start) / (end - start);//Mathf.InverseLerp(start, end, v);
					Current.anchorMin = new Vector2(vi, 0);
					Current.anchorMax = new Vector2(vi, 1);
					Current.anchoredPosition = Vector2.zero;
					Display.text = $"{names[idx]}\n{v:f2}\n<size=70%>{start:f2}                   {end:f2}</size><size=40%>\n{elapsed+=Time.deltaTime:f2}/{duration:f2}</size>";
					await Awaitable.NextFrameAsync(destroyCancellationToken);
				}
				await Awaitable.WaitForSecondsAsync(Interval, destroyCancellationToken);
				idx = (idx + 1) % names.Length;
			}
		}
	}
}

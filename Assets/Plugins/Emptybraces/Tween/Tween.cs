// #define UNITASK
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Emptybraces
{
	public static class Tween
	{
		public enum Easing
		{
			Linear,
			Swing,
			InQuad,
			OutQuad,
			InOutQuad,
			InCubic,
			OutCubic,
			InOutCubic,
			InQuart,
			OutQuart,
			InOutQuart,
			InQuint,
			OutQuint,
			InOutQuint,
			InSine,
			OutSine,
			InOutSine,
			InExpo,
			OutExpo,
			InOutExpo,
			InCirc,
			OutCirc,
			InOutCirc,
			InElastic,
			OutElastic,
			InOutElastic,
			InBack,
			OutBack,
			InOutBack,
			InBounce,
			OutBounce,
			InOutBounce,
		}
		public struct Runner
		{
			float duration, elapsed;
			Easing easing;
			bool isUnscaled;
			float DeltaTime => isUnscaled ? Time.unscaledDeltaTime : Time.deltaTime;
			public Runner(float duration, Easing easing = Easing.OutQuad, bool isUnscaled = false)
			{
				this.duration = duration;
				this.easing = easing;
				this.isUnscaled = isUnscaled;
				elapsed = 0;
			}
			public bool Run01(out float result) => Run(0, 1, out result);
			public bool Run10(out float result) => Run(1, 0, out result);
			public bool Run(float start, float end, out float result)
			{
				if (elapsed == duration)// ぴったりになることはあり得ないことを利用する。
				{
					result = end;
					return false;
				}
				if (duration <= 0f) // 0以下を渡されたら0除算できないので、true返却
				{
					elapsed = duration; // 次で終わらす
					result = end;
					return true;
				}
				elapsed = Mathf.Min(elapsed + DeltaTime, duration); // 到達したら次で終わる
				result = Ease(easing, start, end, elapsed / duration);
				return true;
			}
#if UNITASK
			public UniTask Run01Async(Action<float> f, CancellationToken cancel = default) => RunAsync(0, 1, f, cancel);
			public UniTask Run10Async(Action<float> f, CancellationToken cancel = default) => RunAsync(1, 0, f, cancel);
			public async UniTask RunAsync(float start, float end, Action<float> f, CancellationToken cancel = default)
			{
				while (Run(start, end, out var v))
				{
					f(v);
					await UniTask.Yield(cancel);
				}
			}
#elif UNITY_2023_1_OR_NEWER
			public Task Run01Async(Action<float> f, CancellationToken cancel = default) => RunAsync(0, 1, f, cancel);
			public Task Run10Async(Action<float> f, CancellationToken cancel = default) => RunAsync(1, 0, f, cancel);
			public async Task RunAsync(float start, float end, Action<float> f, CancellationToken cancel = default)
			{
				while (Run(start, end, out var v))
				{
					f(v);
					await Awaitable.NextFrameAsync(cancel);
				}
			}
#endif
		}
		public static float Ease(Easing easing, float start, float end, float t)
		{
			t = Mathf.Clamp01(t);
			float delta = end - start;
			switch (easing)
			{
				case Easing.Linear:
					return start + delta * t;
				case Easing.Swing:
					return start + delta * (0.5f - Mathf.Cos(t * Mathf.PI) * 0.5f);
				case Easing.InQuad:
					return start + delta * (t * t);
				case Easing.OutQuad:
					return start + delta * (1f - (1f - t) * (1f - t));
				case Easing.InOutQuad:
					return start + delta * (t < 0.5f ? 2f * t * t : 1f - Mathf.Pow(-2f * t + 2f, 2f) * 0.5f);
				case Easing.InCubic:
					return start + delta * (t * t * t);
				case Easing.OutCubic:
					return start + delta * (1f - Mathf.Pow(1f - t, 3f));
				case Easing.InOutCubic:
					return start + delta * (t < 0.5f ? 4f * t * t * t : 1f - Mathf.Pow(-2f * t + 2f, 3f) * 0.5f);
				case Easing.InQuart:
					return start + delta * (t * t * t * t);
				case Easing.OutQuart:
					return start + delta * (1f - Mathf.Pow(1f - t, 4f));
				case Easing.InOutQuart:
					return start + delta * (t < 0.5f ? 8f * t * t * t * t : 1f - Mathf.Pow(-2f * t + 2f, 4f) * 0.5f);
				case Easing.InQuint:
					return start + delta * (t * t * t * t * t);
				case Easing.OutQuint:
					return start + delta * (1f - Mathf.Pow(1f - t, 5f));
				case Easing.InOutQuint:
					return start + delta * (t < 0.5f ? 16f * t * t * t * t * t : 1f - Mathf.Pow(-2f * t + 2f, 5f) * 0.5f);
				case Easing.InSine:
					return start + delta * (1f - Mathf.Cos(t * Mathf.PI * 0.5f));
				case Easing.OutSine:
					return start + delta * Mathf.Sin(t * Mathf.PI * 0.5f);
				case Easing.InOutSine:
					return start + delta * (0.5f - 0.5f * Mathf.Cos(t * Mathf.PI));
				case Easing.InExpo:
					return start + delta * (t == 0f ? 0f : Mathf.Pow(2f, 10f * (t - 1f)));
				case Easing.OutExpo:
					return start + delta * (t == 1f ? 1f : 1f - Mathf.Pow(2f, -10f * t));
				case Easing.InOutExpo:
					return start + delta * (t == 0f ? 0f : t == 1f ? 1f : (t < 0.5f ? Mathf.Pow(2f, 20f * t - 10f) * 0.5f : (2f - Mathf.Pow(2f, -20f * t + 10f)) * 0.5f));
				case Easing.InCirc:
					return start + delta * (1f - Mathf.Sqrt(1f - t * t));
				case Easing.OutCirc:
					return start + delta * Mathf.Sqrt(1f - Mathf.Pow(t - 1f, 2f));
				case Easing.InOutCirc:
					return start + delta * (t < 0.5f ? (1f - Mathf.Sqrt(1f - 4f * t * t)) * 0.5f : (Mathf.Sqrt(1f - Mathf.Pow(-2f * t + 2f, 2f)) + 1f) * 0.5f);
				case Easing.InElastic:
					return start + delta * (-Mathf.Pow(2, 10 * (t - 1)) * Mathf.Sin((t - 1.1f) * 5 * Mathf.PI));
				case Easing.OutElastic:
					return start + delta * (Mathf.Pow(2, -10 * t) * Mathf.Sin((t - 0.1f) * 5 * Mathf.PI) + 1);
				case Easing.InOutElastic:
					return start + delta * (t < 0.5f ? -0.5f * Mathf.Pow(2, 10 * (2 * t - 1)) * Mathf.Sin((2 * t - 1.1f) * 5 * Mathf.PI)
													 : 0.5f * Mathf.Pow(2, -10 * (2 * t - 1)) * Mathf.Sin((2 * t - 1.1f) * 5 * Mathf.PI) + 1);
				case Easing.InBack:
					return start + delta * (t * t * (2.70158f * t - 1.70158f));
				case Easing.OutBack:
					t--;
					return start + delta * (t * t * (2.70158f * t + 1.70158f) + 1);
				case Easing.InOutBack:
					t *= 2;
					if (t < 1)
						return start + delta * 0.5f * (t * t * (3.5949095f * t - 2.5949095f));
					t -= 2;
					return start + delta * 0.5f * (t * t * (3.5949095f * t + 2.5949095f) + 2);
				case Easing.InBounce:
					return start + delta * (1 - Ease(Easing.OutBounce, 0, 1, 1 - t));
				case Easing.OutBounce:
					if (t < 1 / 2.75f)
						return start + delta * (7.5625f * t * t);
					else if (t < 2 / 2.75f)
					{
						t -= 1.5f / 2.75f;
						return start + delta * (7.5625f * t * t + 0.75f);
					}
					else if (t < 2.5f / 2.75f)
					{
						t -= 2.25f / 2.75f;
						return start + delta * (7.5625f * t * t + 0.9375f);
					}
					else
					{
						t -= 2.625f / 2.75f;
						return start + delta * (7.5625f * t * t + 0.984375f);
					}
				case Easing.InOutBounce:
					return start + delta * (t < 0.5f ? Ease(Easing.InBounce, 0, 1, t * 2) * 0.5f : Ease(Easing.OutBounce, 0, 1, t * 2 - 1) * 0.5f + 0.5f);
				default:
					return start;
			}
		}
	}
}

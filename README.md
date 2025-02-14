# Unity Tween
Simple tween controller.

![out](https://github.com/user-attachments/assets/826b897a-62e7-446b-a71a-5d68b35b208b)


# Code

```c#
// 1
var value = Tween.Ease(Tween.Easing.InSine, start, end, elpased / duration);
// 2 
var tw = new Tween.Runner(duration, Tween.Easing.OutQuad);
while (tw.Run01(out var value))
{
  _cg.alpha = value;
  await UniTask.Yield(cancel);
}
// 3
await new Tween.Runner(.25f).Run01Async(_cg.SetAlpha);
```

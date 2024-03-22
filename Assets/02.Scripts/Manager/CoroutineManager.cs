using System.Collections;

/// <summary>
/// MonoBehaivour를 상속받지 않는 클래스의 코루틴 실행을 위한 매니저
/// </summary>
public class CoroutineManager : SingletonObject<CoroutineManager>
{
    protected override void Awake()
    {
        base.Awake();
    }

    public void ThisStartCoroutine(UnityEngine.Coroutine coroutine, IEnumerator IEFunc)
    {
        coroutine = StartCoroutine(IEFunc);
    }

    public void ThisStopCoroutine(UnityEngine.Coroutine coroutine)
    {
        StopCoroutine(coroutine);
        coroutine = null;
    }
}

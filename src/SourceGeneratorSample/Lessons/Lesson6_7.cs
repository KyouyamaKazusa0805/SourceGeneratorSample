namespace SourceGeneratorSample.Lessons;

/// <summary>
/// 第六、七节课（实战：创建 <c>MyTuple</c> 系列类型）的源代码调用。
/// </summary>
public sealed class Lesson6_7 : ILessonInvoker
{
	/// <inheritdoc/>
	public static void Invoke()
	{
		var myTuple1 = new MyTuple<int, double>(1, 3.0);
		var myTuple2 = new MyTuple<int, double>(1, 3.0);
		Console.WriteLine(myTuple1 == myTuple2);
	}
}

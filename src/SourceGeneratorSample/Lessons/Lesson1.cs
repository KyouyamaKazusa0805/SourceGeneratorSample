namespace SourceGeneratorSample.Lessons;

/// <summary>
/// 第一节课的源代码调用。
/// </summary>
public sealed class Lesson1 : ILessonInvoker
{
	/// <inheritdoc/>
	public static void Invoke() => Greeting.SayHelloTo("Sunnie");
}

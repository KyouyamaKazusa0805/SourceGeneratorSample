namespace SourceGeneratorSample.Lessons;

/// <summary>
/// 用于第 12 节课的代码调用。
/// </summary>
public sealed class Lesson_12 : ILessonInvoker
{
	/// <inheritdoc/>
	public static void Invoke()
		=> Greeting_UseIncrementalGenerator.SayHelloTo("Sunnie");
}

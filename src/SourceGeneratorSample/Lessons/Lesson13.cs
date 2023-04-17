namespace SourceGeneratorSample.Lessons;

/// <summary>
/// 用于第 13 讲的代码调用。
/// </summary>
public sealed class Lesson13 : ILessonInvoker
{
	/// <inheritdoc/>
	public static void Invoke()
		=> GreetingUsePatrialClass_IncrementalGenerator.SayHelloTo2("Sunnie");
}

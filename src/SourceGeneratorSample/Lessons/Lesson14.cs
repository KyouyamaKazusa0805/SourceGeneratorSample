namespace SourceGeneratorSample.Lessons;

/// <summary>
/// 用于第 14 讲的代码调用。
/// </summary>
public sealed class Lesson14 : ILessonInvoker
{
	/// <inheritdoc/>
	public static void Invoke()
		=> GreetingUseAttribute_IncrementalGenerator.SayHiTo("Sunnie");
}

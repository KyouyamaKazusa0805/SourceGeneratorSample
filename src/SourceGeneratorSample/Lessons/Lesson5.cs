namespace SourceGeneratorSample.Lessons;

/// <summary>
/// 第五节课（使用特性生成的源代码）的源代码调用。
/// </summary>
public sealed class Lesson5 : ILessonInvoker
{
	/// <inheritdoc/>
	public static void Invoke() => GreetingUseAttribute.SayHiTo("Sunnie");
}

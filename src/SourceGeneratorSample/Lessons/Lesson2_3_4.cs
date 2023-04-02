namespace SourceGeneratorSample.Lessons;

/// <summary>
/// 第二、三、四节课（使用分部方法）的源代码调用。
/// </summary>
public sealed class Lesson2_3_4 : ILessonInvoker
{
	/// <inheritdoc/>
	public static void Invoke() => GreetingUsePartialClass_Modified.SayHelloTo("Sunnie");
}

namespace SourceGeneratorSample.Greetings;

/// <summary>
/// 一个分部类型，该类型支持打招呼，并且用特性来和源代码生成器进行交互。
/// </summary>
public static partial class GreetingUseAttribute
{
	[SayHello]
	public static partial void SayHiTo(string name);
}

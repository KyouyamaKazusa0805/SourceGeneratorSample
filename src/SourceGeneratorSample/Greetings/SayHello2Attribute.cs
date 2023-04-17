namespace SourceGeneratorSample.Greetings;

/// <summary>
/// 表示一个特性，表示标记了该特性的方法自动被源代码生成器识别，并生成打招呼的代码逻辑，
/// 使用 <c>IIncrementalGenerator</c> 实现。
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class SayHello2Attribute : Attribute;

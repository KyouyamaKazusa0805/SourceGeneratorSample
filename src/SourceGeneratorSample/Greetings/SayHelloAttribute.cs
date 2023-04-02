namespace SourceGeneratorSample.Greetings;

/// <summary>
/// 表示一个特性，用于一个方法，表示该方法用于源代码生成器生成打招呼的操作的功能。
/// </summary>
[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public sealed class SayHelloAttribute : Attribute;

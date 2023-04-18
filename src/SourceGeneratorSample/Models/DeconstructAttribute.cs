namespace SourceGeneratorSample.Models;

/// <summary>
/// 表示源代码生成器需要使用的特性。该特性标记到目标方法上，
/// 表示这个方法会被源代码生成器自动生成有关解构函数的操作。
/// </summary>
[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public sealed class DeconstructAttribute : Attribute;

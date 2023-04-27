namespace SourceGeneratorSample.Models;

/// <summary>
/// 标记在 <see cref="object"/> 类型提供了的虚方法的重写方法上，
/// 表示该方法的执行逻辑由源代码生成器自动生成。
/// </summary>
[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public sealed class AutoOverriddingAttribute : Attribute;

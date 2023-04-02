namespace SourceGeneratorSample.Lessons;

/// <summary>
/// 表示一个课程教学使用的接口。这个接口里带有一个调用方法，
/// 这个方法提供的是本项目使用的课程使用的源代码生成器生成代码的调用入口点。
/// </summary>
public interface ILessonInvoker
{
	/// <summary>
	/// 触发方法。这个方法作为入口点，将对应项目的源代码生成器的生成文件的成员进行一定关联，用于课程教学期间的使用测试。
	/// </summary>
	static abstract void Invoke();
}

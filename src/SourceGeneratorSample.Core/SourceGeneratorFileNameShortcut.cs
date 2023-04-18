namespace SourceGeneratorSample.Core;

/// <summary>
/// 源代码生成器的生成文件的后缀名称，作为文件名称的区分，区分不同的取名的文件。
/// </summary>
internal static class SourceGeneratorFileNameShortcut
{
	public const string GreetingGenerator_Basic = ".g.ggb.cs";
	public const string GreetingGenerator_PartialMethod = ".g.ggpm.cs";
	public const string GreetingGenerator_UseAttribute = ".g.ggua.cs";
	public const string MyTupleGenerator = ".g.mtg.cs";
	public const string VersionGenerator = ".g.vg.cs";
	public const string GreetingGenerator_UseIncrementalGenerator = ".g.gguig.cs";
	public const string GreetingGenerator_PartialMethod_UseIncrementalGenerator = ".g.ggpm.uig.cs";
	public const string AutoDeconstructMethodGenerator = ".g.admg.cs";
}

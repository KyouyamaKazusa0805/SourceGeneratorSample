namespace SourceGeneratorSample.MyTuple;

/// <summary>
/// 一个源代码生成器，生成一个自定义使用的元组类型。
/// </summary>
[Generator(LanguageNames.CSharp)]
public sealed class MyTupleGenerator : ISourceGenerator
{
	/// <summary>
	/// 一个用于本地文件配置信息错误，导致无法读取的错误提示信息数据的呈现对象。
	/// </summary>
	private static readonly DiagnosticDescriptor Descriptor = new(
		"SG0000",
		"本地配置文件错误",
		"源代码生成器生成成功，但本地配置文件有误。错误情况：{0}。",
		"SourceGenerator",
		DiagnosticSeverity.Warning,
		true
	);


	/// <inheritdoc/>
	public void Execute(GeneratorExecutionContext context)
	{
		// 读取本地配置文件（用 .txt 文件格式表示的），
		// 并取出其中的整数配置数值，然后给替换到这里的 maxCount 变量上。
		// 注意，我们从外部导入进来的 .txt 格式的文件是放在目标项目里的，
		// 也就是源代码生成器生成的那些个代码文件，所存储的那个项目。
		// 而不是这个项目！一定要注意这个问题；否则会导致源代码生成器无法找到 AdditionalFiles
		// 属性的结果，导致生成内容的错误，以及不满足预期的结果。
		var maxCount = 8;
		if (context.AdditionalFiles is [{ Path: var path }])
		{
			var result = File.ReadAllText(path);
			var regex = new Regex("""\d+""");
			var match = regex.Match(result);
			if (!match.Success)
			{
				context.ReportDiagnostic(Diagnostic.Create(Descriptor, null, new[] { "配置文件的内容并不是一个数字" }));
				goto GeneratePart;
			}

			var v = match.Value;
			if (!int.TryParse(v, out var value))
			{
				context.ReportDiagnostic(Diagnostic.Create(Descriptor, null, new[] { "配置文件书写的数字过大" }));
				goto GeneratePart;
			}

			if (value is not (>= 2 and <= 8))
			{
				context.ReportDiagnostic(Diagnostic.Create(Descriptor, null, new[] { "由于源代码生成器的限制，输入的数值只能介于 2 到 8 之间（可含边界）" }));
				goto GeneratePart;
			}

			maxCount = value;
		}

	GeneratePart:
		// 我们的目标是生成一个泛型参数个数不同的同名重载类型，跟 Action、Action<>、Action<,> 等等相似的代码片段。
		// 所以我们需要一个循环去生成它们。
		var list = new List<string>();
		for (var i = 2; i <= maxCount; i++)
		{
			// 基本片段。
			var indices = Enumerable.Range(1, i).ToArray();
			var docParamPart = string.Join(
				"\r\n",
				from index in indices
				select
					$"""
					/// <param name="value{index}">
					/// <inheritdoc cref="Value{index}" path="/summary"/>
					/// </param>
					"""
			);
			var genericArgs = $"<{string.Join(", ", from index in indices select $"T{index}")}>";
			var ctorArgs = string.Join(
				", ",
				from index in indices select $"T{index} value{index}"
			);
			var constraints = string.Join(
				"\r\n\t",
				from index in indices
				select $"where T{index} : notnull, global::System.Numerics.IEqualityOperators<T{index}, T{index}, bool>"
			);
			var properties = string.Join(
				"\r\n\r\n\t",
				from index in indices
				select
					$$"""
					/// <summary>
						/// 表示该元组里的第 {{index}} 个元素。
						/// </summary>
						public T{{index}} Value{{index}} { get; } = value{{index}};
					"""
			);
			var comparison = string.Join(
				" && ",
				from index in indices select $"left.Value{index} == right.Value{index}"
			);
			var paramsInHashCode = string.Join(
				", ",
				from index in indices select $"Value{index}"
			);

			// 将前面的片段嵌入到源代码之中。
			// 这里使用一个 list 挨个保存类型的代码，然后最后将它们放在同一个文件里。
			list.Add(
				$$"""
				/// <summary>
				/// 提供一个自定义的元组类型。带有 {{i}} 个元素。
				/// </summary>
				{{docParamPart}}
				[global::System.Runtime.CompilerServices.CompilerGeneratedAttribute]
				[global::System.CodeDom.Compiler.GeneratedCodeAttribute("{{nameof(MyTupleGenerator)}}", "{{SourceGeneratorVersion.Value}}")]
				public readonly struct MyTuple{{genericArgs}}({{ctorArgs}}) :
					global::System.IEquatable<MyTuple{{genericArgs}}>,
					global::System.Numerics.IEqualityOperators<MyTuple{{genericArgs}}, MyTuple{{genericArgs}}, bool>
					{{constraints}}
				{
					{{properties}}

					/// <inheritdoc/>
					[global::System.Runtime.CompilerServices.CompilerGeneratedAttribute]
					[global::System.CodeDom.Compiler.GeneratedCodeAttribute("{{nameof(MyTupleGenerator)}}", "{{SourceGeneratorVersion.Value}}")]
					public override bool Equals([global::System.Diagnostics.CodeAnalysis.NotNullWhenAttribute(true)] object? obj)
						=> obj is MyTuple{{genericArgs}} comparer && Equals(comparer);

					/// <inheritdoc/>
					[global::System.Runtime.CompilerServices.CompilerGeneratedAttribute]
					[global::System.CodeDom.Compiler.GeneratedCodeAttribute("{{nameof(MyTupleGenerator)}}", "{{SourceGeneratorVersion.Value}}")]
					public bool Equals(MyTuple{{genericArgs}} other) => this == other;

					/// <inheritdoc/>
					[global::System.Runtime.CompilerServices.CompilerGeneratedAttribute]
					[global::System.CodeDom.Compiler.GeneratedCodeAttribute("{{nameof(MyTupleGenerator)}}", "{{SourceGeneratorVersion.Value}}")]
					public override int GetHashCode() => HashCode.Combine({{paramsInHashCode}});


					/// <inheritdoc/>
					[global::System.Runtime.CompilerServices.CompilerGeneratedAttribute]
					[global::System.CodeDom.Compiler.GeneratedCodeAttribute("{{nameof(MyTupleGenerator)}}", "{{SourceGeneratorVersion.Value}}")]
					public static bool operator ==(MyTuple{{genericArgs}} left, MyTuple{{genericArgs}} right)
						=> {{comparison}};

					/// <inheritdoc/>
					[global::System.Runtime.CompilerServices.CompilerGeneratedAttribute]
					[global::System.CodeDom.Compiler.GeneratedCodeAttribute("{{nameof(MyTupleGenerator)}}", "{{SourceGeneratorVersion.Value}}")]
					public static bool operator !=(MyTuple{{genericArgs}} left, MyTuple{{genericArgs}} right)
						=> !(left == right);
				}
				"""
			);
		}

		// 输出文件。
		context.AddSource(
			$"MyTuple{SourceGeneratorFileNameShortcut.MyTupleGenerator}",
			$$"""
			// <auto-generated/>

			#nullable enable
			namespace System;

			{{string.Join("\r\n\r\n", list)}}
			"""
		);
	}

	/// <inheritdoc/>
	public void Initialize(GeneratorInitializationContext context)
	{
	}
}

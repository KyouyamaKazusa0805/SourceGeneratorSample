using static AutoOverridding.CommonMethods;

namespace AutoOverridding;

/// <summary>
/// 一个源代码生成器，生成对于 <see cref="object"/> 和 <see cref="ValueType"/>
/// 类型里提供的虚方法进行一定的实现。
/// </summary>
[Generator(LanguageNames.CSharp)]
public sealed class AutoOverriddingGenerator : IIncrementalGenerator
{
	/// <inheritdoc/>
	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		context.RegisterSourceOutput(
			context.SyntaxProvider.WithAttribute("SourceGeneratorSample.Models.AutoOverriddingAttribute", Transform_Equals),
			Output_Equals
		);

		context.RegisterSourceOutput(
			context.SyntaxProvider.WithAttribute("SourceGeneratorSample.Models.AutoOverriddingAttribute", Transform_GetHashCode),
			Output_GetHashCode
		);

		context.RegisterSourceOutput(
			context.SyntaxProvider.WithAttribute("SourceGeneratorSample.Models.AutoOverriddingAttribute", Transform_ToString),
			Output_ToString
		);


		static Data_Equals? Transform_Equals(GeneratorAttributeSyntaxContext gasc, CancellationToken _)
			=> gasc switch
			{
				{
					TargetNode: MethodDeclarationSyntax
					{
						Modifiers: var methodModifiers and not [],
						Parent: TypeDeclarationSyntax { Modifiers: var typeModifiers and not [] }
					},
					TargetSymbol: IMethodSymbol
					{
						Name: nameof(Equals),
						ReturnType.SpecialType: SpecialType.System_Boolean,
						Parameters: [{ Type.SpecialType: SpecialType.System_Object }],
						ContainingType:
						{
							Name: var typeName,
							TypeKind: var typeKind,
							IsRecord: var isRecord,
							TypeParameters: var typeParameters,
							ContainingNamespace: var namespaceSymbol
						} typeSymbol
					}
				}
				when methodModifiers.Any(SyntaxKind.PartialKeyword)
					&& typeModifiers.Any(SyntaxKind.PartialKeyword)
					=> new(
						namespaceSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)["global::".Length..],
						(isRecord, typeKind) switch
						{
							(true, TypeKind.Class) => "record",
							(true, TypeKind.Struct) => "record struct",
							(_, TypeKind.Class) => "class",
							(_, TypeKind.Struct) => "struct",
							(_, TypeKind.Interface) => "interface",
							_ => throw new InvalidOperationException()
						},
						typeName,
						typeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
						typeParameters switch { [] => null, _ => $"<{string.Join(", ", typeParameters)}>" },
						methodModifiers
					),
				_ => null
			};

		static Data_GetHashCode? Transform_GetHashCode(GeneratorAttributeSyntaxContext gasc, CancellationToken _)
			=> gasc switch
			{
				{
					TargetNode: MethodDeclarationSyntax
					{
						Modifiers: var methodModifiers and not [],
						Parent: TypeDeclarationSyntax { Modifiers: var typeModifiers and not [] }
					},
					TargetSymbol: IMethodSymbol
					{
						Name: nameof(GetHashCode),
						ReturnType.SpecialType: SpecialType.System_Int32,
						Parameters: [],
						ContainingType:
						{
							Name: var typeName,
							TypeKind: var typeKind,
							IsRecord: var isRecord,
							TypeParameters: var typeParameters,
							ContainingNamespace: var namespaceSymbol
						} typeSymbol
					}
				}
				when methodModifiers.Any(SyntaxKind.PartialKeyword)
					&& typeModifiers.Any(SyntaxKind.PartialKeyword)
					=> new(
						namespaceSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)["global::".Length..],
						(isRecord, typeKind) switch
						{
							(true, TypeKind.Class) => "record",
							(true, TypeKind.Struct) => "record struct",
							(_, TypeKind.Class) => "class",
							(_, TypeKind.Struct) => "struct",
							(_, TypeKind.Interface) => "interface",
							_ => throw new InvalidOperationException()
						},
						typeName,
						typeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
						typeParameters switch { [] => null, _ => $"<{string.Join(", ", typeParameters)}>" },
						methodModifiers,
						(
							from member in typeSymbol.GetMembers()
							let memberName = member switch
							{
								IFieldSymbol { Name: var f, DeclaredAccessibility: Accessibility.Public, IsStatic: false }
									=> f,
								IPropertySymbol { Name: var p, DeclaredAccessibility: Accessibility.Public, GetMethod: not null, IsStatic: false }
									=> p,
								_
									=> null
							}
							where memberName is not null
							select memberName
						).ToArray()
					),
				_ => null
			};

		static Data_ToString? Transform_ToString(GeneratorAttributeSyntaxContext gasc, CancellationToken _)
			=> gasc switch
			{
				{
					TargetNode: MethodDeclarationSyntax
					{
						Modifiers: var methodModifiers and not [],
						Parent: TypeDeclarationSyntax { Modifiers: var typeModifiers and not [] }
					},
					TargetSymbol: IMethodSymbol
					{
						Name: nameof(ToString),
						ReturnType:
						{
							SpecialType: SpecialType.System_String,
							NullableAnnotation: var questionMarkToken
						},
						Parameters: [],
						ContainingType:
						{
							Name: var typeName,
							TypeKind: var typeKind,
							IsRecord: var isRecord,
							TypeParameters: var typeParameters,
							ContainingNamespace: var namespaceSymbol
						} typeSymbol
					}
				}
				when methodModifiers.Any(SyntaxKind.PartialKeyword)
					&& typeModifiers.Any(SyntaxKind.PartialKeyword)
					=> new(
						namespaceSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)["global::".Length..],
						(isRecord, typeKind) switch
						{
							(true, TypeKind.Class) => "record",
							(true, TypeKind.Struct) => "record struct",
							(_, TypeKind.Class) => "class",
							(_, TypeKind.Struct) => "struct",
							(_, TypeKind.Interface) => "interface",
							_ => throw new InvalidOperationException()
						},
						typeName,
						typeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
						typeParameters switch { [] => null, _ => $"<{string.Join(", ", typeParameters)}>" },
						methodModifiers,
						(
							from member in typeSymbol.GetMembers()
							let memberName = member switch
							{
								IFieldSymbol { Name: var f, DeclaredAccessibility: Accessibility.Public, IsStatic: false }
									=> f,
								IPropertySymbol { Name: var p, DeclaredAccessibility: Accessibility.Public, GetMethod: not null, IsStatic: false }
									=> p,
								_
									=> null
							}
							where memberName is not null
							select memberName
						).ToArray(),
						questionMarkToken switch
						{
							NullableAnnotation.None => null,
							NullableAnnotation.NotAnnotated => null,
							NullableAnnotation.Annotated => "?",
							_ => throw new InvalidOperationException()
						}
					),
				_ => null
			};

		static void Output_Equals(SourceProductionContext spc, ImmutableArray<Data_Equals> data)
		{
			var list = new List<string>();
			foreach (var (@namespace, kind, typeName, typeFullName, typeParams, methodModifiers) in data)
			{
				list.Add(
					$$"""
					namespace {{@namespace}}
					{
						partial {{kind}} {{typeName}}{{typeParams}}
						{
							/// <inheritdoc/>
							[global::System.Runtime.CompilerServices.CompilerGeneratedAttribute]
							[global::System.CodeDom.Compiler.GeneratedCodeAttribute("{{nameof(AutoOverriddingGenerator)}}", "{{SourceGeneratorVersion.Value}}")]
							{{methodModifiers}} bool Equals([global::System.Diagnostics.CodeAnalysis.NotNullWhenAttribute(true)] object? obj)
								=> obj is {{typeFullName}} comparer && Equals(comparer);
						}
					}
					"""
				);
			}

			spc.AddSource(
				$"AutoOverridding.Equals{SourceGeneratorFileNameShortcut.AutoOverriddingGenerator}",
				$$"""
				// <auto-generated />

				#nullable enable

				{{string.Join("\r\n\r\n", list)}}
				"""
			);
		}

		static void Output_GetHashCode(SourceProductionContext spc, ImmutableArray<Data_GetHashCode> data)
		{
			var list = new List<string>();
			foreach (var (@namespace, kind, typeName, typeFullName, typeParams, methodModifiers, dataMembers) in data)
			{
				string targetExpressionString = dataMembers.Length switch
				{
					0 => "\t\t\t=> 0;",
					<= 8 => $"\t\t\t=> global::System.HashCode.Combine({string.Join(", ", dataMembers)});",
					_ => $$"""
							{
								var hashCode = new global::System.HashCode();

								{{string.Join(
									"\r\n\t\t\t",
									from memberName in dataMembers
									select $"hashCode.Add({memberName});"
								)}}

								return hashCode.ToHashCode();
							}
					"""
				};

				list.Add(
					$$"""
					namespace {{@namespace}}
					{
						partial {{kind}} {{typeName}}{{typeParams}}
						{
							/// <inheritdoc/>
							[global::System.Runtime.CompilerServices.CompilerGeneratedAttribute]
							[global::System.CodeDom.Compiler.GeneratedCodeAttribute("{{nameof(AutoOverriddingGenerator)}}", "{{SourceGeneratorVersion.Value}}")]
							{{methodModifiers}} int GetHashCode()
					{{targetExpressionString}}
						}
					}
					"""
				);
			}

			spc.AddSource(
				$"AutoOverridding.GetHashCode{SourceGeneratorFileNameShortcut.AutoOverriddingGenerator}",
				$$"""
				// <auto-generated />

				#nullable enable

				{{string.Join("\r\n\r\n", list)}}
				"""
			);
		}

		static void Output_ToString(SourceProductionContext spc, ImmutableArray<Data_ToString> data)
		{
			var list = new List<string>();
			foreach (var (@namespace, kind, typeName, typeFullName, typeParams, methodModifiers, dataMembers, nullableAnnotation) in data)
			{
				string expressions = string.Join(
					", ",
					from dataMember in dataMembers
					select $$$""""{{nameof({{{dataMember}}})}} = {{{{{dataMember}}}}}""""
				);

				list.Add(
					$$$""""
					namespace {{{@namespace}}}
					{
						partial {{{kind}}} {{{typeName}}}{{{typeParams}}}
						{
							/// <inheritdoc/>
							[global::System.Runtime.CompilerServices.CompilerGeneratedAttribute]
							[global::System.CodeDom.Compiler.GeneratedCodeAttribute("{{{nameof(AutoOverriddingGenerator)}}}", "{{{SourceGeneratorVersion.Value}}}")]
							{{{methodModifiers}}} string{{{nullableAnnotation}}} ToString()
								=> $$"""{{nameof({{{typeName}}}{{{typeParams}}})}} { {{{expressions}}} }""";
						}
					}
					""""
				);
			}

			spc.AddSource(
				$"AutoOverridding.ToString{SourceGeneratorFileNameShortcut.AutoOverriddingGenerator}",
				$$"""
				// <auto-generated />

				#nullable enable

				{{string.Join("\r\n\r\n", list)}}
				"""
			);
		}
	}
}

file sealed record Data_Equals(
	string NamespaceName,
	string TypeKind,
	string TypeName,
	string TypeFullName,
	string? TypeParametersString,
	SyntaxTokenList MethodModifiers
);

file sealed record Data_GetHashCode(
	string NamespaceName,
	string TypeKind,
	string TypeName,
	string TypeFullName,
	string? TypeParametersString,
	SyntaxTokenList MethodModifiers,
	string[] DataMemberNames
);

file sealed record Data_ToString(
	string NamespaceName,
	string TypeKind,
	string TypeName,
	string TypeFullName,
	string? TypeParametersString,
	SyntaxTokenList MethodModifiers,
	string[] DataMemberNames,
	string? NullableAnnotation
);

/// <summary>
/// 一些常用的方法集。
/// </summary>
file static class CommonMethods
{
	/// <inheritdoc cref="NotNull{T}(T?)"/>
	public static bool NotNull<T>(T? inst) where T : class => inst is not null;

	/// <summary>
	/// 判断对象是否不可为 <see langword="null"/>。
	/// </summary>
	/// <typeparam name="T">一个引用类型，表示该实例的类型。</typeparam>
	/// <param name="inst">该实例。</param>
	/// <returns>返回 <see cref="bool"/> 结果，表示参数是否不为 <see langword="null"/>。</returns>
	public static bool NotNull<T>(T? inst) where T : struct => inst is not null;

	/// <inheritdoc cref="SelectNotNull{T, TOther}(T?, TOther)"/>
	public static T SelectNotNull<T, TOther>(T? inst, TOther _) where T : class => inst!;

	/// <summary>
	/// 将第一个参数 <paramref name="inst"/> 的非空结果取出并返回。该方法会将结果类型从 <typeparamref name="T"/>? 映射为 <typeparamref name="T"/>。
	/// </summary>
	/// <typeparam name="T">表示第一个参数的类型。</typeparam>
	/// <typeparam name="TOther">第二个参数的类型。该参数没有用，但配合前文使用的方法会使用到该参数作为传参。</typeparam>
	/// <param name="inst">实例。</param>
	/// <param name="_">一个不会用到的参数。</param>
	/// <returns>返回结果。</returns>
	public static T SelectNotNull<T, TOther>(T? inst, TOther _) where T : struct => inst!.Value;
}

/// <summary>
/// 以文件形式提供扩展方法。
/// </summary>
file static class Extensions
{
	/// <summary>
	/// 快捷调用 <see cref="SyntaxValueProvider.ForAttributeWithMetadataName{T}(string, Func{SyntaxNode, CancellationToken, bool}, Func{GeneratorAttributeSyntaxContext, CancellationToken, T})"/>。
	/// </summary>
	/// <typeparam name="T">泛型参数。该参数代表的是返回结果的单个元素类型。该参数只能是引用类型，因为上面提供的代码都是引用类型对象。</typeparam>
	/// <param name="this">一个 <see cref="SyntaxValueProvider"/> 实例，提供特性筛选的基本环境。</param>
	/// <param name="attributeFullName">表示你需要抽取的特性的完整名称（包含命名空间）。</param>
	/// <param name="transform">映射方法。该方法会将得到的语义模型以及节点对象改写成为 <typeparamref name="T"/> 类型的实例。</param>
	/// <param name="nodeFilter">
	/// 节点的初筛方法。该方法返回 <see cref="bool"/> 结果表示节点是否可通过初步筛选。该参数可以留空，默认无条件返回 <see langword="true"/>，
	/// 即等价于 <c>static (_, _) => true</c> 的 Lambda 表达式。
	/// </param>
	/// <returns>一个结果，包裹了一系列 <typeparamref name="T"/> 类型的实例，用不可变数组包裹起来。</returns>
	/// <seealso cref="SyntaxValueProvider.ForAttributeWithMetadataName{T}(string, Func{SyntaxNode, CancellationToken, bool}, Func{GeneratorAttributeSyntaxContext, CancellationToken, T})"/>
	public static IncrementalValueProvider<ImmutableArray<T>> WithAttribute<T>(
		this SyntaxValueProvider @this,
		string attributeFullName,
		Func<GeneratorAttributeSyntaxContext, CancellationToken, T?> transform,
		Func<SyntaxNode, CancellationToken, bool>? nodeFilter = null
	) where T : class
		=> @this
			.ForAttributeWithMetadataName(attributeFullName, nodeFilter ?? (static (_, _) => true), transform)
			.Where(NotNull)
			.Select(SelectNotNull)
			.Collect();
}

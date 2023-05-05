namespace AutoOverridding;

/// <summary>
/// 一个源代码生成器，生成对于 <see cref="object"/> 和 <see cref="ValueType"/>
/// 类型里提供的虚方法进行一定的实现。
/// </summary>
[Generator(LanguageNames.CSharp)]
public sealed class AutoOverriddingGenerator : IIncrementalGenerator
{
	private static bool AlwaysTrue<T1, T2>(T1 _, T2 __) => true;
	private static bool NotNull<T>(T? inst) where T : class => inst is not null;
	private static bool NotNull<T>(T? inst) where T : struct => inst is not null;
	private static T SelectNotNull<T, TOther>(T? inst, TOther _) where T : class => inst!;
	private static T SelectNotNull<T, TOther>(T? inst, TOther _) where T : struct => inst!.Value;


	/// <inheritdoc/>
	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		context.RegisterSourceOutput(
			context.SyntaxProvider
				.ForAttributeWithMetadataName(
					"SourceGeneratorSample.Models.AutoOverriddingAttribute",
					AlwaysTrue,
					Transform_Equals
				)
				.Where(NotNull)
				.Select(SelectNotNull)
				.Collect(),
			Output_Equals
		);

		context.RegisterSourceOutput(
			context.SyntaxProvider
				.ForAttributeWithMetadataName(
					"SourceGeneratorSample.Models.AutoOverriddingAttribute",
					AlwaysTrue,
					Transform_GetHashCode
				)
				.Where(NotNull)
				.Select(SelectNotNull)
				.Collect(),
			Output_GetHashCode
		);

		context.RegisterSourceOutput(
			context.SyntaxProvider
				.ForAttributeWithMetadataName(
					"SourceGeneratorSample.Models.AutoOverriddingAttribute",
					AlwaysTrue,
					Transform_ToString
				)
				.Where(NotNull)
				.Select(SelectNotNull)
				.Collect(),
			Output_ToString
		);
	}

	private static Data_Equals? Transform_Equals(GeneratorAttributeSyntaxContext gasc, CancellationToken _)
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

	private static Data_GetHashCode? Transform_GetHashCode(GeneratorAttributeSyntaxContext gasc, CancellationToken _)
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

	private static Data_ToString? Transform_ToString(GeneratorAttributeSyntaxContext gasc, CancellationToken _)
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

	private static void Output_Equals(SourceProductionContext spc, ImmutableArray<Data_Equals> data)
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

	private static void Output_GetHashCode(SourceProductionContext spc, ImmutableArray<Data_GetHashCode> data)
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

	private static void Output_ToString(SourceProductionContext spc, ImmutableArray<Data_ToString> data)
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


	private sealed record Data_Equals(
		string NamespaceName,
		string TypeKind,
		string TypeName,
		string TypeFullName,
		string? TypeParametersString,
		SyntaxTokenList MethodModifiers
	);

	private sealed record Data_GetHashCode(
		string NamespaceName,
		string TypeKind,
		string TypeName,
		string TypeFullName,
		string? TypeParametersString,
		SyntaxTokenList MethodModifiers,
		string[] DataMemberNames
	);

	private sealed record Data_ToString(
		string NamespaceName,
		string TypeKind,
		string TypeName,
		string TypeFullName,
		string? TypeParametersString,
		SyntaxTokenList MethodModifiers,
		string[] DataMemberNames,
		string? NullableAnnotation
	);
}

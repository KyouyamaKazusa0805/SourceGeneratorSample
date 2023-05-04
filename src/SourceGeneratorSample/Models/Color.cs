namespace SourceGeneratorSample.Models;

/// <summary>
/// 表示一种颜色。
/// </summary>
/// <param name="a"><inheritdoc cref="A" path="/summary"/></param>
/// <param name="r"><inheritdoc cref="R" path="/summary"/></param>
/// <param name="g"><inheritdoc cref="G" path="/summary"/></param>
/// <param name="b"><inheritdoc cref="B" path="/summary"/></param>
public readonly partial struct Color(byte a, byte r, byte g, byte b) :
	IEquatable<Color>,
	IEqualityOperators<Color, Color, bool>
{
	/// <summary>
	/// 表示 alpha 分量（透明度）。
	/// </summary>
	public byte A { get; } = a;

	/// <summary>
	/// 表示红色分量。
	/// </summary>
	public byte R { get; } = r;

	/// <summary>
	/// 表示绿色分量。
	/// </summary>
	public byte G { get; } = g;

	/// <summary>
	/// 表示蓝色分量。
	/// </summary>
	public byte B { get; } = b;


	[Deconstruct]
	public partial void Deconstruct(out byte r, out byte g, out byte b);

	[Deconstruct]
	public partial void Deconstruct(out byte a, out byte r, out byte g, out byte b);

	[AutoOverridding]
	public override partial bool Equals(object? obj);

	/// <inheritdoc/>
	public bool Equals(Color other) => (A, R, G, B) == (other.A, other.R, other.G, other.B);

	[AutoOverridding]
	public override partial int GetHashCode();

	/// <inheritdoc cref="object.ToString"/>
	public override string ToString()
		=> $$"""{{nameof(Color)}} { {{nameof(A)}} = {{A}}, {{nameof(R)}} = {{R}}, {{nameof(G)}} = {{G}}, {{nameof(B)}} = {{B}} }""";


	/// <inheritdoc/>
	public static bool operator ==(Color left, Color right) => left.Equals(right);

	/// <inheritdoc/>
	public static bool operator !=(Color left, Color right) => !(left == right);
}

namespace StudioOMS;


/// <summary>
/// 实体基类
/// </summary>
/// <typeparam name="TKey">主键类型</typeparam>
public class Entity<TKey> : IEquatable<Entity<TKey>>
    where TKey : notnull
{
    public required TKey Id { get; init; }


    public static bool operator ==(Entity<TKey> left, Entity<TKey> right)
        => left.Equals(right);
    public static bool operator !=(Entity<TKey> left, Entity<TKey> right)
        => !left.Equals(right);



    public bool Equals(Entity<TKey>? other) => other?.Id.Equals(Id) == true;
    public override bool Equals(object? obj) => Equals(obj as Entity<TKey>);
    public override string ToString() => Id.ToString() ?? string.Empty;
    public override int GetHashCode() => Id.GetHashCode();
}

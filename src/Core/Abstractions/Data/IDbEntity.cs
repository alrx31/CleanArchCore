using System.ComponentModel.DataAnnotations;

namespace Core.Abstractions.Data;

public interface IDbEntity<TKey>
    where TKey : struct, IEquatable<TKey>
{
    [Key]
    [Required]
    public TKey Id { get; set; }
}

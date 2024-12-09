namespace WordSearch.Domain.Interfaces;

/// <summary>
/// Defines the operations related to a word matrix.
/// </summary>
public interface IWordMatrix
{
    /// <summary>
    /// Retrieves the character matrix representation.
    /// </summary>
    List<string> Matrix { get; }
}

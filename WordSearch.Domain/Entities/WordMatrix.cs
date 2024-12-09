namespace WordSearch.Domain.Entities;

/// <summary>
/// Represents the character matrix used for word searches.
/// </summary>
public class WordMatrix
{
    public List<string> Matrix { get; }

    public WordMatrix(IEnumerable<string> matrix)
    {
        if (matrix == null || !matrix.Any())
        {
            throw new ArgumentException("The matrix cannot be empty.", nameof(matrix));
        }

        int length = matrix.First().Length;

        if (matrix.Any(row => row.Length != length))
        {
            throw new ArgumentException("All rows in the matrix must have the same length.");
        }

        Matrix = matrix.ToList();
    }
}

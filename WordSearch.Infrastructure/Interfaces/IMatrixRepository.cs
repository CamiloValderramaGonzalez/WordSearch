namespace WordSearch.Infrastructure.Interfaces;

/// <summary>
/// Defines the operations to manage the matrix in storage.
/// </summary>
public interface IMatrixRepository
{
    /// <summary>
    /// Saves a new matrix.
    /// </summary>
    /// <param name="matrix">The word matrix to save.</param>
    void SaveMatrix(IEnumerable<string> matrix);

    /// <summary>
    /// Retrieves the currently stored matrix.
    /// </summary>
    /// <returns>The stored word matrix.</returns>
    IEnumerable<string> GetMatrix();
}

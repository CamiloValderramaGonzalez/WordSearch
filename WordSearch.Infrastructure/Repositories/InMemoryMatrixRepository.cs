using WordSearch.Infrastructure.Interfaces;

namespace WordSearch.Infrastructure.Repositories;

/// <summary>
/// In-memory repository to manage the word matrix.
/// </summary>
public class InMemoryMatrixRepository : IMatrixRepository
{
    private List<string> _matrix = new List<string>();

    /// <inheritdoc />
    public void SaveMatrix(IEnumerable<string> matrix)
    {
        _matrix = new List<string>(matrix);
    }

    /// <inheritdoc />
    public IEnumerable<string> GetMatrix()
    {
        return _matrix;
    }
}

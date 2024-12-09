using WordSearch.Application.Interfaces;
using WordSearch.Domain.Entities;
using WordSearch.Infrastructure.Interfaces;

namespace WordSearch.Application.Services;

/// <summary>
/// Service to search for words in a matrix.
/// </summary>
public class WordSearchService : IWordSearchService
{
    private readonly IMatrixRepository _matrixRepository;

    /// <summary>
    /// Service constructor, receives a matrix repository.
    /// </summary>
    /// <param name="matrixRepository">Repository that manages the matrix.</param>
    public WordSearchService(IMatrixRepository matrixRepository)
    {
        _matrixRepository = matrixRepository ?? throw new ArgumentNullException(nameof(matrixRepository));
    }

    /// <inheritdoc />
    public IEnumerable<string> FindWords(IEnumerable<string> wordStream)
    {
        if (wordStream == null)
        {
            throw new ArgumentNullException(nameof(wordStream));
        }

        IEnumerable<string> matrix = _matrixRepository.GetMatrix();
        if (!matrix.Any())
        {
            throw new InvalidOperationException("The matrix has not been loaded.");
        }

        WordMatrix wordMatrix = new WordMatrix(matrix);
        Dictionary<string, int> wordCounts = new();

        foreach (string word in wordStream.Distinct())
        {
            int count = CountWordOccurrences(wordMatrix, word);
            if (count > 0)
            {
                wordCounts[word] = count;
            }
        }

        // Sort by frequency (descending) and then alphabetically
        return wordCounts
            .OrderByDescending(kvp => kvp.Value) // First by frequency
            .ThenBy(kvp => kvp.Key)              // Then alphabetically
            .Take(10)                            // Limit to 10 words
            .Select(kvp => kvp.Key);             // Return only the words
    }

    private int CountWordOccurrences(WordMatrix wordMatrix, string word)
    {
        int count = 0;

        // Count in rows (horizontal) in parallel
        Parallel.For(0, wordMatrix.Matrix.Count, row =>
        {
            Interlocked.Add(ref count, CountOccurrences(wordMatrix.Matrix[row], word));
        });

        // Count in columns (vertical) in parallel
        Parallel.For(0, wordMatrix.Matrix[0].Length, col =>
        {
            var column = new string(wordMatrix.Matrix.Select(row => row[col]).ToArray());
            Interlocked.Add(ref count, CountOccurrences(column, word));
        });

        return count;
    }

    private int CountOccurrences(string line, string word)
    {
        int count = 0;
        int index = 0;

        while ((index = line.IndexOf(word, index, StringComparison.Ordinal)) != -1)
        {
            count++;
            index += word.Length; 
        }

        return count;
    }
}

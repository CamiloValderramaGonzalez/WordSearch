namespace WordSearch.Application.Interfaces;

/// <summary>
/// Defines the operations of the word search service.
/// </summary>
public interface IWordSearchService
{
    /// <summary>
    /// Finds the words from the stream within the character matrix.
    /// </summary>
    /// <param name="wordStream">The stream of words to search for.</param>
    /// <returns>A list of found words, sorted by frequency.</returns>
    IEnumerable<string> FindWords(IEnumerable<string> wordStream);
}

using Microsoft.AspNetCore.Mvc;
using WordSearch.Application.Interfaces;
using WordSearch.Domain.Entities;
using WordSearch.Infrastructure.Interfaces;

namespace WordSearch.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WordSearchController : ControllerBase
{
    private readonly IWordSearchService _wordSearchService;
    private readonly IMatrixRepository _matrixRepository;

    public WordSearchController(
        IWordSearchService wordSearchService,
        IMatrixRepository matrixRepository)
    {
        _wordSearchService = wordSearchService;
        _matrixRepository = matrixRepository;
    }

    /// <summary>
    /// Uploads a matrix into memory for searches.
    /// </summary>
    /// <param name="matrix">The word matrix.</param>
    [HttpPost("upload-matrix")]
    public IActionResult UploadMatrix([FromBody] IEnumerable<string> matrix)
    {
        try
        {
            // Validate and save the matrix
            var wordMatrix = new WordMatrix(matrix);
            _matrixRepository.SaveMatrix(matrix);
            return Ok("Matrix uploaded successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest($"Error uploading the matrix: {ex.Message}");
        }
    }

    /// <summary>
    /// Searches for words in the previously uploaded matrix.
    /// </summary>
    /// <param name="wordStream">The stream of words to search for.</param>
    [HttpPost("find-words")]
    public IActionResult FindWords([FromBody] IEnumerable<string> wordStream)
    {
        try
        {
            // Retrieve the current matrix from the repository
            var currentMatrix = _matrixRepository.GetMatrix();
            if (!currentMatrix.Any())
            {
                return BadRequest("No matrix has been uploaded.");
            }

            // Use the injected service to search for words
            var foundWords = _wordSearchService.FindWords(wordStream);

            return Ok(foundWords);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error searching for words: {ex.Message}");
        }
    }
}

using Moq;
using FluentAssertions;
using WordSearch.Application.Services;
using WordSearch.Infrastructure.Interfaces;
using WordSearch.Domain.Entities;

namespace WordSearch.Tests;

public class WordSearchServiceTests
{
    [Fact]
    public void FindWords_ShouldReturnExpectedWords()
    {
        // Arrange
        var matrixMock = new Mock<IMatrixRepository>();
        var matrix = new List<string>
        {
            "abcdefghij",
            "klmnopqrst",
            "uvwxyzabcd",
            "efghijklmn",
            "opqrstuvwx",
            "yzabcdefgi",
            "hijklmnopq",
            "rstuvwxyza",
            "bcdefghijk",
            "lmnopqrstz"
        };
        matrixMock.Setup(repo => repo.GetMatrix()).Returns(matrix);

        var wordStream = new List<string>
        {
            "abcd", "ijkl", "mnop", "qrst", "uvwx",
            "yzab", "efgh", "rstz", "xyz", "mnopqr",
            "ghij", "lmno", "klmn", "zabc", "mn"
        };

        var expected = new List<string>
        {
            "mn", "abcd", "efgh", "ghij", "klmn",
            "lmno", "mnop", "qrst", "uvwx", "ijkl"
        };

        var service = new WordSearchService(matrixMock.Object);

        // Act
        var result = service.FindWords(wordStream);

        // Assert
        result.Should().BeEquivalentTo(expected);
        result.First().Should().Be(expected.First());
        result.Last().Should().Be(expected.Last());
    }

    [Fact]
    public void FindWords_WhenMatrixIsEmpty_ShouldThrowException()
    {
        // Arrange
        var matrixMock = new Mock<IMatrixRepository>();
        matrixMock.Setup(repo => repo.GetMatrix()).Returns(new List<string>());

        var wordStream = new List<string> { "word1", "word2" };

        var service = new WordSearchService(matrixMock.Object);

        // Act
        Action act = () => service.FindWords(wordStream);

        // Assert
        act.Should().Throw<InvalidOperationException>()
           .WithMessage("The matrix has not been loaded.");
    }

    [Fact]
    public void FindWords_WhenWordStreamIsNull_ShouldThrowException()
    {
        // Arrange
        var matrixMock = new Mock<IMatrixRepository>();
        matrixMock.Setup(repo => repo.GetMatrix()).Returns(new List<string> { "abc", "def", "ghi" });

        var service = new WordSearchService(matrixMock.Object);

        // Act
        Action act = () => service.FindWords(null);

        // Assert
        act.Should().Throw<ArgumentNullException>()
           .WithMessage("Value cannot be null. (Parameter 'wordStream')");
    }

    [Fact]
    public void UploadMatrix_WhenRowsHaveDifferentLengths_ShouldThrowException()
    {
        // Arrange
        var matrixMock = new Mock<IMatrixRepository>();
        var invalidMatrix = new List<string>
        {
            "abcdefghij",
            "klmnopqrst",
            "uvwxyz", // This row has a different length
            "efghijklmn",
            "opqrstuvwx"
        };

        // Act
        Action act = () =>
        {
            var wordMatrix = new WordMatrix(invalidMatrix); // This will trigger the validation
            matrixMock.Object.SaveMatrix(invalidMatrix);    // This line should not be reached
        };

        // Assert
        act.Should().Throw<ArgumentException>()
           .WithMessage("All rows in the matrix must have the same length.");
    }
}

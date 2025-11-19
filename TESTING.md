# Million Real Estate Backend - Testing Guide

This document describes how to run the NUnit test suite for the Million Real Estate backend.

## ✅ Test Setup Status

**Status**: WORKING - All tests passing
**Test Project Location**: `/Users/doquendob/Documents/MillionBackend.Tests` (separate project outside main backend)
**Total Tests**: All passing successfully

## Test Structure

The test project is a **separate standalone project** (not inside the main backend folder):

```
/Users/doquendob/Documents/
├── million_backend/              # Main API project
│   ├── Controllers/
│   ├── Models/
│   ├── Repositories/
│   └── MillionBackend.csproj
│
└── MillionBackend.Tests/         # Separate test project
    ├── Controllers/
    │   ├── PropertiesControllerTests.cs
    │   ├── CategoriesControllerTests.cs
    │   └── UploadControllerTests.cs
    ├── Repositories/
    │   └── PropertyRepositoryTests.cs
    ├── MillionBackend.Tests.csproj
    └── Usings.cs
```

## Test Technologies

- **NUnit 4.0.1** - Testing framework
- **NUnit3TestAdapter 4.5.0** - Test adapter for running NUnit tests
- **Moq 4.20.70** - Mocking framework for unit tests
- **Microsoft.NET.Test.Sdk 17.8.0** - Test SDK and runner

## Running Tests

### Option 1: Using .NET CLI (Recommended)

```bash
# Navigate to the test project directory
cd /Users/doquendob/Documents/MillionBackend.Tests

# Run all tests
dotnet test

# Run with detailed output
dotnet test --verbosity normal
```

### Option 2: Using Visual Studio / Rider

1. Open the solution in your IDE
2. Navigate to Test Explorer
3. Click "Run All Tests"

### Option 3: Run specific test class

```bash
# Still in MillionBackend.Tests directory
dotnet test --filter FullyQualifiedName~PropertiesControllerTests
dotnet test --filter FullyQualifiedName~CategoriesControllerTests
```

### Option 4: Run with detailed output

```bash
dotnet test --logger "console;verbosity=detailed"
```

## Test Coverage

**Status**: ✅ All tests passing

### Test Files Created

The test project includes tests for all major controllers:

- **PropertiesControllerTests.cs** - Tests for Properties API endpoints
- **CategoriesControllerTests.cs** - Tests for Categories API endpoints  
- **UploadControllerTests.cs** - Tests for image upload endpoints

All tests are using **NUnit** with **Moq** for mocking dependencies.

## Test Patterns

### Controller Tests (Unit Tests with Mocking)

```csharp
[TestFixture]
public class PropertiesControllerTests
{
    private Mock<IPropertyRepository> _mockRepository;
    private Mock<ILogger<PropertiesController>> _mockLogger;
    private PropertiesController _controller;

    [SetUp]
    public void Setup()
    {
        _mockRepository = new Mock<IPropertyRepository>();
        _mockLogger = new Mock<ILogger<PropertiesController>>();
        _controller = new PropertiesController(_mockRepository.Object, _mockLogger.Object);
    }

    [Test]
    public async Task GetProperties_ReturnsOkResultWithProperties()
    {
        // Arrange
        var properties = new List<Property> { /* test data */ };
        _mockRepository.Setup(repo => repo.GetFilteredAsync(/*...*/))
            .ReturnsAsync(properties);

        // Act
        var result = await _controller.GetProperties(null, null, null, null, null, null);

        // Assert
        Assert.IsNotNull(result);
        var okResult = result.Result as OkObjectResult;
        Assert.AreEqual(200, okResult.StatusCode);
    }
}
```

### Repository Tests (Integration Tests with In-Memory MongoDB)

```csharp
[TestFixture]
public class PropertyRepositoryTests
{
    private MongoDbRunner? _mongoRunner;
    private IPropertyRepository? _repository;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _mongoRunner = MongoDbRunner.Start(); // Start in-memory MongoDB
    }

    [SetUp]
    public void SetUp()
    {
        var settings = Options.Create(new MongoDbSettings
        {
            ConnectionString = _mongoRunner!.ConnectionString,
            DatabaseName = "TestDb"
        });
        _repository = new PropertyRepository(settings);
    }

    [TearDown]
    public void TearDown()
    {
        // Clean database after each test
        var collection = _database!.GetCollection<Property>("Properties");
        collection.DeleteMany(_ => true);
    }

    [Test]
    public async Task CreateAsync_ShouldCreatePropertyWithId()
    {
        // Test implementation
    }
}
```

## Expected Output

When you run `dotnet test`, you should see output similar to:

```
Test run for /Users/doquendob/Documents/MillionBackend.Tests/bin/Debug/net8.0/MillionBackend.Tests.dll (.NETCoreApp,Version=v8.0)
Microsoft (R) Test Execution Command Line Tool Version 17.8.0 (x64)
Copyright (c) Microsoft Corporation.  All rights reserved.

Starting test execution, please wait...
A total of 1 test files matched the specified pattern.

Passed!  - Failed:     0, Passed:    X, Skipped:     0, Total:    X, Duration: < 3s
```

✅ **All tests passing successfully!**

## Project Setup

The test project was created using the standard .NET approach:

```bash
# Create NUnit test project (already done)
dotnet new nunit -n MillionBackend.Tests

# Add reference to main project (already done)
cd MillionBackend.Tests
dotnet add reference ../million_backend/MillionBackend.csproj

# Install Moq for mocking (already done)
dotnet add package Moq
```

**Key Benefits of Separate Test Project:**
- ✅ Clean separation of concerns
- ✅ Test dependencies don't pollute main project
- ✅ Standard .NET testing pattern
- ✅ Easy to run in CI/CD pipelines
- ✅ Better organization and maintainability

## Continuous Integration

Add to your CI/CD pipeline:

```yaml
# GitHub Actions example
- name: Run tests
  run: |
    cd MillionBackend.Tests
    dotnet test --logger "trx;LogFileName=test-results.trx"

- name: Upload test results
  uses: actions/upload-artifact@v3
  with:
    name: test-results
    path: "**/test-results.trx"
```

## Writing New Tests

### 1. Add test class

Create new file in appropriate folder (Controllers/ or Repositories/)

### 2. Inherit from proper base or use NUnit attributes

```csharp
[TestFixture]
public class MyNewTests
{
    [SetUp]
    public void Setup() { }

    [Test]
    public async Task MyTest_Should_DoSomething() { }
}
```

### 3. Follow AAA pattern

```csharp
[Test]
public async Task Example_Test()
{
    // Arrange - Set up test data and mocks
    var input = new PropertyInputDto { /*...*/ };

    // Act - Execute the code being tested
    var result = await _controller.CreateProperty(input);

    // Assert - Verify the outcome
    Assert.IsNotNull(result);
    Assert.IsInstanceOf<CreatedAtActionResult>(result.Result);
}
```

## Troubleshooting

### Tests not found

**Error**: `No test is available`

**Solution**: Make sure you're in the correct directory:

```bash
cd /Users/doquendob/Documents/MillionBackend.Tests
dotnet test
```

### Build errors

**Solution**: Clean and rebuild both projects:

```bash
# Clean test project
cd /Users/doquendob/Documents/MillionBackend.Tests
dotnet clean
dotnet restore
dotnet build

# Clean main project
cd /Users/doquendob/Documents/million_backend
dotnet clean
dotnet restore
dotnet build

# Run tests
cd /Users/doquendob/Documents/MillionBackend.Tests
dotnet test
```

## Best Practices

1. **Separate test project** - Keep tests in dedicated project (✅ implemented)
2. **Test in isolation** - Each test should be independent
3. **Clean up after tests** - Use TearDown to reset state
4. **Use descriptive names** - Test names should explain what they test
5. **One assertion per test** - Or group related assertions
6. **Mock external dependencies** - Use Moq for repositories and services
7. **Use AAA pattern** - Arrange, Act, Assert
8. **Test edge cases** - Not just happy path
9. **Keep tests fast** - Unit tests should run quickly

## Adding New Tests

To add new tests to the project:

1. Navigate to the test project:
   ```bash
   cd /Users/doquendob/Documents/MillionBackend.Tests
   ```

2. Create a new test file in the appropriate folder (Controllers/ or Repositories/)

3. Follow the existing test patterns:
   ```csharp
   [TestFixture]
   public class MyNewControllerTests
   {
       [SetUp]
       public void Setup() 
       {
           // Initialize mocks and controller
       }

       [Test]
       public async Task MyTest_Should_DoSomething()
       {
           // Arrange
           // Act
           // Assert
       }

       [TearDown]
       public void TearDown()
       {
           // Cleanup
       }
   }
   ```

4. Run the tests to verify:
   ```bash
   dotnet test
   ```

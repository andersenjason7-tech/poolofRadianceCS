#!/bin/bash

# Pool of Radiance C# - Build Script

echo "=========================================="
echo "  Pool of Radiance C# - Build Script"
echo "=========================================="
echo ""

# Check if .NET is installed
if ! command -v dotnet &> /dev/null; then
    echo "Error: .NET SDK not found!"
    echo "Please install .NET 8 SDK from: https://dotnet.microsoft.com/download"
    exit 1
fi

echo "✓ .NET SDK found: $(dotnet --version)"
echo ""

# Build the project
echo "Building project..."
dotnet build --configuration Release

if [ $? -eq 0 ]; then
    echo ""
    echo "✓ Build successful!"
    echo ""
    echo "To run the demo:"
    echo "  dotnet run"
    echo ""
    echo "Or run the compiled executable:"
    echo "  ./bin/Release/net8.0/PoolOfRadianceCS"
else
    echo ""
    echo "✗ Build failed!"
    exit 1
fi

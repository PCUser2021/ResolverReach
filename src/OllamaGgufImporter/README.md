# Ollama GGUF Importer (portable Windows x64)

A small self-contained Windows GUI that registers local GGUF model files with Ollama.

## Use

1. Install Ollama for Windows and make sure its local service is running.
2. Launch Ollama-GGUF-Importer.exe. No installer or administrator rights are required.
3. Click Locate .GGUF files and select one or more files.
4. Click Import. The app derives a safe model name from each filename and invokes ollama create with a temporary Modelfile.
5. After success, check the model with ollama list or run it using ollama run <model-name>.

Split GGUF sets named like model-00001-of-00003.gguf are detected and imported using the matching model-*.gguf shard pattern. Keep all shards together and do not rename them.

## User-specific default path

The app derives the current profile path from Windows rather than hardcoding a username. It displays the conventional default model store: %USERPROFILE%\.ollama\models. For a user named googe, this will usually be C:\Users\googe\.ollama\models. A custom OLLAMA_MODELS setting or service configuration can change where the Ollama service stores models.

Important: .ollama\models is an internal Ollama blob/manifest store. The app intentionally does not copy a GGUF file directly into it, because that alone does not register a model. Instead, it uses Ollama's supported Modelfile + ollama create process. The source GGUF is read in place and is not moved or uploaded by this application. Ollama ingests/stores the model using its own configured model directory.

## Notes

- Selected models are imported sequentially.
- Names are lowercase and sanitized; duplicate names in one batch receive numeric suffixes.
- Import requires the Ollama CLI. If it is not detected in PATH or usual Windows install locations, the app explains that requirement.
- The model architecture and GGUF metadata must be supported by the installed Ollama version. This tool does not convert or quantize weights.
- Import can require substantial disk space and time. Do not delete or move source files until Ollama reports success.

## Build

Requires .NET 10 SDK on Windows:

    dotnet publish src/OllamaGgufImporter/OllamaGgufImporter.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:DebugType=None -p:DebugSymbols=false -o artifacts/ollama-gguf-importer

See .github/workflows/ollama-gguf-importer.yml for the portable release artifact.

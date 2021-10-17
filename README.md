# SmallBasic

[![](https://github.com/sb/smallbasic-editor/actions/workflows/ci.yaml/badge.svg)](https://github.com/sb/smallbasic-editor/actions/workflows/ci.yaml)

This is home to the new SmallBasic editor. It has the following projects:
* **SmallBasic.Analyzers**: a set of C# analyzers to help maintain the quality of the codebase.
* **SmallBasic.Client**: a webpack project to bundle client typescript, sass, and image resources.
* **SmallBasic.Compiler**: the compiler/main engine used to run programs.
* **SmallBasic.Editor**: a blazor application that renders editor UI components.
* **SmallBasic.Generators**: a collection of scripts to automatically generate thousands of C#/typescript lines used throughout the codebase.
* **SmallBasic.Server**: an ASP server used to serve/deploy the editor.
* **SmallBasic.Tests**: C# unit tests to test the compiler/engine.
* **SmallBasic.Utilities**: helper utilities shared between different projects.

#### Developer Workflow

1. Open **SmallBasic.sln** in Visual Studio 2017.
2. Choose **SmallBasic.Editor** and press F5 to run in the browser. This will also listen to C# changes and recompile when needed.
3. If you're editing **SmallBasic.Client** typescript files, you have to recompile that project, or run `dotnet build Source/SmallBasic.Client /p:Watch=True` from a command line to also listen/recompile these changes.
4. If changing any auto-generated files, rebuild **SmallBasic.Generators** to regenerate the files.

NOTE: [this issue](https://github.com/sb/smallbasic-editor/issues/2) is currently tracking improving this workflow.

### Contributing to this project

Please open an issue or comment on an existing one before you start working on a change, to make sure the effort is not duplicated/merged appropriately.

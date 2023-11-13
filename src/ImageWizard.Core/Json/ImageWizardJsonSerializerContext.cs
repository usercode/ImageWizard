// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System.Text.Json;
using System.Text.Json.Serialization;

namespace ImageWizard.Core.Json;

[JsonSerializable(typeof(Metadata))]

//defaults
[JsonSourceGenerationOptions(JsonSerializerDefaults.Web, WriteIndented = true)]
public partial class ImageWizardJsonSerializerContext : JsonSerializerContext
{

}

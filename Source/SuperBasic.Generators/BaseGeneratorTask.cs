// <copyright file="BaseGeneratorTask.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Generators
{
    using System;
    using System.IO;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;
    using Microsoft.Build.Utilities;
    using SuperBasic.Utilities;

    public abstract class BaseGeneratorTask : Task
    {
        public string RootDirectory { get; set; }

        public sealed override bool Execute()
        {
            this.ExecuteConversion();
            return !this.Log.HasLoggedErrors;
        }

        protected abstract void ExecuteConversion();

        protected void WriteFile<TModel>(string inputFilePath, string outputFilePath, Func<TModel, string> converter)
        {
            var settings = new XmlReaderSettings();
            settings.ValidationEventHandler += (sender, args) =>
            {
                switch (args.Severity)
                {
                    case XmlSeverityType.Error:
                        this.Log.LogError(args.Message);
                        break;
                    case XmlSeverityType.Warning:
                        this.Log.LogWarning(args.Message);
                        break;
                    default:
                        throw ExceptionUtilities.UnexpectedValue(args.Severity);
                }
            };

            using (var stream = new MemoryStream(File.ReadAllBytes(inputFilePath)))
            {
                using (var xmlReader = XmlReader.Create(stream, settings))
                {
                    var serializer = new XmlSerializer(typeof(TModel));
                    var model = (TModel)serializer.Deserialize(xmlReader);

                    File.WriteAllText(outputFilePath, converter(model));
                }
            }
        }
    }
}

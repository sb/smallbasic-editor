// <copyright file="BaseConverterTask.cs" company="MIT License">
// Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Generators
{
    using System;
    using System.IO;
    using System.Text;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;
    using SmallBasic.Utilities;

    public abstract class BaseConverterTask : BaseTask
    {
        public abstract int Execute(string inputFile, string outputFile);
    }

    public abstract class BaseConverterTask<TModel> : BaseConverterTask
    {
        public override sealed int Execute(string inputFile, string outputFile)
        {
            var settings = new XmlReaderSettings();
            settings.ValidationEventHandler += (sender, args) =>
            {
                switch (args.Severity)
                {
                    case XmlSeverityType.Error:
                        this.LogError(args.Message);
                        break;
                    case XmlSeverityType.Warning:
                        this.LogError(args.Message);
                        break;
                    default:
                        throw ExceptionUtilities.UnexpectedValue(args.Severity);
                }
            };

            using (var stream = new MemoryStream(File.ReadAllBytes(inputFile)))
            {
                using (var xmlReader = XmlReader.Create(stream, settings))
                {
                    var serializer = new XmlSerializer(typeof(TModel));
                    var model = (TModel)serializer.Deserialize(xmlReader);

                    this.GenerateDocHeader(outputFile);
                    this.Generate(model);

                    return this.SaveAndExit(outputFile);
                }
            }
        }

        protected abstract void Generate(TModel model);
    }
}

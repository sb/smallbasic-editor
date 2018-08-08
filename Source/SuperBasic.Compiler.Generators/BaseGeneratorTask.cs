using Microsoft.Build.Utilities;
using SuperBasic.Utilities;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace SuperBasic.Compiler.Generators
{
    public abstract class BaseGeneratorTask<TModel> : Task
    {
        public string Input { get; set; }
        public string Output { get; set; }

        protected abstract string GenerateDocumentContents(TModel model);

        public override bool Execute()
        {
            using (var stream = new FileStream(this.Input, FileMode.Open))
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

                using (var xmlReader = XmlReader.Create(stream, settings))
                {
                    var serializer = new XmlSerializer(typeof(TModel));
                    var model = (TModel)serializer.Deserialize(xmlReader);

                    File.WriteAllText(this.Output, this.GenerateDocumentContents(model));
                }
            }

            return !this.Log.HasLoggedErrors;
        }
    }
}

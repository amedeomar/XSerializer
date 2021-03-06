﻿using System;
using System.Diagnostics;
using System.IO;
using System.Xml;
using NUnit.Framework;
using XSerializer.Encryption;

namespace XSerializer.Tests.Performance
{
    public class DeserializationPerformanceTests
    {
        private const string _xmlWithAbstract = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Container xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <Id>A</Id>
  <One xsi:type=""OneWithAbstract"">
    <Id>B</Id>
    <Two xsi:type=""TwoWithAbstract"">
      <Id>C</Id>
      <Value>ABC</Value>
    </Two>
  </One>
</Container>";

        private const string _xmlWithInterface = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Container xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <Id>A</Id>
  <One xsi:type=""OneWithInterface"">
    <Id>B</Id>
    <Two xsi:type=""TwoWithInterface"">
      <Id>C</Id>
      <Value>ABC</Value>
    </Two>
  </One>
</Container>";

        [Test]
        public void Benchmark()
        {
            const int Iterations = 50000;

            var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(ContainerWithAbstract), null, null, null, null);
            var customSerializer = CustomSerializer.GetSerializer(typeof(ContainerWithInterface), null, TestXmlSerializerOptions.Empty);

            var xmlSerializerStopwatch = Stopwatch.StartNew();

            for (int i = 0; i < Iterations; i++)
            {
                using (var stringReader = new StringReader(_xmlWithAbstract))
                {
                    using (var reader = new XmlTextReader(stringReader))
                    {
                        xmlSerializer.Deserialize(reader);
                    }
                }
            }

            xmlSerializerStopwatch.Stop();

            var options = new TestSerializeOptions();

            var customSerializerStopwatch = Stopwatch.StartNew();

            for (int i = 0; i < Iterations; i++)
            {
                using (var stringReader = new StringReader(_xmlWithInterface))
                {
                    using (var xmlReader = new XmlTextReader(stringReader))
                    {
                        using (var reader = new XSerializerXmlReader(xmlReader, options.GetEncryptionMechanism(), options.EncryptKey))
                        {
                            customSerializer.DeserializeObject(reader, options);
                        }
                    }
                }
            }

            customSerializerStopwatch.Stop();

            Console.WriteLine("XmlSerializer Elapsed Time: {0}", xmlSerializerStopwatch.Elapsed);
            Console.WriteLine("CustomSerializer Elapsed Time: {0}", customSerializerStopwatch.Elapsed);
        } 
    }
}
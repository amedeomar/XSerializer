﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace XSerializer.Tests
{
    public class SerializeToStreamTests
    {
        [Test]
        public void VerifyStreamIsWrittenTo()
        {
            var foo = new Foo { Bar = "abc" };

            var serializer = new XmlSerializer<Foo>();

            var memoryStream = new MemoryStream();

            serializer.Serialize(memoryStream, foo);

            memoryStream.Flush();

            var data = memoryStream.ToArray();

            Assert.That(data, Is.Not.Empty);
        }

        public class Foo
        {
            public string Bar { get; set; }
        }
    }
}

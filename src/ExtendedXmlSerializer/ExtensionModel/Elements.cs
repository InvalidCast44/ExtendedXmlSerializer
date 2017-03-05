// MIT License
// 
// Copyright (c) 2016 Wojciech Nag�rski
//                    Michael DeMond
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System.Reflection;
using ExtendedXmlSerialization.ContentModel;
using ExtendedXmlSerialization.ContentModel.Content;
using ExtendedXmlSerialization.ContentModel.Xml;
using ExtendedXmlSerialization.ContentModel.Xml.Namespacing;
using ExtendedXmlSerialization.Core;
using ExtendedXmlSerialization.Core.Sources;

namespace ExtendedXmlSerialization.ExtensionModel
{
	class Elements : IElements
	{
		readonly IObjectNamespaces _namespaces;
		readonly IElements _elements;

		public Elements(IObjectNamespaces namespaces, IElements elements)
		{
			_namespaces = namespaces;
			_elements = elements;
		}

		public IWriter Get(TypeInfo parameter) => new Writer(_namespaces, _elements.Get(parameter));

		class Writer : ReferenceCache<IXmlWriter, ConditionMonitor>, IWriter
		{
			readonly IObjectNamespaces _namespaces;
			readonly IWriter _writer;

			public Writer(IObjectNamespaces namespaces, IWriter writer) : base(_ => new ConditionMonitor())
			{
				_namespaces = namespaces;
				_writer = writer;
			}

			public void Write(IXmlWriter writer, object instance)
			{
				_writer.Write(writer, instance);

				if (instance == writer.Root && Get(writer).Apply())
				{
					var namespaces = _namespaces.Get(writer.Root);
					var length = namespaces.Length;
					for (var i = 0; i < length; i++)
					{
						var ns = namespaces[i];
						var @namespace = !ns.Name.Equals(Defaults.Xmlns.Name) ? (Namespace?) Defaults.Xmlns : null;
						var attribute = new Attribute(ns.Name, ns.Identifier, @namespace);
						writer.Attribute(attribute);
					}
				}
			}
		}
	}
}
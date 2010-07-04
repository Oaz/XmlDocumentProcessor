
using System;
using NUnit.Framework;
using System.Xml;
using src;
using System.Text;
using NUnit.Framework.SyntaxHelpers;

namespace tests
{
	class XmlElementsSequencer : IProcessXmlElements
	{
		private StringBuilder Sequence { get; set; }
		
		public XmlElementsSequencer()
		{
			Sequence = new StringBuilder();
		}
		
		public override string ToString()
		{
			return Sequence.ToString();
		}

		public void Open (XmlElement e)
		{
			if( e.Attributes.Count > 0 )
				Sequence.AppendFormat("open {0}[{1}];",e.Name,e.Attributes["id"].Value);
			else
				Sequence.AppendFormat("open {0};",e.Name);
		}
		
		public void Close (XmlElement e)
		{
			if( e.Attributes.Count > 0 )
				Sequence.AppendFormat("close {0}[{1}];",e.Name,e.Attributes["id"].Value);
			else
				Sequence.AppendFormat("close {0};",e.Name);
		}

		public void OpenChild (XmlElement e, XmlElement child)
		{
			Sequence.AppendFormat("begin {0}>{1};",e.Name, child.Name);
		}

		public void CloseChild (XmlElement e, XmlElement child)
		{
			Sequence.AppendFormat("end {1}>{0};",e.Name, child.Name);
		}
	}
	
	[TestFixture]
	public class XmlDocumentProcessorTest
	{
		[Test]
		public void SingleRoot()
		{
			var proc = new XmlDocumentProcessor();
			var rootProc = new XmlElementsSequencer();
			proc.Register("root",rootProc);
			
			var doc = new XmlDocument();
			doc.LoadXml(@"<root/>");
			
			proc.Execute(doc);
			
			Assert.That( rootProc.ToString(), Is.EqualTo("open root;close root;") );
		}
		
		[Test]
		public void RootAndSingleChild()
		{
			var proc = new XmlDocumentProcessor();
			var seq = new XmlElementsSequencer();
			proc.Register("root",seq);
			proc.Register("a",seq);
			
			var doc = new XmlDocument();
			doc.LoadXml(@"<root><a/></root>");
			
			proc.Execute(doc);
			
			Assert.That( seq.ToString(), Is.EqualTo("open root;begin root>a;open a;close a;end a>root;close root;") );
		}
		
		[Test]
		public void RootAndTwoChildren()
		{
			var proc = new XmlDocumentProcessor();
			var seq = new XmlElementsSequencer();
			proc.Register("root",seq);
			proc.Register("a",seq);
			
			var doc = new XmlDocument();
			doc.LoadXml(@"<root><a/><a/></root>");
			
			proc.Execute(doc);
			
			Assert.That( seq.ToString(), Is.EqualTo("open root;begin root>a;open a;close a;end a>root;begin root>a;open a;close a;end a>root;close root;") );
		}
		
		[Test]
		public void ComplexDocument()
		{
			var proc = new XmlDocumentProcessor();
			var seq = new XmlElementsSequencer();
			proc.Register("root",seq);
			proc.Register("a",seq);
			proc.Register("b",seq);
			proc.Register("c",seq);
			
			var doc = new XmlDocument();
			doc.LoadXml(@"<root><a id='1'><b>foo</b><c>bar</c></a><a id='2'/></root>");
			
			proc.Execute(doc);
			
			Assert.That( seq.ToString(), Is.EqualTo("open root;begin root>a;open a[1];begin a>b;open b;close b;end b>a;begin a>c;open c;close c;end c>a;close a[1];end a>root;begin root>a;open a[2];close a[2];end a>root;close root;") );
		}
	}
}

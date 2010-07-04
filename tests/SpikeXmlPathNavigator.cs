
using System;
using NUnit.Framework;
using System.Xml;
using System.Xml.XPath;
using NUnit.Framework.SyntaxHelpers;

namespace tests
{
	[TestFixture]
	public class SpikeXmlPathNavigator
	{

		[Test]
		public void DiscoverMoveToFollowing()
		{
			var doc = new XmlDocument();
			doc.LoadXml(@"<root><a id='1'><b>foo</b><c>bar</c></a><a id='2'/></root>");
			Assert.That(doc.InnerText.Length, Is.GreaterThan(5));
			var nav = doc.CreateNavigator();
			
			Assert.That( nav.MoveToFollowing(XPathNodeType.Element), Is.True );
			Assert.That( nav.Name, Is.EqualTo("root") );
			
			Assert.That( nav.MoveToFollowing(XPathNodeType.Element), Is.True );
			Assert.That( nav.Name, Is.EqualTo("a") );
			Assert.That( nav.GetAttribute("id",""), Is.EqualTo("1") );
			
			Assert.That( nav.MoveToFollowing(XPathNodeType.Element), Is.True );
			Assert.That( nav.Name, Is.EqualTo("b") );
			
			Assert.That( nav.MoveToFollowing(XPathNodeType.Element), Is.True );
			Assert.That( nav.Name, Is.EqualTo("c") );
			
			Assert.That( nav.MoveToFollowing(XPathNodeType.Element), Is.True );
			Assert.That( nav.Name, Is.EqualTo("a") );
			Assert.That( nav.GetAttribute("id",""), Is.EqualTo("2") );
			
			Assert.That( nav.MoveToFollowing(XPathNodeType.Element), Is.False );
		}
		
		[Test]
		public void DiscoverMoveToNext()
		{
			var doc = new XmlDocument();
			doc.LoadXml(@"<root><a id='1'><b>foo</b><c>bar</c></a><a id='2'/></root>");
			Assert.That(doc.InnerText.Length, Is.GreaterThan(5));
			var nav = doc.CreateNavigator();
			
			Assert.That( nav.MoveToNext(XPathNodeType.Element), Is.False );
			Assert.That( nav.MoveToChild(XPathNodeType.Element), Is.True );
			
			Assert.That( nav.Name, Is.EqualTo("root") );
			
			Assert.That( nav.MoveToChild(XPathNodeType.Element), Is.True );
			Assert.That( nav.Name, Is.EqualTo("a") );
			Assert.That( nav.GetAttribute("id",""), Is.EqualTo("1") );
			
			Assert.That( nav.MoveToChild(XPathNodeType.Element), Is.True );
			Assert.That( nav.Name, Is.EqualTo("b") );
			
			Assert.That( nav.MoveToChild(XPathNodeType.Element), Is.False );
			Assert.That( nav.MoveToNext(XPathNodeType.Element), Is.True );
			Assert.That( nav.Name, Is.EqualTo("c") );

			Assert.That( nav.MoveToNext(XPathNodeType.Element), Is.False );
			Assert.That( nav.MoveToParent(), Is.True );
			Assert.That( nav.Name, Is.EqualTo("a") );
			Assert.That( nav.GetAttribute("id",""), Is.EqualTo("1") );

			Assert.That( nav.MoveToNext(XPathNodeType.Element), Is.True );
			Assert.That( nav.Name, Is.EqualTo("a") );
			Assert.That( nav.GetAttribute("id",""), Is.EqualTo("2") );
			
			Assert.That( nav.MoveToChild(XPathNodeType.Element), Is.False );
			Assert.That( nav.MoveToNext(XPathNodeType.Element), Is.False );
			Assert.That( nav.MoveToParent(), Is.True );
			
			Assert.That( nav.Name, Is.EqualTo("root") );
			Assert.That( nav.MoveToNext(XPathNodeType.Element), Is.False );
			Assert.That( nav.MoveToParent(), Is.True );
			
			Assert.That( nav.Name, Is.EqualTo(string.Empty) );
			Assert.That( nav.MoveToParent(), Is.False );
		}
	}
}

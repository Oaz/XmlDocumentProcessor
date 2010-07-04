
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.XPath;

namespace src
{
	public class XmlDocumentProcessor
	{
		private IDictionary<string,IProcessXmlElements> _processors;
		private XPathNavigator _navigator;
		
		public XmlDocumentProcessor ()
		{
			_processors = new Dictionary<string, IProcessXmlElements>();
		}

		public void Register (string tagName, IProcessXmlElements processor)
		{
			_processors[tagName] = processor;
		}

		public void Execute(XmlDocument doc)
		{
			_navigator = doc.CreateNavigator();
			_navigator.MoveToChild(XPathNodeType.Element);
			ExecuteElement(CurrentElement);
		}
		
		public XmlElement CurrentElement  {
			get
			{
				return _navigator.UnderlyingObject as XmlElement;
			}
		}
		
		private void ExecuteElement(XmlElement e)
		{
			if( !_processors.ContainsKey(_navigator.Name) )
				return; // do nothing if tag was not registered
			var processor = _processors[_navigator.Name];
			processor.Open(e);
			if( _navigator.MoveToChild(XPathNodeType.Element) )
			{
				do
				{
					processor.OpenChild(e,CurrentElement);
					ExecuteElement(CurrentElement);
					processor.CloseChild(e,CurrentElement);
				} while(_navigator.MoveToNext(XPathNodeType.Element));
				_navigator.MoveToParent();
			}
			processor.Close(e);
		}
	}
}

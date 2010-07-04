
using System;
using System.Xml;

namespace src
{
	public interface IProcessXmlElements
	{
		void Open(XmlElement e);
		void Close(XmlElement e);
		void OpenChild(XmlElement e,XmlElement child);
		void CloseChild(XmlElement e,XmlElement child);
	}
}

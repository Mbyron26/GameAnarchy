//ResxReader Cited source: https://github.com/MacSergey/ModsCommon Implemented by MacSergey.
namespace MbyronModsCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Runtime.Serialization;
using System.Xml;

public class ResxReader : IResourceReader, IEnumerable<DataNodeInfo> {
    private readonly string fileName;
    private TextReader reader;
    private Stream stream;
    private Dictionary<string, DataNodeInfo> resData;

    public ResxReader(string fileName) {
        this.fileName = fileName;
    }

    public ResxReader(TextReader reader) {
        this.reader = reader;
    }

    public ResxReader(Stream stream) {
        this.stream = stream;
    }
    ~ResxReader() {
        Dispose(disposing: false);
    }
    public void Close() {
        ((IDisposable)this).Dispose();
    }
    void IDisposable.Dispose() {
        GC.SuppressFinalize(this);
        Dispose(disposing: true);
    }
    protected virtual void Dispose(bool disposing) {
        if (disposing) {
            if (fileName != null && stream != null) {
                stream.Close();
                stream = null;
            }

            if (reader != null) {
                reader.Close();
                reader = null;
            }
        }
    }

    private void SetupNameTable(XmlReader reader) {
        reader.NameTable.Add("type");
        reader.NameTable.Add("name");
        reader.NameTable.Add("data");
        reader.NameTable.Add("value");
    }

    private void EnsureResData() {
        if (resData != null) {
            return;
        }

        resData = new Dictionary<string, DataNodeInfo>();
        XmlTextReader xmlTextReader = null;
        try {
            if (reader != null) {
                xmlTextReader = new XmlTextReader(reader);
            } else if (fileName != null || stream != null) {
                stream ??= new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);

                xmlTextReader = new XmlTextReader(stream);
            }

            SetupNameTable(xmlTextReader);
            xmlTextReader.WhitespaceHandling = WhitespaceHandling.None;
            ParseXml(xmlTextReader);
        } finally {
            if (fileName != null && stream != null) {
                stream.Close();
                stream = null;
            }
        }
    }

    IEnumerator<DataNodeInfo> IEnumerable<DataNodeInfo>.GetEnumerator() {
        EnsureResData();
        return resData.Values.GetEnumerator();
    }
    IEnumerator IEnumerable.GetEnumerator() {
        EnsureResData();
        return resData.Values.GetEnumerator();
    }
    IDictionaryEnumerator IResourceReader.GetEnumerator() {
        throw new NotImplementedException();
    }

    private void GetPosition(XmlReader reader, out int line, out int position) {
        if (reader is IXmlLineInfo xmlLineInfo) {
            line = xmlLineInfo.LineNumber;
            position = xmlLineInfo.LinePosition;
        } else {
            line = 0;
            position = 0;
        }
    }

    private void ParseXml(XmlTextReader reader) {
        bool flag = false;
        try {
            while (reader.Read()) {
                if (reader.NodeType == XmlNodeType.Element) {
                    if (reader.LocalName.Equals("data")) {
                        ParseDataNode(reader);
                    }
                }
            }

            flag = true;
        } catch (SerializationException ex) {
            GetPosition(reader, out var line, out var position);
            XmlException innerException = new ("SerializationException", ex, line, position);
            throw new SerializationException("SerializationException", innerException);
        } catch (TargetInvocationException ex) {
            GetPosition(reader, out var line, out var position);
            XmlException inner = new ("InvocationException", ex.InnerException, line, position);
            throw new TargetInvocationException("InvocationException", inner);
        } catch (XmlException ex) {
            throw new ArgumentException("InvalidResXFile", ex);
        } catch (Exception ex) {
            GetPosition(reader, out var line, out var position);
            XmlException innerEx = new (ex.Message, ex, line, position);
            throw new ArgumentException("InvalidResXFile", innerEx);
        } finally {
            if (!flag)
                resData = null;
        }
    }
    private void ParseDataNode(XmlTextReader reader) {
        DataNodeInfo dataNodeInfo = new() {
            Name = reader["name"],
            TypeName = reader["type"]
        };

        bool flag = false;
        GetPosition(reader, out dataNodeInfo.Line, out dataNodeInfo.Position);
        while (!flag && reader.Read()) {
            if (reader.NodeType == XmlNodeType.EndElement && reader.LocalName.Equals("data")) {
                flag = true;
            } else if (reader.NodeType == XmlNodeType.Element) {
                if (reader.Name.Equals("value")) {
                    WhitespaceHandling whitespaceHandling = reader.WhitespaceHandling;
                    try {
                        reader.WhitespaceHandling = WhitespaceHandling.Significant;
                        dataNodeInfo.Value = reader.ReadString();
                    } finally {
                        reader.WhitespaceHandling = whitespaceHandling;
                    }
                } else if (reader.Name.Equals("comment")) {
                    dataNodeInfo.Comment = reader.ReadString();
                }
            } else {
                dataNodeInfo.Value = reader.Value.Trim();
            }
        }

        if (dataNodeInfo.Name == null) {
            throw new ArgumentException();
        }

        resData[dataNodeInfo.Name] = dataNodeInfo;
    }
}

public class DataNodeInfo {
    public string Name;

    public string Comment;

    public string TypeName;

    public string Value;

    public int Line;

    public int Position;

    public DataNodeInfo Clone() {
        DataNodeInfo dataNodeInfo = new() {
            Name = Name,
            Comment = Comment,
            TypeName = TypeName,
            Value = Value,
            Line = Line,
            Position = Position
        };
        return dataNodeInfo;
    }
}

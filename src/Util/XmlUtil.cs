using System.Reflection;
using System.Xml.Serialization;
using System.Text;

namespace sodoff.Util;
public class XmlUtil {
    public static T DeserializeXml<T>(string xmlString) {
        var serializer = new XmlSerializer(typeof(T));
        using (var reader = new StringReader(xmlString))
            return (T)serializer.Deserialize(reader);
    }

    private class Utf8StringWriter : StringWriter {
        public override Encoding Encoding => Encoding.UTF8;
    }

    public static string SerializeXml<T>(T xmlObject) {
        var serializer = new XmlSerializer(typeof(T));
        using (var writer = new Utf8StringWriter()) {
            serializer.Serialize(writer, xmlObject);
            return writer.ToString();
        }
    }

    public static string ReadResourceXmlString(string name) {
        string result = "";
        var assembly = Assembly.GetExecutingAssembly();
        string resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith($"{name}.xml"));

        using (Stream stream = assembly.GetManifestResourceStream(resourceName))
        using (StreamReader reader = new StreamReader(stream))
            result = reader.ReadToEnd();
        return result;
    }

    public static uint HexToUint(string hex) {
        if (hex.StartsWith("0x", StringComparison.CurrentCultureIgnoreCase))
            hex = hex.Substring(2);
        return Convert.ToUInt32(hex, 16);
    }
}

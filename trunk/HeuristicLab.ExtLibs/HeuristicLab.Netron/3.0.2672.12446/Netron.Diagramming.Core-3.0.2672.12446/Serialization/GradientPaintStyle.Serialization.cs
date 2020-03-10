using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
namespace Netron.Diagramming.Core {
  /// <summary>
  /// Complementary partial class related to (de)serialization.
  /// </summary>
  [Serializable]
  public partial class GradientPaintStyle : ISerializable, IXmlSerializable, IDeserializationCallback {
    #region Deserialization constructor
    /// <summary>
    /// Deserialization constructor
    /// </summary>
    /// <param name="info">The info.</param>
    /// <param name="context">The context.</param>
    protected GradientPaintStyle(SerializationInfo info, StreamingContext context) {

      if (Tracing.BinaryDeserializationSwitch.Enabled)
        Trace.WriteLine("Deserializing the fields of 'GradientPaintStyle'.");

      double version = info.GetDouble("GradientPaintStyleVersion");

      mAngle = info.GetSingle("Angle");
      mEndColor = (Color)info.GetValue("EndColor", typeof(Color));
      mStartColor = (Color)info.GetValue("StartColor", typeof(Color));

    }
    #endregion

    #region Serialization events
    /*
        [OnSerializing]
        void OnSerializing(StreamingContext context)
        {
            Trace.WriteLine("Starting to serializing the 'GradientPaintStyle' class...");
        }
        [OnSerialized]
        void OnSerialized(StreamingContext context)
        {
            Trace.WriteLine("...serialization of 'GradientPaintStyle' finished");
        }
        */
    #endregion

    #region Deserialization events
    /*
        [OnDeserializing]
        void OnDeserializing(StreamingContext context)
        {
            Trace.Indent();
            Trace.WriteLine("Starting deserializing the 'GradientPaintStyle' class...");
        }
        [OnDeserialized]
        void OnDeserialized(StreamingContext context)
         {
             Trace.WriteLine("...deserialization of 'GradientPaintStyle' finished");
             Trace.Unindent();
        }
       */
    #endregion

    #region Serialization
    /// <summary>
    /// Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> with the data needed to serialize the target object.
    /// </summary>
    /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> to populate with data.</param>
    /// <param name="context">The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext"></see>) for this serialization.</param>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
    public void GetObjectData(SerializationInfo info, StreamingContext context) {
      if (Tracing.BinarySerializationSwitch.Enabled)
        Trace.WriteLine("Serializing the fields of 'GradientPaintStyle'.");

      info.AddValue(
          "GradientPaintStyleVersion",
          gradientPaintStyleVersion);

      info.AddValue("StartColor", this.mStartColor, typeof(Color));
      info.AddValue("EndColor", this.mEndColor, typeof(Color));
      info.AddValue("Angle", this.mAngle);
    }
    #endregion

    #region Xml serialization
    /// <summary>
    /// This property is reserved, apply the <see cref="T:System.Xml.Serialization.XmlSchemaProviderAttribute"></see> to the class instead.
    /// </summary>
    /// <returns>
    /// An <see cref="T:System.Xml.Schema.XmlSchema"></see> that describes the XML representation of the object that is produced by the <see cref="M:System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)"></see> method and consumed by the <see cref="M:System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)"></see> method.
    /// </returns>
    public XmlSchema GetSchema() {
      throw new NotImplementedException("The method or operation is not implemented.");
    }

    /// <summary>
    /// Generates an object from its XML representation.
    /// </summary>
    /// <param name="reader">The <see cref="T:System.Xml.XmlReader"></see> stream from which the object is deserialized.</param>
    public void ReadXml(System.Xml.XmlReader reader) {
      throw new NotImplementedException("The method or operation is not implemented.");
    }

    /// <summary>
    /// Converts an object into its XML representation.
    /// </summary>
    /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"></see> stream to which the object is serialized.</param>
    public void WriteXml(System.Xml.XmlWriter writer) {
      throw new NotImplementedException("The method or operation is not implemented.");
    }
    #endregion

    /// <summary>
    /// Runs when the entire object graph has been deserialized.
    /// </summary>
    /// <param name="sender">The object that initiated the callback. The functionality for this parameter is not currently implemented.</param>
    public void OnDeserialization(object sender) {
      if (Tracing.BinaryDeserializationSwitch.Enabled)
        Trace.WriteLine("IDeserializationCallback of 'GradientPaintStyle' called.");
    }
  }
}

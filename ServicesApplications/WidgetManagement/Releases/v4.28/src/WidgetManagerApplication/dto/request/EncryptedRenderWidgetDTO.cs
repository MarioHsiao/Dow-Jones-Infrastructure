using System;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using Factiva.BusinessLayerLogic.Attributes;
using EMG.widgets.ui.dto.lob;
using EMG.widgets.ui.exception;
using EMG.widgets.ui.Managers;
using Simplicit.Net.Lzo;
using Encryption = FactivaEncryption.encryption;

namespace EMG.widgets.ui.dto.request
{
    /// <summary>
    /// 
    /// </summary>
    public class EncryptedRenderWidgetDTO : AbstractWidgetRequestDTO
    {
        /// <summary>
        /// Encrypted token
        /// </summary>
        [ParameterName("cmpTkn")] public string compositeToken;

        /// <summary>
        /// Initializes a new instance of the <see cref="EncryptedRenderWidgetDTO"/> class.
        /// </summary>
        /// <param name="renderWidgetDTO">The render widget DTO.</param>
        public EncryptedRenderWidgetDTO(RenderWidgetDTO renderWidgetDTO)
        {
            Serialize(renderWidgetDTO);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EncryptedRenderWidgetDTO"/> class.
        /// </summary>
        public EncryptedRenderWidgetDTO()
        {
        }


        /// <summary>
        /// Determines whether this instance is valid.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance is valid; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsValid()
        {
            if (string.IsNullOrEmpty(compositeToken) || string.IsNullOrEmpty(compositeToken.Trim()))
                return false;
            return base.IsValid();
        }

        /// <summary>
        /// Deserializes this instance.
        /// </summary>
        /// <returns></returns>
        public RenderWidgetDTO Deserialize()
        {
            if (IsValid())
            {
                LZOCompressor lzo = new LZOCompressor();
                UTF8Encoding utfEncoder = new UTF8Encoding();
                try
                {
                    // Decompress the compressed string.
                    byte[] byteB;
                    try
                    {
                        int discarded;
                        byteB = HexEncoding.GetBytes(compositeToken, out discarded);
                    }
                    catch
                    {
                        try
                        {
                            int discarded;
                            byteB = HexEncoding.GetBytes(HttpUtility.UrlDecode(compositeToken), out discarded); 
                        }
                        catch
                        {
                            throw new EmgWidgetsUIException("Unable to convert to byte[] compositeToken");
                        }
                    }
                    byte[] byteA = lzo.Decompress(byteB);
                    string temp = utfEncoder.GetString(byteA);
                    // Deserialize Decompressed string int FactivaAcessObject
                    using (StringReader reader = new StringReader(temp))
                    {
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof (RenderWidgetDTO));
                        return (RenderWidgetDTO) xmlSerializer.Deserialize(reader);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Write(ex.Message);
                }
                return null;
            }
            return null;
        }

        /// <summary>
        /// Serializes the specified render widget DTO.
        /// </summary>
        /// <param name="renderWidgetDTO">The render widget DTO.</param>
        public void Serialize(RenderWidgetDTO renderWidgetDTO)
        {
            LZOCompressor lzo = new LZOCompressor();
            UTF8Encoding utfEncoder = new UTF8Encoding();
            try
            {
                using (StringWriter sw = new StringWriter(new StringBuilder()))
                {
                    XmlSerializerNamespaces faker = new XmlSerializerNamespaces();
                    faker.Add("", null);
                    XmlTextWriter writer = new XmlTextWriter(sw);
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof (RenderWidgetDTO), "");
                    writer.WriteRaw("");
                    xmlSerializer.Serialize(writer, renderWidgetDTO, faker);
                    writer.Flush();
                    byte[] byteA = utfEncoder.GetBytes(sw.ToString());
                    byte[] byteB = lzo.Compress(byteA);

                    compositeToken = HexEncoding.ToString(byteB);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex);
            }
        }
    }
}
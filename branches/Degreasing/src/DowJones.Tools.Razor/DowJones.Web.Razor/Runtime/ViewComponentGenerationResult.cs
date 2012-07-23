using System;
using System.Collections.Generic;
using System.Text;

namespace DowJones.Web.Razor.Runtime
{
    public class ViewComponentGenerationResult
    {
        public string GeneratedCode { get; set; }

        public IEnumerable<AuxiliaryContent.AuxiliaryContent> AuxiliaryContent { get; set; }


        public byte[] GetGeneratedCodeBytes()
        {
            // Save as UTF8
            Encoding enc = Encoding.UTF8;

            //Get the preamble (byte-order mark) for our encoding
            byte[] preamble = enc.GetPreamble();
            int preambleLength = preamble.Length;

            //Convert the writer contents to a byte array
            byte[] body = enc.GetBytes(GeneratedCode);

            //Prepend the preamble to body (store result in resized preamble array)
            Array.Resize(ref preamble, preambleLength + body.Length);
            Array.Copy(body, 0, preamble, preambleLength, body.Length);

            //Return the combined byte array
            return preamble;
        }
    }
}
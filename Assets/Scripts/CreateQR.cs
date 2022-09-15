using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Drawing;
using ZXing;
using ZXing.QrCode.Internal;
using ZXing.Rendering;
using ZXing.Common;
using ZXing.QrCode;
using TMPro;

public class CreateQR : MonoBehaviour
{
    public string m_TextToEncode = "Write Your QR Code Message Here";

    public TMP_Text m_TextInQR;

    public RawImage m_QRCode;

    private void Start()
    {
        CreateQRCode();
    }

    private static Color32[] Encode(string textForEncoding, int width, int height)
    {
        var encodeOptions = new QrCodeEncodingOptions
        {
            Height = height,
            Width = width,
            Margin = 1,
            PureBarcode = false
        };

        encodeOptions.Hints.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.H);

        var writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = encodeOptions
        };

        return writer.Write(textForEncoding);
    }

    private Texture2D generateQR(string text)
    {
        var encoded = new Texture2D(256, 256);
        var color32 = Encode(text, encoded.width, encoded.height);

        encoded.SetPixels32(color32);
        encoded.Apply();

        return encoded;
    }

    public void CreateQRCode()
    {
        Texture2D myQR = generateQR(m_TextToEncode);

        m_QRCode.texture = myQR;
        m_TextInQR.text = "<u>Text in QR:</u>\n\n" + m_TextToEncode;
    }
}

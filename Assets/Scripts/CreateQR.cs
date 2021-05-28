using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using ZXing;
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
        var writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Height = height,
                Width = width
            }
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

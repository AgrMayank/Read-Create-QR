using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using ZXing;
using ZXing.QrCode;
using TMPro;

public class ReadQR : MonoBehaviour
{
    private WebCamTexture m_webCamTexture;

    public RawImage m_QRCode;

    private Rect screenRect;

    public TMP_Text m_TextInQR;

    void Start()
    {
        screenRect = m_QRCode.rectTransform.rect;
        m_webCamTexture = new WebCamTexture();

        m_QRCode.texture = m_webCamTexture;

        StartCoroutine(ReadQRCode());
    }

    IEnumerator ReadQRCode()
    {
        try
        {
            if (!m_webCamTexture.isPlaying)
            {
                m_webCamTexture.requestedHeight = (int)m_QRCode.rectTransform.rect.height;
                m_webCamTexture.requestedWidth = (int)m_QRCode.rectTransform.rect.height;

                if (m_webCamTexture != null)
                {
                    m_webCamTexture.Play();
                }
            }

            IBarcodeReader barcodeReader = new BarcodeReader();

            // Decode the current frame
            var result = barcodeReader.Decode(m_webCamTexture.GetPixels32(), m_webCamTexture.width, m_webCamTexture.height);

            if (result != null)
            {
                Debug.Log("DECODED TEXT FROM QR: " + result.Text);
                m_TextInQR.text = result.Text;
            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning(ex.Message);
        }

        yield return new WaitForSeconds(0.1f);
        StartCoroutine(ReadQRCode());
    }
}

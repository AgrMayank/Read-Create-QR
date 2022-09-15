using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using ZXing;
using ZXing.QrCode;
using TMPro;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ReadQR : MonoBehaviour
{
    public TMP_Text m_TextInQR;

    [SerializeField]
    ARCameraManager m_CameraManager;

    public ARCameraManager cameraManager
    {
        get => m_CameraManager;
        set => m_CameraManager = value;
    }

    IEnumerator ReadQRCode()
    {
        if (!cameraManager.TryAcquireLatestCpuImage(out XRCpuImage cpuImage))
        {
            yield return null;
        }

        var texture = new Texture2D(cpuImage.width, cpuImage.height, TextureFormat.RGBA32, false);

        // For display, we need to mirror about the vertical access.
        var conversionParams = new XRCpuImage.ConversionParams(cpuImage, TextureFormat.RGBA32, XRCpuImage.Transformation.MirrorY);

        // Get the Texture2D's underlying pixel buffer.
        var rawTextureData = texture.GetRawTextureData<byte>();

        // Make sure the destination buffer is large enough to hold the converted data (they should be the same size)
        Debug.Assert(rawTextureData.Length == cpuImage.GetConvertedDataSize(conversionParams.outputDimensions, conversionParams.outputFormat),
            "The Texture2D is not the same size as the converted data.");

        // Perform the conversion.
        cpuImage.Convert(conversionParams, rawTextureData);

        // "Apply" the new pixel data to the Texture2D.
        texture.Apply();

        cpuImage.Dispose();

        IBarcodeReader barcodeReader = new BarcodeReader();

        // Decode the current frame
        var result = barcodeReader.Decode(texture.GetPixels32(), texture.width, texture.height);

        if (result != null)
        {
            Debug.Log("DECODED TEXT FROM QR: " + result.Text);
            m_TextInQR.text = result.Text;

            /* TODO: Call function to check QR Contents
                If - Valid map code, then Start Localization
                Else - Call this coroutine again
            */
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(ReadQRCode());
        }
    }
}

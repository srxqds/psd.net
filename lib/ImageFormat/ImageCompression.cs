/*
 * File: Assets/Editor/PSD/ImageFormat/ImageCompression.cs
 * Project: Demo
 * Company: com.luckygz
 * Porter: D.S.Qiu
 * Create Date: 7/17/2015 8:15:25 PM
 */
using UnityEngine;
using System.Collections;

namespace Com.Lucky.PhotoShop
{
    /// <summary>
    /// The possible Compression methods.
    /// </summary>
    public enum ImageCompression
    {
        /// <summary>
        /// Raw data
        /// </summary>
        Raw = 0,
        /// <summary>
        /// RLE compressed
        /// </summary>
        Rle = 1,
        /// <summary>
        /// ZIP without prediction.
        /// </summary>
        Zip = 2,
        /// <summary>
        /// ZIP with prediction.
        /// </summary>
        ZipPrediction = 3
    }
}
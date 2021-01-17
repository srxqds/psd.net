/*
 * File: Assets/Editor/PSD/Exception.cs
 * Project: Demo
 * Company: com.luckygz
 * Porter: D.S.Qiu
 * Create Date: 7/17/2015 4:03:06 PM
 */
using UnityEngine;
using System.Collections;
using System;

namespace Com.Lucky.PhotoShop
{
    public class PsdInvalidException : Exception
    {
        public PsdInvalidException()
        {
        }

        public PsdInvalidException(string message)
            : base(message)
        {
        }
    }
}

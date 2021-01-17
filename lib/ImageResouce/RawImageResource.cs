/*
 * File: Assets/Editor/PSD/ImageResouce/RawImageResource.cs
 * Project: Demo
 * Company: com.luckygz
 * Porter: D.S.Qiu
 * Create Date: 7/17/2015 4:32:03 PM
 */
using UnityEngine;
using System.Collections;

namespace Com.Lucky.PhotoShop
{
    /// <summary>
  /// Stores the raw data for unimplemented image resource types.
  /// </summary>
    public class RawImageResource : ImageResource
    {
        public byte[] Data { get; private set; }

        private ResourceID _id;
        public override ResourceID id
        {
            get { return _id; }
        }
       
        public RawImageResource(PsdBinaryReader reader,
          ResourceID resourceId, string name, int numBytes)
            : base(name)
        {
            this._id = resourceId;
            Data = reader.ReadBytes(numBytes);
        }

    }
}

/*
 * File: Assets/Editor/PSD/ImageResources.cs
 * Project: Demo
 * Company: com.luckygz
 * Porter: D.S.Qiu
 * Create Date: 7/17/2015 4:19:54 PM
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Com.Lucky.PhotoShop
{

    public class ImageResources
    {
        public ImageResourceList resourceList { get; private set; }

        public ResolutionInfo resolution
        {
            get
            {
                return (ResolutionInfo)resourceList.Get(ResourceID.ResolutionInfo);
            }

            set
            {
                resourceList.Set(value);
            }
        }

        public ImageResources(PsdBinaryReader reader)
        {
            resourceList = new ImageResourceList();
            var imageResourcesLength = reader.ReadUInt32();
            if (imageResourcesLength <= 0)
                return;

            var startPosition = reader.BaseStream.Position;
            var endPosition = startPosition + imageResourcesLength;
            while (reader.BaseStream.Position < endPosition)
            {
                var imageResource = ImageResourceFactory.CreateImageResource(reader);
                resourceList.Add(imageResource);
            }

            //-----------------------------------------------------------------------
            // make sure we are not on a wrong offset, so set the stream position 
            // manually
            reader.BaseStream.Position = endPosition;
        }
    }
}
/*----------------------------------------------------------------
// Copyright (C) 2015 广州，Lucky Game
//
// 模块名：
// 创建者：D.S.Qiu
// 修改者列表：
// 创建日期：7/17/2015 2:07:06 PM
// 模块描述：
//----------------------------------------------------------------*/
using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System;

namespace Com.Lucky.PhotoShop
{
    public class PsdFile
    {
        public PsdBinaryReader reader;
       
        public Header header { get; private set; }

        public ColorModeData colorModeData { get; private set; }

        public ImageResources imageResources { get; private set; }

        public LayerMaskInfo layerMaskInfo { get; private set; }

        public ImageData imageData { get; private set; }

        public PsdFile(string fileName,Encoding encoding)
        {
            var stream = new FileStream(fileName, FileMode.Open);
            this.reader = new PsdBinaryReader(stream,encoding);
        }

        public void Parse()
        {
            try
            {
                this.header = new Header(this.reader);
                this.colorModeData = new ColorModeData(this.reader);
                this.imageResources = new ImageResources(this.reader);
                this.layerMaskInfo = new LayerMaskInfo(this.reader, this.header, this.colorModeData);
                this.imageData = new ImageData(this.reader, this.header);

				this.reader.Dispose();
				

            }
            catch (Exception ex)
            {
				Debug.LogError(ex.Message);
                Debug.LogError(ex.StackTrace);
                this.reader.Dispose();
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;

namespace testing_ne1_HWDR_in_Cs
{
    public struct RectAngle
    {
        public int x1, x2, y1, y2;
    };
    public static class ResampleImage
    {
        public static byte[,] Resample(byte[,] srcImg)
        {
            byte[,] newData = new byte[28, 28];

            RectAngle tmpRect = GetImageRectAngle(srcImg);
            int DestSizeW = (tmpRect.x2 - tmpRect.x1) / 3;
            int DestSizeH = (tmpRect.y2 - tmpRect.y1) / 3;
            int StartPointX = (28 - DestSizeW) / 2;
            int StartPointY = (28 - DestSizeH) / 2;

            for (int y = StartPointY + 1; y <= StartPointY + DestSizeH; y++)
            {
                for (int x = StartPointX - 1; x <= StartPointX + DestSizeW; x++)
                {
                    int tx, ty;
                    if (x < 1) x = 1;
                    tx = x - StartPointX;
                    if (tx < 0) tx = 0;
                    ty = y - StartPointY;
                    if (ty < 0) ty = 0;
                    newData[(x - 1), (y - 1)] = srcImg[tmpRect.x1 + tx * 3, tmpRect.y1 + ty * 3];
                }
            }
            return newData;
        }
        public static RectAngle GetImageRectAngle(byte[,] image)
        {
            RectAngle tmpRect = new RectAngle();
            //get x1
            for (int x = 0; x < 90; x++)
            {
                for (int y = 0; y < 90; y++)
                {
                    if (image[x, y] == 1)
                    {
                        tmpRect.x1 = x;
                        goto GETX2;
                    }
                }
            }
        GETX2:
            //Get X2
            for (int x = 90 - 1; x > -1; x--)
            {
                for (int y = 0; y < 90; y++)
                {
                    if (image[x, y] == 1)
                    {
                        tmpRect.x2 = x;
                        goto GETY1;
                    }
                }
            }
        GETY1:
            //Get Y1
            for (int y = 0; y < 90; y++)
            {
                for (int x = 0; x < 90; x++)
                {
                    if (image[x, y] == 1)
                    {
                        tmpRect.y1 = y;
                        goto GETY2;
                    }
                }
            }
        GETY2:
            //Get Y2
            for (int y = 90 - 1; y > -1; y--)
            {
                {
                    for (int x = 0; x < 90; x++)
                        if (image[x, y] == 1)
                        {
                            tmpRect.y2 = y;
                            goto END;
                        }
                }
            }
        END:
            return tmpRect;
        }
        //reshape 28*28 array into 1*784 array
        public static byte[] Reshape(byte[,] img)
        {
            byte[] image = new byte[784];
            int counter = 0;
            for (int y = 0; y < 28; y++)
                for (int x = 0; x < 28; x++)
                    image[counter++] = img[x, y];
            return image;
        }
        // Get image from its bytes array
        public static Bitmap GetImage(byte[,] image, int w, int h)
        {
            Bitmap tmp = new Bitmap(w, h);
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    if (image[x, y] == 0)
                        tmp.SetPixel(x, y, Color.White);
                    else
                        tmp.SetPixel(x, y, Color.Black);
                }
            }
            return (Bitmap)tmp.Clone();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace TopLearnLand_Core.Convertors
{
    public interface IImageConvertor
    {
        void ImageResize(string input_Image_Path, string output_Image_Path, int new_Width);
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace NewsSystem.Services.Clodinary
{
    public static class OptionsHelper
    {
        public enum Crop
        {

            /// <summary>
            /// Change the size of the image exactly to the given width and height without necessarily retaining the original aspect ratio: all original image parts are visible but might be stretched or shrunk.
            /// </summary>
            scale,
            /// <summary>
            /// Same as the 'fit' mode but only if the original image is smaller than the given minimum (width and height), in which case the image is scaled up so that it takes up as much space as possible within a bounding box defined by the given width and height parameters. The original aspect ratio is retained and all of the original image is visible.

            /// </summary>
            mfit,
            /// <summary>
            /// The image is resized so that it takes up as much space as possible within a bounding box defined by the given width and height parameters. The original aspect ratio is retained and all of the original image is visible.
            /// </summary>
            fit,
            /// <summary>
            /// Create an image with the exact given width and height without distorting the image. This option first scales as much as needed to at least fill both of the given dimensions. If the requested aspect ratio is different than the original, cropping will occur on the dimension that exceeds the requested size after scaling.

            /// </summary>
            fill,
            /// <summary>
            /// 	Same as the 'fill' mode, but only if the original image is larger than the specified resolution limits, in which case the image is scaled down to fill the given width and height without distoring the image, and then the dimension that exceeds the request is cropped. If the original dimensions are both smaller than the requested size, it is not resized at all.

            /// </summary>
            ifill,
            /// <summary>
            /// Same as the 'fit' mode but only if the original image is larger than the given limit (width and height), in which case the image is scaled down so that it takes up as much space as possible within a bounding box defined by the given width and height parameters. The original aspect ratio is retained and all of the original image is visible.

            /// </summary>
            limit,
            /// <summary>
            /// Resize the image to fill the given width and height while retaining the original aspect ratio. If the proportions of the original image do not match the given width and height, padding is added to the image to reach the required size.

            /// </summary>
            pad,
            /// <summary>
            /// Same as the 'pad' mode but only if the original image is larger than the given limit (width and height), in which case the image is scaled down to fill the given width and height while retaining the original aspect ratio. If the proportions of the original image do not match the given width and height, padding is added to the image to reach the required size.
            Ipad,
            /// <summary>
            /// Same as the 'pad' mode but only if the original image is smaller than the given minimum (width and height), in which case the image is scaled up to fill the given width and height while retaining the original aspect ratio. If the proportions of the original image do not match the given width and height, padding is added to the image to reach the required size.
            /// </summary>
            mpad,
            /// <summary>
            /// Tries to prevent a "bad crop" by first attempting to use the fill mode, but adding padding if it is determined that more of the original image needs to be included in the final image. Only supported in conjunction with Automatic cropping (g_auto).
            /// </summary>
            fill_pad,
            /// <summary>
            /// Used to extract a given width & height out of the original image. The original proportions are retained and so is the size of the graphics.
            /// </summary>
            crop,
            /// <summary>
            /// Generate a thumbnail using face detection in combination with the 'face' or 'faces' gravity.
            /// </summary>
            thumb,
            /// <summary>
            /// Crop your image based on automatically calculated areas of interest within each specific photo. See the Imagga Crop and Scale add-on documentation for more information.
            /// </summary>
            imagga_crop,
            /// <summary>
            /// Scale your image based on automatically calculated areas of interest within each specific photo. See the Imagga Crop and Scale add-on documentation for more information.
            /// </summary>
            imagga_scale
            ///</summary>
        }

        /// <summary>
        /// Decides which part of the image to keep while the 'crop', 'pad', 'thumb' and 'fill' crop modes are used. For overlays, this decides where to place the overlay.
        /// </summary>
        [SuppressMessage("ReSharper", "InvalidXmlDocComment")]
        public enum Gravity
        {
            /// <summary>
            /// Decides which part of the image to keep while the 'crop', 'pad', 'thumb' and 'fill' crop modes are used. For overlays, this decides where to place the overlay.
            /// </summary>
            north,
            north_east,
            west,
            center,
            east,
            south_west,
            south,
            xy_center,
            face,
            faces,
            /// <summary>
            /// Use liquid rescaling to change the aspect ratio of an image while retaining all important content and avoiding unnatural distortions. For more details and guidelines, see liquid gravity.
            /// </summary>
            liquid,
            body,
            /// <summary>
            /// Detect all text elements in an image using the OCR Text Detection and Extraction add-on and use the detected bounding box coordinates as the basis of the transformation.
            /// </summary>
            ocr_text    
            
        }

        /// <summary>
        /// Resize or crop the image to a new aspect ratio. This parameter is used together with a specified crop mode that determines how the image is adjusted to the new dimensions.
        /// </summary>
        public static string Ratio
        {
            get
            {
                throw new NotImplementedException();
            }

            //            set => throw new NotImplementedException();
        }
    }
}

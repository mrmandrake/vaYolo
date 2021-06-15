
using Avalonia.Controls;

namespace vaYolo.Model
{
    public class VaSetup
    {        
        // change line batch to[`batch = 64`]
        // (https://github.com/AlexeyAB/darknet/blob/0039fd26786ab5f71d5af725fc18b3f521e7acfd/cfg/yolov3.cfg#L3)
        public int batch = 64;

        // change line subdivisions to[`subdivisions = 16`]
        // (https://github.com/AlexeyAB/darknet/blob/0039fd26786ab5f71d5af725fc18b3f521e7acfd/cfg/yolov3.cfg#L4)
        public int subdivision = 16;

        // change line max_batches to(`classes*2000`, but not less than number of training images and not less than `6000`),
        // f.e. [`max_batches = 6000`]
        // (https://github.com/AlexeyAB/darknet/blob/0039fd26786ab5f71d5af725fc18b3f521e7acfd/cfg/yolov3.cfg#L20) if you train for 3 classes
        public int max_batches = 6000;

        // set network size `width=416 height=416` or any value multiple of 32:
        // https://github.com/AlexeyAB/darknet/blob/0039fd26786ab5f71d5af725fc18b3f521e7acfd/cfg/yolov3.cfg#L8-L9
        public int network_size_width = 416;
        public int network_size_height = 416;

        // change line `classes=80` to your number of objects in each of 3 `[yolo]`-layers:
        public int classes = 3;

        // change[`filters = 255`] to filters=(classes + 5)x3 in the 3 `[convolutional]`
        // before each `[yolo]` layer, keep in mind that it only has to be the last `[convolutional]` before each of the `[yolo]` layers.
        public int filters() {
            return (classes + 5) * 3;
        }
    }
}
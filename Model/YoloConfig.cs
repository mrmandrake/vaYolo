using System;
using System.Collections.Generic;

namespace vaYolo
{
    public class YoloCfg
    {
        public string ShortName { get; set; }
        public string RelatedName { get; set; }
        public string Notes { get; set; }
        public string Link { get; set; }
        public string WeightSize { get; set; }

        public List<YoloCfg> Configs = new() {
            new YoloCfg()
            {
                ShortName = "yolov3_5l.cfg",
                RelatedName = "yolov3.cfg",
                WeightSize = "236.2 MiB",
                Notes = "\"5l\" refers to 5 YOLO layers; \"...or very small objects, or if you want to set high network resolution\"",
                Link = "5092"
            },
            new YoloCfg()
            {
                ShortName = "yolov3.cfg",
                RelatedName = "yolov3.cfg",
                WeightSize = "234.9 MiB",
                Notes = "contains 3 YOLO layers",
            },
            new YoloCfg()
            {
                ShortName = "yolov3-spp.cfg",
                RelatedName = "yolov3.cfg",
                WeightSize = "234.9 MiB",
                Link = "2859",
                Notes = "YOLOv3 but with extra layers for spacial pyramid pooling"
            },
            new YoloCfg()
            {
                ShortName = "yolov3-tiny_3l.cfg",
                RelatedName = "yolov3-tiny.cfg",
                WeightSize = "34.4 MiB",
                Notes = "better at finding small objects; \"3l\" refers to 3 YOLO layers vs the usual 2 in \"tiny\""
            },
            new YoloCfg()
            {
                ShortName = "yolov3-tiny.cfg",
                WeightSize = "33.1 MiB",
                Notes = "contains 2 YOLO layers"
            },
            new YoloCfg()
            {
                ShortName = "yolov3-tiny-prn.cfg",
                RelatedName = "yolov3-tiny-prn.cfg",
                Link = "2552",
                Notes = "partial residual network Enriching Variety of Layer-wise Learning Information by Gradient Combination"
            },
            new YoloCfg()
            {
                ShortName = "yolov4.cfg.cfg",
                RelatedName = "yolov4.cfg.cfg",
                Link = "2552",
                Notes = "https://arxiv.org/pdf/2004.10934.pdf"
            },
            new YoloCfg()
            {
                ShortName = "yolov4-custom.cfg",
                RelatedName = "yolov4.cfg",
                Link = "2552",
                Notes = "nearly identical; lower learning rate; one additional change to a convolutional layer"
            },
            new YoloCfg()
            {
                ShortName = "yolov4-tiny-3l.cfg",
                RelatedName = "yolov4-tiny.cfg",
                Link = "2552",
                Notes = "better at finding small objects; 3l refers to 3 YOLO layers vs the usual 2 in tiny"
            },
            new YoloCfg()
            {
                ShortName = "yolov4-tiny.cfg",
                RelatedName = "yolov4-tiny.cfg",
                Link = "2552",
                Notes = "https://github.com/AlexeyAB/darknet/issues/5346#issuecomment-649566598"
            },
            new YoloCfg()
            {
                ShortName = "yolov4-tiny-custom.cfg",
                RelatedName = "yolov4-tiny.cfg",
                Link = "https://github.com/AlexeyAB/darknet/issues/5346#issuecomment-649566598",
                Notes = "similar to yolov4-tiny.cfg, but contains 1 minor change to the first YOLO layer"
            }
        };
    }
}

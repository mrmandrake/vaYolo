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
                ShortName = "cd53paspp-gamma.cfg",
                RelatedName = "yolov4.cfg",
                Notes = "detector, the same as yolov4.cfg, but with leaky instead of mish",
            },
            new YoloCfg()
            {
                ShortName = "csdarknet53-omega.cfg",
                RelatedName = "yolov4.cfg",
                Notes = "classifier, backbone for yolov4.cfg"
            },
            new YoloCfg()
            {
                ShortName = "rnn.train.cfg",
                Link = "1624"
            },
            new YoloCfg()
            {
                ShortName = "rnn.train.cfg",
                RelatedName = "cspx-p7-mish.cfg",
                Notes = "classifier, backbone for cspx-p7-mish.cfg"
            },
            new YoloCfg()
            {
                ShortName = "cspx-p7-mish.cfg",
                RelatedName = "cspx-p7-mish.cfg",
                Notes = "detector, yolov4-p7-large"
            },
            new YoloCfg()
            {
                ShortName = "cspx-p7-mish_hp.cfg",
                Notes = "detector, experimental cfg file"
            },
            new YoloCfg()
            {
                ShortName = "csresnext50-panet-spp.cfg",
                Link = "2859"
            },
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
                ShortName = "yolov3-tiny_occlusion_track.cfg",
                RelatedName = "yolov3-tiny.cfg",
                WeightSize = "33.1 MiB",
                Link = "2552",
                Notes = "object Detection & Tracking using conv-rnn layer on frames from video"
            },
            new YoloCfg()
            {
                ShortName = "yolov3-tiny-prn.cfg",
                RelatedName = "yolov3-tiny.cfg",
                Link = "2552",
                Notes = "partial residual network Enriching Variety of Layer-wise Learning Information by Gradient Combination"
            },
            new YoloCfg()
            {
                ShortName = "yolov3-tiny_xnor.cfg",
                RelatedName = "yolov3-tiny.cfg",
                Link = "3054",
                Notes = "XNOR-net ~2x faster than cuDNN on CUDA"
            },
            new YoloCfg()
            {
                ShortName = "yolov3-voc.cfg",
                RelatedName = "yolov3.cfg",
                Notes = "similar to the usual yolov3.cfg but pre-configured for 20 VOC classes"
            }
        };
    }
}

// else if (short_name == "yolov4.cfg")
// {
// 	row.field[Fields::kNotes		] = "contains 3 YOLO layers";
// 	row.field[Fields::kWeightsSize	] = "245.7 MiB";
// 	row.field[Fields::kLink1		] = "YOLOv4 whitepaper";
// 	row.field[Fields::kUrl1			] = "https://arxiv.org/pdf/2004.10934.pdf";
// }
// else if (short_name == "yolov4-custom.cfg")
// {
// 	row.field[Fields::kRelated		] = "yolov4.cfg";
// 	row.field[Fields::kWeightsSize	] = "245.7 MiB";
// 	row.field[Fields::kLink1		] = "YOLOv4 whitepaper";
// 	row.field[Fields::kUrl1			] = "https://arxiv.org/pdf/2004.10934.pdf";
// 	row.field[Fields::kNotes		] = "nearly identical; lower learning rate; one additional change to a convolutional layer";
// }
// else if (short_name == "yolov4-tiny-3l.cfg")
// {
// 	row.field[Fields::kWeightsSize	] = "23.3 MiB";
// 	row.field[Fields::kRelated		] = "yolov4-tiny.cfg";
// 	row.field[Fields::kNotes		] = "better at finding small objects; \"3l\" refers to 3 YOLO layers vs the usual 2 in \"tiny\"";
// }
// else if (short_name == "yolov4-tiny.cfg")
// {
// 	row.field[Fields::kNotes		] = "contains 2 YOLO layers";
// 	row.field[Fields::kWeightsSize	] = "23.1 MiB";
// 	row.field[Fields::kLink1		] = "5346";
// 	row.field[Fields::kUrl1			] = "https://github.com/AlexeyAB/darknet/issues/5346#issuecomment-649566598";
// }
// else if (short_name == "yolov4-tiny-custom.cfg")
// {
// 	row.field[Fields::kNotes		] = "similar to yolov4-tiny.cfg, but contains 1 minor change to the first YOLO layer";
// 	row.field[Fields::kRelated		] = "yolov4-tiny.cfg";
// 	row.field[Fields::kWeightsSize	] = "23.1 MiB";
// 	row.field[Fields::kLink1		] = "5346";
// 	row.field[Fields::kUrl1			] = "https://github.com/AlexeyAB/darknet/issues/5346#issuecomment-649566598";
// }
// else if (short_name == "yolov4-tiny_contrastive.cfg")
// {
// 	row.field[Fields::kNotes		] = "\"experimental\"; \"suitable for un-supervised learning and for multi-camera object tracking\"";
// 	row.field[Fields::kRelated		] = "yolov4-tiny.cfg";
// 	row.field[Fields::kWeightsSize	] = "27.5 MiB";
// 	row.field[Fields::kLink1		] = "6892";
// 	row.field[Fields::kUrl1			] = "https://github.com/AlexeyAB/darknet/issues/6892";
// }
// else if (short_name == "yolov4-csp.cfg")
// {
// 	row.field[Fields::kNotes		] = "cross-stage-partial; more accurate and faster than YOLOv4";
// 	row.field[Fields::kRelated		] = "yolov4.cfg";
// 	row.field[Fields::kWeightsSize	] = "202.1 MiB";
// 	row.field[Fields::kLink1		] = "Scaled YOLO v4";
// 	row.field[Fields::kUrl1			] = "https://alexeyab84.medium.com/scaled-yolo-v4-is-the-best-neural-network-for-object-detection-on-ms-coco-dataset-39dfa22fa982";
// 	row.field[Fields::kLink2		] = "YOLOv4-CSP whitepaper";
// 	row.field[Fields::kUrl2			] = "https://arxiv.org/pdf/2011.08036.pdf";
// }
// else if (short_name == "yolov4x-mish.cfg")
// {
// 	row.field[Fields::kNotes		] = "detector; something between yolov4-csp and yolov4-p5; more suitable for high resolutions 640x640 - 832x832 than yolov4.cfg; should be trained longer";
// 	row.field[Fields::kRelated		] = "yolov4-csp.cfg";
// 	row.field[Fields::kWeightsSize	] = "380.9 MiB";
// 	row.field[Fields::kLink1		] = "7131";
// 	row.field[Fields::kUrl1			] = "https://github.com/AlexeyAB/darknet/issues/7131";
// }

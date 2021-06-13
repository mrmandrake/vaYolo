using System;
using System.Collections.Generic;
using vaYolo.Models;
namespace vaYolo
{
    public class YoloCfg
    {
        public string Name { get; set; }
        public string CfgName { get; set; }
        public string Pretrained { get; set; }

        public List<YoloCfg> Configs = new() {
            new YoloCfg()
            {
                Name = "yolo v3 5 layers",
                CfgName = "yolov3_5l.cfg",
                Pretrained = "darknet53.conv.74.weights"
            },
            new YoloCfg()
            {
                Name = "yolo v3",
                CfgName = "yolov3.cfg",
                Pretrained = "darknet53.conv.74.weights"
            },
            new YoloCfg()
            {
                Name = "yolo v3 spp",
                CfgName = "yolov3-spp.cfg",
                Pretrained = "darknet53.conv.74.weights"
            },
            new YoloCfg()
            {
                Name = "yolo v3 tiny 3 layers",
                CfgName = "yolov3-tiny_3l.cfg",
                Pretrained = "yolov3-tiny.conv.11.weights"
            },
            new YoloCfg()
            {
                Name = "yolo v3 tiny",
                CfgName = "yolov3-tiny.cfg",
                Pretrained = "yolov3-tiny.conv.11.weights"
            },
            new YoloCfg()
            {
                Name = "yolo v3 tiny prn",
                CfgName = "yolov3-tiny-prn.cfg",
                Pretrained = "yolov3-tiny.conv.11.weights"
            },
            new YoloCfg()
            {
                Name = "yolo v4",
                CfgName = "yolov4.cfg",
                Pretrained = "yolov4.conv.137.weights"
            },
            new YoloCfg()
            {
                Name = "yolo v4 custom",
                CfgName = "yolov4-custom.cfg",
                Pretrained = "yolov4.conv.137.weights"
            },
            new YoloCfg()
            {
                Name = "yolo v4 tiny 3 layers",
                CfgName = "yolov4-tiny-3l.cfg",
                Pretrained = "yolov4-tiny.conv.29.weights"
            },
            new YoloCfg()
            {
                Name = "yolo v4 tiny",
                CfgName = "yolov4-tiny.cfg",
                Pretrained = "yolov4-tiny.conv.29.weights"
            },
            new YoloCfg()
            {
                Name = "yolo v4 tiny custom",
                CfgName = "yolov4-tiny-custom.cfg",
                Pretrained = "yolov4-tiny.conv.29.weights"
            }
        };

        public static void Create(string name, VaSetup setup) {
        }
    }
}

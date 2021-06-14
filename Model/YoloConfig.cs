using System;
using System.Collections.Generic;
using System.IO;
using vaYolo.Models;
namespace vaYolo
{
    public class YoloCfg
    {
        public string Name { get; set; }
        public string CfgName { get; set; }
        public string Pretrained { get; set; }

        public List<YoloCfg> Configs = new()
        {
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

        private static string GetCfg(string name)
        {
            return string.Empty;
        }

        public static void Create(string name, VaSetup setup)
        {
            var lines = File.ReadAllLines(GetCfg(name));

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("batch="))
                    lines[i] = String.Format("batch={0}", setup.batch);

                if (lines[i].Contains("subdivision="))
                    lines[i] = String.Format("subdivision={0}", setup.subdivision);

                if (lines[i].Contains("width="))
                    lines[i] = String.Format("width={0}", setup.network_size_width);

                if (lines[i].Contains("height="))
                    lines[i] = String.Format("height={0}", setup.network_size_height);

                if (lines[i].Contains("max_batches="))
                    lines[i] = String.Format("max_batches={0}", setup.max_batches);

                if (lines[i].Contains("steps="))
                    lines[i] = String.Format("steps={0},{1}", setup.max_batches * 0.8, setup.max_batches * 0.9);
            }

            List<int> yolopos = new();
            for (int i = 0; i < lines.Length; i++)
                if (lines[i] == "[yolo]")
                    yolopos.Add(i);

            yolopos.ForEach((pos) =>
            {
                int cnt = 0;
                while (cnt++ < 10)
                    if (lines[cnt + pos].Contains("classes="))
                    {
                        lines[cnt + pos] = String.Format("classes={0}", setup.classes);
                        break;
                    }

                if (cnt == 10)
                    throw new Exception("Yolo -> classes not found!");
            });

            yolopos.ForEach((pos) =>
            {
                int cnt = 0;
                while (cnt++ < 10)
                    if (lines[cnt - pos].Contains("filters="))
                    {
                        lines[cnt - pos] = String.Format("filters={0}", (setup.classes + 5) * 3);
                        break;
                    }

                if (cnt == 10)
                    throw new Exception("convolutional-> filters before yolo not found!");
            });
        }
    }
}

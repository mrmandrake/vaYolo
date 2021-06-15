using System;
using System.Collections.Generic;
using System.IO;

namespace vaYolo.Model
{
    public class YoloCfg
    {
        private static Dictionary<string, YoloCfg> Configs = new() {
            {
                "yolov3 5 layers",
                new YoloCfg()
                {
                    CfgName = "yolov3_5l.cfg",
                    Pretrained = "darknet53.conv.74.weights"
                }
            },
            {
                "yolov3",
                new YoloCfg()
                {
                    CfgName = "yolov3.cfg",
                    Pretrained = "darknet53.conv.74.weights"
                }
            },
            {
                "yolov3 spp",
                new YoloCfg()
                {
                    CfgName = "yolov3-spp.cfg",
                    Pretrained = "darknet53.conv.74.weights"
                }
            },
            {
                "yolov3 tiny 3 layers",
                new YoloCfg()
                {
                    CfgName = "yolov3-tiny_3l.cfg",
                    Pretrained = "yolov3-tiny.conv.11.weights"
                }
            },
            {
                "yolov3 tiny",
                new YoloCfg()
                {
                    CfgName = "yolov3-tiny.cfg",
                    Pretrained = "yolov3-tiny.conv.11.weights"
                }
            },
            {
                "yolo v3 tiny prn",
                new YoloCfg()
                {
                    CfgName = "yolov3-tiny-prn.cfg",
                    Pretrained = "yolov3-tiny.conv.11.weights"
                }
            },
            {
                "yolo v4",
                new YoloCfg()
                {
                    CfgName = "yolov4.cfg",
                    Pretrained = "yolov4.conv.137.weights"
                }
            },
            {
                "yolo v4 custom",
                new YoloCfg()
                {
                    CfgName = "yolov4-custom.cfg",
                    Pretrained = "yolov4.conv.137.weights"
                }
            },
            {
                "yolo v4 tiny 3 layers",
                new YoloCfg()
                {
                    CfgName = "yolov4-tiny-3l.cfg",
                    Pretrained = "yolov4-tiny.conv.29.weights"
                }
            },
            {
                "yolo v4 tiny",
                new YoloCfg()
                {
                    CfgName = "yolov4-tiny.cfg",
                    Pretrained = "yolov4-tiny.conv.29.weights"
                }
            },
            {
                "yolo v4 tiny custom",
                new YoloCfg()
                {
                    CfgName = "yolov4-tiny-custom.cfg",
                    Pretrained = "yolov4-tiny.conv.29.weights"
                }
            }
        };

        public static YoloCfg FromTemplate(string templateName, VaSetup setup)
        {
            var cfg = Configs[templateName];
            cfg.Settings = setup;
            return cfg;
        }

        public string CfgName { get; set; }

        public string Pretrained { get; set; }

        public VaSetup Settings { get; set; }

        public string Save(string path)
        { 
            var lines = File.ReadAllLines(CfgName);

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("batch="))
                    lines[i] = String.Format("batch={0}", Settings.batch);

                if (lines[i].Contains("subdivision="))
                    lines[i] = String.Format("subdivision={0}", Settings.subdivision);

                if (lines[i].Contains("width="))
                    lines[i] = String.Format("width={0}", Settings.network_size_width);

                if (lines[i].Contains("height="))
                    lines[i] = String.Format("height={0}", Settings.network_size_height);

                if (lines[i].Contains("max_batches="))
                    lines[i] = String.Format("max_batches={0}", Settings.max_batches);

                if (lines[i].Contains("steps="))
                    lines[i] = String.Format("steps={0},{1}", Settings.max_batches * 0.8, Settings.max_batches * 0.9);
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
                        lines[cnt + pos] = String.Format("classes={0}", Settings.classes);
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
                        lines[cnt - pos] = String.Format("filters={0}", (Settings.classes + 5) * 3);
                        break;
                    }

                if (cnt == 10)
                    throw new Exception("convolutional-> filters before yolo not found!");
            });

            File.WriteAllLines(path, lines);
            return path;
        }
    }
}

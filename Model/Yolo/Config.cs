using System;
using System.Collections.Generic;
using System.IO;

namespace vaYolo.Model.Yolo
{
    public class Config
    {
        public static Dictionary<string, Config> Configs = new() {
            {
                "yolov3 5 layers",
                new Config()
                {
                    Name = "yolov3_5l.cfg",
                    PretrainedWeights = "darknet53.conv.74.weights"
                }
            },
            {
                "yolov3",
                new Config()
                {
                    Name = "yolov3.cfg",
                    PretrainedWeights = "darknet53.conv.74.weights"
                }
            },
            {
                "yolov3 spp",
                new Config()
                {
                    Name = "yolov3-spp.cfg",
                    PretrainedWeights = "darknet53.conv.74.weights"
                }
            },
            {
                "yolov3 tiny 3 layers",
                new Config()
                {
                    Name = "yolov3-tiny_3l.cfg",
                    PretrainedWeights = "yolov3-tiny.conv.11.weights"
                }
            },
            {
                "yolov3 tiny",
                new Config()
                {
                    Name = "yolov3-tiny.cfg",
                    PretrainedWeights = "yolov3-tiny.conv.11.weights"
                }
            },
            {
                "yolo v3 tiny prn",
                new Config()
                {
                    Name = "yolov3-tiny-prn.cfg",
                    PretrainedWeights = "yolov3-tiny.conv.11.weights"
                }
            },
            {
                "yolo v4",
                new Config()
                {
                    Name = "yolov4.cfg",
                    PretrainedWeights = "yolov4.conv.137.weights"
                }
            },
            {
                "yolo v4 custom",
                new Config()
                {
                    Name = "yolov4-custom.cfg",
                    PretrainedWeights = "yolov4.conv.137.weights"
                }
            },
            {
                "yolo v4 tiny 3 layers",
                new Config()
                {
                    Name = "yolov4-tiny-3l.cfg",
                    PretrainedWeights = "yolov4-tiny.conv.29.weights"
                }
            },
            {
                "yolo v4 tiny",
                new Config()
                {
                    Name = "yolov4-tiny.cfg",
                    PretrainedWeights = "yolov4-tiny.conv.29.weights"
                }
            },
            {
                "yolo v4 tiny custom",
                new Config()
                {
                    Name = "yolov4-tiny-custom.cfg",
                    PretrainedWeights = "yolov4-tiny.conv.29.weights"
                }
            }
        };

        public static Config FromTemplate(string templateName, AlgoSettings setup)
        {
            var cfg = Configs[templateName];
            cfg.Settings = setup;
            return cfg;
        }

        public string Name { get; set; }

        public string PretrainedWeights { get; set; }

        public AlgoSettings Settings { get; set; }

        public string Save(string path)
        { 
            var lines = File.ReadAllLines(Name);

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
                        lines[cnt - pos] = String.Format("filters={0}", Settings.filters);
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

using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace vaYolo.Views
{
    public partial class Setup : Window
    {
        public Setup()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public static void Show(Window parent)
        {
            var dlg = new Setup();
            if (parent != null)
                dlg.ShowDialog(parent);
            else 
                dlg.Show();
        }

        // public void Save() {
        //     string darknetDir;
        //     string cfgTemplate;
        //     int image_width;
        //     int image_height;
        //     int batch_size;
        //     int subdivisions;

        //     if (!File.Exists(darknetDir)) {
        //     }

        //     if (!File.Exists(cfgTemplate)) {
        //     }

        //     if (image_width % 32) {
        //         // width multiple 32
        //     }

        //     if (image_height % 32) {
        //         // width multiple 32
        //     }

        //     if (subdivisions > batch_size) {
        //         // subdivision must be less batchsize

        //     }

        //     if (batch_size % subdivisions) {
        //         // batch size must be multiple subdiv
        //     }

        //     Apply();
        // }

        // public void Apply() {
        //     cfg().setValue(/* this one is a global value */ "darknet_dir"		, v_darknet_dir					);
        //     cfg().setValue(content.cfg_prefix + "darknet_cfg_template"			, v_cfg_template				);
        //     cfg().setValue(content.cfg_prefix + "darknet_train_with_all_images"	, v_train_with_all_images		);
        //     cfg().setValue(content.cfg_prefix + "darknet_training_percentage"	, v_training_images_percentage	);
        //     cfg().setValue(content.cfg_prefix + "darknet_image_width"			, v_image_width					);
        //     cfg().setValue(content.cfg_prefix + "darknet_image_height"			, v_image_height				);
        //     cfg().setValue(content.cfg_prefix + "darknet_batch_size"			, v_batch_size					);
        //     cfg().setValue(content.cfg_prefix + "darknet_subdivisions"			, v_subdivisions				);
        //     cfg().setValue(content.cfg_prefix + "darknet_iterations"			, v_iterations					);
        //     cfg().setValue(content.cfg_prefix + "darknet_learning_rate"			, v_learning_rate				);
        //     cfg().setValue(content.cfg_prefix + "darknet_max_chart_loss"		, v_max_chart_loss				);
        //     cfg().setValue(content.cfg_prefix + "darknet_do_not_resize_images"	, v_do_not_resize_images		);
        //     cfg().setValue(content.cfg_prefix + "darknet_resize_images"			, v_resize_images				);
        //     cfg().setValue(content.cfg_prefix + "darknet_tile_images"			, v_tile_images					);
        //     cfg().setValue(content.cfg_prefix + "darknet_zoom_images"			, v_zoom_images					);
        //     cfg().setValue(content.cfg_prefix + "darknet_recalculate_anchors"	, v_recalculate_anchors			);
        //     cfg().setValue(content.cfg_prefix + "darknet_anchor_clusters"		, v_anchor_clusters				);
        //     cfg().setValue(content.cfg_prefix + "darknet_class_imbalance"		, v_class_imbalance				);
        //     cfg().setValue(content.cfg_prefix + "darknet_restart_training"		, v_restart_training			);
        //     cfg().setValue(content.cfg_prefix + "darknet_delete_temp_weights"	, v_delete_temp_weights			);
        //     cfg().setValue(content.cfg_prefix + "darknet_saturation"			, v_saturation					);
        //     cfg().setValue(content.cfg_prefix + "darknet_exposure"				, v_exposure					);
        //     cfg().setValue(content.cfg_prefix + "darknet_hue"					, v_hue							);
        //     cfg().setValue(content.cfg_prefix + "darknet_enable_flip"			, v_enable_flip					);
        //     cfg().setValue(content.cfg_prefix + "darknet_angle"					, v_angle						);
        //     cfg().setValue(content.cfg_prefix + "darknet_mosaic"				, v_mosaic						);
        //     cfg().setValue(content.cfg_prefix + "darknet_cutmix"				, v_cutmix						);
        //     cfg().setValue(content.cfg_prefix + "darknet_mixup"					, v_mixup						);            

        // }
    }
}




// void dm::DarknetWnd::create_Darknet_configuration_file()
// {
// 	const size_t number_of_classes		= content.names.size() - 1;
// 	const bool enable_mosaic			= info.enable_mosaic;
// 	const bool enable_cutmix			= info.enable_cutmix;
// 	const bool enable_mixup				= info.enable_mixup;
// 	const bool enable_flip				= info.enable_flip;
// 	const float learning_rate			= info.learning_rate;
// 	const float max_chart_loss			= info.max_chart_loss;
// 	const float saturation				= info.saturation;
// 	const float exposure				= info.exposure;
// 	const float hue						= info.hue;
// 	const int angle						= info.angle;
// 	const size_t number_of_iterations	= info.iterations;
// 	const size_t step1					= std::round(0.8 * number_of_iterations);
// 	const size_t step2					= std::round(0.9 * number_of_iterations);
// 	const size_t batch					= info.batch_size;
// 	const size_t subdivisions			= info.subdivisions;
// //	const size_t filters				= number_of_classes * 3 + 15;
// 	const size_t width					= info.image_width;
// 	const size_t height					= info.image_height;
// 	const bool recalculate_anchors		= info.recalculate_anchors;
// 	const size_t anchor_clusters		= info.anchor_clusters;
// 	const bool class_imbalance			= info.class_imbalance;

// 	MStr m =
// 	{
// 		{"use_cuda_graph"	, "0"													},
// 		{"flip"				, enable_flip	? "1" : "0"								},
// 		{"mosaic"			, enable_mosaic	? "1" : "0"								},
// 		{"cutmix"			, enable_cutmix	? "1" : "0"								},
// 		{"mixup"			, enable_mixup	? "1" : "0"								},
// 		{"learning_rate"	, std::to_string(learning_rate)							},
// 		{"max_chart_loss"	, std::to_string(max_chart_loss)						},
// 		{"hue"				, std::to_string(hue)									},
// 		{"saturation"		, std::to_string(saturation)							},
// 		{"exposure"			, std::to_string(exposure)								},
// 		{"max_batches"		, std::to_string(number_of_iterations)					},
// 		{"steps"			, std::to_string(step1) + "," + std::to_string(step2)	},
// 		{"batch"			, std::to_string(batch)									},
// 		{"subdivisions"		, std::to_string(subdivisions)							},
// 		{"height"			, std::to_string(height)								},
// 		{"width"			, std::to_string(width)									},
// 		{"angle"			, std::to_string(angle)									}
// 	};

// 	if (v_show_receptive_field.getValue().operator bool())
// 	{
// 		m["show_receptive_field"] = "1";
// 	}

// 	cfg_handler.modify_all_sections("[net]", m);

// 	m.clear();
// 	if (recalculate_anchors)
// 	{
// 		std::string new_anchors;
// 		std::string counters_per_class;
// 		float avg_iou = 0.0f;

// 		calc_anchors(info.train_filename, anchor_clusters, info.image_width, info.image_height, number_of_classes, new_anchors, counters_per_class, avg_iou);
// 		if (avg_iou > 0.0f)
// 		{
// 			dm::Log("avg IoU: " + std::to_string(avg_iou));
// 			dm::Log("new anchors: " + new_anchors);
// 			dm::Log("new counters: " + counters_per_class);

// 			m["anchors"] = new_anchors;

// 			if (class_imbalance)
// 			{
// 				m["counters_per_class"] = counters_per_class;
// 			}

// 		}
// 	}

// 	m["classes"] = std::to_string(number_of_classes);

// 	cfg_handler.modify_all_sections("[yolo]", m);
// 	cfg_handler.fix_filters_before_yolo();
// 	cfg_handler.output(info);

// 	return;
// }


// void dm::DarknetWnd::create_Darknet_training_and_validation_files(
// 		ThreadWithProgressWindow & progress_window,
// 		size_t & number_of_files_train		,
// 		size_t & number_of_files_valid		,
// 		size_t & number_of_skipped_files	,
// 		size_t & number_of_marks			,
// 		size_t & number_of_resized_images	,
// 		size_t & number_of_tiles_created	,
// 		size_t & number_of_zooms_created	)
// {
// 	if (true)
// 	{
// 		std::ofstream fs(info.data_filename);
// 		fs	<< "classes = "	<< content.names.size() - 1						<< std::endl
// 			<< "train = "	<< info.train_filename							<< std::endl
// 			<< "valid = "	<< info.valid_filename							<< std::endl
// 			<< "names = "	<< cfg().get_str(content.cfg_prefix + "names")	<< std::endl
// 			<< "backup = "	<< info.project_dir								<< std::endl;
// 	}

// 	number_of_files_train		= 0;
// 	number_of_files_valid		= 0;
// 	number_of_skipped_files		= 0;
// 	number_of_marks				= 0;
// 	number_of_resized_images	= 0;
// 	number_of_tiles_created		= 0;
// 	number_of_zooms_created		= 0;

// 	File dir = File(info.project_dir).getChildFile("darkmark_image_cache");
// 	dir.deleteRecursively();

// 	// these vectors will have the full path of the images we need to use (or which have been skipped)
// 	VStr annotated_images;
// 	VStr skipped_images;
// 	VStr all_output_images;
// 	find_all_annotated_images(progress_window, annotated_images, skipped_images, number_of_marks);
// 	number_of_skipped_files = skipped_images.size();

// 	Log("total number of annotated input images: " + std::to_string(annotated_images.size()));
// 	Log("total number of skipped input images: " + std::to_string(skipped_images.size()));

// 	if (info.do_not_resize_images)
// 	{
// 		Log("not resizing any images");
// 		all_output_images = annotated_images;
// 	}
// 	if (info.resize_images)
// 	{
// 		Log("resizing all images");
// 		resize_images(progress_window, annotated_images, all_output_images, number_of_resized_images);
// 		Log("number of images resized: " + std::to_string(number_of_resized_images));
// 	}
// 	if (info.tile_images)
// 	{
// 		Log("tiling all images");
// 		tile_images(progress_window, annotated_images, all_output_images, number_of_marks, number_of_tiles_created);
// 		Log("number of tiles created: " + std::to_string(number_of_tiles_created));
// 	}
// 	if (info.zoom_images)
// 	{
// 		Log("crop+zoom all images");
// 		random_zoom_images(progress_window, annotated_images, all_output_images, number_of_marks, number_of_zooms_created);
// 		Log("number of crop+zoom images created: " + std::to_string(number_of_zooms_created));
// 	}

// 	// now that we know the exact set of images (including resized and tiled images)
// 	// we can create the training and validation .txt files

// 	double work_done = 0.0;
// 	double work_to_do = all_output_images.size() + 1.0;
// 	progress_window.setProgress(0.0);
// 	progress_window.setStatusMessage("Writing training and validation files...");
// 	Log("total number of output images: " + std::to_string(all_output_images.size()));

// 	std::random_shuffle(all_output_images.begin(), all_output_images.end());
// 	const bool use_all_images = info.train_with_all_images;
// 	number_of_files_train = std::round(info.training_images_percentage * all_output_images.size());
// 	number_of_files_valid = all_output_images.size() - number_of_files_train;

// 	if (use_all_images)
// 	{
// 		number_of_files_train = all_output_images.size();
// 		number_of_files_valid = all_output_images.size();
// 	}

// 	Log("total number of training images: " + std::to_string(number_of_files_train) + " (" + info.train_filename + ")");
// 	Log("total number of validation images: " + std::to_string(number_of_files_valid) + " (" + info.valid_filename + ")");

// 	std::ofstream fs_train(info.train_filename);
// 	std::ofstream fs_valid(info.valid_filename);

// 	for (size_t idx = 0; idx < all_output_images.size(); idx ++)
// 	{
// 		work_done ++;
// 		progress_window.setProgress(work_done / work_to_do);

// 		if (use_all_images or idx < number_of_files_train)
// 		{
// 			fs_train << all_output_images[idx] << std::endl;
// 		}

// 		if (use_all_images or idx >= number_of_files_train)
// 		{
// 			fs_valid << all_output_images[idx] << std::endl;
// 		}
// 	}

// 	Log("training and validation files have been saved to disk");

// 	return;
// }


// void dm::DarknetWnd::create_Darknet_shell_scripts()
// {
// 	std::string header;

// 	if (true)
// 	{
// 		std::stringstream ss;
// 		ss	<< "#!/bin/bash -e"				<< std::endl
// 			<< ""							<< std::endl
// 			<< "cd " << info.project_dir	<< std::endl
// 			<< ""							<< std::endl
// 			<< "# Warning: this file is automatically created/updated by DarkMark v" << DARKMARK_VERSION << "!" << std::endl
// 			<< "# Created on " << Time::getCurrentTime().formatted("%a %Y-%m-%d %H:%M:%S %Z").toStdString()
// 			<< " by " << SystemStats::getLogonName().toStdString()
// 			<< "@" << SystemStats::getComputerName().toStdString() << "." << std::endl;
// 		header = ss.str();
// 	}

// 	if (true)
// 	{
// 		std::string cmd = info.darknet_dir + "/darknet detector -map" + (v_keep_augmented_images.getValue() ? " -show_imgs" : "") + " -dont_show train " + info.data_filename + " " + info.cfg_filename;
// 		if (info.restart_training)
// 		{
// 			cmd += " " + cfg().get_str(content.cfg_prefix + "weights");
// 			cmd += " -clear";
// 		}

// 		std::stringstream ss;
// 		ss	<< header
// 			<< ""												<< std::endl
// 			<< "rm -f output.log"								<< std::endl
// 			<< "rm -f chart.png"								<< std::endl
// 			<< ""												<< std::endl
// 			<< "echo \"creating new log file\" > output.log"	<< std::endl
// 			<< "date >> output.log"								<< std::endl
// 			<< ""												<< std::endl
// 			<< "ts1=$(date)"									<< std::endl
// 			<< "ts2=$(date +%s)"								<< std::endl
// 			<< "echo \"initial ts1: ${ts1}\" >> output.log"		<< std::endl
// 			<< "echo \"initial ts2: ${ts2}\" >> output.log"		<< std::endl
// 			<< "echo \"cmd: " << cmd << "\" >> output.log"		<< std::endl
// 			<< ""												<< std::endl
// 			<< "/usr/bin/time --verbose " << cmd << " 2>&1 | tee --append output.log" << std::endl
// 			<< ""												<< std::endl
// 			<< "ts3=$(date)"									<< std::endl
// 			<< "ts4=$(date +%s)"								<< std::endl
// 			<< "echo \"ts1: ${ts1}\" >> output.log"				<< std::endl
// 			<< "echo \"ts2: ${ts2}\" >> output.log"				<< std::endl
// 			<< "echo \"ts3: ${ts3}\" >> output.log"				<< std::endl
// 			<< "echo \"ts4: ${ts4}\" >> output.log"				<< std::endl
// 			<< ""												<< std::endl;

// 		if (info.delete_temp_weights)
// 		{
// 			ss	<< "find " << info.project_dir << " -maxdepth 1 -regex \".+_[0-9]+\\.weights\" -print -delete >> output.log" << std::endl
// 				<< "" << std::endl;
// 		}

// 		const std::string data = ss.str();
// 		File f(info.command_filename);
// 		f.replaceWithData(data.c_str(), data.size());	// do not use replaceWithText() since it converts the file to CRLF endings which confuses bash
// 		f.setExecutePermission(true);
// 	}

// 	if (true)
// 	{
// 		std::stringstream ss;
// 		ss	<< header
// 			<< "#"																								<< std::endl
// 			<< "# This script assumes you have 2 computers:"													<< std::endl
// 			<< "#"																								<< std::endl
// 			<< "# - the first is the desktop where you run DarkMark and this script,"							<< std::endl
// 			<< "# - the second has a decent GPU and is where you train the neural network."						<< std::endl
// 			<< "#"																								<< std::endl
// 			<< "# It also assumes the directory structure for where neural networks are saved"					<< std::endl
// 			<< "# on disk is identical between both computers."													<< std::endl
// 			<< "#"																								<< std::endl
// 			<< "# Running this script *FROM THE DESKTOP COMPUTER* will retrieve the results"					<< std::endl
// 			<< "# (the .weights files) from 'gpurig' where training took place."								<< std::endl
// 			<< ""																								<< std::endl
// 			<< "ping -c 1 -W 1 gpurig >/dev/null 2>&1"															<< std::endl
// 			<< "if [ $? -ne 0 ]; then"																			<< std::endl
// 			<< "	echo \"Make sure the hostname 'gpurig' can be resolved or exists in the /etc/hosts file!\""	<< std::endl
// 			<< "else"																							<< std::endl
// 			<< "#	rm -f " << info.project_name << "*.weights"													<< std::endl
// 			<< "#	rm -f output.log"																			<< std::endl
// 			<< "	rm -f chart.png"																			<< std::endl
// 			<< ""																								<< std::endl
// 			<< "	rsync --progress --times --compress gpurig:" << info.project_dir << "/\\* ."				<< std::endl
// 			<< ""																								<< std::endl;

// 			if (info.delete_temp_weights)
// 			{
// 				ss	<< "	find " << info.project_dir << " -maxdepth 1 -regex \".+_[0-9]+\\.weights\" -print -delete" << std::endl
// 					<< "" << std::endl;
// 			}

// 		ss	<< "	if [ -e chart.png ]; then"																	<< std::endl
// 			<< "		eog chart.png"																			<< std::endl
// 			<< "	fi"																							<< std::endl
// 			<< "fi"																								<< std::endl
// 			<< ""																								<< std::endl;

// 		const std::string data = ss.str();
// 		File f = File(info.project_dir).getChildFile("get_results_from_gpu_rig.sh");
// 		f.replaceWithData(data.c_str(), data.size());
// 		f.setExecutePermission(true);
// 	}

// 	if (true)
// 	{
// 		std::stringstream ss;
// 		ss	<< header
// 			<< "#"																								<< std::endl
// 			<< "# This script assumes you have 2 computers:"													<< std::endl
// 			<< "#"																								<< std::endl
// 			<< "# - the first is the desktop where you run DarkMark and this script,"							<< std::endl
// 			<< "# - the second has a decent GPU and is where you train the neural network."						<< std::endl
// 			<< "#"																								<< std::endl
// 			<< "# It also assumes the directory structure for where neural networks are saved"					<< std::endl
// 			<< "# on disk is identical between both computers."													<< std::endl
// 			<< "#"																								<< std::endl
// 			<< "# Running this script *FROM THE DESKTOP COMPUTER* will copy all of the"							<< std::endl
// 			<< "# necessary files (images, .txt, .names, .cfg, etc) from the desktop computer"					<< std::endl
// 			<< "# to the rig with the decent GPU so you can then start the training process."					<< std::endl
// 			<< "#"																								<< std::endl
// 			<< "# After this script has finished running, ssh to the GPU rig and run this to train:"			<< std::endl
// 			<< "#"																								<< std::endl
// 			<< "#		" << info.command_filename																<< std::endl
// 			<< "#"																								<< std::endl
// 			<< ""																								<< std::endl
// 			<< "ping -c 1 -W 1 gpurig >/dev/null 2>&1"															<< std::endl
// 			<< "if [ $? -ne 0 ]; then"																			<< std::endl
// 			<< "	echo \"Make sure the hostname 'gpurig' can be resolved or exists in the /etc/hosts file!\""	<< std::endl
// 			<< "else"																							<< std::endl
// 			<< "	rsync --recursive --progress --times --compress . gpurig:" << info.project_dir				<< std::endl
// 			<< "fi"																								<< std::endl
// 			<< ""																								<< std::endl;
// 		const std::string data = ss.str();
// 		File f = File(info.project_dir).getChildFile("send_files_to_gpu_rig.sh");
// 		f.replaceWithData(data.c_str(), data.size());
// 		f.setExecutePermission(true);
// 	}

// 	return;
// }

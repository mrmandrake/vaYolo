    Very Anonymous Yolo labeling Toolkit in the Covid Era (Optimised 4 Macos + IPad + Pencil

![vayolo](https://user-images.githubusercontent.com/50768285/121197227-6d90d500-c871-11eb-841e-7bdf9898b320.png)

with vaYolo you can label any dataset at Light speed!

Best configuration: **MACBOOK  <------> IPAD + PENCIL** with useful ASTROPAD Studio

**What??**

It will create .txt-file for each .jpg-image-file in the same directory and with the same name, but with .txt-extension

and put to file: object number and object coordinates on this image, for each object in new line:

<object-class> <x_center> <y_center> <width> <height>

Where:

<object-class> - integer object number from 0 to (classes-1)
<x_center> <y_center> <width> <height> - float values relative to width and height of image, it can be equal from (0.0 to 1.0]

for example: <x> = <absolute_x> / <image_width> or <height> = <absolute_height> / <image_height>
attention: <x_center> <y_center> - are center of rectangle (are not top-left corner)

For example for img1.jpg you will be created img1.txt containing:

1 0.716797 0.395833 0.216406 0.147222
0 0.687109 0.379167 0.255469 0.158333
1 0.420312 0.395833 0.140625 0.166667

Important Note on Normal Image Markup (from really useful DarkMark): 
- Being consistent is very important when marking up images. When possible, you'll want to make certain that all instances of a particular class are all marked the same way.
Non-Square Markup: 

- If the object is not "square", or viewed from an angle, then make sure the markup includes all parts of the object visible in the image. Again, no more, and no less.
Multiple Object Markup: 

- When multiple instances of an object appears in an image, you must ensure that every single one is properly marked. Otherwise, training will be negatively impacted. If you want your network to recognize cars, and you fail to mark up several cars in an image, then the training will incorrectly think it has learned the wrong thing when it identifies those cars during training/validation.

Overlapping Object Markup: 

- Having overlapping objects is perfectly acceptable

Variation: 
- You can never have too many images when training a neural network. But, there is a diminishing rate of return after you reach a certain stage. If you have thousands of nearly identical images, by the time you mark your 1000th image, the neural network has probably already learned what the class looks like.

- If instead you have 5 big variations and your image set contains 200 images of each one, the neural network trained on those 1000 images will probably be much better than the one from the previous example with 1000 nearly identical images.

- The MAP (mean average precision) and the LOSS charts can somewhat help with that during training, letting you know if the neural network training is successful. But if things aren't working as expected, it wont tell you why, which images, or which classes are causing a problem.

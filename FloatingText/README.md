# Floating text

This script (Singleton) allow you to integrate a floating text in screen space using object pool to maximize performance.

![alt text](./img/img_floating_text.JPG)

# Requirement

- Text mesh pro

# Usage

![alt text](./img/img_floating_text_components.JPG)

  1. Put the script on your main canvas
  2. Setup component with desired values
  3. Call the method 'CreateFloatingText' using the instance of this class

eg : FloatingText.Instance.CreateFloatingText(worldPosition, text, presetName);

If you want to add 'Outline', 'Glow' ... you can edit the materiel in the 'TMP_Font Asset' from your content.

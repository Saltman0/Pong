shader_type canvas_item;

uniform int mode : hint_range(0, 3) = 3; // 0: Normal, 1: Protanopia, 2: Deuteranopia, 3: Tritanopia
uniform sampler2D SCREEN_TEXTURE : hint_screen_texture, filter_linear_mipmap;

void fragment() {
    vec4 color = texture(SCREEN_TEXTURE, SCREEN_UV);
    float r = color.r;
    float g = color.g;
    float b = color.b;

    if (mode == 1) { // Protanopia
        color.r = 0.567 * r + 0.433 * g;
        color.g = 0.558 * r + 0.442 * g;
        color.b = 0.242 * g + 0.758 * b;
    } else if (mode == 2) { // Deuteranopia
        color.r = 0.625 * r + 0.375 * g;
        color.g = 0.700 * r + 0.300 * g;
        color.b = 0.300 * g + 0.700 * b;
    } else if (mode == 3) { // Tritanopia
        color.r = 0.950 * r + 0.050 * g;
        color.g = 0.433 * r + 0.567 * g;
        color.b = 0.475 * g + 0.525 * b;
    }

    COLOR = color;
}
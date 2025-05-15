#version 330 core                      

layout(location = 0) in vec3 position; // pozice bude na lokaci 0
layout(location = 1) in vec3 normal;
layout(location = 2) in vec2 texCoord;


uniform mat4 model;                    // konstanty predane z programu 
uniform mat4 view;
uniform mat4 projection;

out vec3 normalWorld;
out vec3 fragmentWorld;
out vec2 texCoordVS;

void main() {
    gl_Position = projection * view * model * vec4(position, 1.0);
    fragmentWorld = vec3(model*vec4(position, 1.0));
    normalWorld = mat3(transpose(inverse(model)))*normal;
    texCoordVS = texCoord;
}

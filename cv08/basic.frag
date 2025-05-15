#version 330 core

struct Material {
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
    float shininess;
};
uniform Material material;

uniform vec3 cameraPosWorld;

uniform vec4 lightPosWorld;
uniform vec3 lightColor;
uniform float lightIntensity;


uniform sampler2D dayTex;
uniform sampler2D nightTex; 

in vec3 normalWorld;
in vec3 fragmentWorld;
in vec2 texCoordVS;

out vec4 outColor;

void main() {
    vec3 ambient = material.diffuse * lightColor * 0.1;

    vec3 norm = normalize(normalWorld);
    
    vec3 lightDir = lightPosWorld.w == 0.0
        ? normalize(-lightPosWorld.xyz)
        : normalize(lightPosWorld.xyz - fragmentWorld);


    vec3 colorDay = texture(dayTex, texCoordVS).xyz;
    vec3 colorNight = texture(nightTex, texCoordVS).xyz;

    float dayNightCoeff = dot(norm, lightDir);


    vec3 diffuse = colorDay * dayNightCoeff * lightColor * lightIntensity + (1.0f - dayNightCoeff) * colorNight;

    

    vec3 reflectDir = reflect(-lightDir.xyz, norm);
    vec3 viewDir = normalize(cameraPosWorld - fragmentWorld);

    vec3 specular = material.specular * max(0, pow(dot(reflectDir, viewDir) , material.shininess)) * lightColor * lightIntensity;

    vec3 finalColor = ambient + diffuse + specular;

    outColor = vec4(finalColor, 1.0);
}
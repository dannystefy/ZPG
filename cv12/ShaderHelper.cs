using OpenTK.Graphics.OpenGL4;

/// <summary>
/// Třída starající se o statické shadery
/// </summary>
public static class ShaderHelper
{
    /// <summary>
    /// Vytvoření primitivního shaderu
    /// </summary>
    /// <returns>ID shader programu</returns>
    public static int CreateSimpleShader()
    {
        string vertexShaderSource = """
            #version 410 core
            layout(location = 0) in vec2 aPosition;

            void main()
            {
                gl_Position = vec4(aPosition.x / 400.0 - 1.0, -(aPosition.y / 300.0 - 1.0), 0.0, 1.0);
            }
            """;

        string fragmentShaderSource = """
            #version 410 core
            out vec4 FragColor;

            void main()
            {
                FragColor = vec4(1.0, 1.0, 1.0, 1.0);
            }
            """;

        int vertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexShader, vertexShaderSource);
        GL.CompileShader(vertexShader);

        int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentShader, fragmentShaderSource);
        GL.CompileShader(fragmentShader);

        int shaderProgram = GL.CreateProgram();
        GL.AttachShader(shaderProgram, vertexShader);
        GL.AttachShader(shaderProgram, fragmentShader);
        GL.LinkProgram(shaderProgram);

        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragmentShader);

        return shaderProgram;
    }

}


using cv12;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

/// <summary>
/// Třída reprezentující čáru
/// </summary>
public class Line : AbstractLine
{
    /// <summary>
    /// poctaceni a koncovy bod
    /// </summary>
    protected Vector2 Start;
    protected Vector2 End;


    public Line(Vector2 start, Vector2 end, int segments)
    {
        Start = start;
        End = end;
        Segments = segments;

        Initialize();
    }



    protected override void GeneratePoints(List<Vector2> output)
    {
        output.Clear();
        for (int i = 0; i <= Segments; i++)
        {
            float t = i / (float)Segments;
            Vector2 point = Vector2.Lerp(Start, End, t);
            output.Add(point);
        }
    }





    /// <summary>
    /// Ziskani seznamu rididich bodu
    /// </summary>
    /// <returns></returns>
    public override List<Vector2> GetControlPoints()
    {
        return new List<Vector2> { Start, End };
    }

    /// <summary>
    /// Nastaveni ridiciho bodu a prepocitani cary
    /// </summary>
    /// <param name="index"></param>
    /// <param name="position"></param>
    public virtual void SetControlPoint(int index, Vector2 position)
    {
        if (index == 0) Start = position;
        else if (index == 1) End = position;
        UpdateBuffer();
    }
}


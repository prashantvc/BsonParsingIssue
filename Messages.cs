using System.Reflection;
using System.Xml;
public enum PixelFormat
{
    Rgb565,
    Rgba8888,
    Bgra8888,
    MaxValue = Bgra8888
}

[AvaloniaRemoteMessageGuid("6E3C5310-E2B1-4C3D-8688-01183AA48C5B")]
public class MeasureViewportMessage
{
    public double Width { get; set; }
    public double Height { get; set; }
}

[AvaloniaRemoteMessageGuid("BD7A8DE6-3DB8-4A13-8583-D6D4AB189A31")]
public class ClientViewportAllocatedMessage
{
    public double Width { get; set; }
    public double Height { get; set; }
    public double DpiX { get; set; }
    public double DpiY { get; set; }
}

[AvaloniaRemoteMessageGuid("9B47B3D8-61DF-4C38-ACD4-8C1BB72554AC")]
public class RequestViewportResizeMessage
{
    public double Width { get; set; }
    public double Height { get; set; }
}

[AvaloniaRemoteMessageGuid("63481025-7016-43FE-BADC-F2FD0F88609E")]
public class ClientSupportedPixelFormatsMessage
{
    public PixelFormat[] Formats { get; set; }
}

[AvaloniaRemoteMessageGuid("7A3C25D3-3652-438D-8EF1-86E942CC96C0")]
public class ClientRenderInfoMessage
{
    public double DpiX { get; set; }
    public double DpiY { get; set; }

    public override string ToString()
    {
        return $"DpiX: {DpiX}, DpiY: {DpiY}";
    }
}

[AvaloniaRemoteMessageGuid("68014F8A-289D-4851-8D34-5367EDA7F827")]
public class FrameReceivedMessage
{
    public long SequenceId { get; set; }
}


[AvaloniaRemoteMessageGuid("F58313EE-FE69-4536-819D-F52EDF201A0E")]
public class FrameMessage
{
    public long SequenceId { get; set; }
    public PixelFormat Format { get; set; }
    public byte[] Data { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public int Stride { get; set; }
    public double DpiX { get; set; }
    public double DpiY { get; set; }
}


/// <summary>
/// Keep this in sync with InputModifiers in the main library
/// </summary>
[Flags]
public enum InputModifiers
{
    Alt,
    Control,
    Shift,
    Windows,
    LeftMouseButton,
    RightMouseButton,
    MiddleMouseButton
}

/// <summary>
/// Keep this in sync with InputModifiers in the main library
/// </summary>
public enum MouseButton
{
    None,
    Left,
    Right,
    Middle
}

public abstract class InputEventMessageBase
{
    public InputModifiers[] Modifiers { get; set; }
}

public abstract class PointerEventMessageBase : InputEventMessageBase
{
    public double X { get; set; }
    public double Y { get; set; }
}

[AvaloniaRemoteMessageGuid("6228F0B9-99F2-4F62-A621-414DA2881648")]
public class PointerMovedEventMessage : PointerEventMessageBase
{

}

[AvaloniaRemoteMessageGuid("7E9E2818-F93F-411A-800E-6B1AEB11DA46")]
public class PointerPressedEventMessage : PointerEventMessageBase
{
    public MouseButton Button { get; set; }
}

[AvaloniaRemoteMessageGuid("4ADC84EE-E7C8-4BCF-986C-DE3A2F78EDE4")]
public class PointerReleasedEventMessage : PointerEventMessageBase
{
    public MouseButton Button { get; set; }
}

[AvaloniaRemoteMessageGuid("79301A05-F02D-4B90-BB39-472563B504AE")]
public class ScrollEventMessage : PointerEventMessageBase
{
    public double DeltaX { get; set; }
    public double DeltaY { get; set; }
}




[AvaloniaRemoteMessageGuid("9AEC9A2E-6315-4066-B4BA-E9A9EFD0F8CC")]
public class UpdateXamlMessage
{
    public string Xaml { get; set; }
    public string AssemblyPath { get; set; }
    public string XamlFileProjectPath { get; set; }
}

[AvaloniaRemoteMessageGuid("B7A70093-0C5D-47FD-9261-22086D43A2E2")]
public class UpdateXamlResultMessage
{
    public string Error { get; set; }
    public string Handle { get; set; }
    public ExceptionDetails Exception { get; set; }
}

[AvaloniaRemoteMessageGuid("854887CF-2694-4EB6-B499-7461B6FB96C7")]
public class StartDesignerSessionMessage
{
    public string SessionId { get; set; }
}

public class ExceptionDetails
{
    public ExceptionDetails()
    {
    }

    public ExceptionDetails(Exception e)
    {
        if (e is TargetInvocationException)
        {
            e = e.InnerException;
        }

        ExceptionType = e.GetType().Name;
        Message = e.Message;

        if (e is XmlException xml)
        {
            LineNumber = xml.LineNumber;
            LinePosition = xml.LinePosition;
        }
    }

    public string ExceptionType { get; set; }
    public string Message { get; set; }
    public int? LineNumber { get; set; }
    public int? LinePosition { get; set; }
}

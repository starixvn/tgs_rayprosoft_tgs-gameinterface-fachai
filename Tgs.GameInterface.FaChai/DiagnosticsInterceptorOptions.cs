using System.Diagnostics;

namespace Tgs.GameInterface.FaChai;

public class DiagnosticsInterceptorOptions
{
    public ActivitySource InterceptorActivitySource { get; set; } =
        new ActivitySource($"Tgs.GameInterface.FaChai.DiagnosticsInterceptor");
}

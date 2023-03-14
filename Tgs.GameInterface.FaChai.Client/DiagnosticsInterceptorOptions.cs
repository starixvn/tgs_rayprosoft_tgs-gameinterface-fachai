using System.Diagnostics;

namespace Tgs.GameInterface.FaChai.Client;

public class DiagnosticsInterceptorOptions
{
    public ActivitySource InterceptorActivitySource { get; set; } =
        new ActivitySource($"Tgs.GameInterface.FaChai.Client.DiagnosticsInterceptor");
}

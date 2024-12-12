namespace Tgs.GameInterface.FaChai.Client;

public class GameInterfaceFaChaiClientOptions
{
	public Uri BaseUrl { get; set; } = default!;
	public TimeSpan GrpcConnectTimeout { get; set; } = TimeSpan.FromSeconds(5);

}

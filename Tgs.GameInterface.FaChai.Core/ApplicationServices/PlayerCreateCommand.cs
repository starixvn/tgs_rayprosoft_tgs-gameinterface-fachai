namespace Tgs.GameInterface.FaChai.Core.ApplicationServices;

public record PlayerCreateCommand(
	Uri ApiUrl,
	string AgentCode,
	string AgentKey,
	string Currency,
	string PlayerCode);

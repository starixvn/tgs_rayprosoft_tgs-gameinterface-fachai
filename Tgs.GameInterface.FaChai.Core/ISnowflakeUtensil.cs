namespace Tgs.GameInterface.FaChai.Core;

public interface ISnowflakeUtensil
{
    long GetDatacenterId();

    long GetWorkerId();

    long NextId();
}

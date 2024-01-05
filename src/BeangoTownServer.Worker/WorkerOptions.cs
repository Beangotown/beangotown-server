namespace BeangoTownServer.Worker;

public class WorkerOptions
{
    public const string RankLockKeyPrefix = "BeangoTownServer:Rank";
    public const string BatchLockKeyPrefix = "BeangoTownServer:Batch";
    public const string GameSyncLockKeyPrefix = "BeangoTownServer:GameSync";
    public const string RankBlockHeightPrefix = "RankBlockheight";
    public const string RankWeekTaskPrefix = "RankWeekTask:";
    public bool BatchStart { get; set; } = false;
    public bool GameSyncStart { get; set; } = false;
    
    public int RankTimePeriod { get; set; } = 30000;
    public  int TaskExpireDays { get; set; } = 30;
}
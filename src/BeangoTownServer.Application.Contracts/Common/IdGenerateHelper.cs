using System.Collections.Generic;

namespace BeangoTown.Indexer.Plugin;

public static class IdGenerateHelper
{
    public static string GenerateId(params object[] inputs)
    {
        return inputs.JoinAsString("-");
    }
}
namespace KP.Worker.Partitioner;

public static class CustomPartitioner
{
    public static int Partition(ReadOnlySpan<byte> keyData, ILogger logger)
    {
        var key = int.Parse(System.Text.Encoding.UTF8.GetString(keyData));
        
        logger.LogInformation($"Key: {key}");
        
        return key % 2 == 0 ? 0 : 1;
    }
}
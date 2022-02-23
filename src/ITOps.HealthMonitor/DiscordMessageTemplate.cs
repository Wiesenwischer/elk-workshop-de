namespace ITOps.HealthMonitor
{
    public static class DiscordMessageTemplate
    {
        public static string GetPayload()
        {
            return Payload;
        }

        public static string GetRestorePayload()
        {
            return RestorePayload;
        }

        const string Payload = "{\"content\":\"[[LIVENESS]]: [[FAILURE]] - Description: [[DESCRIPTIONS]]\"}";

        const string RestorePayload = "{\"content\":\"[[LIVENESS]] is back to life\"}";
    }
}

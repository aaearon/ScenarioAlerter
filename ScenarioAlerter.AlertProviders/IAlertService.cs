namespace ScenarioAlerter.AlertServices
{
    public interface IAlertService
    {
        public void SendAlertAsync(string message);
    }
}

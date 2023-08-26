namespace HappyHour.Interfaces
{
    public interface ICarrier
    {
        int MaxCarryAmount { get; }
        int CurrentCarryAmount { get; set; }
        void ReturnToBase();
        void DeliverResources();
    }
}
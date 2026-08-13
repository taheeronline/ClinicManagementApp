using System;

namespace ClinicManagement.Client.Services
{
    public class LoadingService
    {
        // Action: (isLoading, message)
        public event Action<bool, string?>? OnChanged;

        public void Show(string? message = null)
        {
            OnChanged?.Invoke(true, message);
        }

        public void Hide()
        {
            OnChanged?.Invoke(false, null);
        }
    }
}

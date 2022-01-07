using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    //public interface IPreferenceManager
    //{
    //    Task SetPreference(IPreference preference);

    //    Task<IPreference> GetPreference();

    //    Task ChangeLanguageAsync(string languageCode);
    //}

    //public class PreferenceManager : IPreferenceManager
    //{
    //    public async Task<IPreference> GetPreference()
    //    {
    //        return await _localStorageService.GetItemAsync<ClientPreference>(StorageConstants.Local.Preference) ?? new ClientPreference();
    //    }

    //    public async Task SetPreference(IPreference preference)
    //    {
    //        await _localStorageService.SetItemAsync(StorageConstants.Local.Preference, preference as ClientPreference);
    //    }
    //}

    //public interface IPreference
    //{
    //    public string LanguageCode { get; set; }
    //}
}

using Domain.Services;
using Messenger;
using Acciona.Domain.Model;
using ServiceLocator;
using Acciona.Domain.UseCase;

namespace Acciona.Domain
{
    public static class DomainConfiguration
    {
        public static void Init()
        {
            Locator.CurrentMutable.RegisterLazySingleton<IMessenger>(() => new MessengerHub());

            //UseCases
            Locator.CurrentMutable.RegisterLazySingleton(() => new GetVersionsUseCase());            
            Locator.CurrentMutable.RegisterLazySingleton(() => new LogoutUseCase());
            Locator.CurrentMutable.RegisterLazySingleton(() => new GetUserUseCase());
            Locator.CurrentMutable.RegisterLazySingleton(() => new GetAlertsUseCase());
            Locator.CurrentMutable.RegisterLazySingleton(() => new MarkAlertReadUseCase());
            Locator.CurrentMutable.RegisterLazySingleton(() => new GetPassportUseCase());
            Locator.CurrentMutable.RegisterLazySingleton(() => new GetPassportStatesUseCase());
            Locator.CurrentMutable.RegisterLazySingleton(() => new GetPassportStatesColorsUseCase());            
            Locator.CurrentMutable.RegisterLazySingleton(() => new GetRiskFactorsUseCase());
            Locator.CurrentMutable.RegisterLazySingleton(() => new GetSymptomTypesUseCase());
            Locator.CurrentMutable.RegisterLazySingleton(() => new GetMedicalMonitorsUseCase());
            Locator.CurrentMutable.RegisterLazySingleton(() => new GetFichaUseCase());
            Locator.CurrentMutable.RegisterLazySingleton(() => new SendRiskFactorsUseCase());
            Locator.CurrentMutable.RegisterLazySingleton(() => new SendSymptomsUseCase());
            Locator.CurrentMutable.RegisterLazySingleton(() => new SincroPendingUseCase());
            Locator.CurrentMutable.RegisterLazySingleton(() => new SaveTemperatureUseCase());
            Locator.CurrentMutable.RegisterLazySingleton(() => new GetSecurityPassportUseCase());
            Locator.CurrentMutable.RegisterLazySingleton(() => new ModifyFichaUseCase());
            Locator.CurrentMutable.RegisterLazySingleton(() => new GetResourceURLUseCase());
            Locator.CurrentMutable.RegisterLazySingleton(() => new GetOfflinePassportUseCase());
            Locator.CurrentMutable.RegisterLazySingleton(() => new GetUserPaperUseCase());
            Locator.CurrentMutable.RegisterLazySingleton(() => new GenerateManualUseCase());
            Locator.CurrentMutable.RegisterLazySingleton(() => new GetSecurityLocationsUseCase());
            Locator.CurrentMutable.RegisterLazySingleton(() => new GetLocationsUseCase());

            //Model Singletones
            Locator.CurrentMutable.RegisterLazySingleton(() => new AppSession());
        }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acciona.Domain;
using Acciona.Domain.Model;
using Acciona.Domain.Model.Employee;
using Acciona.Domain.Services;
using Acciona.Domain.UseCase;
using Acciona.Presentation.Navigation;
using Domain.Services;
using Newtonsoft.Json;
using Presentation.UI.Base;
using ServiceLocator;

namespace Acciona.Presentation.UI.Features.Login
{
    public class LoginPresenter : BasePresenter<LoginUI, ILoginNavigator>
    {                
        private readonly ISettingsService settingsService;
        private readonly GetUserUseCase getUserUseCase;
        private readonly GetPassportUseCase getPassportUseCase;
        //private readonly GetPassportStatesUseCase getPassportStatesUseCase;
        //private readonly GetPassportStatesColorsUseCase getPassportStatesColorsUseCase;        
        private readonly GetRiskFactorsUseCase getRiskFactorsUseCase;
        private readonly GetSymptomTypesUseCase getSymptomTypesUseCase;
        //private readonly GetMedicalMonitorsUseCase getMedicalMonitorsUseCase;        
        private readonly SendRiskFactorsUseCase senrRiskFactorsUseCase;


        private string oldUser;

        public LoginPresenter()
        {            
            settingsService = Locator.Current.GetService<ISettingsService>();
            getUserUseCase = Locator.Current.GetService<GetUserUseCase>();
            getPassportUseCase = Locator.Current.GetService<GetPassportUseCase>();
            //getPassportStatesUseCase = Locator.Current.GetService<GetPassportStatesUseCase>();
            //getPassportStatesColorsUseCase = Locator.Current.GetService<GetPassportStatesColorsUseCase>();            
            getRiskFactorsUseCase = Locator.Current.GetService<GetRiskFactorsUseCase>();
            getSymptomTypesUseCase = Locator.Current.GetService<GetSymptomTypesUseCase>();
             //getMedicalMonitorsUseCase = Locator.Current.GetService<GetMedicalMonitorsUseCase>();
            senrRiskFactorsUseCase = Locator.Current.GetService<SendRiskFactorsUseCase>();
        }

        public override void OnCreate()
        {
            base.OnCreate();
            oldUser = settingsService.GetValueOrDefault<string>(DomainConstants.LAST_USER, "");
            View.SetLastUser(oldUser);
        }        

        public async Task Authorized(string token)
        {
            
            
            var appSession = Locator.Current.GetService<AppSession>();
            appSession.AccesToken = token;

            View.ShowLoading();
            var userResponse = await getUserUseCase.Execute();
            if (userResponse.ErrorCode > 0)
            {
                View.HideLoading();
                if (userResponse.ErrorCode == 401)
                    View.ShowDialog("error_401_user", "msg_ok", null);
                else
                    View.ShowDialog(userResponse.Message, "msg_ok", null);
                View.ShowRetry();
            }
            else
            {
                settingsService.AddOrUpdateValue<string>(DomainConstants.LAST_SESSION, appSession.AccesToken);
                settingsService.AddOrUpdateValue<string>(DomainConstants.LAST_USER, appSession.User);
                settingsService.AddOrUpdateValue<long>(DomainConstants.LAST_DATE, DateTime.Now.Ticks);
                settingsService.AddOrUpdateValue<string>(DomainConstants.LAST_USER_INFO, JsonConvert.SerializeObject(appSession.UserInfo));
                var passportResponse=await getPassportUseCase.Execute(true);
                //await getPassportStatesUseCase.Execute(true);
                //await getPassportStatesColorsUseCase.Execute(true);                
                await getRiskFactorsUseCase.Execute(true);
                await getSymptomTypesUseCase.Execute(true);
                //await getMedicalMonitorsUseCase.Execute(true);
                View.HideLoading();
                if (passportResponse.ErrorCode > 0)
                {
                    View.ShowDialog(passportResponse.Message, "msg_ok", null);
                }
                else
                {
                    if (passportResponse.Data == null)
                    {
                        //navigator.GoToMedicalTest();
                        var resFactors=await getRiskFactorsUseCase.Execute();
                        if (resFactors.ErrorCode > 0)
                        {
                            View.ShowDialog(resFactors.Message, "msg_ok", null);
                            return;
                        }
                        var factors = resFactors.Data;
                        List<RequestValue> values = new List<RequestValue>();
                        for (int i = 0; i < factors.Count(); i++)
                        {
                            var value = new RequestValue()
                            {
                                Id = factors.ElementAt(i).IdRiskFactor,
                                Value = (bool?)null
                            };
                            values.Add(value);
                        }
                        var response = await senrRiskFactorsUseCase.Execute(values);
                        if (response.ErrorCode > 0)
                        {                            
                            View.ShowDialog(response.Message, "msg_ok", null);
                        }
                        else
                        {
                            navigator.GoToMain();
                        }
                    }
                    else
                        navigator.GoToMain();
                }
            } 
        }
    }
}

